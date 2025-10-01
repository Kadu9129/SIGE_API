using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGE.API.Models
{
    [Table("perfis_acesso")]
    public class PerfilAcesso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string NomePerfil { get; set; } = string.Empty;

        [Column(TypeName = "TEXT")]
        public string? Descricao { get; set; }

        public string? Permissoes { get; set; } // JSON

        public bool Status { get; set; } = true;
    }

    [Table("sessoes")]
    public class Sessao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Token { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime DataExpiracao { get; set; }

        [MaxLength(45)]
        public string? IpAddress { get; set; }

        [Column(TypeName = "TEXT")]
        public string? UserAgent { get; set; }

        // Navigation Properties
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;
    }
}