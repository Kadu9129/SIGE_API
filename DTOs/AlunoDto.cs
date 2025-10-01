using System.ComponentModel.DataAnnotations;
using SIGE.API.Models;

namespace SIGE.API.DTOs
{
    public class AlunoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string EmailUsuario { get; set; } = string.Empty;
        public string Matricula { get; set; } = string.Empty;
        public string NomeCompleto { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public Sexo Sexo { get; set; }
        public string? RG { get; set; }
        public string? CPF { get; set; }
        public string? Endereco { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? CEP { get; set; }
        public string? TelefoneResponsavel { get; set; }
        public string? EmailResponsavel { get; set; }
        public StatusAluno Status { get; set; }
        public DateTime DataMatricula { get; set; }
        public int EscolaId { get; set; }
        public string NomeEscola { get; set; } = string.Empty;
        public string? FotoPerfil { get; set; }
        public List<MatriculaDto>? Matriculas { get; set; }
        public List<ResponsavelDto>? Responsaveis { get; set; }
    }

    public class CreateAlunoDto
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Matrícula é obrigatória")]
        [StringLength(20, ErrorMessage = "Matrícula deve ter no máximo 20 caracteres")]
        public string Matricula { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nome completo é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome completo deve ter no máximo 150 caracteres")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Sexo é obrigatório")]
        public Sexo Sexo { get; set; }

        [StringLength(15, ErrorMessage = "RG deve ter no máximo 15 caracteres")]
        public string? RG { get; set; }

        [StringLength(14, ErrorMessage = "CPF deve ter no máximo 14 caracteres")]
        public string? CPF { get; set; }

        [StringLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres")]
        public string? Endereco { get; set; }

        [StringLength(100, ErrorMessage = "Cidade deve ter no máximo 100 caracteres")]
        public string? Cidade { get; set; }

        [StringLength(2, ErrorMessage = "Estado deve ter no máximo 2 caracteres")]
        public string? Estado { get; set; }

        [StringLength(10, ErrorMessage = "CEP deve ter no máximo 10 caracteres")]
        public string? CEP { get; set; }

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? TelefoneResponsavel { get; set; }

        [EmailAddress(ErrorMessage = "Email do responsável deve ter formato válido")]
        [StringLength(100, ErrorMessage = "Email do responsável deve ter no máximo 100 caracteres")]
        public string? EmailResponsavel { get; set; }

        [Required(ErrorMessage = "EscolaId é obrigatório")]
        public int EscolaId { get; set; }

    public StatusAluno Status { get; set; } = StatusAluno.Matriculado;
    }

    public class UpdateAlunoDto
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nome completo é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome completo deve ter no máximo 150 caracteres")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Sexo é obrigatório")]
        public Sexo Sexo { get; set; }

        [StringLength(15, ErrorMessage = "RG deve ter no máximo 15 caracteres")]
        public string? RG { get; set; }

        [StringLength(14, ErrorMessage = "CPF deve ter no máximo 14 caracteres")]
        public string? CPF { get; set; }

        [StringLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres")]
        public string? Endereco { get; set; }

        [StringLength(100, ErrorMessage = "Cidade deve ter no máximo 100 caracteres")]
        public string? Cidade { get; set; }

        [StringLength(2, ErrorMessage = "Estado deve ter no máximo 2 caracteres")]
        public string? Estado { get; set; }

        [StringLength(10, ErrorMessage = "CEP deve ter no máximo 10 caracteres")]
        public string? CEP { get; set; }

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? TelefoneResponsavel { get; set; }

        [EmailAddress(ErrorMessage = "Email do responsável deve ter formato válido")]
        [StringLength(100, ErrorMessage = "Email do responsável deve ter no máximo 100 caracteres")]
        public string? EmailResponsavel { get; set; }

        [Required(ErrorMessage = "EscolaId é obrigatório")]
        public int EscolaId { get; set; }

    public StatusAluno Status { get; set; }
    }

    public class ResponsavelDto
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string? RG { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? Endereco { get; set; }
        public Parentesco Parentesco { get; set; }
        public bool Principal { get; set; }
        public DateTime DataVinculo { get; set; }
    }
}