using FluentValidation;
using SIGE.API.DTOs;

namespace SIGE.API.Validators
{
    public class CreateAlunoValidator : AbstractValidator<CreateAlunoDto>
    {
        public CreateAlunoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve ter um formato válido")
                .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres");

            RuleFor(x => x.Matricula)
                .NotEmpty().WithMessage("Matrícula é obrigatória")
                .MaximumLength(20).WithMessage("Matrícula deve ter no máximo 20 caracteres");

            RuleFor(x => x.NomeCompleto)
                .NotEmpty().WithMessage("Nome completo é obrigatório")
                .MinimumLength(2).WithMessage("Nome completo deve ter pelo menos 2 caracteres")
                .MaximumLength(150).WithMessage("Nome completo deve ter no máximo 150 caracteres");

            RuleFor(x => x.DataNascimento)
                .NotEmpty().WithMessage("Data de nascimento é obrigatória")
                .LessThan(DateTime.Today).WithMessage("Data de nascimento deve ser anterior a hoje")
                .GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("Data de nascimento inválida");

            RuleFor(x => x.Sexo)
                .IsInEnum().WithMessage("Sexo deve ser um valor válido");

            RuleFor(x => x.CPF)
                .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$|^\d{11}$")
                .When(x => !string.IsNullOrEmpty(x.CPF))
                .WithMessage("CPF deve ter formato válido (XXX.XXX.XXX-XX ou 11 dígitos)");

            RuleFor(x => x.CEP)
                .Matches(@"^\d{5}-?\d{3}$")
                .When(x => !string.IsNullOrEmpty(x.CEP))
                .WithMessage("CEP deve ter formato válido (XXXXX-XXX)");

            RuleFor(x => x.EmailResponsavel)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.EmailResponsavel))
                .WithMessage("Email do responsável deve ter formato válido");

            RuleFor(x => x.EscolaId)
                .GreaterThan(0).WithMessage("EscolaId deve ser um valor válido");
        }
    }

    public class UpdateAlunoValidator : AbstractValidator<UpdateAlunoDto>
    {
        public UpdateAlunoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve ter um formato válido")
                .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres");

            RuleFor(x => x.NomeCompleto)
                .NotEmpty().WithMessage("Nome completo é obrigatório")
                .MinimumLength(2).WithMessage("Nome completo deve ter pelo menos 2 caracteres")
                .MaximumLength(150).WithMessage("Nome completo deve ter no máximo 150 caracteres");

            RuleFor(x => x.DataNascimento)
                .NotEmpty().WithMessage("Data de nascimento é obrigatória")
                .LessThan(DateTime.Today).WithMessage("Data de nascimento deve ser anterior a hoje")
                .GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("Data de nascimento inválida");

            RuleFor(x => x.Sexo)
                .IsInEnum().WithMessage("Sexo deve ser um valor válido");

            RuleFor(x => x.CPF)
                .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$|^\d{11}$")
                .When(x => !string.IsNullOrEmpty(x.CPF))
                .WithMessage("CPF deve ter formato válido (XXX.XXX.XXX-XX ou 11 dígitos)");

            RuleFor(x => x.CEP)
                .Matches(@"^\d{5}-?\d{3}$")
                .When(x => !string.IsNullOrEmpty(x.CEP))
                .WithMessage("CEP deve ter formato válido (XXXXX-XXX)");

            RuleFor(x => x.EmailResponsavel)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.EmailResponsavel))
                .WithMessage("Email do responsável deve ter formato válido");

            RuleFor(x => x.EscolaId)
                .GreaterThan(0).WithMessage("EscolaId deve ser um valor válido");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status deve ser um valor válido");
        }
    }

    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve ter formato válido");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres");
        }
    }

    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.SenhaAtual)
                .NotEmpty().WithMessage("Senha atual é obrigatória");

            RuleFor(x => x.NovaSenha)
                .NotEmpty().WithMessage("Nova senha é obrigatória")
                .MinimumLength(6).WithMessage("Nova senha deve ter pelo menos 6 caracteres")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
                .WithMessage("Nova senha deve conter pelo menos uma letra minúscula, uma maiúscula e um número");

            RuleFor(x => x.ConfirmacaoSenha)
                .Equal(x => x.NovaSenha)
                .WithMessage("Confirmação de senha deve ser igual à nova senha");
        }
    }

    public class CreateUsuarioValidator : AbstractValidator<CreateUsuarioDto>
    {
        public CreateUsuarioValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MinimumLength(2).WithMessage("Nome deve ter pelo menos 2 caracteres")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve ter formato válido")
                .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres");

            RuleFor(x => x.TipoUsuario)
                .IsInEnum().WithMessage("Tipo de usuário deve ser um valor válido");

            RuleFor(x => x.Telefone)
                .Matches(@"^\(\d{2}\)\s\d{4,5}-\d{4}$")
                .When(x => !string.IsNullOrEmpty(x.Telefone))
                .WithMessage("Telefone deve ter formato válido (XX) XXXXX-XXXX");

            RuleFor(x => x.CPF)
                .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$|^\d{11}$")
                .When(x => !string.IsNullOrEmpty(x.CPF))
                .WithMessage("CPF deve ter formato válido (XXX.XXX.XXX-XX ou 11 dígitos)");
        }
    }
}