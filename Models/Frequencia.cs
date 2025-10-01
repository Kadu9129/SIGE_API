using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGE.API.Models
{
    public enum StatusChamada
    {
        Realizada,
        Cancelada,
        Reposta
    }

    [Table("frequencias")]
    public class Frequencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AlunoId { get; set; }

        [Required]
        public int DisciplinaId { get; set; }

        public DateTime DataAula { get; set; }

        public bool Presente { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Justificativa { get; set; }

        [Required]
        public int ProfessorId { get; set; }

        [Required]
        public int HorarioId { get; set; }

        // Navigation Properties
        [ForeignKey("AlunoId")]
        public virtual Aluno Aluno { get; set; } = null!;

        [ForeignKey("DisciplinaId")]
        public virtual Disciplina Disciplina { get; set; } = null!;

        [ForeignKey("ProfessorId")]
        public virtual Professor Professor { get; set; } = null!;

        [ForeignKey("HorarioId")]
        public virtual Horario Horario { get; set; } = null!;
    }

    [Table("chamadas")]
    public class Chamada
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

        public DateTime DataChamada { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFim { get; set; }

        [Column(TypeName = "TEXT")]
        public string? ConteudoMinistrado { get; set; }

        public StatusChamada Status { get; set; } = StatusChamada.Realizada;

        // Navigation Properties
        [ForeignKey("TurmaId")]
        public virtual Turma Turma { get; set; } = null!;

        [ForeignKey("DisciplinaId")]
        public virtual Disciplina Disciplina { get; set; } = null!;

        [ForeignKey("ProfessorId")]
        public virtual Professor Professor { get; set; } = null!;
    }
}