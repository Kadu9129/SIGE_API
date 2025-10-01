using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGE.API.Models
{
    public enum Turno
    {
        Matutino,
        Vespertino,
        Noturno,
        Integral
    }

    public enum StatusTurma
    {
        Ativa,
        Finalizada,
        Cancelada
    }

    public enum StatusMatricula
    {
        Ativa,
        Trancada,
        Transferida,
        Concluida
    }

    public enum DiaSemana
    {
        Segunda,
        Terca,
        Quarta,
        Quinta,
        Sexta,
        Sabado
    }

    [Table("turmas")]
    public class Turma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        public int AnoLetivo { get; set; }

        [MaxLength(20)]
        public string? Serie { get; set; }

        public Turno Turno { get; set; }

        public int CapacidadeMaxima { get; set; }

        [Required]
        public int CursoId { get; set; }

        public int? ProfessorCoordenadorId { get; set; }

        [MaxLength(20)]
        public string? Sala { get; set; }

        public StatusTurma Status { get; set; } = StatusTurma.Ativa;

        // Navigation Properties
        [ForeignKey("CursoId")]
        public virtual Curso Curso { get; set; } = null!;

        [ForeignKey("ProfessorCoordenadorId")]
        public virtual Professor? ProfessorCoordenador { get; set; }

        public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
        public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();
        public virtual ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
        public virtual ICollection<Chamada> Chamadas { get; set; } = new List<Chamada>();
    }

    [Table("matriculas")]
    public class Matricula
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string NumeroMatricula { get; set; } = string.Empty;

        [Required]
        public int AlunoId { get; set; }

        [Required]
        public int TurmaId { get; set; }

        public int AnoLetivo { get; set; }

        public DateTime DataMatricula { get; set; }

        public StatusMatricula Status { get; set; } = StatusMatricula.Ativa;

        [Column(TypeName = "TEXT")]
        public string? Observacoes { get; set; }

        // Navigation Properties
        [ForeignKey("AlunoId")]
        public virtual Aluno Aluno { get; set; } = null!;

        [ForeignKey("TurmaId")]
        public virtual Turma Turma { get; set; } = null!;
    }

    [Table("horarios")]
    public class Horario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TurmaId { get; set; }

        [Required]
        public int DisciplinaId { get; set; }

        [Required]
        public int ProfessorId { get; set; }

        public DiaSemana DiaSemana { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFim { get; set; }

        [MaxLength(20)]
        public string? Sala { get; set; }

        // Navigation Properties
        [ForeignKey("TurmaId")]
        public virtual Turma Turma { get; set; } = null!;

        [ForeignKey("DisciplinaId")]
        public virtual Disciplina Disciplina { get; set; } = null!;

        [ForeignKey("ProfessorId")]
        public virtual Professor Professor { get; set; } = null!;

        public virtual ICollection<Frequencia> Frequencias { get; set; } = new List<Frequencia>();
    }
}