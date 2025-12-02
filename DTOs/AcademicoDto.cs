using System.ComponentModel.DataAnnotations;
using SIGE.API.Models;

namespace SIGE.API.DTOs
{
    public class TurmaDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public int AnoLetivo { get; set; }
        public string? Serie { get; set; }
        public Turno Turno { get; set; }
        public int CapacidadeMaxima { get; set; }
        public int CursoId { get; set; }
        public string NomeCurso { get; set; } = string.Empty;
        public int? ProfessorCoordenadorId { get; set; }
        public string? NomeProfessorCoordenador { get; set; }
        public string? Sala { get; set; }
        public StatusTurma Status { get; set; }
        public List<TurmaAlunoResumoDto> Alunos { get; set; } = new();
    }

    public class TurmaAlunoResumoDto
    {
        public int MatriculaId { get; set; }
        public int AlunoId { get; set; }
        public string NomeAluno { get; set; } = string.Empty;
        public string NumeroMatricula { get; set; } = string.Empty;
        public StatusMatricula Status { get; set; }
    }

    public class TurmaCatalogoProfessorDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Especialidade { get; set; }
    }

    public class TurmaCatalogoAlunoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Serie { get; set; }
        public string? Matricula { get; set; }
    }

    public class TurmaCatalogoCursoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string? Nivel { get; set; }
    }

    public class TurmaCatalogosDto
    {
        public List<TurmaCatalogoCursoDto> Cursos { get; set; } = new();
        public List<TurmaCatalogoProfessorDto> Professores { get; set; } = new();
        public List<TurmaCatalogoAlunoDto> Alunos { get; set; } = new();
    }

    public class CreateTurmaDto
    {
        [Required(ErrorMessage = "Código é obrigatório")]
        [StringLength(15, ErrorMessage = "Código deve ter no máximo 15 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ano letivo é obrigatório")]
        [Range(2020, 2050, ErrorMessage = "Ano letivo deve estar entre 2020 e 2050")]
        public int AnoLetivo { get; set; }

        [StringLength(20, ErrorMessage = "Série deve ter no máximo 20 caracteres")]
        public string? Serie { get; set; }

        [Required(ErrorMessage = "Turno é obrigatório")]
        public Turno Turno { get; set; }

        [Required(ErrorMessage = "Capacidade máxima é obrigatória")]
        [Range(1, 100, ErrorMessage = "Capacidade deve ser entre 1 e 100 alunos")]
        public int CapacidadeMaxima { get; set; }

        [Required(ErrorMessage = "CursoId é obrigatório")]
        public int CursoId { get; set; }

        public int? ProfessorCoordenadorId { get; set; }

        [StringLength(20, ErrorMessage = "Sala deve ter no máximo 20 caracteres")]
        public string? Sala { get; set; }

        public StatusTurma Status { get; set; } = StatusTurma.Ativa;

        public List<int> AlunoIds { get; set; } = new();
    }

    public class UpdateTurmaDto
    {
        [Required(ErrorMessage = "Código é obrigatório")]
        [StringLength(15, ErrorMessage = "Código deve ter no máximo 15 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ano letivo é obrigatório")]
        [Range(2020, 2050, ErrorMessage = "Ano letivo deve estar entre 2020 e 2050")]
        public int AnoLetivo { get; set; }

        [StringLength(20, ErrorMessage = "Série deve ter no máximo 20 caracteres")]
        public string? Serie { get; set; }

        [Required(ErrorMessage = "Turno é obrigatório")]
        public Turno Turno { get; set; }

        [Required(ErrorMessage = "Capacidade máxima é obrigatória")]
        [Range(1, 100, ErrorMessage = "Capacidade deve ser entre 1 e 100 alunos")]
        public int CapacidadeMaxima { get; set; }

        [Required(ErrorMessage = "CursoId é obrigatório")]
        public int CursoId { get; set; }

        public int? ProfessorCoordenadorId { get; set; }

        [StringLength(20, ErrorMessage = "Sala deve ter no máximo 20 caracteres")]
        public string? Sala { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public StatusTurma Status { get; set; }

        public List<int> AlunoIds { get; set; } = new();
    }

    public class MatriculaDto
    {
        public int Id { get; set; }
        public string NumeroMatricula { get; set; } = string.Empty;
        public int AlunoId { get; set; }
        public string NomeAluno { get; set; } = string.Empty;
        public string MatriculaAluno { get; set; } = string.Empty;
        public int TurmaId { get; set; }
        public string NomeTurma { get; set; } = string.Empty;
        public string CodigoTurma { get; set; } = string.Empty;
        public int AnoLetivo { get; set; }
        public DateTime DataMatricula { get; set; }
        public StatusMatricula Status { get; set; }
        public string? Observacoes { get; set; }
    }

    public class CreateMatriculaDto
    {
        [Required(ErrorMessage = "Número da matrícula é obrigatório")]
        [StringLength(20, ErrorMessage = "Número da matrícula deve ter no máximo 20 caracteres")]
        public string NumeroMatricula { get; set; } = string.Empty;

        [Required(ErrorMessage = "AlunoId é obrigatório")]
        public int AlunoId { get; set; }

        [Required(ErrorMessage = "TurmaId é obrigatório")]
        public int TurmaId { get; set; }

        [Required(ErrorMessage = "Ano letivo é obrigatório")]
        [Range(2020, 2050, ErrorMessage = "Ano letivo deve estar entre 2020 e 2050")]
        public int AnoLetivo { get; set; }

        public StatusMatricula Status { get; set; } = StatusMatricula.Ativa;

        public string? Observacoes { get; set; }
    }

    public class UpdateMatriculaDto
    {
        [Required(ErrorMessage = "Número da matrícula é obrigatório")]
        [StringLength(20, ErrorMessage = "Número da matrícula deve ter no máximo 20 caracteres")]
        public string NumeroMatricula { get; set; } = string.Empty;

        [Required(ErrorMessage = "AlunoId é obrigatório")]
        public int AlunoId { get; set; }

        [Required(ErrorMessage = "TurmaId é obrigatório")]
        public int TurmaId { get; set; }

        [Required(ErrorMessage = "Ano letivo é obrigatório")]
        [Range(2020, 2050, ErrorMessage = "Ano letivo deve estar entre 2020 e 2050")]
        public int AnoLetivo { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public StatusMatricula Status { get; set; }

        public string? Observacoes { get; set; }
    }

    public class AvaliacaoDto
    {
        public int Id { get; set; }
        public int DisciplinaId { get; set; }
        public string NomeDisciplina { get; set; } = string.Empty;
        public int TurmaId { get; set; }
        public string NomeTurma { get; set; } = string.Empty;
        public int ProfessorId { get; set; }
        public string NomeProfessor { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public TipoAvaliacao Tipo { get; set; }
        public DateTime DataAplicacao { get; set; }
        public decimal ValorMaximo { get; set; }
        public decimal Peso { get; set; }
        public int Bimestre { get; set; }
        public bool Status { get; set; }
    }

    public class CreateAvaliacaoDto
    {
        [Required(ErrorMessage = "DisciplinaId é obrigatório")]
        public int DisciplinaId { get; set; }

        [Required(ErrorMessage = "TurmaId é obrigatório")]
        public int TurmaId { get; set; }

        [Required(ErrorMessage = "ProfessorId é obrigatório")]
        public int ProfessorId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo é obrigatório")]
        public TipoAvaliacao Tipo { get; set; }

        [Required(ErrorMessage = "Data de aplicação é obrigatória")]
        public DateTime DataAplicacao { get; set; }

        [Required(ErrorMessage = "Valor máximo é obrigatório")]
        [Range(0.01, 100, ErrorMessage = "Valor máximo deve ser entre 0.01 e 100")]
        public decimal ValorMaximo { get; set; }

        [Required(ErrorMessage = "Peso é obrigatório")]
        [Range(0.01, 10, ErrorMessage = "Peso deve ser entre 0.01 e 10")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "Bimestre é obrigatório")]
        [Range(1, 4, ErrorMessage = "Bimestre deve ser entre 1 e 4")]
        public int Bimestre { get; set; }

        public bool Status { get; set; } = true;
    }

    public class UpdateAvaliacaoDto
    {
        [Required(ErrorMessage = "DisciplinaId é obrigatório")]
        public int DisciplinaId { get; set; }

        [Required(ErrorMessage = "TurmaId é obrigatório")]
        public int TurmaId { get; set; }

        [Required(ErrorMessage = "ProfessorId é obrigatório")]
        public int ProfessorId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo é obrigatório")]
        public TipoAvaliacao Tipo { get; set; }

        [Required(ErrorMessage = "Data de aplicação é obrigatória")]
        public DateTime DataAplicacao { get; set; }

        [Required(ErrorMessage = "Valor máximo é obrigatório")]
        [Range(0.01, 100, ErrorMessage = "Valor máximo deve ser entre 0.01 e 100")]
        public decimal ValorMaximo { get; set; }

        [Required(ErrorMessage = "Peso é obrigatório")]
        [Range(0.01, 10, ErrorMessage = "Peso deve ser entre 0.01 e 10")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "Bimestre é obrigatório")]
        [Range(1, 4, ErrorMessage = "Bimestre deve ser entre 1 e 4")]
        public int Bimestre { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public bool Status { get; set; }
    }

    public class NotaDto
    {
        public int Id { get; set; }
        public int AvaliacaoId { get; set; }
        public string NomeAvaliacao { get; set; } = string.Empty;
        public int AlunoId { get; set; }
        public string NomeAluno { get; set; } = string.Empty;
        public string MatriculaAluno { get; set; } = string.Empty;
        public decimal NotaValor { get; set; }
        public string? Observacoes { get; set; }
        public DateTime DataLancamento { get; set; }
        public int ProfessorId { get; set; }
        public string NomeProfessor { get; set; } = string.Empty;
    }

    public class CreateNotaDto
    {
        [Required(ErrorMessage = "AvaliacaoId é obrigatório")]
        public int AvaliacaoId { get; set; }

        [Required(ErrorMessage = "AlunoId é obrigatório")]
        public int AlunoId { get; set; }

        [Required(ErrorMessage = "Nota é obrigatória")]
        [Range(0, 100, ErrorMessage = "Nota deve ser entre 0 e 100")]
        public decimal NotaValor { get; set; }

        public string? Observacoes { get; set; }

        [Required(ErrorMessage = "ProfessorId é obrigatório")]
        public int ProfessorId { get; set; }
    }

    public class UpdateNotaDto
    {
        [Required(ErrorMessage = "AvaliacaoId é obrigatório")]
        public int AvaliacaoId { get; set; }

        [Required(ErrorMessage = "AlunoId é obrigatório")]
        public int AlunoId { get; set; }

        [Required(ErrorMessage = "Nota é obrigatória")]
        [Range(0, 100, ErrorMessage = "Nota deve ser entre 0 e 100")]
        public decimal NotaValor { get; set; }

        public string? Observacoes { get; set; }

        [Required(ErrorMessage = "ProfessorId é obrigatório")]
        public int ProfessorId { get; set; }
    }
}