using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGE.API.Models
{
    public enum Sexo
    {
        M,
        F,
        Outro
    }

    public enum StatusAluno
    {
        Matriculado,
        Transferido,
        Evadido,
        Formado
    }

    public enum Parentesco
    {
        Pai,
        Mae,
        Avo,
        Ava,
        Tutor,
        Outro
    }

    [Table("alunos")]
    public class Aluno
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Matricula { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string NomeCompleto { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }

        public Sexo Sexo { get; set; }

        [MaxLength(15)]
        public string? RG { get; set; }

        [MaxLength(14)]
        public string? CPF { get; set; }

        [MaxLength(255)]
        public string? Endereco { get; set; }

        [MaxLength(100)]
        public string? Cidade { get; set; }

        [MaxLength(2)]
        public string? Estado { get; set; }

        [MaxLength(9)]
        public string? CEP { get; set; }

        [MaxLength(20)]
        public string? TelefoneResponsavel { get; set; }

        [MaxLength(100)]
        public string? EmailResponsavel { get; set; }

        public StatusAluno Status { get; set; } = StatusAluno.Matriculado;

        public DateTime DataMatricula { get; set; }

        [Required]
        public int EscolaId { get; set; }

        // Navigation Properties
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        [ForeignKey("EscolaId")]
        public virtual Escola Escola { get; set; } = null!;

        public virtual ICollection<AlunoResponsavel> AlunosResponsaveis { get; set; } = new List<AlunoResponsavel>();
        public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
        public virtual ICollection<Nota> Notas { get; set; } = new List<Nota>();
        public virtual ICollection<Boletim> Boletins { get; set; } = new List<Boletim>();
        public virtual ICollection<Frequencia> Frequencias { get; set; } = new List<Frequencia>();
        public virtual ICollection<FinanceiroAluno> FinanceiroAluno { get; set; } = new List<FinanceiroAluno>();
    }

    [Table("responsaveis")]
    public class Responsavel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(150)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        [MaxLength(14)]
        public string CPF { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? RG { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(255)]
        public string? Endereco { get; set; }

        public Parentesco Parentesco { get; set; }

        public bool Principal { get; set; } = false;

        // Navigation Properties
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        public virtual ICollection<AlunoResponsavel> AlunosResponsaveis { get; set; } = new List<AlunoResponsavel>();
    }

    [Table("aluno_responsavel")]
    public class AlunoResponsavel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AlunoId { get; set; }

        [Required]
        public int ResponsavelId { get; set; }

        public DateTime DataVinculo { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("AlunoId")]
        public virtual Aluno Aluno { get; set; } = null!;

        [ForeignKey("ResponsavelId")]
        public virtual Responsavel Responsavel { get; set; } = null!;
    }
}