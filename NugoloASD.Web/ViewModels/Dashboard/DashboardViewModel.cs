namespace NugoloASD.Web.ViewModels.Dashboard;

/// <summary>
/// ViewModel per la dashboard principale
/// </summary>
public class DashboardViewModel
{
    public KpiData Kpi { get; set; } = new();
    public List<AttivitaRecente> AttivitaRecenti { get; set; } = new();
    public List<ScadenzaImminente> ScadenzeImminenti { get; set; } = new();
    public GraficoData GraficoIscrizioni { get; set; } = new();
    public GraficoData GraficoIncassi { get; set; } = new();
}

/// <summary>
/// KPI principali
/// </summary>
public class KpiData
{
    public int SociAttivi { get; set; }
    public int CorsiAttivi { get; set; }
    public decimal IncassiMese { get; set; }
    public int TesseramentiInScadenza { get; set; }
    public decimal PercentualeVariazioneSoci { get; set; }
    public decimal PercentualeVariazioneIncassi { get; set; }
}

/// <summary>
/// Attività recente nel sistema
/// </summary>
public class AttivitaRecente
{
    public string Tipo { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public string Icona { get; set; } = string.Empty;
    public string ColorClass { get; set; } = string.Empty;
    public DateTime Data { get; set; }
}

/// <summary>
/// Scadenze imminenti (certificati, tesseramenti, ecc.)
/// </summary>
public class ScadenzaImminente
{
    public string Tipo { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public DateTime DataScadenza { get; set; }
    public int GiorniMancanti { get; set; }
    public string PrioritaClass { get; set; } = string.Empty;
}

/// <summary>
/// Dati per i grafici
/// </summary>
public class GraficoData
{
    public List<string> Labels { get; set; } = new();
    public List<decimal> Values { get; set; } = new();
}
