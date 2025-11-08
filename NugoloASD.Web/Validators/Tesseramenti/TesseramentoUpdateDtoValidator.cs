using NugoloASD.Web.DTOs.Tesseramenti;
using FluentValidation;

namespace NugoloASD.Web.Validators.Tesseramenti;

/// <summary>
/// Validator per TesseramentoUpdateDto
/// </summary>
public class TesseramentoUpdateDtoValidator : AbstractValidator<TesseramentoUpdateDto>
{
    public TesseramentoUpdateDtoValidator()
    {
        RuleFor(x => x.TesseramentoId)
            .GreaterThan(0).WithMessage("L'ID del tesseramento deve essere maggiore di 0");

        RuleFor(x => x.SocioId)
            .GreaterThan(0).WithMessage("Il socio è obbligatorio");

        RuleFor(x => x.FederazioneId)
            .GreaterThan(0).WithMessage("La federazione è obbligatoria");

        RuleFor(x => x.NumeroTessera)
            .NotEmpty().WithMessage("Il numero tessera è obbligatorio")
            .MaximumLength(100).WithMessage("Il numero tessera non può superare i 100 caratteri");

        RuleFor(x => x.AnnoSportivo)
            .NotEmpty().WithMessage("L'anno sportivo è obbligatorio")
            .Matches(@"^\d{4}/\d{4}$").WithMessage("L'anno sportivo deve essere nel formato YYYY/YYYY (es. 2024/2025)");

        RuleFor(x => x.TipoTessera)
            .Must(x => new[] { "Atleta", "Tecnico", "Dirigente", "Arbitro" }.Contains(x))
            .WithMessage("Il tipo tessera non è valido")
            .When(x => !string.IsNullOrEmpty(x.TipoTessera));

        RuleFor(x => x.Categoria)
            .MaximumLength(100).WithMessage("La categoria non può superare i 100 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Categoria));

        RuleFor(x => x.Qualifica)
            .MaximumLength(100).WithMessage("La qualifica non può superare i 100 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Qualifica));

        RuleFor(x => x.DataEmissione)
            .NotEmpty().WithMessage("La data di emissione è obbligatoria")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("La data di emissione non può essere nel futuro");

        RuleFor(x => x.DataScadenza)
            .NotEmpty().WithMessage("La data di scadenza è obbligatoria")
            .GreaterThan(x => x.DataEmissione).WithMessage("La data di scadenza deve essere successiva alla data di emissione");

        RuleFor(x => x.Importo)
            .GreaterThanOrEqualTo(0).WithMessage("L'importo deve essere maggiore o uguale a 0")
            .LessThanOrEqualTo(10000).WithMessage("L'importo non può superare i 10000")
            .When(x => x.Importo.HasValue);

        RuleFor(x => x.DataPagamento)
            .NotEmpty().WithMessage("La data di pagamento è obbligatoria quando il tesseramento è pagato")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("La data di pagamento non può essere nel futuro")
            .When(x => x.Pagato);

        RuleFor(x => x.StatoTesseramento)
            .NotEmpty().WithMessage("Lo stato del tesseramento è obbligatorio")
            .Must(x => new[] { "Attivo", "Sospeso", "Scaduto", "Annullato" }.Contains(x))
            .WithMessage("Lo stato del tesseramento non è valido");

        RuleFor(x => x.DocumentoTesseraUrl)
            .MaximumLength(500).WithMessage("L'URL del documento non può superare i 500 caratteri")
            .When(x => !string.IsNullOrEmpty(x.DocumentoTesseraUrl));

        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Le note non possono superare i 500 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Note));
    }
}
