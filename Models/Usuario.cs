using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGE.API.Models
{
    public enum TipoUsuario
    {
        Admin,
        Diretor,
        Professor,
        Aluno,
        Responsavel
    }

    public enum StatusUsuario
    {
        Ativo,
        Inativo,
        Suspenso
    }

    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string SenhaHash { get; set; } = string.Empty;

        [Required]
        public TipoUsuario TipoUsuario { get; set; }

        [Required]
        public StatusUsuario Status { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime DataUltimaAtualizacao { get; set; } = DateTime.UtcNow;

        [MaxLength(255)]
        public string? FotoPerfil { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        [MaxLength(14)]
        public string? CPF { get; set; }

        // Navigation Properties
        public virtual ICollection<Sessao> Sessoes { get; set; } = new List<Sessao>();
        public virtual ICollection<Escola> EscolasDirigidas { get; set; } = new List<Escola>();
        public virtual Aluno? Aluno { get; set; }
        public virtual Responsavel? Responsavel { get; set; }
        public virtual Professor? Professor { get; set; }
        public virtual ICollection<Comunicado> Comunicados { get; set; } = new List<Comunicado>();
        public virtual ICollection<Mensagem> MensagensEnviadas { get; set; } = new List<Mensagem>();
        public virtual ICollection<Mensagem> MensagensRecebidas { get; set; } = new List<Mensagem>();
        public virtual ICollection<ConfiguracaoSistema> ConfiguracoesAlteradas { get; set; } = new List<ConfiguracaoSistema>();
        public virtual ICollection<LogSistema> Logs { get; set; } = new List<LogSistema>();
        public virtual ICollection<RelatorioGerado> RelatoriosGerados { get; set; } = new List<RelatorioGerado>();
    }
}