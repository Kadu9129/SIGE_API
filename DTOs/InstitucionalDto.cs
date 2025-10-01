using System.ComponentModel.DataAnnotations;
using SIGE.API.Models;

namespace SIGE.API.DTOs
{
    public class EscolaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public string? Endereco { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? CEP { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public int? DiretorId { get; set; }
        public string? NomeDiretor { get; set; }
        public StatusEscola Status { get; set; }
    }

    public class CreateEscolaDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "CNPJ é obrigatório")]
        [StringLength(18, ErrorMessage = "CNPJ deve ter no máximo 18 caracteres")]
        public string CNPJ { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Endereço deve ter no máximo 255 caracteres")]
        public string? Endereco { get; set; }

        [StringLength(100, ErrorMessage = "Cidade deve ter no máximo 100 caracteres")]
        public string? Cidade { get; set; }

        [StringLength(2, ErrorMessage = "Estado deve ter no máximo 2 caracteres")]
        public string? Estado { get; set; }

        [StringLength(9, ErrorMessage = "CEP deve ter no máximo 9 caracteres")]
        public string? CEP { get; set; }

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string? Email { get; set; }

        public int? DiretorId { get; set; }
        public StatusEscola Status { get; set; } = StatusEscola.Ativa;
    }

    public class UpdateEscolaDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "CNPJ é obrigatório")]
        [StringLength(18, ErrorMessage = "CNPJ deve ter no máximo 18 caracteres")]
        public string CNPJ { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Endereço deve ter no máximo 255 caracteres")]
        public string? Endereco { get; set; }

        [StringLength(100, ErrorMessage = "Cidade deve ter no máximo 100 caracteres")]
        public string? Cidade { get; set; }

        [StringLength(2, ErrorMessage = "Estado deve ter no máximo 2 caracteres")]
        public string? Estado { get; set; }

        [StringLength(9, ErrorMessage = "CEP deve ter no máximo 9 caracteres")]
        public string? CEP { get; set; }

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string? Email { get; set; }

        public int? DiretorId { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public StatusEscola Status { get; set; }
    }

    public class CursoDto
    {
        public int Id { get; set; }
        public int EscolaId { get; set; }
        public string NomeEscola { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int DuracaoAnos { get; set; }
        public NivelEnsino NivelEnsino { get; set; }
        public bool Status { get; set; }
    }

    public class CreateCursoDto
    {
        [Required(ErrorMessage = "EscolaId é obrigatório")]
        public int EscolaId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Código é obrigatório")]
        [StringLength(10, ErrorMessage = "Código deve ter no máximo 10 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Duração em anos é obrigatória")]
        [Range(1, 10, ErrorMessage = "Duração deve ser entre 1 e 10 anos")]
        public int DuracaoAnos { get; set; }

        [Required(ErrorMessage = "Nível de ensino é obrigatório")]
        public NivelEnsino NivelEnsino { get; set; }

        public bool Status { get; set; } = true;
    }

    public class UpdateCursoDto
    {
        [Required(ErrorMessage = "EscolaId é obrigatório")]
        public int EscolaId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Código é obrigatório")]
        [StringLength(10, ErrorMessage = "Código deve ter no máximo 10 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Duração em anos é obrigatória")]
        [Range(1, 10, ErrorMessage = "Duração deve ser entre 1 e 10 anos")]
        public int DuracaoAnos { get; set; }

        [Required(ErrorMessage = "Nível de ensino é obrigatório")]
        public NivelEnsino NivelEnsino { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public bool Status { get; set; }
    }

    public class DisciplinaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int CargaHorariaTotal { get; set; }
        public int CursoId { get; set; }
        public string NomeCurso { get; set; } = string.Empty;
        public bool Status { get; set; }
    }

    public class CreateDisciplinaDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Código é obrigatório")]
        [StringLength(10, ErrorMessage = "Código deve ter no máximo 10 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Carga horária total é obrigatória")]
        [Range(1, 9999, ErrorMessage = "Carga horária deve ser maior que 0")]
        public int CargaHorariaTotal { get; set; }

        [Required(ErrorMessage = "CursoId é obrigatório")]
        public int CursoId { get; set; }

        public bool Status { get; set; } = true;
    }

    public class UpdateDisciplinaDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Código é obrigatório")]
        [StringLength(10, ErrorMessage = "Código deve ter no máximo 10 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Carga horária total é obrigatória")]
        [Range(1, 9999, ErrorMessage = "Carga horária deve ser maior que 0")]
        public int CargaHorariaTotal { get; set; }

        [Required(ErrorMessage = "CursoId é obrigatório")]
        public int CursoId { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public bool Status { get; set; }
    }
}