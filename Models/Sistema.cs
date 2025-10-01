using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGE.API.Models
{
    public enum StatusRelatorio
    {
        Processando,
        Concluido,
        Erro
    }

    [Table("relatorios_gerados")]
    public class RelatorioGerado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string TipoRelatorio { get; set; } = string.Empty;

        [Required]
        public int UsuarioId { get; set; }

        public string? Parametros { get; set; } // JSON

        public DateTime DataGeracao { get; set; } = DateTime.UtcNow;

        [MaxLength(255)]
        public string? ArquivoPath { get; set; }

        public StatusRelatorio Status { get; set; } = StatusRelatorio.Processando;

        // Navigation Properties
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;
    }

    [Table("configuracoes_sistema")]
    public class ConfiguracaoSistema
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Chave { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "TEXT")]
        public string Valor { get; set; } = string.Empty;

        [Column(TypeName = "TEXT")]
        public string? Descricao { get; set; }

        [MaxLength(50)]
        public string? Categoria { get; set; }

        public DateTime DataAlteracao { get; set; } = DateTime.UtcNow;

        public int? UsuarioAlteracaoId { get; set; }

        // Navigation Properties
        [ForeignKey("UsuarioAlteracaoId")]
        public virtual Usuario? UsuarioAlteracao { get; set; }
    }

    [Table("logs_sistema")]
    public class LogSistema
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? UsuarioId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Acao { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? TabelaAfetada { get; set; }

        public int? RegistroId { get; set; }

        public string? DadosAnteriores { get; set; } // JSON

        public string? DadosNovos { get; set; } // JSON

        [MaxLength(45)]
        public string? IpAddress { get; set; }

        public DateTime DataAcao { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}