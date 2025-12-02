using Microsoft.AspNetCore.Mvc;
using SIGE.API.DTOs;
using SIGE.API.Interfaces;
using SIGE.API.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SIGE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurmasController : ControllerBase
    {
        private readonly ITurmaService _turmaService;

        public TurmasController(ITurmaService turmaService)
        {
            _turmaService = turmaService;
        }

        /// <summary>
        /// Obter catálogos auxiliares (cursos, professores e alunos)
        /// </summary>
        [HttpGet("catalogos")]
        public async Task<ActionResult<ApiResponse<TurmaCatalogosDto>>> GetCatalogos()
        {
            var result = await _turmaService.GetCatalogosAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Listar turmas com filtros e paginação
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<TurmaDto>>>> GetTurmas(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] int? cursoId = null,
            [FromQuery] int? anoLetivo = null,
            [FromQuery] StatusTurma? status = null)
        {
            var result = await _turmaService.GetTurmasAsync(page, pageSize, search, cursoId, anoLetivo, status);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Obter detalhes de uma turma
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TurmaDto>>> GetTurma(int id)
        {
            var result = await _turmaService.GetTurmaByIdAsync(id);

            if (!result.Success)
            {
                return result.StatusCode == 404 ? NotFound(result) : BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Criar nova turma
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TurmaDto>>> CreateTurma([FromBody] CreateTurmaDto createTurmaDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<object>.Error($"Dados inválidos: {string.Join(", ", errors)}"));
            }

            var result = await _turmaService.CreateTurmaAsync(createTurmaDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetTurma), new { id = result.Data!.Id }, result);
        }

        /// <summary>
        /// Atualizar turma
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TurmaDto>>> UpdateTurma(int id, [FromBody] UpdateTurmaDto updateTurmaDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(ApiResponse<object>.Error($"Dados inválidos: {string.Join(", ", errors)}"));
            }

            var result = await _turmaService.UpdateTurmaAsync(id, updateTurmaDto);

            if (!result.Success)
            {
                return result.StatusCode == 404 ? NotFound(result) : BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Excluir turma
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteTurma(int id)
        {
            var result = await _turmaService.DeleteTurmaAsync(id);

            if (!result.Success)
            {
                return result.StatusCode == 404 ? NotFound(result) : BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Alterar status da turma
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<ApiResponse<bool>>> ChangeStatus(int id, [FromBody] [Required] StatusTurma status)
        {
            var result = await _turmaService.ChangeStatusAsync(id, status);

            if (!result.Success)
            {
                return result.StatusCode == 404 ? NotFound(result) : BadRequest(result);
            }

            return Ok(result);
        }
    }
}
