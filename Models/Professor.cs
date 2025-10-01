using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGE.API.Models
{
    public enum StatusProfessor
    {
        Ativo,
        Licenca,
        Afastado,
        Demitido
    }

    [Table("professores")]
    public class Professor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(15)]
        public string CodigoProfessor { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        [MaxLength(14)]
        public string CPF { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? RG { get; set; }

        public DateTime DataNascimento { get; set; }

        [MaxLength(255)]
        public string? Formacao { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Especializacao { get; set; }

        public DateTime DataAdmissao { get; set; }

        public StatusProfessor Status { get; set; } = StatusProfessor.Ativo;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Salario { get; set; }

        public int CargaHorariaSemanal { get; set; }

        [Required]
        public int EscolaId { get; set; }

        // Navigation Properties
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        [ForeignKey("EscolaId")]
        public virtual Escola Escola { get; set; } = null!;

        public virtual ICollection<ProfessorDisciplina> ProfessoresDisciplinas { get; set; } = new List<ProfessorDisciplina>();
        public virtual ICollection<Turma> TurmasCoordenadas { get; set; } = new List<Turma>();
        public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();
        public virtual ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
        public virtual ICollection<Nota> Notas { get; set; } = new List<Nota>();
        public virtual ICollection<Frequencia> Frequencias { get; set; } = new List<Frequencia>();
        public virtual ICollection<Chamada> Chamadas { get; set; } = new List<Chamada>();
    }

    [Table("professor_disciplina")]
    public class ProfessorDisciplina
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProfessorId { get; set; }

        [Required]
        public int DisciplinaId { get; set; }

        public int AnoLetivo { get; set; }

        public bool Status { get; set; } = true;

        // Navigation Properties
        [ForeignKey("ProfessorId")]
        public virtual Professor Professor { get; set; } = null!;

        [ForeignKey("DisciplinaId")]
        public virtual Disciplina Disciplina { get; set; } = null!;
    }
}