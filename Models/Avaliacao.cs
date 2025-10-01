using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGE.API.Models
{
    public enum TipoAvaliacao
    {
        Prova,
        Trabalho,
        Seminario,
        Atividade,
        Participacao
    }

    public enum SituacaoBoletim
    {
        Aprovado,
        Recuperacao,
        Reprovado
    }

    [Table("avaliacoes")]
    public class Avaliacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int DisciplinaId { get; set; }

        [Required]
        public int TurmaId { get; set; }

        [Required]
        public int ProfessorId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        public TipoAvaliacao Tipo { get; set; }

        public DateTime DataAplicacao { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal ValorMaximo { get; set; }

        [Column(TypeName = "decimal(3,2)")]
        public decimal Peso { get; set; }

        public int Bimestre { get; set; }

        public bool Status { get; set; } = true;

        // Navigation Properties
        [ForeignKey("DisciplinaId")]
        public virtual Disciplina Disciplina { get; set; } = null!;

        [ForeignKey("TurmaId")]
        public virtual Turma Turma { get; set; } = null!;

        [ForeignKey("ProfessorId")]
        public virtual Professor Professor { get; set; } = null!;

        public virtual ICollection<Nota> Notas { get; set; } = new List<Nota>();
    }

    [Table("notas")]
    public class Nota
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AvaliacaoId { get; set; }

        [Required]
        public int AlunoId { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal NotaValor { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Observacoes { get; set; }

        public DateTime DataLancamento { get; set; } = DateTime.UtcNow;

        [Required]
        public int ProfessorId { get; set; }

        // Navigation Properties
        [ForeignKey("AvaliacaoId")]
        public virtual Avaliacao Avaliacao { get; set; } = null!;

        [ForeignKey("AlunoId")]
        public virtual Aluno Aluno { get; set; } = null!;

        [ForeignKey("ProfessorId")]
        public virtual Professor Professor { get; set; } = null!;
    }

    [Table("boletins")]
    public class Boletim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AlunoId { get; set; }

        [Required]
        public int DisciplinaId { get; set; }

        public int Bimestre { get; set; }

        public int AnoLetivo { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? NotaFinal { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? MediaBimestre { get; set; }

        public SituacaoBoletim Situacao { get; set; }

        public int Faltas { get; set; }

        // Navigation Properties
        [ForeignKey("AlunoId")]
        public virtual Aluno Aluno { get; set; } = null!;

        [ForeignKey("DisciplinaId")]
        public virtual Disciplina Disciplina { get; set; } = null!;
    }
}