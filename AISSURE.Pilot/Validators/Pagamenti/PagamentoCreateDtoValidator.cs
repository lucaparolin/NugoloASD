using AISSURE.Pilot.DTOs.Pagamenti;
using FluentValidation;

namespace AISSURE.Pilot.Validators.Pagamenti;

/// <summary>
/// Validator per PagamentoCreateDto
/// </summary>
public class PagamentoCreateDtoValidator : AbstractValidator<PagamentoCreateDto>
{
    public PagamentoCreateDtoValidator()
    {
        RuleFor(x => x.SocioId)
            .GreaterThan(0).WithMessage("Il socio è obbligatorio");

        RuleFor(x => x.Causale)
            .NotEmpty().WithMessage("La causale è obbligatoria")
            .MaximumLength(300).WithMessage("La causale non può superare i 300 caratteri");

        RuleFor(x => x.Descrizione)
            .MaximumLength(500).WithMessage("La descrizione non può superare i 500 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Descrizione));

        RuleFor(x => x.TipoPagamento)
            .Must(x => new[] { "Iscrizione Corso", "Tesseramento", "Abbonamento", "Prenotazione", "Quota Sociale", "Altro" }.Contains(x))
            .WithMessage("Il tipo di pagamento non è valido")
            .When(x => !string.IsNullOrEmpty(x.TipoPagamento));

        RuleFor(x => x.Importo)
            .GreaterThan(0).WithMessage("L'importo deve essere maggiore di 0")
            .LessThanOrEqualTo(100000).WithMessage("L'importo non può superare i 100000");

        RuleFor(x => x.DataScadenza)
            .GreaterThanOrEqualTo(DateTime.Today.AddDays(-30)).WithMessage("La data di scadenza non può essere più di 30 giorni nel passato")
            .When(x => x.DataScadenza.HasValue);

        // Validazioni per pagamenti rateali
        RuleFor(x => x.NumeroRata)
            .GreaterThan(0).WithMessage("Il numero rata deve essere maggiore di 0")
            .LessThanOrEqualTo(x => x.TotaleRate).WithMessage("Il numero rata non può essere maggiore del totale rate")
            .When(x => x.PagamentoRateale && x.NumeroRata.HasValue);

        RuleFor(x => x.TotaleRate)
            .GreaterThan(1).WithMessage("Il totale rate deve essere maggiore di 1")
            .LessThanOrEqualTo(60).WithMessage("Il totale rate non può superare i 60")
            .When(x => x.PagamentoRateale && x.TotaleRate.HasValue);

        RuleFor(x => x)
            .Must(x => x.NumeroRata.HasValue && x.TotaleRate.HasValue)
            .WithMessage("Per i pagamenti rateali, numero rata e totale rate sono obbligatori")
            .When(x => x.PagamentoRateale);

        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Le note non possono superare i 500 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Note));
    }
}
