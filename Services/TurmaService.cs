using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SIGE.API.Data;
using SIGE.API.DTOs;
using SIGE.API.Interfaces;
using SIGE.API.Models;

namespace SIGE.API.Services
{
    public class TurmaService : ITurmaService
    {
        private readonly SIGEDbContext _context;
        private readonly IMapper _mapper;

        public TurmaService(SIGEDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PaginatedResponse<TurmaDto>>> GetTurmasAsync(int page = 1, int pageSize = 10, string? search = null, int? cursoId = null, int? anoLetivo = null, StatusTurma? status = null)
        {
            try
            {
                page = Math.Max(page, 1);
                pageSize = Math.Clamp(pageSize, 1, 100);

                var query = BuildTurmaQuery();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(t => t.Nome.Contains(search) || t.Codigo.Contains(search));
                }

                if (cursoId.HasValue)
                {
                    query = query.Where(t => t.CursoId == cursoId.Value);
                }

                if (anoLetivo.HasValue)
                {
                    query = query.Where(t => t.AnoLetivo == anoLetivo.Value);
                }

                if (status.HasValue)
                {
                    query = query.Where(t => t.Status == status.Value);
                }

                var totalItems = await query.CountAsync();
                var turmas = await query
                    .OrderByDescending(t => t.AnoLetivo)
                    .ThenBy(t => t.Nome)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var turmasDto = _mapper.Map<List<TurmaDto>>(turmas);
                var paginated = new PaginatedResponse<TurmaDto>
                {
                    Items = turmasDto,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
                };

                return ApiResponse<PaginatedResponse<TurmaDto>>.Ok(paginated, "Turmas listadas com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginatedResponse<TurmaDto>>.Error($"Erro ao listar turmas: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TurmaDto>> GetTurmaByIdAsync(int id)
        {
            try
            {
                var turma = await BuildTurmaQuery().FirstOrDefaultAsync(t => t.Id == id);
                if (turma == null)
                {
                    return ApiResponse<TurmaDto>.Error("Turma não encontrada", 404);
                }

                var turmaDto = _mapper.Map<TurmaDto>(turma);
                return ApiResponse<TurmaDto>.Ok(turmaDto, "Turma encontrada com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<TurmaDto>.Error($"Erro ao buscar turma: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TurmaDto>> CreateTurmaAsync(CreateTurmaDto createTurmaDto)
        {
            try
            {
                var codigoNormalizado = createTurmaDto.Codigo.Trim().ToUpperInvariant();
                var codigoEmUso = await _context.Turmas.AnyAsync(t => t.Codigo == codigoNormalizado);
                if (codigoEmUso)
                {
                    return ApiResponse<TurmaDto>.Error("Já existe uma turma com este código");
                }

                var curso = await _context.Cursos.FindAsync(createTurmaDto.CursoId);
                if (curso == null)
                {
                    return ApiResponse<TurmaDto>.Error("Curso não encontrado", 404);
                }

                if (createTurmaDto.ProfessorCoordenadorId.HasValue)
                {
                    var professor = await _context.Professores.FindAsync(createTurmaDto.ProfessorCoordenadorId.Value);
                    if (professor == null)
                    {
                        return ApiResponse<TurmaDto>.Error("Professor coordenador não encontrado", 404);
                    }
                }

                var turma = _mapper.Map<Turma>(createTurmaDto);
                turma.Codigo = codigoNormalizado;

                _context.Turmas.Add(turma);
                await _context.SaveChangesAsync();

                await SincronizarMatriculasAsync(turma, createTurmaDto.AlunoIds);
                await _context.SaveChangesAsync();

                var turmaCompleta = await ObterTurmaCompletaAsync(turma.Id);
                var turmaDto = _mapper.Map<TurmaDto>(turmaCompleta);
                return ApiResponse<TurmaDto>.Ok(turmaDto, "Turma criada com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<TurmaDto>.Error($"Erro ao criar turma: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TurmaDto>> UpdateTurmaAsync(int id, UpdateTurmaDto updateTurmaDto)
        {
            try
            {
                var turma = await BuildTurmaQuery().FirstOrDefaultAsync(t => t.Id == id);
                if (turma == null)
                {
                    return ApiResponse<TurmaDto>.Error("Turma não encontrada", 404);
                }

                var codigoNormalizado = updateTurmaDto.Codigo.Trim().ToUpperInvariant();
                var codigoEmUso = await _context.Turmas.AnyAsync(t => t.Id != id && t.Codigo == codigoNormalizado);
                if (codigoEmUso)
                {
                    return ApiResponse<TurmaDto>.Error("Já existe outra turma com este código");
                }

                if (updateTurmaDto.ProfessorCoordenadorId.HasValue)
                {
                    var professor = await _context.Professores.FindAsync(updateTurmaDto.ProfessorCoordenadorId.Value);
                    if (professor == null)
                    {
                        return ApiResponse<TurmaDto>.Error("Professor coordenador não encontrado", 404);
                    }
                }

                if (await _context.Cursos.FindAsync(updateTurmaDto.CursoId) == null)
                {
                    return ApiResponse<TurmaDto>.Error("Curso não encontrado", 404);
                }

                _mapper.Map(updateTurmaDto, turma);
                turma.Codigo = codigoNormalizado;

                await SincronizarMatriculasAsync(turma, updateTurmaDto.AlunoIds);
                await _context.SaveChangesAsync();

                var turmaAtualizada = await ObterTurmaCompletaAsync(id);
                var turmaDto = _mapper.Map<TurmaDto>(turmaAtualizada);
                return ApiResponse<TurmaDto>.Ok(turmaDto, "Turma atualizada com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<TurmaDto>.Error($"Erro ao atualizar turma: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteTurmaAsync(int id)
        {
            try
            {
                var turma = await _context.Turmas
                    .Include(t => t.Matriculas)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (turma == null)
                {
                    return ApiResponse<bool>.Error("Turma não encontrada", 404);
                }

                if (turma.Matriculas.Any())
                {
                    _context.Matriculas.RemoveRange(turma.Matriculas);
                }

                _context.Turmas.Remove(turma);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Ok(true, "Turma excluída com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Error($"Erro ao excluir turma: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ChangeStatusAsync(int id, StatusTurma status)
        {
            try
            {
                var turma = await _context.Turmas.FindAsync(id);
                if (turma == null)
                {
                    return ApiResponse<bool>.Error("Turma não encontrada", 404);
                }

                turma.Status = status;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Ok(true, "Status da turma atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Error($"Erro ao atualizar status da turma: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TurmaCatalogosDto>> GetCatalogosAsync()
        {
            try
            {
                var cursos = await _context.Cursos
                    .OrderBy(c => c.Nome)
                    .Select(c => new TurmaCatalogoCursoDto
                    {
                        Id = c.Id,
                        Nome = c.Nome,
                        Codigo = c.Codigo,
                        Nivel = c.NivelEnsino.ToString()
                    })
                    .ToListAsync();

                var professores = await _context.Professores
                    .OrderBy(p => p.NomeCompleto)
                    .Select(p => new TurmaCatalogoProfessorDto
                    {
                        Id = p.Id,
                        Nome = p.NomeCompleto,
                        Especialidade = p.Especializacao
                    })
                    .ToListAsync();

                var alunos = await _context.Alunos
                    .OrderBy(a => a.NomeCompleto)
                    .Select(a => new TurmaCatalogoAlunoDto
                    {
                        Id = a.Id,
                        Nome = a.NomeCompleto,
                        Serie = a.Matriculas
                            .OrderByDescending(m => m.AnoLetivo)
                            .Select(m => m.Turma.Nome)
                            .FirstOrDefault(),
                        Matricula = a.Matricula
                    })
                    .ToListAsync();

                var catalogos = new TurmaCatalogosDto
                {
                    Cursos = cursos,
                    Professores = professores,
                    Alunos = alunos
                };

                return ApiResponse<TurmaCatalogosDto>.Ok(catalogos, "Catálogos carregados com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<TurmaCatalogosDto>.Error($"Erro ao carregar catálogos: {ex.Message}");
            }
        }

        private IQueryable<Turma> BuildTurmaQuery()
        {
            return _context.Turmas
                .Include(t => t.Curso)
                .Include(t => t.ProfessorCoordenador)
                .Include(t => t.Matriculas)
                    .ThenInclude(m => m.Aluno);
        }

        private async Task<Turma?> ObterTurmaCompletaAsync(int id)
        {
            return await BuildTurmaQuery().FirstOrDefaultAsync(t => t.Id == id);
        }

        private async Task SincronizarMatriculasAsync(Turma turma, IEnumerable<int>? alunoIds)
        {
            if (alunoIds == null)
            {
                return;
            }

            var ids = alunoIds.Distinct().ToList();
            var matriculasExistentes = await _context.Matriculas
                .Where(m => m.TurmaId == turma.Id)
                .ToListAsync();

            var idsExistentes = matriculasExistentes.Select(m => m.AlunoId).ToHashSet();

            var remover = matriculasExistentes
                .Where(m => !ids.Contains(m.AlunoId))
                .ToList();

            if (remover.Any())
            {
                _context.Matriculas.RemoveRange(remover);
            }

            var novosIds = ids.Where(id => !idsExistentes.Contains(id)).ToList();
            if (novosIds.Any())
            {
                var alunosValidos = await _context.Alunos
                    .Where(a => novosIds.Contains(a.Id))
                    .Select(a => a.Id)
                    .ToListAsync();

                foreach (var alunoId in alunosValidos)
                {
                    _context.Matriculas.Add(new Matricula
                    {
                        NumeroMatricula = GerarNumeroMatricula(turma, alunoId),
                        AlunoId = alunoId,
                        TurmaId = turma.Id,
                        AnoLetivo = turma.AnoLetivo,
                        DataMatricula = DateTime.UtcNow,
                        Status = StatusMatricula.Ativa
                    });
                }
            }
        }

        private static string GerarNumeroMatricula(Turma turma, int alunoId)
        {
            return $"T{turma.Id:D3}-A{alunoId:D4}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";
        }
    }
}
