using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGE.API.Models
{
    public enum StatusFinanceiro
    {
        Pendente,
        Pago,
        Atrasado,
        Cancelado
    }

    [Table("planos_pagamento")]
    public class PlanoPagamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorMensalidade { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Descricao { get; set; }

        [Required]
        public int CursoId { get; set; }

        public bool Ativo { get; set; } = true;

        // Navigation Properties
        [ForeignKey("CursoId")]
        public virtual Curso Curso { get; set; } = null!;

        public virtual ICollection<FinanceiroAluno> FinanceiroAlunos { get; set; } = new List<FinanceiroAluno>();
    }

    [Table("financeiro_aluno")]
    public class FinanceiroAluno
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AlunoId { get; set; }

        [Required]
        public int PlanoId { get; set; }

        public DateTime MesReferencia { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorDevido { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? ValorPago { get; set; }

        public DateTime DataVencimento { get; set; }

        public DateTime? DataPagamento { get; set; }

        public StatusFinanceiro Status { get; set; } = StatusFinanceiro.Pendente;

        [Column(TypeName = "TEXT")]
        public string? Observacoes { get; set; }

        // Navigation Properties
        [ForeignKey("AlunoId")]
        public virtual Aluno Aluno { get; set; } = null!;

        [ForeignKey("PlanoId")]
        public virtual PlanoPagamento Plano { get; set; } = null!;
    }
}