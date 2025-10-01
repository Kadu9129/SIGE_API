using Microsoft.AspNetCore.Mvc;
using SIGE.API.DTOs;
using SIGE.API.Interfaces;
using SIGE.API.Models;

namespace SIGE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login do usuário
        /// </summary>
        /// <param name="loginDto">Dados de login</param>
        /// <returns>Token JWT e dados do usuário</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDto);
            
            if (result == null)
                return Unauthorized(new { message = "Credenciais inválidas" });

            return Ok(result);
        }

        /// <summary>
        /// Logout do usuário
        /// </summary>
        /// <returns>Status da operação</returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            
            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token não fornecido" });

            var result = await _authService.LogoutAsync(token);
            
            if (!result)
                return BadRequest(new { message = "Erro ao fazer logout" });

            return Ok(new { message = "Logout realizado com sucesso" });
        }

        /// <summary>
        /// Renovar token
        /// </summary>
        /// <returns>Novo token</returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            
            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token não fornecido" });

            var newToken = await _authService.RefreshTokenAsync(token);
            
            if (newToken == null)
                return Unauthorized(new { message = "Token inválido ou expirado" });

            return Ok(new { token = newToken, expiracao = DateTime.UtcNow.AddHours(8) });
        }

        /// <summary>
        /// Alterar senha
        /// </summary>
        /// <param name="changePasswordDto">Dados para alterar senha</param>
        /// <returns>Status da operação</returns>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Aqui você extrairia o userId do token JWT
            var userId = GetCurrentUserId();
            
            if (userId == null)
                return Unauthorized();

            var result = await _authService.ChangePasswordAsync(userId.Value, changePasswordDto);
            
            if (!result)
                return BadRequest(new { message = "Senha atual incorreta" });

            return Ok(new { message = "Senha alterada com sucesso" });
        }

        /// <summary>
        /// Solicitar redefinição de senha
        /// </summary>
        /// <param name="forgotPasswordDto">Email para redefinição</param>
        /// <returns>Status da operação</returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ForgotPasswordAsync(forgotPasswordDto);
            
            // Sempre retorna sucesso por segurança, mesmo se o email não existir
            return Ok(new { message = "Se o email existir, instruções de redefinição foram enviadas" });
        }

        /// <summary>
        /// Redefinir senha
        /// </summary>
        /// <param name="resetPasswordDto">Dados para redefinição</param>
        /// <returns>Status da operação</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ResetPasswordAsync(resetPasswordDto);
            
            if (!result)
                return BadRequest(new { message = "Token inválido ou expirado" });

            return Ok(new { message = "Senha redefinida com sucesso" });
        }

        /// <summary>
        /// Obter dados do usuário logado
        /// </summary>
        /// <returns>Dados do usuário</returns>
        [HttpGet("me")]
        public async Task<ActionResult<UsuarioDto>> GetCurrentUser()
        {
            var userId = GetCurrentUserId();
            
            if (userId == null)
                return Unauthorized();

            var user = await _authService.GetCurrentUserAsync(userId.Value);
            
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Verificar validade do token
        /// </summary>
        /// <returns>Status da validade</returns>
        [HttpGet("verify-token")]
        public async Task<IActionResult> VerifyToken()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            
            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token não fornecido" });

            var isValid = await _authService.VerifyTokenAsync(token);
            
            return Ok(new { valid = isValid });
        }

        private int? GetCurrentUserId()
        {
            // Aqui você extrairia o userId do token JWT
            // Por agora, retornando um valor fixo para exemplo
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : null;
        }
    }
}