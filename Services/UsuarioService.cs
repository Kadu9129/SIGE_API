using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SIGE.API.Data;
using SIGE.API.DTOs;
using SIGE.API.Interfaces;
using SIGE.API.Models;

namespace SIGE.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly SIGEDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsuarioService(SIGEDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<UsuarioListDto>> GetAllAsync(int page = 1, int pageSize = 15, string? search = null)
        {
            var query = _context.Usuarios.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.Nome.Contains(search) || u.Email.Contains(search));
            }

            var usuarios = await query
                .OrderBy(u => u.Nome)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UsuarioListDto>>(usuarios);
        }

        public async Task<UsuarioDto?> GetByIdAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            return usuario != null ? _mapper.Map<UsuarioDto>(usuario) : null;
        }

        public async Task<UsuarioDto?> CreateAsync(CreateUsuarioDto createUsuarioDto)
        {
            // Verificar se email já existe
            if (await _context.Usuarios.AnyAsync(u => u.Email == createUsuarioDto.Email))
                return null;

            // Verificar se CPF já existe (se fornecido)
            if (!string.IsNullOrEmpty(createUsuarioDto.CPF) && 
                await _context.Usuarios.AnyAsync(u => u.CPF == createUsuarioDto.CPF))
                return null;

            var usuario = _mapper.Map<Usuario>(createUsuarioDto);
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(createUsuarioDto.Senha);
            usuario.DataCriacao = DateTime.UtcNow;
            usuario.DataUltimaAtualizacao = DateTime.UtcNow;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto?> UpdateAsync(int id, UpdateUsuarioDto updateUsuarioDto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return null;

            // Verificar se email já existe em outro usuário
            if (await _context.Usuarios.AnyAsync(u => u.Email == updateUsuarioDto.Email && u.Id != id))
                return null;

            // Verificar se CPF já existe em outro usuário (se fornecido)
            if (!string.IsNullOrEmpty(updateUsuarioDto.CPF) && 
                await _context.Usuarios.AnyAsync(u => u.CPF == updateUsuarioDto.CPF && u.Id != id))
                return null;

            _mapper.Map(updateUsuarioDto, usuario);
            usuario.DataUltimaAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            // Verificar se usuário pode ser excluído (não está sendo usado em outras entidades)
            var temDependencias = await _context.Escolas.AnyAsync(e => e.DiretorId == id) ||
                                   await _context.Alunos.AnyAsync(a => a.UsuarioId == id) ||
                                   await _context.Professores.AnyAsync(p => p.UsuarioId == id) ||
                                   await _context.Responsaveis.AnyAsync(r => r.UsuarioId == id);

            if (temDependencias)
                return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(int id, StatusUsuario status)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            usuario.Status = status;
            usuario.DataUltimaAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UploadPhotoAsync(int id, IFormFile photo)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            try
            {
                var uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "fotos");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var fileName = $"{id}_{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                // Deletar foto anterior se existir
                if (!string.IsNullOrEmpty(usuario.FotoPerfil))
                {
                    var oldPhotoPath = Path.Combine(_webHostEnvironment.WebRootPath, usuario.FotoPerfil.TrimStart('/'));
                    if (File.Exists(oldPhotoPath))
                        File.Delete(oldPhotoPath);
                }

                usuario.FotoPerfil = $"/uploads/fotos/{fileName}";
                usuario.DataUltimaAtualizacao = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetUserTypesAsync()
        {
            return await Task.FromResult(Enum.GetNames(typeof(TipoUsuario)));
        }
    }
}