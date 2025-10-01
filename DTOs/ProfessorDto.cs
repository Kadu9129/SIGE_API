using System.ComponentModel.DataAnnotations;
using SIGE.API.Models;

namespace SIGE.API.DTOs
{
    public class ProfessorDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string EmailUsuario { get; set; } = string.Empty;
        public string CodigoProfessor { get; set; } = string.Empty;
        public string NomeCompleto { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string? RG { get; set; }
        public DateTime DataNascimento { get; set; }
        public string? Formacao { get; set; }
        public string? Especializacao { get; set; }
        public DateTime DataAdmissao { get; set; }
        public StatusProfessor Status { get; set; }
        public decimal? Salario { get; set; }
        public int CargaHorariaSemanal { get; set; }
        public int EscolaId { get; set; }
        public string NomeEscola { get; set; } = string.Empty;
        public string? FotoPerfil { get; set; }
        public List<DisciplinaDto>? Disciplinas { get; set; }
    }

    public class CreateProfessorDto
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Código do professor é obrigatório")]
        [StringLength(20, ErrorMessage = "Código deve ter no máximo 20 caracteres")]
        public string CodigoProfessor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nome completo é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome completo deve ter no máximo 150 caracteres")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(14, ErrorMessage = "CPF deve ter no máximo 14 caracteres")]
        public string CPF { get; set; } = string.Empty;

        [StringLength(15, ErrorMessage = "RG deve ter no máximo 15 caracteres")]
        public string? RG { get; set; }

        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        [StringLength(200, ErrorMessage = "Formação deve ter no máximo 200 caracteres")]
        public string? Formacao { get; set; }

        [StringLength(200, ErrorMessage = "Especialização deve ter no máximo 200 caracteres")]
        public string? Especializacao { get; set; }

        [Required(ErrorMessage = "Data de admissão é obrigatória")]
        public DateTime DataAdmissao { get; set; }

        public decimal? Salario { get; set; }

        [Range(1, 60, ErrorMessage = "Carga horária deve estar entre 1 e 60 horas")]
        public int CargaHorariaSemanal { get; set; } = 20;

        [Required(ErrorMessage = "EscolaId é obrigatório")]
        public int EscolaId { get; set; }

        public StatusProfessor Status { get; set; } = StatusProfessor.Ativo;
    }

    public class UpdateProfessorDto
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nome completo é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome completo deve ter no máximo 150 caracteres")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(14, ErrorMessage = "CPF deve ter no máximo 14 caracteres")]
        public string CPF { get; set; } = string.Empty;

        [StringLength(15, ErrorMessage = "RG deve ter no máximo 15 caracteres")]
        public string? RG { get; set; }

        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        [StringLength(200, ErrorMessage = "Formação deve ter no máximo 200 caracteres")]
        public string? Formacao { get; set; }

        [StringLength(200, ErrorMessage = "Especialização deve ter no máximo 200 caracteres")]
        public string? Especializacao { get; set; }

        [Required(ErrorMessage = "Data de admissão é obrigatória")]
        public DateTime DataAdmissao { get; set; }

        public decimal? Salario { get; set; }

        [Range(1, 60, ErrorMessage = "Carga horária deve estar entre 1 e 60 horas")]
        public int CargaHorariaSemanal { get; set; }

        [Required(ErrorMessage = "EscolaId é obrigatório")]
        public int EscolaId { get; set; }

        public StatusProfessor Status { get; set; }
    }
}