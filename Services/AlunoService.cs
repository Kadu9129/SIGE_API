using Microsoft.EntityFrameworkCore;
using SIGE.API.Data;
using SIGE.API.DTOs;
using SIGE.API.Interfaces;
using SIGE.API.Models;
using AutoMapper;

namespace SIGE.API.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly SIGEDbContext _context;
        private readonly IMapper _mapper;

        public AlunoService(SIGEDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PaginatedResponse<AlunoDto>>> GetAlunosAsync(
            int page = 1, 
            int pageSize = 10, 
            string? search = null,
            int? escolaId = null,
            StatusAluno? status = null)
        {
            try
            {
                var query = _context.Alunos
                    .Include(a => a.Usuario)
                    .Include(a => a.Escola)
                    .AsQueryable();

                // Filtros
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(a => a.NomeCompleto.Contains(search) || 
                                           a.Matricula.Contains(search) ||
                                           a.CPF.Contains(search));
                }

                if (escolaId.HasValue)
                {
                    query = query.Where(a => a.EscolaId == escolaId.Value);
                }

                if (status.HasValue)
                {
                    query = query.Where(a => a.Status == status.Value);
                }

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var alunos = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var alunosDto = _mapper.Map<List<AlunoDto>>(alunos);

                var paginatedResponse = new PaginatedResponse<AlunoDto>
                {
                    Items = alunosDto,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize
                };

                return ApiResponse<PaginatedResponse<AlunoDto>>.Ok(paginatedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginatedResponse<AlunoDto>>.Error($"Erro ao buscar alunos: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AlunoDto>> GetAlunoByIdAsync(int id)
        {
            try
            {
                var aluno = await _context.Alunos
                    .Include(a => a.Usuario)
                    .Include(a => a.Escola)
                    .Include(a => a.Matriculas)
                        .ThenInclude(m => m.Turma)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (aluno == null)
                {
                    return ApiResponse<AlunoDto>.Error("Aluno não encontrado", 404);
                }

                var alunoDto = _mapper.Map<AlunoDto>(aluno);
                return ApiResponse<AlunoDto>.Ok(alunoDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<AlunoDto>.Error($"Erro ao buscar aluno: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AlunoDto>> GetAlunoByUsuarioIdAsync(int usuarioId)
        {
            try
            {
                var aluno = await _context.Alunos
                    .Include(a => a.Usuario)
                    .Include(a => a.Escola)
                    .Include(a => a.Matriculas)
                        .ThenInclude(m => m.Turma)
                    .FirstOrDefaultAsync(a => a.UsuarioId == usuarioId);

                if (aluno == null)
                {
                    return ApiResponse<AlunoDto>.Error("Aluno não encontrado para o usuário informado", 404);
                }

                var alunoDto = _mapper.Map<AlunoDto>(aluno);
                return ApiResponse<AlunoDto>.Ok(alunoDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<AlunoDto>.Error($"Erro ao buscar aluno por usuário: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AlunoDto>> CreateAlunoAsync(CreateAlunoDto createAlunoDto)
        {
            try
            {
                // Verificar se o email já existe
                if (await _context.Usuarios.AnyAsync(u => u.Email == createAlunoDto.Email))
                {
                    return ApiResponse<AlunoDto>.Error("Email já está em uso");
                }

                // Verificar se o CPF já existe
                if (!string.IsNullOrEmpty(createAlunoDto.CPF) && 
                    await _context.Alunos.AnyAsync(a => a.CPF == createAlunoDto.CPF))
                {
                    return ApiResponse<AlunoDto>.Error("CPF já está em uso");
                }

                // Verificar se a matrícula já existe
                if (await _context.Alunos.AnyAsync(a => a.Matricula == createAlunoDto.Matricula))
                {
                    return ApiResponse<AlunoDto>.Error("Matrícula já está em uso");
                }

                // Gerar senha padrão
                var senhaHash = BCrypt.Net.BCrypt.HashPassword("123456");

                // Criar usuário
                var usuario = new Usuario
                {
                    Nome = createAlunoDto.NomeCompleto,
                    Email = createAlunoDto.Email,
                    SenhaHash = senhaHash,
                    TipoUsuario = TipoUsuario.Aluno,
                    Status = StatusUsuario.Ativo,
                    DataCriacao = DateTime.UtcNow,
                    DataUltimaAtualizacao = DateTime.UtcNow
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Criar aluno
                var aluno = _mapper.Map<Aluno>(createAlunoDto);
                aluno.UsuarioId = usuario.Id;
                aluno.DataMatricula = DateTime.UtcNow;

                _context.Alunos.Add(aluno);
                await _context.SaveChangesAsync();

                // Buscar aluno completo
                var alunoCompleto = await GetAlunoByIdAsync(aluno.Id);
                return alunoCompleto;
            }
            catch (Exception ex)
            {
                return ApiResponse<AlunoDto>.Error($"Erro ao criar aluno: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AlunoDto>> UpdateAlunoAsync(int id, UpdateAlunoDto updateAlunoDto)
        {
            try
            {
                var aluno = await _context.Alunos
                    .Include(a => a.Usuario)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (aluno == null)
                {
                    return ApiResponse<AlunoDto>.Error("Aluno não encontrado", 404);
                }

                // Verificar se o email já existe (exceto o próprio)
                if (await _context.Usuarios.AnyAsync(u => u.Email == updateAlunoDto.Email && u.Id != aluno.UsuarioId))
                {
                    return ApiResponse<AlunoDto>.Error("Email já está em uso");
                }

                // Verificar se o CPF já existe (exceto o próprio)
                if (!string.IsNullOrEmpty(updateAlunoDto.CPF) && 
                    await _context.Alunos.AnyAsync(a => a.CPF == updateAlunoDto.CPF && a.Id != id))
                {
                    return ApiResponse<AlunoDto>.Error("CPF já está em uso");
                }

                // Atualizar usuário
                aluno.Usuario.Nome = updateAlunoDto.NomeCompleto;
                aluno.Usuario.Email = updateAlunoDto.Email;
                aluno.Usuario.DataUltimaAtualizacao = DateTime.UtcNow;

                // Atualizar aluno
                _mapper.Map(updateAlunoDto, aluno);

                await _context.SaveChangesAsync();

                var alunoCompleto = await GetAlunoByIdAsync(id);
                return alunoCompleto;
            }
            catch (Exception ex)
            {
                return ApiResponse<AlunoDto>.Error($"Erro ao atualizar aluno: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAlunoAsync(int id)
        {
            try
            {
                var aluno = await _context.Alunos
                    .Include(a => a.Usuario)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (aluno == null)
                {
                    return ApiResponse<bool>.Error("Aluno não encontrado", 404);
                }

                // Verificar se o aluno tem dependências
                var temMatriculas = await _context.Matriculas.AnyAsync(m => m.AlunoId == id);
                var temNotas = await _context.Notas.AnyAsync(n => n.AlunoId == id);

                if (temMatriculas || temNotas)
                {
                    // Soft delete - marcar como transferido (ou evadido) para manter histórico
                    aluno.Status = StatusAluno.Transferido;
                    aluno.Usuario.Status = StatusUsuario.Inativo;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Hard delete
                    _context.Alunos.Remove(aluno);
                    _context.Usuarios.Remove(aluno.Usuario);
                    await _context.SaveChangesAsync();
                }

                return ApiResponse<bool>.Ok(true, "Aluno removido com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Error($"Erro ao remover aluno: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<AlunoDto>>> GetAlunosByTurmaAsync(int turmaId)
        {
            try
            {
                var alunos = await _context.Matriculas
                    .Where(m => m.TurmaId == turmaId && m.Status == StatusMatricula.Ativa)
                    .Include(m => m.Aluno)
                        .ThenInclude(a => a.Usuario)
                    .Include(m => m.Aluno)
                        .ThenInclude(a => a.Escola)
                    .Select(m => m.Aluno)
                    .ToListAsync();

                var alunosDto = _mapper.Map<List<AlunoDto>>(alunos);
                return ApiResponse<List<AlunoDto>>.Ok(alunosDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AlunoDto>>.Error($"Erro ao buscar alunos da turma: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ChangeStatusAsync(int id, StatusAluno status)
        {
            try
            {
                var aluno = await _context.Alunos
                    .Include(a => a.Usuario)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (aluno == null)
                {
                    return ApiResponse<bool>.Error("Aluno não encontrado", 404);
                }

                aluno.Status = status;
                aluno.Usuario.Status = status == StatusAluno.Matriculado ? StatusUsuario.Ativo : StatusUsuario.Inativo;
                aluno.Usuario.DataUltimaAtualizacao = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Ok(true, "Status do aluno alterado com sucesso");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Error($"Erro ao alterar status do aluno: {ex.Message}");
            }
        }
    }
}