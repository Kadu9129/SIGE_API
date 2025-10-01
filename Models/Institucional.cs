using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGE.API.Models
{
    public enum StatusEscola
    {
        Ativa,
        Inativa
    }

    public enum NivelEnsino
    {
        Infantil,
        Fundamental1,
        Fundamental2,
        Medio,
        Tecnico,
        Superior
    }

    [Table("escolas")]
    public class Escola
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(18)]
        public string CNPJ { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Endereco { get; set; }

        [MaxLength(100)]
        public string? Cidade { get; set; }

        [MaxLength(2)]
        public string? Estado { get; set; }

        [MaxLength(9)]
        public string? CEP { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        public int? DiretorId { get; set; }

        public StatusEscola Status { get; set; } = StatusEscola.Ativa;

        // Navigation Properties
        [ForeignKey("DiretorId")]
        public virtual Usuario? Diretor { get; set; }

        public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();
        public virtual ICollection<Aluno> Alunos { get; set; } = new List<Aluno>();
        public virtual ICollection<Professor> Professores { get; set; } = new List<Professor>();
    }

    [Table("cursos")]
    public class Curso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int EscolaId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string Codigo { get; set; } = string.Empty;

        [Column(TypeName = "TEXT")]
        public string? Descricao { get; set; }

        public int DuracaoAnos { get; set; }

        public NivelEnsino NivelEnsino { get; set; }

        public bool Status { get; set; } = true;

        // Navigation Properties
        [ForeignKey("EscolaId")]
        public virtual Escola Escola { get; set; } = null!;

        public virtual ICollection<Disciplina> Disciplinas { get; set; } = new List<Disciplina>();
        public virtual ICollection<Turma> Turmas { get; set; } = new List<Turma>();
        public virtual ICollection<PlanoPagamento> PlanosPagamento { get; set; } = new List<PlanoPagamento>();
    }

    [Table("disciplinas")]
    public class Disciplina
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string Codigo { get; set; } = string.Empty;

        [Column(TypeName = "TEXT")]
        public string? Descricao { get; set; }

        public int CargaHorariaTotal { get; set; }

        [Required]
        public int CursoId { get; set; }

        public bool Status { get; set; } = true;

        // Navigation Properties
        [ForeignKey("CursoId")]
        public virtual Curso Curso { get; set; } = null!;

        public virtual ICollection<ProfessorDisciplina> ProfessoresDisciplinas { get; set; } = new List<ProfessorDisciplina>();
        public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();
        public virtual ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
        public virtual ICollection<Nota> Notas { get; set; } = new List<Nota>();
        public virtual ICollection<Boletim> Boletins { get; set; } = new List<Boletim>();
        public virtual ICollection<Frequencia> Frequencias { get; set; } = new List<Frequencia>();
        public virtual ICollection<Chamada> Chamadas { get; set; } = new List<Chamada>();
    }
}