using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SIGE.API.Data;
using SIGE.API.DTOs;
using SIGE.API.Interfaces;
using SIGE.API.Models;

namespace SIGE.API.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly SIGEDbContext _context;
        private readonly IMapper _mapper;

        public ProfessorService(SIGEDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PaginatedResponse<ProfessorDto>>> GetProfessoresAsync(int page = 1, int pageSize = 10, string? search = null, int? escolaId = null, StatusProfessor? status = null)
        {
            try
            {
                var query = _context.Professores
                    .Include(p => p.Escola)
                    .Include(p => p.ProfessoresDisciplinas)
                        .ThenInclude(pd => pd.Disciplina)
                    .AsQueryable();

                if (escolaId.HasValue)
                    query = query.Where(p => p.EscolaId == escolaId.Value);

                if (status.HasValue)
                    query = query.Where(p => p.Status == status.Value);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(p => 
                        p.NomeCompleto.Contains(search) ||
                        p.CPF.Contains(search) ||
                        p.CodigoProfessor.Contains(search));
                }

                var totalItems = await query.CountAsync();
                var items = await query
                    .OrderBy(p => p.NomeCompleto)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var professoresDto = _mapper.Map<List<ProfessorDto>>(items);
                var paginatedResponse = new PaginatedResponse<ProfessorDto>
                {
                    Items = professoresDto,
                    TotalItems = totalItems,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
                };

                return ApiResponse<PaginatedResponse<ProfessorDto>>.Ok(paginatedResponse, "Professores listados com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginatedResponse<ProfessorDto>>.Error($"Erro ao listar professores: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ProfessorDto>> GetProfessorByIdAsync(int id)
        {
            try
            {
                var professor = await _context.Professores
                    .Include(p => p.Escola)
                    .Include(p => p.ProfessoresDisciplinas)
                        .ThenInclude(pd => pd.Disciplina)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (professor == null)
                    return ApiResponse<ProfessorDto>.Error("Professor não encontrado", 404);

                var professorDto = _mapper.Map<ProfessorDto>(professor);
                return ApiResponse<ProfessorDto>.Ok(professorDto, "Professor encontrado com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProfessorDto>.Error($"Erro ao buscar professor: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ProfessorDto>> CreateProfessorAsync(CreateProfessorDto createProfessorDto)
        {
            try
            {
                // Verificar se já existe professor com mesmo CPF ou email
                var existingProfessor = await _context.Professores
                    .FirstOrDefaultAsync(p => p.CPF == createProfessorDto.CPF || p.CodigoProfessor == createProfessorDto.CodigoProfessor);

                if (existingProfessor != null)
                    return ApiResponse<ProfessorDto>.Error("Já existe um professor com este CPF ou código");

                // Verificar se a escola existe
                var escola = await _context.Escolas.FindAsync(createProfessorDto.EscolaId);
                if (escola == null)
                    return ApiResponse<ProfessorDto>.Error("Escola não encontrada", 404);

                var professor = _mapper.Map<Professor>(createProfessorDto);
                professor.DataAdmissao = DateTime.UtcNow;
                professor.Status = StatusProfessor.Ativo;

                _context.Professores.Add(professor);
                await _context.SaveChangesAsync();

                // Recarregar com includes
                var createdProfessor = await _context.Professores
                    .Include(p => p.Escola)
                    .FirstOrDefaultAsync(p => p.Id == professor.Id);

                var professorDto = _mapper.Map<ProfessorDto>(createdProfessor);
                return ApiResponse<ProfessorDto>.Ok(professorDto, "Professor criado com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProfessorDto>.Error($"Erro ao criar professor: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ProfessorDto>> UpdateProfessorAsync(int id, UpdateProfessorDto updateProfessorDto)
        {
            try
            {
                var professor = await _context.Professores
                    .Include(p => p.Escola)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (professor == null)
                    return ApiResponse<ProfessorDto>.Error("Professor não encontrado", 404);

                // Verificar se CPF ou email já existem em outro professor
                var existingProfessor = await _context.Professores
                    .FirstOrDefaultAsync(p => p.Id != id && (p.CPF == updateProfessorDto.CPF));

                if (existingProfessor != null)
                    return ApiResponse<ProfessorDto>.Error("Já existe outro professor com este CPF ou código");

                _mapper.Map(updateProfessorDto, professor);
                // professor não possui campo DataUltimaAtualizacao no modelo atual

                await _context.SaveChangesAsync();

                // Recarregar com includes
                var updatedProfessor = await _context.Professores
                    .Include(p => p.Escola)
                    .Include(p => p.ProfessoresDisciplinas)
                        .ThenInclude(pd => pd.Disciplina)
                    .FirstOrDefaultAsync(p => p.Id == id);

                var professorDto = _mapper.Map<ProfessorDto>(updatedProfessor);
                return ApiResponse<ProfessorDto>.Ok(professorDto, "Professor atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProfessorDto>.Error($"Erro ao atualizar professor: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteProfessorAsync(int id)
        {
            try
            {
                var professor = await _context.Professores.FindAsync(id);
                if (professor == null)
                    return ApiResponse<bool>.Error("Professor não encontrado", 404);

                // Verificar se professor possui disciplinas atribuídas
                var hasDisciplinas = await _context.ProfessoresDisciplinas
                    .AnyAsync(pd => pd.ProfessorId == id);

                if (hasDisciplinas)
                    return ApiResponse<bool>.Error("Não é possível excluir professor com disciplinas atribuídas. Remova as disciplinas primeiro.");

                _context.Professores.Remove(professor);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Ok(true, "Professor excluído com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Error($"Erro ao excluir professor: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<DisciplinaDto>>> GetDisciplinasProfessorAsync(int id, int? anoLetivo = null)
        {
            try
            {
                var query = _context.ProfessoresDisciplinas
                    .Include(pd => pd.Disciplina)
                        .ThenInclude(d => d.Curso)
                    .Where(pd => pd.ProfessorId == id);

                if (anoLetivo.HasValue)
                    query = query.Where(pd => pd.AnoLetivo == anoLetivo.Value);

                var disciplinas = await query
                    .Select(pd => pd.Disciplina)
                    .ToListAsync();

                var disciplinasDto = _mapper.Map<List<DisciplinaDto>>(disciplinas);
                return ApiResponse<List<DisciplinaDto>>.Ok(disciplinasDto, "Disciplinas do professor listadas com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DisciplinaDto>>.Error($"Erro ao listar disciplinas do professor: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<TurmaDto>>> GetTurmasProfessorAsync(int id, int? anoLetivo = null)
        {
            try
            {
                // Não há entidade TurmaDisciplina no modelo atual; obter turmas via horarios
                var horarios = _context.Horarios
                    .Include(h => h.Turma)
                        .ThenInclude(t => t.Curso)
                    .Where(h => h.ProfessorId == id);

                if (anoLetivo.HasValue)
                    horarios = horarios.Where(h => h.Turma.AnoLetivo == anoLetivo.Value);

                var turmas = await horarios.Select(h => h.Turma).Distinct().ToListAsync();
                var turmasDto = _mapper.Map<List<TurmaDto>>(turmas);
                return ApiResponse<List<TurmaDto>>.Ok(turmasDto, "Turmas do professor listadas com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TurmaDto>>.Error($"Erro ao listar turmas do professor: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> AtribuirDisciplinaAsync(int professorId, int disciplinaId, int anoLetivo)
        {
            try
            {
                var professor = await _context.Professores.FindAsync(professorId);
                if (professor == null)
                    return ApiResponse<bool>.Error("Professor não encontrado", 404);

                var disciplina = await _context.Disciplinas.FindAsync(disciplinaId);
                if (disciplina == null)
                    return ApiResponse<bool>.Error("Disciplina não encontrada", 404);

                var existingAtribuicao = await _context.ProfessoresDisciplinas
                    .FirstOrDefaultAsync(pd => pd.ProfessorId == professorId && 
                                               pd.DisciplinaId == disciplinaId && 
                                               pd.AnoLetivo == anoLetivo);

                if (existingAtribuicao != null)
                    return ApiResponse<bool>.Error("Disciplina já está atribuída ao professor para este ano letivo");

                var professorDisciplina = new ProfessorDisciplina
                {
                    ProfessorId = professorId,
                    DisciplinaId = disciplinaId,
                    AnoLetivo = anoLetivo,
                    Status = true
                };

                _context.ProfessoresDisciplinas.Add(professorDisciplina);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Ok(true, "Disciplina atribuída ao professor com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Error($"Erro ao atribuir disciplina: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> RemoverDisciplinaAsync(int professorId, int disciplinaId, int anoLetivo)
        {
            try
            {
                var professorDisciplina = await _context.ProfessoresDisciplinas
                    .FirstOrDefaultAsync(pd => pd.ProfessorId == professorId && 
                                               pd.DisciplinaId == disciplinaId && 
                                               pd.AnoLetivo == anoLetivo);

                if (professorDisciplina == null)
                    return ApiResponse<bool>.Error("Atribuição não encontrada", 404);

                _context.ProfessoresDisciplinas.Remove(professorDisciplina);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Ok(true, "Disciplina removida do professor com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Error($"Erro ao remover disciplina: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ChangeStatusAsync(int id, StatusProfessor status)
        {
            try
            {
                var professor = await _context.Professores.FindAsync(id);
                if (professor == null)
                    return ApiResponse<bool>.Error("Professor não encontrado", 404);

                professor.Status = status;
                // Sem campo de atualização no modelo atual

                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Ok(true, $"Status do professor alterado para {status} com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Error($"Erro ao alterar status do professor: {ex.Message}");
            }
        }
    }
}