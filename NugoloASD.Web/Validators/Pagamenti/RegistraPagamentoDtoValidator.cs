using NugoloASD.Web.DTOs.Pagamenti;
using FluentValidation;

namespace NugoloASD.Web.Validators.Pagamenti;

/// <summary>
/// Validator per RegistraPagamentoDto
/// </summary>
public class RegistraPagamentoDtoValidator : AbstractValidator<RegistraPagamentoDto>
{
    public RegistraPagamentoDtoValidator()
    {
        RuleFor(x => x.PagamentoId)
            .GreaterThan(0).WithMessage("L'ID del pagamento deve essere maggiore di 0");

        RuleFor(x => x.ImportoPagato)
            .GreaterThan(0).WithMessage("L'importo pagato deve essere maggiore di 0")
            .LessThanOrEqualTo(100000).WithMessage("L'importo pagato non può superare i 100000");

        RuleFor(x => x.MetodoPagamento)
            .NotEmpty().WithMessage("Il metodo di pagamento è obbligatorio")
            .Must(x => new[] { "Contanti", "Bonifico", "Carta", "PayPal", "Stripe", "Satispay", "POS" }.Contains(x))
            .WithMessage("Il metodo di pagamento non è valido");

        RuleFor(x => x.RiferimentoTransazione)
            .MaximumLength(200).WithMessage("Il riferimento transazione non può superare i 200 caratteri")
            .When(x => !string.IsNullOrEmpty(x.RiferimentoTransazione));
    }
}
