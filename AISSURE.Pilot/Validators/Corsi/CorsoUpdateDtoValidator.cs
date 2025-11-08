using AISSURE.Pilot.DTOs.Corsi;
using FluentValidation;

namespace AISSURE.Pilot.Validators.Corsi;

/// <summary>
/// Validator per CorsoUpdateDto
/// </summary>
public class CorsoUpdateDtoValidator : AbstractValidator<CorsoUpdateDto>
{
    public CorsoUpdateDtoValidator()
    {
        RuleFor(x => x.CorsoId)
            .GreaterThan(0).WithMessage("L'ID del corso deve essere maggiore di 0");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Il nome del corso è obbligatorio")
            .MaximumLength(200).WithMessage("Il nome non può superare i 200 caratteri");

        RuleFor(x => x.Descrizione)
            .MaximumLength(2000).WithMessage("La descrizione non può superare i 2000 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Descrizione));

        RuleFor(x => x.Categoria)
            .MaximumLength(100).WithMessage("La categoria non può superare i 100 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Categoria));

        RuleFor(x => x.Livello)
            .MaximumLength(50).WithMessage("Il livello non può superare i 50 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Livello));

        // Periodo
        RuleFor(x => x.DataInizio)
            .NotEmpty().WithMessage("La data di inizio è obbligatoria");

        RuleFor(x => x.DataFine)
            .NotEmpty().WithMessage("La data di fine è obbligatoria")
            .GreaterThan(x => x.DataInizio).WithMessage("La data di fine deve essere successiva alla data di inizio");

        RuleFor(x => x.AnnoSportivo)
            .Matches(@"^\d{4}/\d{4}$").WithMessage("L'anno sportivo deve essere nel formato YYYY/YYYY (es. 2024/2025)")
            .When(x => !string.IsNullOrEmpty(x.AnnoSportivo));

        // Orari
        RuleFor(x => x.OrarioFine)
            .GreaterThan(x => x.OrarioInizio).WithMessage("L'orario di fine deve essere successivo all'orario di inizio")
            .When(x => x.OrarioInizio.HasValue && x.OrarioFine.HasValue);

        RuleFor(x => x.GiorniSettimana)
            .MaximumLength(200).WithMessage("I giorni della settimana non possono superare i 200 caratteri")
            .When(x => !string.IsNullOrEmpty(x.GiorniSettimana));

        // Capacità
        RuleFor(x => x.PostiDisponibili)
            .GreaterThan(0).WithMessage("I posti disponibili devono essere maggiori di 0")
            .LessThanOrEqualTo(1000).WithMessage("I posti disponibili non possono superare i 1000")
            .When(x => x.PostiDisponibili.HasValue);

        // Prezzi
        RuleFor(x => x.PrezzoPieno)
            .GreaterThanOrEqualTo(0).WithMessage("Il prezzo pieno deve essere maggiore o uguale a 0")
            .LessThanOrEqualTo(10000).WithMessage("Il prezzo pieno non può superare i 10000")
            .When(x => x.PrezzoPieno.HasValue);

        RuleFor(x => x.PrezzoRidotto)
            .GreaterThanOrEqualTo(0).WithMessage("Il prezzo ridotto deve essere maggiore o uguale a 0")
            .LessThan(x => x.PrezzoPieno).WithMessage("Il prezzo ridotto deve essere inferiore al prezzo pieno")
            .When(x => x.PrezzoRidotto.HasValue && x.PrezzoPieno.HasValue);

        RuleFor(x => x.DescrizioneRiduzione)
            .NotEmpty().WithMessage("La descrizione della riduzione è obbligatoria quando è presente un prezzo ridotto")
            .MaximumLength(500).WithMessage("La descrizione della riduzione non può superare i 500 caratteri")
            .When(x => x.PrezzoRidotto.HasValue);
    }
}
