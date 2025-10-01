using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SIGE.API.DTOs;
using SIGE.API.Interfaces;
using SIGE.API.Models;
using SIGE.API.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace SIGE.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly SIGEDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(SIGEDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Status == StatusUsuario.Ativo);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.SenhaHash))
                return null;

            var token = GenerateJwtToken(usuario);
            var expiracao = DateTime.UtcNow.AddHours(8);

            // Salvar sessão
            var sessao = new Sessao
            {
                UsuarioId = usuario.Id,
                Token = token,
                DataExpiracao = expiracao
            };

            _context.Sessoes.Add(sessao);
            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                Token = token,
                Expiracao = expiracao,
                Usuario = _mapper.Map<UsuarioDto>(usuario)
            };
        }

        public async Task<bool> LogoutAsync(string token)
        {
            var sessao = await _context.Sessoes.FirstOrDefaultAsync(s => s.Token == token);
            
            if (sessao != null)
            {
                _context.Sessoes.Remove(sessao);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<string?> RefreshTokenAsync(string token)
        {
            var sessao = await _context.Sessoes
                .Include(s => s.Usuario)
                .FirstOrDefaultAsync(s => s.Token == token);

            if (sessao == null || sessao.DataExpiracao <= DateTime.UtcNow)
                return null;

            var novoToken = GenerateJwtToken(sessao.Usuario);
            sessao.Token = novoToken;
            sessao.DataExpiracao = DateTime.UtcNow.AddHours(8);

            await _context.SaveChangesAsync();
            return novoToken;
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            var usuario = await _context.Usuarios.FindAsync(userId);
            
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(changePasswordDto.SenhaAtual, usuario.SenhaHash))
                return false;

            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NovaSenha);
            usuario.DataUltimaAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);

            if (usuario == null)
                return false;

            // Aqui você implementaria o envio de email com o token de reset
            // Por agora, apenas retornamos true
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            // Aqui você validaria o token de reset
            // Por agora, apenas implementando a estrutura básica
            return await Task.FromResult(false);
        }

        public async Task<UsuarioDto?> GetCurrentUserAsync(int userId)
        {
            var usuario = await _context.Usuarios.FindAsync(userId);
            return usuario != null ? _mapper.Map<UsuarioDto>(usuario) : null;
        }

        public async Task<bool> VerifyTokenAsync(string token)
        {
            var sessao = await _context.Sessoes
                .FirstOrDefaultAsync(s => s.Token == token && s.DataExpiracao > DateTime.UtcNow);

            return sessao != null;
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.TipoUsuario.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}