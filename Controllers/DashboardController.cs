using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGE.API.Data;
using SIGE.API.DTOs;
using SIGE.API.Models;

namespace SIGE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly SIGEDbContext _context;

        public DashboardController(SIGEDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter dados gerais do dashboard
        /// </summary>
        /// <returns>Dados do dashboard</returns>
        [HttpGet("geral")]
        public async Task<ActionResult<DashboardDto>> GetDashboardGeral()
        {
            var dashboard = new DashboardDto
            {
                TotalEscolas = await _context.Escolas.CountAsync(e => e.Status == StatusEscola.Ativa),
                TotalAlunos = await _context.Alunos.CountAsync(a => a.Status == StatusAluno.Matriculado),
                TotalProfessores = await _context.Professores.CountAsync(p => p.Status == StatusProfessor.Ativo),
                TotalTurmas = await _context.Turmas.CountAsync(t => t.Status == StatusTurma.Ativa),
                TotalUsuarios = await _context.Usuarios.CountAsync(u => u.Status == StatusUsuario.Ativo)
            };

            // Calcular percentual de frequência (últimos 30 dias)
            var inicioMes = DateTime.UtcNow.AddDays(-30);
            var totalPresencas = await _context.Frequencias
                .CountAsync(f => f.DataAula >= inicioMes && f.Presente);
            var totalAulas = await _context.Frequencias
                .CountAsync(f => f.DataAula >= inicioMes);

            dashboard.PercentualFrequencia = totalAulas > 0 ? (decimal)totalPresencas / totalAulas * 100 : 0;

            // Calcular média geral de notas (bimestre atual)
            var bimestreAtual = GetBimestreAtual();
            var mediaNotas = await _context.Notas
                .Include(n => n.Avaliacao)
                .Where(n => n.Avaliacao.Bimestre == bimestreAtual)
                .AverageAsync(n => (double?)n.NotaValor) ?? 0;

            dashboard.MediaGeralNotas = (decimal)mediaNotas;

            return Ok(dashboard);
        }

        /// <summary>
        /// Obter estatísticas principais
        /// </summary>
        /// <returns>Estatísticas do sistema</returns>
        [HttpGet("estatisticas")]
        public async Task<ActionResult<IEnumerable<EstatisticasDto>>> GetEstatisticas()
        {
            var estatisticas = new List<EstatisticasDto>();

            // Estatísticas de usuários
            var totalUsuarios = await _context.Usuarios.CountAsync();
            var usuariosAtivos = await _context.Usuarios.CountAsync(u => u.Status == StatusUsuario.Ativo);
            
            estatisticas.Add(new EstatisticasDto
            {
                Categoria = "Usuários",
                Total = totalUsuarios,
                Ativos = usuariosAtivos,
                Inativos = totalUsuarios - usuariosAtivos,
                Percentual = totalUsuarios > 0 ? (decimal)usuariosAtivos / totalUsuarios * 100 : 0
            });

            // Estatísticas de alunos
            var totalAlunos = await _context.Alunos.CountAsync();
            var alunosMatriculados = await _context.Alunos.CountAsync(a => a.Status == StatusAluno.Matriculado);
            
            estatisticas.Add(new EstatisticasDto
            {
                Categoria = "Alunos",
                Total = totalAlunos,
                Ativos = alunosMatriculados,
                Inativos = totalAlunos - alunosMatriculados,
                Percentual = totalAlunos > 0 ? (decimal)alunosMatriculados / totalAlunos * 100 : 0
            });

            // Estatísticas de professores
            var totalProfessores = await _context.Professores.CountAsync();
            var professoresAtivos = await _context.Professores.CountAsync(p => p.Status == StatusProfessor.Ativo);
            
            estatisticas.Add(new EstatisticasDto
            {
                Categoria = "Professores",
                Total = totalProfessores,
                Ativos = professoresAtivos,
                Inativos = totalProfessores - professoresAtivos,
                Percentual = totalProfessores > 0 ? (decimal)professoresAtivos / totalProfessores * 100 : 0
            });

            // Estatísticas de turmas
            var totalTurmas = await _context.Turmas.CountAsync();
            var turmasAtivas = await _context.Turmas.CountAsync(t => t.Status == StatusTurma.Ativa);
            
            estatisticas.Add(new EstatisticasDto
            {
                Categoria = "Turmas",
                Total = totalTurmas,
                Ativos = turmasAtivas,
                Inativos = totalTurmas - turmasAtivas,
                Percentual = totalTurmas > 0 ? (decimal)turmasAtivas / totalTurmas * 100 : 0
            });

            return Ok(estatisticas);
        }

        /// <summary>
        /// Obter dados para gráficos
        /// </summary>
        /// <returns>Dados dos gráficos</returns>
        [HttpGet("graficos")]
        public async Task<ActionResult> GetGraficos()
        {
            var ultimosSeisMeses = Enumerable.Range(0, 6)
                .Select(i => DateTime.UtcNow.AddMonths(-i))
                .OrderBy(d => d)
                .ToList();

            // Gráfico de matrículas por mês
            var matriculasPorMes = new List<object>();
            foreach (var mes in ultimosSeisMeses)
            {
                var inicioMes = new DateTime(mes.Year, mes.Month, 1);
                var fimMes = inicioMes.AddMonths(1).AddDays(-1);
                
                var count = await _context.Matriculas
                    .CountAsync(m => m.DataMatricula >= inicioMes && m.DataMatricula <= fimMes);
                
                matriculasPorMes.Add(new
                {
                    mes = mes.ToString("MM/yyyy"),
                    total = count
                });
            }

            // Gráfico de notas por bimestre
            var notasPorBimestre = new List<object>();
            for (int bimestre = 1; bimestre <= 4; bimestre++)
            {
                var media = await _context.Notas
                    .Include(n => n.Avaliacao)
                    .Where(n => n.Avaliacao.Bimestre == bimestre)
                    .AverageAsync(n => (double?)n.NotaValor) ?? 0;
                
                notasPorBimestre.Add(new
                {
                    bimestre = $"{bimestre}º Bimestre",
                    media = Math.Round(media, 2)
                });
            }

            // Gráfico de frequência por mês
            var frequenciaPorMes = new List<object>();
            foreach (var mes in ultimosSeisMeses)
            {
                var inicioMes = new DateTime(mes.Year, mes.Month, 1);
                var fimMes = inicioMes.AddMonths(1).AddDays(-1);
                
                var totalPresencas = await _context.Frequencias
                    .CountAsync(f => f.DataAula >= inicioMes && f.DataAula <= fimMes && f.Presente);
                var totalAulas = await _context.Frequencias
                    .CountAsync(f => f.DataAula >= inicioMes && f.DataAula <= fimMes);
                
                var percentual = totalAulas > 0 ? (double)totalPresencas / totalAulas * 100 : 0;
                
                frequenciaPorMes.Add(new
                {
                    mes = mes.ToString("MM/yyyy"),
                    percentual = Math.Round(percentual, 2)
                });
            }

            return Ok(new
            {
                matriculasPorMes,
                notasPorBimestre,
                frequenciaPorMes
            });
        }

        /// <summary>
        /// Obter alertas do sistema
        /// </summary>
        /// <returns>Lista de alertas</returns>
        [HttpGet("alertas")]
        public async Task<ActionResult> GetAlertas()
        {
            var alertas = new List<object>();

            // Alunos com baixa frequência (menos de 75%)
            var alunosBaixaFrequencia = await _context.Alunos
                .Where(a => a.Status == StatusAluno.Matriculado)
                .Select(a => new
                {
                    a.Id,
                    a.NomeCompleto,
                    a.Matricula,
                    TotalAulas = _context.Frequencias.Count(f => f.AlunoId == a.Id),
                    TotalPresencas = _context.Frequencias.Count(f => f.AlunoId == a.Id && f.Presente)
                })
                .ToListAsync();

            foreach (var aluno in alunosBaixaFrequencia)
            {
                if (aluno.TotalAulas > 0)
                {
                    var percentualFrequencia = (double)aluno.TotalPresencas / aluno.TotalAulas * 100;
                    if (percentualFrequencia < 75)
                    {
                        alertas.Add(new
                        {
                            tipo = "Baixa Frequência",
                            prioridade = "Alta",
                            mensagem = $"Aluno {aluno.NomeCompleto} ({aluno.Matricula}) com {percentualFrequencia:F1}% de frequência",
                            data = DateTime.UtcNow
                        });
                    }
                }
            }

            // Pagamentos em atraso
            var pagamentosAtrasados = await _context.FinanceiroAlunos
                .Include(f => f.Aluno)
                .Where(f => f.Status == StatusFinanceiro.Atrasado && f.DataVencimento < DateTime.UtcNow)
                .CountAsync();

            if (pagamentosAtrasados > 0)
            {
                alertas.Add(new
                {
                    tipo = "Financeiro",
                    prioridade = "Média",
                    mensagem = $"{pagamentosAtrasados} pagamento(s) em atraso",
                    data = DateTime.UtcNow
                });
            }

            // Comunicados expirados
            var comunicadosExpirados = await _context.Comunicados
                .Where(c => c.Status == StatusComunicado.Publicado && c.DataExpiracao < DateTime.UtcNow)
                .CountAsync();

            if (comunicadosExpirados > 0)
            {
                alertas.Add(new
                {
                    tipo = "Comunicados",
                    prioridade = "Baixa",
                    mensagem = $"{comunicadosExpirados} comunicado(s) expirado(s)",
                    data = DateTime.UtcNow
                });
            }

            return Ok(alertas.Take(10)); // Retorna apenas os 10 primeiros alertas
        }

        /// <summary>
        /// Obter atividades recentes
        /// </summary>
        /// <returns>Lista de atividades recentes</returns>
        [HttpGet("atividades-recentes")]
        public async Task<ActionResult> GetAtividadesRecentes()
        {
            var atividades = new List<dynamic>();

            // Últimas matrículas (últimos 7 dias)
            var ultimasMatriculas = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Turma)
                .Where(m => m.DataMatricula >= DateTime.UtcNow.AddDays(-7))
                .OrderByDescending(m => m.DataMatricula)
                .Take(5)
                .Select(m => new
                {
                    tipo = "Matrícula",
                    descricao = $"Aluno {m.Aluno.NomeCompleto} matriculado na turma {m.Turma.Nome}",
                    data = m.DataMatricula,
                    usuario = "Sistema"
                })
                .ToListAsync();

            atividades.AddRange(ultimasMatriculas);

            // Últimas notas lançadas (últimos 7 dias)
            var ultimasNotas = await _context.Notas
                .Include(n => n.Aluno)
                .Include(n => n.Professor)
                .Include(n => n.Avaliacao)
                .Where(n => n.DataLancamento >= DateTime.UtcNow.AddDays(-7))
                .OrderByDescending(n => n.DataLancamento)
                .Take(5)
                .Select(n => new
                {
                    tipo = "Nota",
                    descricao = $"Nota {n.NotaValor} lançada para {n.Aluno.NomeCompleto} na avaliação {n.Avaliacao.Nome}",
                    data = n.DataLancamento,
                    usuario = n.Professor.NomeCompleto
                })
                .ToListAsync();

            atividades.AddRange(ultimasNotas);

            // Últimos comunicados (últimos 7 dias)
            var ultimosComunicados = await _context.Comunicados
                .Include(c => c.Autor)
                .Where(c => c.DataPublicacao >= DateTime.UtcNow.AddDays(-7))
                .OrderByDescending(c => c.DataPublicacao)
                .Take(5)
                .Select(c => new
                {
                    tipo = "Comunicado",
                    descricao = $"Comunicado '{c.Titulo}' publicado",
                    data = c.DataPublicacao,
                    usuario = c.Autor.Nome
                })
                .ToListAsync();

            atividades.AddRange(ultimosComunicados);

            return Ok(atividades.OrderByDescending(a => a.data).Take(15));
        }

        private int GetBimestreAtual()
        {
            var mes = DateTime.UtcNow.Month;
            return mes switch
            {
                >= 2 and <= 4 => 1,
                >= 5 and <= 7 => 2,
                >= 8 and <= 10 => 3,
                _ => 4
            };
        }
    }
}