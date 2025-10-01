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
    public class ProfessoresController : ControllerBase
    {
        private readonly IProfessorService _professorService;

        public ProfessoresController(IProfessorService professorService)
        {
            _professorService = professorService;
        }

        /// <summary>
        /// Obter lista de professores com paginação e filtros
        /// </summary>
        /// <param name="page">Página (padrão: 1)</param>
        /// <param name="pageSize">Itens por página (padrão: 10)</param>
        /// <param name="search">Busca por nome, código ou CPF</param>
        /// <param name="escolaId">Filtrar por escola</param>
        /// <param name="status">Filtrar por status</param>
        /// <returns>Lista paginada de professores</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ProfessorDto>>>> GetProfessores(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] int? escolaId = null,
            [FromQuery] StatusProfessor? status = null)
        {
            var result = await _professorService.GetProfessoresAsync(page, pageSize, search, escolaId, status);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Obter professor por ID
        /// </summary>
        /// <param name="id">ID do professor</param>
        /// <returns>Dados do professor</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProfessorDto>>> GetProfessor(int id)
        {
            var result = await _professorService.GetProfessorByIdAsync(id);
            
            if (!result.Success)
            {
                return result.StatusCode == 404 ? NotFound(result) : BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Criar novo professor
        /// </summary>
        /// <param name="createProfessorDto">Dados do professor</param>
        /// <returns>Professor criado</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Diretor")]
        public async Task<ActionResult<ApiResponse<ProfessorDto>>> CreateProfessor(
            [FromBody] CreateProfessorDto createProfessorDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(ApiResponse<object>.Error($"Dados inválidos: {string.Join(", ", errors)}"));
            }

            var result = await _professorService.CreateProfessorAsync(createProfessorDto);
            
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetProfessor), new { id = result.Data!.Id }, result);
        }

        /// <summary>
        /// Atualizar dados do professor
        /// </summary>
        /// <param name="id">ID do professor</param>
        /// <param name="updateProfessorDto">Dados para atualização</param>
        /// <returns>Professor atualizado</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Diretor")]
        public async Task<ActionResult<ApiResponse<ProfessorDto>>> UpdateProfessor(
            int id,
            [FromBody] UpdateProfessorDto updateProfessorDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(ApiResponse<object>.Error($"Dados inválidos: {string.Join(", ", errors)}"));
            }

            var result = await _professorService.UpdateProfessorAsync(id, updateProfessorDto);
            
            if (!result.Success)
            {
                return result.StatusCode == 404 ? NotFound(result) : BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Remover professor
        /// </summary>
        /// <param name="id">ID do professor</param>
        /// <returns>Confirmação da remoção</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProfessor(int id)
        {
            var result = await _professorService.DeleteProfessorAsync(id);
            
            if (!result.Success)
            {
                return result.StatusCode == 404 ? NotFound(result) : BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Obter disciplinas do professor
        /// </summary>
        /// <param name="id">ID do professor</param>
        /// <param name="anoLetivo">Ano letivo (opcional)</param>
        /// <returns>Lista de disciplinas</returns>
        [HttpGet("{id}/disciplinas")]
        public async Task<ActionResult<ApiResponse<List<DisciplinaDto>>>> GetDisciplinasProfessor(
            int id,
            [FromQuery] int? anoLetivo = null)
        {
            var result = await _professorService.GetDisciplinasProfessorAsync(id, anoLetivo);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Obter turmas do professor
        /// </summary>
        /// <param name="id">ID do professor</param>
        /// <param name="anoLetivo">Ano letivo (opcional)</param>
        /// <returns>Lista de turmas</returns>
        [HttpGet("{id}/turmas")]
        public async Task<ActionResult<ApiResponse<List<TurmaDto>>>> GetTurmasProfessor(
            int id,
            [FromQuery] int? anoLetivo = null)
        {
            var result = await _professorService.GetTurmasProfessorAsync(id, anoLetivo);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Atribuir disciplina ao professor
        /// </summary>
        /// <param name="id">ID do professor</param>
        /// <param name="disciplinaId">ID da disciplina</param>
        /// <param name="anoLetivo">Ano letivo</param>
        /// <returns>Confirmação da atribuição</returns>
        [HttpPost("{id}/disciplinas/{disciplinaId}")]
        [Authorize(Roles = "Admin,Diretor")]
        public async Task<ActionResult<ApiResponse<bool>>> AtribuirDisciplina(
            int id,
            int disciplinaId,
            [FromBody] [Required] int anoLetivo)
        {
            var result = await _professorService.AtribuirDisciplinaAsync(id, disciplinaId, anoLetivo);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Remover disciplina do professor
        /// </summary>
        /// <param name="id">ID do professor</param>
        /// <param name="disciplinaId">ID da disciplina</param>
        /// <param name="anoLetivo">Ano letivo</param>
        /// <returns>Confirmação da remoção</returns>
        [HttpDelete("{id}/disciplinas/{disciplinaId}")]
        [Authorize(Roles = "Admin,Diretor")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoverDisciplina(
            int id,
            int disciplinaId,
            [FromQuery] int anoLetivo)
        {
            var result = await _professorService.RemoverDisciplinaAsync(id, disciplinaId, anoLetivo);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Alterar status do professor
        /// </summary>
        /// <param name="id">ID do professor</param>
        /// <param name="status">Novo status</param>
        /// <returns>Confirmação da alteração</returns>
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Diretor")]
        public async Task<ActionResult<ApiResponse<bool>>> ChangeStatus(
            int id,
            [FromBody] [Required] StatusProfessor status)
        {
            var result = await _professorService.ChangeStatusAsync(id, status);
            
            if (!result.Success)
            {
                return result.StatusCode == 404 ? NotFound(result) : BadRequest(result);
            }

            return Ok(result);
        }
    }
}