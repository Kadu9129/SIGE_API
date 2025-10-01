using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGE.API.Models
{
    public enum PublicoAlvo
    {
        Todos,
        Alunos,
        Professores,
        Responsaveis,
        Administrativo
    }

    public enum Prioridade
    {
        Baixa,
        Media,
        Alta,
        Urgente
    }

    public enum StatusComunicado
    {
        Rascunho,
        Publicado,
        Expirado
    }

    public enum StatusMensagem
    {
        Enviada,
        Lida,
        Respondida
    }

    [Table("comunicados")]
    public class Comunicado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "TEXT")]
        public string Conteudo { get; set; } = string.Empty;

        [Required]
        public int AutorId { get; set; }

        public DateTime DataPublicacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataExpiracao { get; set; }

        public PublicoAlvo PublicoAlvo { get; set; }

        public Prioridade Prioridade { get; set; } = Prioridade.Media;

        public StatusComunicado Status { get; set; } = StatusComunicado.Rascunho;

        public string? Anexos { get; set; } // JSON

        // Navigation Properties
        [ForeignKey("AutorId")]
        public virtual Usuario Autor { get; set; } = null!;
    }

    [Table("mensagens")]
    public class Mensagem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int RemetenteId { get; set; }

        [Required]
        public int DestinatarioId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Assunto { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "TEXT")]
        public string Conteudo { get; set; } = string.Empty;

        public DateTime DataEnvio { get; set; } = DateTime.UtcNow;

        public DateTime? DataLeitura { get; set; }

        public StatusMensagem Status { get; set; } = StatusMensagem.Enviada;

        public string? Anexos { get; set; } // JSON

        // Navigation Properties
        [ForeignKey("RemetenteId")]
        public virtual Usuario Remetente { get; set; } = null!;

        [ForeignKey("DestinatarioId")]
        public virtual Usuario Destinatario { get; set; } = null!;
    }
}