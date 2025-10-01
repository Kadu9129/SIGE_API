using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGE.API.DTOs;
using SIGE.API.Interfaces;

namespace SIGE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EscolasController : ControllerBase
    {
        private readonly IEscolaService _escolaService;

        public EscolasController(IEscolaService escolaService)
        {
            _escolaService = escolaService;
        }

        /// <summary>
        /// Listar escolas com filtros e paginação
        /// </summary>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="search">Termo de busca</param>
        /// <returns>Lista de escolas</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EscolaDto>>> GetEscolas(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 15,
            [FromQuery] string? search = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 15;

            var escolas = await _escolaService.GetAllAsync(page, pageSize, search);
            return Ok(escolas);
        }

        /// <summary>
        /// Obter escola por ID
        /// </summary>
        /// <param name="id">ID da escola</param>
        /// <returns>Dados da escola</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EscolaDto>> GetEscola(int id)
        {
            var escola = await _escolaService.GetByIdAsync(id);
            
            if (escola == null)
                return NotFound(new { message = "Escola não encontrada" });

            return Ok(escola);
        }

        /// <summary>
        /// Criar nova escola
        /// </summary>
        /// <param name="createEscolaDto">Dados da escola</param>
        /// <returns>Escola criada</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EscolaDto>> CreateEscola([FromBody] CreateEscolaDto createEscolaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var escola = await _escolaService.CreateAsync(createEscolaDto);
            
            if (escola == null)
                return BadRequest(new { message = "CNPJ já existe" });

            return CreatedAtAction(nameof(GetEscola), new { id = escola.Id }, escola);
        }

        /// <summary>
        /// Atualizar escola
        /// </summary>
        /// <param name="id">ID da escola</param>
        /// <param name="updateEscolaDto">Dados atualizados</param>
        /// <returns>Escola atualizada</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Diretor")]
        public async Task<ActionResult<EscolaDto>> UpdateEscola(int id, [FromBody] UpdateEscolaDto updateEscolaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var escola = await _escolaService.UpdateAsync(id, updateEscolaDto);
            
            if (escola == null)
                return NotFound(new { message = "Escola não encontrada ou CNPJ já existe" });

            return Ok(escola);
        }

        /// <summary>
        /// Deletar escola
        /// </summary>
        /// <param name="id">ID da escola</param>
        /// <returns>Status da operação</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEscola(int id)
        {
            var result = await _escolaService.DeleteAsync(id);
            
            if (!result)
                return BadRequest(new { message = "Escola não encontrada ou possui dependências" });

            return NoContent();
        }

        /// <summary>
        /// Obter estatísticas da escola
        /// </summary>
        /// <param name="id">ID da escola</param>
        /// <returns>Estatísticas</returns>
        [HttpGet("{id}/estatisticas")]
        public async Task<ActionResult> GetEstatisticas(int id)
        {
            var estatisticas = await _escolaService.GetEstatisticasAsync(id);
            
            if (estatisticas == null)
                return NotFound(new { message = "Escola não encontrada" });

            return Ok(estatisticas);
        }
    }
}