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

            // Usuários adicionais (Diretor, Professores, Alunos, Responsável)
            var senhaPadrao = BCrypt.Net.BCrypt.HashPassword("123456");
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 2, Nome = "Diretor Geral", Email = "diretor@sige.edu.br", SenhaHash = senhaPadrao, TipoUsuario = TipoUsuario.Diretor, Status = StatusUsuario.Ativo, DataCriacao = DateTime.UtcNow, DataUltimaAtualizacao = DateTime.UtcNow },
                new Usuario { Id = 3, Nome = "Prof. Ana Souza", Email = "ana.souza@sige.edu.br", SenhaHash = senhaPadrao, TipoUsuario = TipoUsuario.Professor, Status = StatusUsuario.Ativo, DataCriacao = DateTime.UtcNow, DataUltimaAtualizacao = DateTime.UtcNow },
                new Usuario { Id = 4, Nome = "Prof. Carlos Lima", Email = "carlos.lima@sige.edu.br", SenhaHash = senhaPadrao, TipoUsuario = TipoUsuario.Professor, Status = StatusUsuario.Ativo, DataCriacao = DateTime.UtcNow, DataUltimaAtualizacao = DateTime.UtcNow },
                new Usuario { Id = 5, Nome = "Aluno João Silva", Email = "joao.silva@sige.edu.br", SenhaHash = senhaPadrao, TipoUsuario = TipoUsuario.Aluno, Status = StatusUsuario.Ativo, DataCriacao = DateTime.UtcNow, DataUltimaAtualizacao = DateTime.UtcNow },
                new Usuario { Id = 6, Nome = "Aluno Maria Santos", Email = "maria.santos@sige.edu.br", SenhaHash = senhaPadrao, TipoUsuario = TipoUsuario.Aluno, Status = StatusUsuario.Ativo, DataCriacao = DateTime.UtcNow, DataUltimaAtualizacao = DateTime.UtcNow },
                new Usuario { Id = 7, Nome = "Aluno Pedro Alves", Email = "pedro.alves@sige.edu.br", SenhaHash = senhaPadrao, TipoUsuario = TipoUsuario.Aluno, Status = StatusUsuario.Ativo, DataCriacao = DateTime.UtcNow, DataUltimaAtualizacao = DateTime.UtcNow },
                new Usuario { Id = 8, Nome = "Aluno Luana Costa", Email = "luana.costa@sige.edu.br", SenhaHash = senhaPadrao, TipoUsuario = TipoUsuario.Aluno, Status = StatusUsuario.Ativo, DataCriacao = DateTime.UtcNow, DataUltimaAtualizacao = DateTime.UtcNow },
                new Usuario { Id = 9, Nome = "Aluno Rafael Pereira", Email = "rafael.pereira@sige.edu.br", SenhaHash = senhaPadrao, TipoUsuario = TipoUsuario.Aluno, Status = StatusUsuario.Ativo, DataCriacao = DateTime.UtcNow, DataUltimaAtualizacao = DateTime.UtcNow },
                new Usuario { Id = 10, Nome = "Resp. Fernanda Silva", Email = "fernanda.silva@sige.edu.br", SenhaHash = senhaPadrao, TipoUsuario = TipoUsuario.Responsavel, Status = StatusUsuario.Ativo, DataCriacao = DateTime.UtcNow, DataUltimaAtualizacao = DateTime.UtcNow }
            );

            // Escola e Curso
            modelBuilder.Entity<Escola>().HasData(
                new Escola
                {
                    Id = 1,
                    Nome = "Escola Central",
                    CNPJ = "00.000.000/0001-00",
                    Endereco = "Rua Principal, 100",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01000-000",
                    Telefone = "1130000000",
                    Email = "contato@escolacentral.edu.br",
                    DiretorId = 2,
                    Status = StatusEscola.Ativa
                }
            );

            modelBuilder.Entity<Curso>().HasData(
                new Curso
                {
                    Id = 1,
                    EscolaId = 1,
                    Nome = "Ensino Fundamental",
                    Codigo = "FUND1",
                    Descricao = "Curso de Ensino Fundamental Anos Iniciais",
                    DuracaoAnos = 5,
                    NivelEnsino = NivelEnsino.Fundamental1,
                    Status = true
                }
            );

            // Professores
            modelBuilder.Entity<Professor>().HasData(
                new Professor
                {
                    Id = 1,
                    UsuarioId = 3,
                    CodigoProfessor = "PROF001",
                    NomeCompleto = "Ana Souza",
                    CPF = "11111111111",
                    RG = "1111111",
                    DataNascimento = new DateTime(1985, 5, 10),
                    Formacao = "Licenciatura em Matemática",
                    Especializacao = "Educação Inclusiva",
                    DataAdmissao = new DateTime(2022, 2, 1),
                    Status = StatusProfessor.Ativo,
                    Salario = 4500.00m,
                    CargaHorariaSemanal = 40,
                    EscolaId = 1
                },
                new Professor
                {
                    Id = 2,
                    UsuarioId = 4,
                    CodigoProfessor = "PROF002",
                    NomeCompleto = "Carlos Lima",
                    CPF = "22222222222",
                    RG = "2222222",
                    DataNascimento = new DateTime(1980, 8, 20),
                    Formacao = "Licenciatura em Português",
                    Especializacao = "Literatura Brasileira",
                    DataAdmissao = new DateTime(2021, 3, 15),
                    Status = StatusProfessor.Ativo,
                    Salario = 4700.00m,
                    CargaHorariaSemanal = 40,
                    EscolaId = 1
                }
            );

            // Turmas
            modelBuilder.Entity<Turma>().HasData(
                new Turma
                {
                    Id = 1,
                    Codigo = "TURMA1",
                    Nome = "1º Ano A",
                    AnoLetivo = 2025,
                    Serie = "1º",
                    Turno = Turno.Matutino,
                    CapacidadeMaxima = 30,
                    CursoId = 1,
                    ProfessorCoordenadorId = 1,
                    Sala = "101",
                    Status = StatusTurma.Ativa
                },
                new Turma
                {
                    Id = 2,
                    Codigo = "TURMA2",
                    Nome = "2º Ano A",
                    AnoLetivo = 2025,
                    Serie = "2º",
                    Turno = Turno.Vespertino,
                    CapacidadeMaxima = 30,
                    CursoId = 1,
                    ProfessorCoordenadorId = 2,
                    Sala = "102",
                    Status = StatusTurma.Ativa
                }
            );

            // Responsável
            modelBuilder.Entity<Responsavel>().HasData(
                new Responsavel
                {
                    Id = 1,
                    UsuarioId = 10,
                    NomeCompleto = "Fernanda Silva",
                    CPF = "33333333333",
                    RG = "3333333",
                    Telefone = "11999990000",
                    Email = "fernanda.silva@sige.edu.br",
                    Endereco = "Rua Principal, 100",
                    Parentesco = Parentesco.Mae,
                    Principal = true
                }
            );

            // Alunos
            modelBuilder.Entity<Aluno>().HasData(
                new Aluno
                {
                    Id = 1,
                    UsuarioId = 5,
                    Matricula = "ALU001",
                    NomeCompleto = "João Silva",
                    DataNascimento = new DateTime(2016, 3, 15),
                    Sexo = Sexo.M,
                    CPF = "44444444441",
                    Endereco = "Rua A, 10",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01000-001",
                    Status = StatusAluno.Matriculado,
                    DataMatricula = new DateTime(2025, 1, 10),
                    EscolaId = 1
                },
                new Aluno
                {
                    Id = 2,
                    UsuarioId = 6,
                    Matricula = "ALU002",
                    NomeCompleto = "Maria Santos",
                    DataNascimento = new DateTime(2016, 6, 21),
                    Sexo = Sexo.F,
                    CPF = "44444444442",
                    Endereco = "Rua B, 20",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01000-002",
                    Status = StatusAluno.Matriculado,
                    DataMatricula = new DateTime(2025, 1, 10),
                    EscolaId = 1
                },
                new Aluno
                {
                    Id = 3,
                    UsuarioId = 7,
                    Matricula = "ALU003",
                    NomeCompleto = "Pedro Alves",
                    DataNascimento = new DateTime(2016, 9, 5),
                    Sexo = Sexo.M,
                    CPF = "44444444443",
                    Endereco = "Rua C, 30",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01000-003",
                    Status = StatusAluno.Matriculado,
                    DataMatricula = new DateTime(2025, 1, 10),
                    EscolaId = 1
                },
                new Aluno
                {
                    Id = 4,
                    UsuarioId = 8,
                    Matricula = "ALU004",
                    NomeCompleto = "Luana Costa",
                    DataNascimento = new DateTime(2016, 11, 18),
                    Sexo = Sexo.F,
                    CPF = "44444444444",
                    Endereco = "Rua D, 40",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01000-004",
                    Status = StatusAluno.Matriculado,
                    DataMatricula = new DateTime(2025, 1, 10),
                    EscolaId = 1
                },
                new Aluno
                {
                    Id = 5,
                    UsuarioId = 9,
                    Matricula = "ALU005",
                    NomeCompleto = "Rafael Pereira",
                    DataNascimento = new DateTime(2016, 12, 2),
                    Sexo = Sexo.M,
                    CPF = "44444444445",
                    Endereco = "Rua E, 50",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    CEP = "01000-005",
                    Status = StatusAluno.Matriculado,
                    DataMatricula = new DateTime(2025, 1, 10),
                    EscolaId = 1
                }
            );

            // Vínculos Aluno-Responsável
            modelBuilder.Entity<AlunoResponsavel>().HasData(
                new AlunoResponsavel { Id = 1, AlunoId = 1, ResponsavelId = 1, DataVinculo = DateTime.UtcNow },
                new AlunoResponsavel { Id = 2, AlunoId = 2, ResponsavelId = 1, DataVinculo = DateTime.UtcNow }
            );

            // Matrículas (ligam Aluno à Turma)
            modelBuilder.Entity<Matricula>().HasData(
                new Matricula { Id = 1, NumeroMatricula = "MAT-2025-001", AlunoId = 1, TurmaId = 1, AnoLetivo = 2025, DataMatricula = new DateTime(2025,1,10), Status = StatusMatricula.Ativa },
                new Matricula { Id = 2, NumeroMatricula = "MAT-2025-002", AlunoId = 2, TurmaId = 1, AnoLetivo = 2025, DataMatricula = new DateTime(2025,1,10), Status = StatusMatricula.Ativa },
                new Matricula { Id = 3, NumeroMatricula = "MAT-2025-003", AlunoId = 3, TurmaId = 1, AnoLetivo = 2025, DataMatricula = new DateTime(2025,1,10), Status = StatusMatricula.Ativa },
                new Matricula { Id = 4, NumeroMatricula = "MAT-2025-004", AlunoId = 4, TurmaId = 2, AnoLetivo = 2025, DataMatricula = new DateTime(2025,1,10), Status = StatusMatricula.Ativa },
                new Matricula { Id = 5, NumeroMatricula = "MAT-2025-005", AlunoId = 5, TurmaId = 2, AnoLetivo = 2025, DataMatricula = new DateTime(2025,1,10), Status = StatusMatricula.Ativa }
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