using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGE.API.DTOs;
using SIGE.API.Interfaces;
using SIGE.API.Models;
using System.ComponentModel.DataAnnotations;

namespace SIGE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        /// <summary>
        /// Obter lista de alunos com paginação e filtros
        /// </summary>
        /// <param name="page">Página (padrão: 1)</param>
        /// <param name="pageSize">Itens por página (padrão: 10)</param>
        /// <param name="search">Busca por nome, matrícula ou CPF</param>
        /// <param name="escolaId">Filtrar por escola</param>
        /// <param name="status">Filtrar por status</param>
        /// <returns>Lista paginada de alunos</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<AlunoDto>>>> GetAlunos(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] int? escolaId = null,
            [FromQuery] StatusAluno? status = null)
        {
            var result = await _alunoService.GetAlunosAsync(page, pageSize, search, escolaId, status);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Obter aluno por ID
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Dados do aluno</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AlunoDto>>> GetAluno(int id)
        {
            var result = await _alunoService.GetAlunoByIdAsync(id);
            
            if (!result.Success)
            {
                return result.StatusCode == 404 ? NotFound(result) : BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Criar novo aluno
        /// </summary>
        /// <param name="createAlunoDto">Dados do aluno</param>
        /// <returns>Aluno criado</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Diretor")]
        public async Task<ActionResult<ApiResponse<AlunoDto>>> CreateAluno(
            [FromBody] CreateAlunoDto createAlunoDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(ApiResponse<object>.Error($"Dados inválidos: {string.Join(", ", errors)}"));
            }

            var result = await _alunoService.CreateAlunoAsync(createAlunoDto);
            
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetAluno), new { id = result.Data!.Id }, result);
        }

        /// <summary>
        /// Atualizar dados do aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <param name="updateAlunoDto">Dados para atualização</param>
        /// <returns>Aluno atualizado</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Diretor")]
        public async Task<ActionResult<ApiResponse<AlunoDto>>> UpdateAluno(
            int id,
            [FromBody] UpdateAlunoDto updateAlunoDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(ApiResponse<object>.Error($"Dados inválidos: {string.Join(", ", errors)}"));
            }

            var result = await _alunoService.UpdateAlunoAsync(id, updateAlunoDto);
            
            if (!result.Success)
            {
                return result.StatusCode == 404 ? NotFound(result) : BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Remover aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Confirmação da remoção</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteAluno(int id)
        {
            var result = await _alunoService.DeleteAlunoAsync(id);
            
            if (!result.Success)
            {
                return result.StatusCode == 404 ? NotFound(result) : BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Obter alunos de uma turma específica
        /// </summary>
        /// <param name="turmaId">ID da turma</param>
        /// <returns>Lista de alunos da turma</returns>
        [HttpGet("turma/{turmaId}")]
        public async Task<ActionResult<ApiResponse<List<AlunoDto>>>> GetAlunosByTurma(int turmaId)
        {
            var result = await _alunoService.GetAlunosByTurmaAsync(turmaId);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Alterar status do aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <param name="status">Novo status</param>
        /// <returns>Confirmação da alteração</returns>
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Diretor")]
        public async Task<ActionResult<ApiResponse<bool>>> ChangeStatus(
            int id,
            [FromBody] [Required] StatusAluno status)
        {
            var result = await _alunoService.ChangeStatusAsync(id, status);
            
            if (!result.Success)
            {
                return result.StatusCode == 404 ? NotFound(result) : BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Upload de foto do aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <param name="foto">Arquivo da foto</param>
        /// <returns>URL da foto</returns>
        [HttpPost("{id}/foto")]
        [Authorize(Roles = "Admin,Diretor")]
        public async Task<ActionResult<ApiResponse<string>>> UploadFoto(
            int id,
            IFormFile foto)
        {
            if (foto == null || foto.Length == 0)
            {
                return BadRequest(ApiResponse<string>.Error("Nenhum arquivo foi enviado"));
            }

            // Validar tipo de arquivo
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(foto.ContentType))
            {
                return BadRequest(ApiResponse<string>.Error("Tipo de arquivo não permitido. Use apenas: JPG, PNG, GIF"));
            }

            // Validar tamanho (5MB)
            if (foto.Length > 5 * 1024 * 1024)
            {
                return BadRequest(ApiResponse<string>.Error("Arquivo muito grande. Tamanho máximo: 5MB"));
            }

            try
            {
                // Verificar se o aluno existe
                var alunoResult = await _alunoService.GetAlunoByIdAsync(id);
                if (!alunoResult.Success)
                {
                    return NotFound(alunoResult);
                }

                // Gerar nome único para o arquivo
                var fileName = $"aluno_{id}_{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
                var filePath = Path.Combine("wwwroot", "uploads", "fotos", fileName);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                // Garantir que o diretório existe
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

                // Salvar arquivo
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }

                // URL relativa para retornar
                var photoUrl = $"/uploads/fotos/{fileName}";

                return Ok(ApiResponse<string>.Ok(photoUrl, "Foto enviada com sucesso"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Error($"Erro ao fazer upload da foto: {ex.Message}"));
            }
        }
    }
}