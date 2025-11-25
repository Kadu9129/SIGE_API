using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGE.API.DTOs;
using SIGE.API.Interfaces;
using SIGE.API.Models;

namespace SIGE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Listar usuários com filtros e paginação
        /// </summary>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="search">Termo de busca</param>
        /// <returns>Lista de usuários</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioListDto>>> GetUsuarios(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 15,
            [FromQuery] string? search = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 15;

            var usuarios = await _usuarioService.GetAllAsync(page, pageSize, search);
            return Ok(usuarios);
        }

        /// <summary>
        /// Obter usuário por ID
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns>Dados do usuário</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            
            if (usuario == null)
                return NotFound(new { message = "Usuário não encontrado" });

            return Ok(usuario);
        }

        /// <summary>
        /// Criar novo usuário
        /// </summary>
        /// <param name="createUsuarioDto">Dados do usuário</param>
        /// <returns>Usuário criado</returns>
        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> CreateUsuario([FromBody] CreateUsuarioDto createUsuarioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _usuarioService.CreateAsync(createUsuarioDto);
            
            if (usuario == null)
                return BadRequest(new { message = "Email ou CPF já existe" });

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        /// <summary>
        /// Atualizar usuário
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="updateUsuarioDto">Dados atualizados</param>
        /// <returns>Usuário atualizado</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioDto>> UpdateUsuario(int id, [FromBody] UpdateUsuarioDto updateUsuarioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _usuarioService.UpdateAsync(id, updateUsuarioDto);
            
            if (usuario == null)
                return NotFound(new { message = "Usuário não encontrado ou email/CPF já existe" });

            return Ok(usuario);
        }

        /// <summary>
        /// Deletar usuário
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns>Status da operação</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var result = await _usuarioService.DeleteAsync(id);
            
            if (!result)
                return BadRequest(new { message = "Usuário não encontrado ou possui dependências" });

            return NoContent();
        }

        /// <summary>
        /// Alterar status do usuário
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="status">Novo status</param>
        /// <returns>Status da operação</returns>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUsuario status)
        {
            var result = await _usuarioService.UpdateStatusAsync(id, status);
            
            if (!result)
                return NotFound(new { message = "Usuário não encontrado" });

            return Ok(new { message = "Status atualizado com sucesso" });
        }

        /// <summary>
        /// Upload de foto de perfil
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="photo">Arquivo de foto</param>
        /// <returns>Status da operação</returns>
        [HttpPost("{id}/foto")]
        public async Task<IActionResult> UploadPhoto(int id, IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
                return BadRequest(new { message = "Arquivo de foto é obrigatório" });

            // Validar tipo de arquivo
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(photo.ContentType.ToLower()))
                return BadRequest(new { message = "Apenas arquivos de imagem são permitidos" });

            // Validar tamanho (5MB)
            if (photo.Length > 5 * 1024 * 1024)
                return BadRequest(new { message = "Arquivo deve ter no máximo 5MB" });

            var result = await _usuarioService.UploadPhotoAsync(id, photo);
            
            if (!result)
                return BadRequest(new { message = "Erro ao fazer upload da foto" });

            return Ok(new { message = "Foto atualizada com sucesso" });
        }

        /// <summary>
        /// Listar tipos de usuário disponíveis
        /// </summary>
        /// <returns>Lista de tipos</returns>
        [HttpGet("tipos")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserTypes()
        {
            var types = await _usuarioService.GetUserTypesAsync();
            return Ok(types);
        }
    }
}