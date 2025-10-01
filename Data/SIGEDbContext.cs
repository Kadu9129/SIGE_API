using Microsoft.EntityFrameworkCore;
using SIGE.API.Models;

namespace SIGE.API.Data
{
    public class SIGEDbContext : DbContext
    {
        public SIGEDbContext(DbContextOptions<SIGEDbContext> options) : base(options)
        {
        }

        // DbSets - Tabelas do banco
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PerfilAcesso> PerfisAcesso { get; set; }
        public DbSet<Sessao> Sessoes { get; set; }
        public DbSet<Escola> Escolas { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Responsavel> Responsaveis { get; set; }
        public DbSet<AlunoResponsavel> AlunosResponsaveis { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<ProfessorDisciplina> ProfessoresDisciplinas { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Nota> Notas { get; set; }
        public DbSet<Boletim> Boletins { get; set; }
        public DbSet<Frequencia> Frequencias { get; set; }
        public DbSet<Chamada> Chamadas { get; set; }
        public DbSet<Comunicado> Comunicados { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }
        public DbSet<PlanoPagamento> PlanosPagamento { get; set; }
        public DbSet<FinanceiroAluno> FinanceiroAlunos { get; set; }
        public DbSet<RelatorioGerado> RelatoriosGerados { get; set; }
        public DbSet<ConfiguracaoSistema> ConfiguracoesSistema { get; set; }
        public DbSet<LogSistema> LogsSistema { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações de relacionamentos e índices

            // Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.CPF)
                .IsUnique();

            // Escola
            modelBuilder.Entity<Escola>()
                .HasIndex(e => e.CNPJ)
                .IsUnique();

            modelBuilder.Entity<Escola>()
                .HasOne(e => e.Diretor)
                .WithMany(u => u.EscolasDirigidas)
                .HasForeignKey(e => e.DiretorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Curso
            modelBuilder.Entity<Curso>()
                .HasIndex(c => c.Codigo)
                .IsUnique();

            // Disciplina
            modelBuilder.Entity<Disciplina>()
                .HasIndex(d => d.Codigo)
                .IsUnique();

            // Aluno
            modelBuilder.Entity<Aluno>()
                .HasIndex(a => a.Matricula)
                .IsUnique();

            modelBuilder.Entity<Aluno>()
                .HasIndex(a => a.CPF)
                .IsUnique();

            modelBuilder.Entity<Aluno>()
                .HasOne(a => a.Usuario)
                .WithOne(u => u.Aluno)
                .HasForeignKey<Aluno>(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Responsavel
            modelBuilder.Entity<Responsavel>()
                .HasIndex(r => r.CPF)
                .IsUnique();

            modelBuilder.Entity<Responsavel>()
                .HasOne(r => r.Usuario)
                .WithOne(u => u.Responsavel)
                .HasForeignKey<Responsavel>(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Professor
            modelBuilder.Entity<Professor>()
                .HasIndex(p => p.CodigoProfessor)
                .IsUnique();

            modelBuilder.Entity<Professor>()
                .HasIndex(p => p.CPF)
                .IsUnique();

            modelBuilder.Entity<Professor>()
                .HasOne(p => p.Usuario)
                .WithOne(u => u.Professor)
                .HasForeignKey<Professor>(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Turma
            modelBuilder.Entity<Turma>()
                .HasIndex(t => t.Codigo)
                .IsUnique();

            modelBuilder.Entity<Turma>()
                .HasOne(t => t.ProfessorCoordenador)
                .WithMany(p => p.TurmasCoordenadas)
                .HasForeignKey(t => t.ProfessorCoordenadorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Matricula
            modelBuilder.Entity<Matricula>()
                .HasIndex(m => m.NumeroMatricula)
                .IsUnique();

            // Sessao
            modelBuilder.Entity<Sessao>()
                .HasIndex(s => s.Token)
                .IsUnique();

            // Mensagem - configurar relacionamentos múltiplos
            modelBuilder.Entity<Mensagem>()
                .HasOne(m => m.Remetente)
                .WithMany(u => u.MensagensEnviadas)
                .HasForeignKey(m => m.RemetenteId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Mensagem>()
                .HasOne(m => m.Destinatario)
                .WithMany(u => u.MensagensRecebidas)
                .HasForeignKey(m => m.DestinatarioId)
                .OnDelete(DeleteBehavior.NoAction);

            // ConfiguracaoSistema
            modelBuilder.Entity<ConfiguracaoSistema>()
                .HasIndex(c => c.Chave)
                .IsUnique();

            // Configurações de enum para string
            modelBuilder.Entity<Usuario>()
                .Property(u => u.TipoUsuario)
                .HasConversion<string>();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Escola>()
                .Property(e => e.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Curso>()
                .Property(c => c.NivelEnsino)
                .HasConversion<string>();

            modelBuilder.Entity<Aluno>()
                .Property(a => a.Sexo)
                .HasConversion<string>();

            modelBuilder.Entity<Aluno>()
                .Property(a => a.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Responsavel>()
                .Property(r => r.Parentesco)
                .HasConversion<string>();

            modelBuilder.Entity<Professor>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Turma>()
                .Property(t => t.Turno)
                .HasConversion<string>();

            modelBuilder.Entity<Turma>()
                .Property(t => t.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Matricula>()
                .Property(m => m.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Horario>()
                .Property(h => h.DiaSemana)
                .HasConversion<string>();

            modelBuilder.Entity<Avaliacao>()
                .Property(a => a.Tipo)
                .HasConversion<string>();

            modelBuilder.Entity<Boletim>()
                .Property(b => b.Situacao)
                .HasConversion<string>();

            modelBuilder.Entity<Chamada>()
                .Property(c => c.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Comunicado>()
                .Property(c => c.PublicoAlvo)
                .HasConversion<string>();

            modelBuilder.Entity<Comunicado>()
                .Property(c => c.Prioridade)
                .HasConversion<string>();

            modelBuilder.Entity<Comunicado>()
                .Property(c => c.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Mensagem>()
                .Property(m => m.Status)
                .HasConversion<string>();

            modelBuilder.Entity<FinanceiroAluno>()
                .Property(f => f.Status)
                .HasConversion<string>();

            modelBuilder.Entity<RelatorioGerado>()
                .Property(r => r.Status)
                .HasConversion<string>();

            // Seed data inicial
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Usuário administrador padrão
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nome = "Administrador",
                    Email = "admin@sige.edu.br",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"), // Senha: 123456
                    TipoUsuario = TipoUsuario.Admin,
                    Status = StatusUsuario.Ativo,
                    DataCriacao = DateTime.UtcNow,
                    DataUltimaAtualizacao = DateTime.UtcNow
                }
            );

            // Configurações iniciais do sistema
            modelBuilder.Entity<ConfiguracaoSistema>().HasData(
                new ConfiguracaoSistema
                {
                    Id = 1,
                    Chave = "SISTEMA_NOME",
                    Valor = "SIGE - Sistema Integrado de Gestão Escolar",
                    Descricao = "Nome do sistema",
                    Categoria = "Geral"
                },
                new ConfiguracaoSistema
                {
                    Id = 2,
                    Chave = "SISTEMA_VERSAO",
                    Valor = "1.0.0",
                    Descricao = "Versão do sistema",
                    Categoria = "Geral"
                }
            );
        }
    }
}