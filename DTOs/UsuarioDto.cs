using System.ComponentModel.DataAnnotations;
using SIGE.API.Models;

namespace SIGE.API.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public TipoUsuario TipoUsuario { get; set; }
        public StatusUsuario Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
        public string? FotoPerfil { get; set; }
        public string? Telefone { get; set; }
        public string? CPF { get; set; }
    }

    public class CreateUsuarioDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo de usuário é obrigatório")]
        public TipoUsuario TipoUsuario { get; set; }

        public StatusUsuario Status { get; set; } = StatusUsuario.Ativo;

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [StringLength(14, ErrorMessage = "CPF deve ter no máximo 14 caracteres")]
        public string? CPF { get; set; }
    }

    public class UpdateUsuarioDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo de usuário é obrigatório")]
        public TipoUsuario TipoUsuario { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public StatusUsuario Status { get; set; }

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [StringLength(14, ErrorMessage = "CPF deve ter no máximo 14 caracteres")]
        public string? CPF { get; set; }
    }

    public class UsuarioListDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public string? Telefone { get; set; }
    }
}