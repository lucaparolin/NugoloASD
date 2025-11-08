# NugoloASD.Web - Database Schema

## Descrizione

Schema database completo per la piattaforma SaaS multi-tenant di gestione associazioni sportive dilettantistiche.

## Struttura Database

### Script di Creazione (eseguire in ordine)

1. **01_CreateDatabase.sql** - Creazione database
2. **02_CreateTables_Core.sql** - Tabelle core (Associazioni, Utenti, Ruoli, Permessi, AuditLog)
3. **03_CreateTables_Soci.sql** - Tabelle modulo soci (Soci, Documenti, Certificati, Storico)
4. **04_CreateTables_Corsi.sql** - Tabelle modulo corsi (Corsi, Iscrizioni, Lezioni, Presenze, Valutazioni)
5. **05_CreateTables_Impianti.sql** - Tabelle modulo impianti (Impianti, Prenotazioni, Abbonamenti)
6. **06_CreateTables_Tesseramenti.sql** - Tabelle modulo tesseramenti (Federazioni, Affiliazioni, Tesseramenti)
7. **07_CreateTables_Pagamenti.sql** - Tabelle modulo economico (Pagamenti, Fatture, Contabilità)
8. **08_CreateTables_Eventi.sql** - Tabelle modulo eventi (Eventi, Iscrizioni, Risultati)
9. **09_CreateTables_CRM.sql** - Tabelle modulo CRM (Contatti, Interazioni, Campagne, Sondaggi)
10. **10_CreateTables_Comunicazioni.sql** - Tabelle modulo comunicazioni (Comunicazioni, Template, Notifiche, Scadenze)
11. **11_InsertInitialData.sql** - Dati iniziali (Ruoli, Permessi, Federazioni, Template)

## Esecuzione Scripts

### SQL Server Management Studio (SSMS)

```bash
sqlcmd -S localhost -U sa -P YourPassword123! -i 01_CreateDatabase.sql
sqlcmd -S localhost -U sa -P YourPassword123! -i 02_CreateTables_Core.sql
sqlcmd -S localhost -U sa -P YourPassword123! -i 03_CreateTables_Soci.sql
# ... continua per tutti gli script
```

### Da riga di comando (Linux/Docker)

```bash
for file in Database/*.sql; do
  sqlcmd -S localhost -U sa -P YourPassword123! -i "$file"
done
```

## Architettura Multi-Tenant

Ogni tabella include il campo `AssociazioneId` per garantire l'isolamento dei dati tra le diverse associazioni.

**Importante**: Tutte le query devono includere il filtro `WHERE AssociazioneId = @TenantId` per garantire la sicurezza dei dati.

## Tabelle Principali

### Core
- **Associazioni**: Tenant (associazioni sportive)
- **Utenti**: Utenti del sistema con credenziali
- **Ruoli**: Definizione ruoli (Amministratore, Segretario, Istruttore, Tesserato, Genitore)
- **Permessi**: Permessi granulari
- **AuditLog**: Log completo di tutte le operazioni

### Soci
- **Soci**: Anagrafica completa soci/atleti/tecnici/dirigenti
- **Documenti**: Gestione documenti (certificati, liberatorie, etc.)
- **CertificatiMedici**: Tracking specifico certificati medici
- **StoricoSoci**: Storico attività socio

### Corsi
- **Corsi**: Corsi e attività sportive
- **Iscrizioni**: Iscrizioni ai corsi
- **Lezioni**: Singole lezioni dei corsi
- **Presenze**: Registro presenze
- **ValutazioniAtleti**: Note tecniche istruttori

### Impianti
- **Impianti**: Strutture sportive
- **DisponibilitaImpianti**: Orari disponibilità
- **Prenotazioni**: Prenotazioni impianti
- **AbbonamentiImpianti**: Abbonamenti/carnet

### Tesseramenti
- **Federazioni**: Elenco federazioni sportive
- **Affiliazioni**: Affiliazioni associazioni
- **Tesseramenti**: Tesseramenti federali soci

### Pagamenti
- **Pagamenti**: Gestione pagamenti
- **Fatture**: Fatture elettroniche
- **RigheFattura**: Dettaglio righe fattura
- **MovimentiContabili**: Contabilità gestionale

### Eventi
- **Eventi**: Gare, tornei, manifestazioni
- **IscrizioniEventi**: Iscrizioni eventi
- **Risultati**: Risultati gare/tornei

### CRM
- **Contatti**: Leads e prospect
- **InterazioniContatti**: Storico interazioni
- **Campagne**: Campagne marketing
- **Sondaggi**: Feedback e sondaggi
- **RisposteSondaggi**: Risposte sondaggi

### Comunicazioni
- **Comunicazioni**: Messaggi email/SMS
- **ComunicazioniDestinatari**: Tracking destinatari
- **TemplatesComunicazioni**: Template predefiniti
- **Notifiche**: Notifiche in-app
- **ScadenzeAutomatiche**: Configurazione reminder automatici

## Campi Audit

Tutte le tabelle includono i seguenti campi di audit (pattern BaseAuditEntity):

- `DataInserimento` (DATETIME)
- `DataModifica` (DATETIME NULL)
- `UtenteInserimento` (NVARCHAR(100))
- `UtenteModifica` (NVARCHAR(100) NULL)

## Indici

Sono stati creati indici su:
- Chiavi esterne (FK)
- Campi utilizzati frequentemente nei filtri (AssociazioneId, Email, DataScadenza, etc.)
- Campi utilizzati negli ordinamenti

## Stored Procedures

Le stored procedures per le operazioni CRUD verranno create nel prossimo step dell'implementazione.

## Note di Sicurezza

1. Modificare la password di default nel connection string
2. Implementare sempre il filtro tenant nelle query
3. Utilizzare parametri SQL per prevenire SQL Injection
4. Abilitare always encrypted per dati sensibili in produzione
5. Configurare backup automatici

## Compatibilità

- SQL Server 2019+
- Azure SQL Database
- SQL Server Express (con limitazioni di dimensione)

## Manutenzione

### Backup

```sql
BACKUP DATABASE NugoloASD.Web
TO DISK = 'C:\Backups\NugoloASD.Web.bak'
WITH FORMAT;
```

### Restore

```sql
RESTORE DATABASE NugoloASD.Web
FROM DISK = 'C:\Backups\NugoloASD.Web.bak'
WITH REPLACE;
```
