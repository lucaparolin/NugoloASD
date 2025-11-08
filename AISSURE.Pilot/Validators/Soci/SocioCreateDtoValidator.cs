using AISSURE.Pilot.DTOs.Soci;
using FluentValidation;

namespace AISSURE.Pilot.Validators.Soci;

/// <summary>
/// Validator per SocioCreateDto
/// </summary>
public class SocioCreateDtoValidator : AbstractValidator<SocioCreateDto>
{
    public SocioCreateDtoValidator()
    {
        // Dati anagrafici obbligatori
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Il nome è obbligatorio")
            .MaximumLength(100).WithMessage("Il nome non può superare i 100 caratteri");

        RuleFor(x => x.Cognome)
            .NotEmpty().WithMessage("Il cognome è obbligatorio")
            .MaximumLength(100).WithMessage("Il cognome non può superare i 100 caratteri");

        RuleFor(x => x.CodiceFiscale)
            .NotEmpty().WithMessage("Il codice fiscale è obbligatorio")
            .Length(16).WithMessage("Il codice fiscale deve essere di 16 caratteri")
            .Matches("^[A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[A-Z][0-9]{3}[A-Z]$")
            .WithMessage("Il codice fiscale non è in un formato valido");

        RuleFor(x => x.DataNascita)
            .NotEmpty().WithMessage("La data di nascita è obbligatoria")
            .LessThan(DateTime.Today).WithMessage("La data di nascita deve essere nel passato")
            .GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("La data di nascita non è valida");

        RuleFor(x => x.Sesso)
            .NotEmpty().WithMessage("Il sesso è obbligatorio")
            .Must(x => x == "M" || x == "F").WithMessage("Il sesso deve essere M o F");

        // Contatti
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("L'email non è in un formato valido")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Telefono)
            .MaximumLength(20).WithMessage("Il telefono non può superare i 20 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Telefono));

        RuleFor(x => x.Cellulare)
            .MaximumLength(20).WithMessage("Il cellulare non può superare i 20 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Cellulare));

        // Almeno un contatto deve essere fornito
        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Email) || !string.IsNullOrEmpty(x.Telefono) || !string.IsNullOrEmpty(x.Cellulare))
            .WithMessage("Deve essere fornito almeno un contatto (email, telefono o cellulare)")
            .WithName("Contatti");

        // Residenza
        RuleFor(x => x.CAP)
            .Matches("^[0-9]{5}$").WithMessage("Il CAP deve essere di 5 cifre")
            .When(x => !string.IsNullOrEmpty(x.CAP));

        RuleFor(x => x.Provincia)
            .Length(2).WithMessage("La provincia deve essere di 2 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Provincia));

        // Dati socio
        RuleFor(x => x.TipoSocio)
            .NotEmpty().WithMessage("Il tipo di socio è obbligatorio")
            .Must(x => new[] { "Atleta", "Istruttore", "Dirigente", "Genitore", "Staff" }.Contains(x))
            .WithMessage("Il tipo di socio non è valido");

        RuleFor(x => x.StatoSocio)
            .NotEmpty().WithMessage("Lo stato del socio è obbligatorio")
            .Must(x => new[] { "Attivo", "Sospeso", "Storico" }.Contains(x))
            .WithMessage("Lo stato del socio non è valido");

        // Validazioni per minorenne
        RuleFor(x => x.Minorenne)
            .Must((dto, minorenne) =>
            {
                if (minorenne)
                {
                    var eta = DateTime.Today.Year - dto.DataNascita.Year;
                    if (dto.DataNascita.Date > DateTime.Today.AddYears(-eta)) eta--;
                    return eta < 18;
                }
                return true;
            })
            .WithMessage("Il flag minorenne non corrisponde alla data di nascita");

        RuleFor(x => x.NomeGenitore)
            .NotEmpty().WithMessage("Il nome del genitore è obbligatorio per i minorenni")
            .When(x => x.Minorenne);

        RuleFor(x => x.CognomeGenitore)
            .NotEmpty().WithMessage("Il cognome del genitore è obbligatorio per i minorenni")
            .When(x => x.Minorenne);

        RuleFor(x => x.EmailGenitore)
            .NotEmpty().WithMessage("L'email del genitore è obbligatoria per i minorenni")
            .EmailAddress().WithMessage("L'email del genitore non è in un formato valido")
            .When(x => x.Minorenne);

        // Certificato medico
        RuleFor(x => x.DataScadenzaCertificato)
            .GreaterThan(DateTime.Today).WithMessage("Il certificato medico è scaduto")
            .When(x => x.CertificatoMedicoValido);

        // Privacy (obbligatori)
        RuleFor(x => x.ConsensoPrivacy)
            .Equal(true).WithMessage("Il consenso privacy è obbligatorio");

        // Note
        RuleFor(x => x.Note)
            .MaximumLength(1000).WithMessage("Le note non possono superare i 1000 caratteri")
            .When(x => !string.IsNullOrEmpty(x.Note));
    }
}
