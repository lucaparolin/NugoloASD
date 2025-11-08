# AISSURE Pilot

**Piattaforma SaaS Multi-Tenant per la Gestione di Associazioni Sportive Dilettantistiche**

## Descrizione

AISSURE Pilot è una web application SaaS completa per la gestione di associazioni sportive dilettantistiche (ASD). La piattaforma offre un sistema multi-tenant che permette a più associazioni di operare in modo indipendente sullo stesso sistema, con completo isolamento dei dati.

## Architettura Tecnologica

- **Framework**: ASP.NET Core 8.0
- **Linguaggio**: C#
- **Database**: SQL Server
- **Data Access**: ADO.NET (senza Entity Framework)
- **Pattern Architetturale**: MVC con principi SOLID
- **Autenticazione**: JWT (JSON Web Tokens)
- **Logging**: Serilog
- **API Documentation**: Swagger/OpenAPI

## Struttura del Progetto

```
AISSURE.Pilot/
├── API/                      # Controller API REST
├── Controllers/              # Controller MVC per le Views
├── Data/                     # Factory per connessioni database
├── Mappers/                  # Mapping tra layer (da implementare)
├── Middleware/               # Middleware custom (Tenant, etc.)
├── Models/
│   ├── Base/                # Classi base (BaseAuditEntity)
│   └── Entities/            # Entity models
├── Repositories/
│   ├── Interfaces/          # Interfacce repository
│   └── Implementations/     # Implementazioni concrete ADO.NET
├── Services/
│   ├── Interfaces/          # Interfacce servizi
│   └── Implementations/     # Implementazioni servizi
├── Utilities/               # Helper e utilities
├── Validators/              # Validatori (da implementare)
├── Views/                   # Views Razor MVC
└── wwwroot/                 # File statici

Database/
├── 01_CreateDatabase.sql
├── 02_CreateTables_Core.sql
├── 03_CreateTables_Soci.sql
├── 04_CreateTables_Corsi.sql
├── 05_CreateTables_Impianti.sql
├── 06_CreateTables_Tesseramenti.sql
├── 07_CreateTables_Pagamenti.sql
├── 08_CreateTables_Eventi.sql
├── 09_CreateTables_CRM.sql
├── 10_CreateTables_Comunicazioni.sql
└── 11_InsertInitialData.sql
```

## Moduli Principali

### 1. Core Multi-Tenant
- Gestione tenant (associazioni)
- Autenticazione e autorizzazione JWT
- Ruoli e permessi granulari
- Middleware per isolamento dati

### 2. Anagrafiche e Gestione Soci
- Registrazione soci, atleti, tecnici, dirigenti
- Gestione minori con tutori
- Upload e gestione documenti
- Certificati medici con scadenziario automatico
- Storico attività del socio

### 3. Corsi, Attività e Iscrizioni
- Creazione e gestione corsi
- Iscrizioni online e manuali
- Liste d'attesa automatiche
- Registro presenze (QR code, NFC, manuale)
- Valutazioni tecniche degli atleti

### 4. Gestione Impianti e Prenotazioni
- Mappatura strutture sportive
- Sistema di prenotazione online
- Gestione disponibilità in tempo reale
- Abbonamenti e pacchetti ore

### 5. Tesseramenti e Affiliazioni
- Gestione tesseramenti federali
- Affiliazioni annuali ASD
- Integrazione con federazioni (CONI, FIP, FIGC, etc.)
- Esportazione dati per enti

### 6. Gestione Economica
- Emissione fatture elettroniche (XML SDI)
- Gestione pagamenti multipli (Stripe, PayPal, bonifico, POS)
- Pagamenti rateali e abbonamenti ricorrenti
- Contabilità gestionale con centri di costo

### 7. Comunicazioni e Notifiche
- Email, SMS, notifiche push
- Comunicazioni automatiche per scadenze
- Template personalizzabili
- Newsletter segmentate

### 8. Eventi, Gare e Tornei
- Creazione eventi con iscrizione online
- Gestione quote, categorie e premi
- Gestione classifiche e tabelloni

### 9. CRM e Fidelizzazione
- Gestione contatti e prospect
- Storico interazioni
- Campagne marketing mirate
- Sondaggi e feedback

### 10. Reportistica e Dashboard
- Dashboard con KPI personalizzabili
- Report esportabili (PDF, Excel, CSV)
- Analisi presenze, incassi, utilizzo impianti

## Installazione e Setup

### Prerequisiti

- .NET SDK 8.0 o superiore
- SQL Server 2019+ (o Azure SQL Database)
- Editor di codice (Visual Studio 2022, VS Code, Rider)

### 1. Clonare il Repository

```bash
git clone https://github.com/lucaparolin/NugoloASD.git
cd NugoloASD
```

### 2. Configurare il Database

Eseguire in ordine gli script SQL nella cartella `Database/`:

```bash
# Con sqlcmd (Windows/Linux)
sqlcmd -S localhost -U sa -P YourPassword -i Database/01_CreateDatabase.sql
sqlcmd -S localhost -U sa -P YourPassword -i Database/02_CreateTables_Core.sql
# ... continuare con tutti gli script
```

Oppure da SQL Server Management Studio (SSMS):
- Aprire e eseguire ogni script in ordine sequenziale

### 3. Configurare la Connection String

Modificare `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=AISSURE_Pilot;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "CHANGE_THIS_TO_A_SECURE_SECRET_KEY_AT_LEAST_32_CHARS",
    "Issuer": "AISSURE.Pilot",
    "Audience": "AISSURE.Pilot.Users",
    "ExpirationInMinutes": 1440
  }
}
```

### 4. Avviare l'Applicazione

```bash
cd AISSURE.Pilot
dotnet restore
dotnet build
dotnet run
```

L'applicazione sarà disponibile su:
- **HTTPS**: https://localhost:7000
- **HTTP**: http://localhost:5000
- **Swagger UI**: https://localhost:7000/swagger

## Uso delle API

### Autenticazione

#### Registrazione Utente

```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "mario.rossi",
  "email": "mario.rossi@example.com",
  "password": "SecurePassword123!",
  "nome": "Mario",
  "cognome": "Rossi",
  "associazioneId": 1
}
```

#### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "usernameOrEmail": "mario.rossi",
  "password": "SecurePassword123!"
}
```

Risposta:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "utenteId": 1,
    "username": "mario.rossi",
    "email": "mario.rossi@example.com",
    "nome": "Mario",
    "cognome": "Rossi",
    "associazioneId": 1
  }
}
```

### Utilizzo del Token JWT

Per tutte le chiamate API autenticate, includere l'header:

```http
Authorization: Bearer YOUR_JWT_TOKEN
```

### Esempio: Gestione Soci

```http
GET /api/soci
Authorization: Bearer YOUR_JWT_TOKEN
X-Tenant-Id: 1
```

## Sistema Multi-Tenant

Il sistema supporta l'isolamento dei dati tra tenant tramite:

1. **Header HTTP**: `X-Tenant-Id: 1`
2. **JWT Claim**: Il token contiene il TenantId dell'associazione
3. **Sottodominio**: (futuro) `associazione1.aissure.com`

Tutte le query al database includono automaticamente il filtro `WHERE AssociazioneId = @TenantId`.

## Sicurezza

### Password Hashing
- Algoritmo: BCrypt con salt
- Salt generato randomicamente per ogni utente

### JWT
- Algoritmo: HS256 (HMAC-SHA256)
- Scadenza: 1440 minuti (24 ore) - configurabile
- Claims inclusi: UserId, Username, Email, TenantId, Ruoli

### Protezione Account
- Blocco account dopo 5 tentativi di login falliti
- Reset password via email
- Two-Factor Authentication (2FA) - da implementare

### GDPR Compliance
- Consensi espliciti e revocabili
- Tracciamento accessi tramite AuditLog
- Possibilità di esportazione dati utente
- Right to be forgotten (cancellazione dati)

## Database Schema

Il database include 40+ tabelle organizzate in moduli:

- **Core**: Associazioni, Utenti, Ruoli, Permessi, AuditLog
- **Soci**: Soci, Documenti, CertificatiMedici, StoricoSoci
- **Corsi**: Corsi, Iscrizioni, Lezioni, Presenze, ValutazioniAtleti
- **Impianti**: Impianti, DisponibilitaImpianti, Prenotazioni, AbbonamentiImpianti
- **Tesseramenti**: Federazioni, Affiliazioni, Tesseramenti
- **Economia**: Pagamenti, Fatture, RigheFattura, MovimentiContabili
- **Eventi**: Eventi, IscrizioniEventi, Risultati
- **CRM**: Contatti, InterazioniContatti, Campagne, Sondaggi
- **Comunicazioni**: Comunicazioni, ComunicazioniDestinatari, TemplatesComunicazioni, Notifiche

Vedi `Database/README.md` per documentazione completa.

## Roadmap

### Fase 1: Foundation (Completata)
- [x] Struttura progetto ASP.NET Core 8.0
- [x] Schema database completo
- [x] Sistema multi-tenant
- [x] Autenticazione JWT
- [x] Repository pattern con ADO.NET
- [x] API base per autenticazione

### Fase 2: Core Features (In Corso)
- [ ] Completare implementazione repository ADO.NET
- [ ] Views MVC per dashboard
- [ ] CRUD completo per modulo Soci
- [ ] CRUD completo per modulo Corsi
- [ ] Sistema di upload documenti

### Fase 3: Advanced Features
- [ ] Integrazione pagamenti (Stripe, PayPal, Satispay)
- [ ] Fatturazione elettronica XML SDI
- [ ] Sistema comunicazioni (Email/SMS)
- [ ] Area riservata soci
- [ ] Area istruttori

### Fase 4: Enhancement
- [ ] Reportistica avanzata con grafici
- [ ] Dashboard personalizzabili
- [ ] App mobile (React Native o Flutter)
- [ ] Integrazione federazioni API
- [ ] Sistema di booking impianti con calendario

## Contributi

Questo è un progetto privato. Per informazioni sui contributi, contattare il proprietario del repository.

## Licenza

Proprietario: Luca Parolin
Tutti i diritti riservati.

## Supporto

Per domande o supporto, contattare: [email protected]

## Tecnologie Utilizzate

- ASP.NET Core 8.0
- C# 12
- SQL Server
- ADO.NET
- JWT Authentication
- Swagger/OpenAPI
- Serilog
- BCrypt.Net
- Bootstrap (per UI - da implementare)

## Note per lo Sviluppo

### Principi SOLID Applicati

- **Single Responsibility**: Ogni classe ha una sola responsabilità
- **Open/Closed**: Estensibile senza modificare il codice esistente
- **Liskov Substitution**: Le implementazioni sono intercambiabili
- **Interface Segregation**: Interfacce specifiche per ogni caso d'uso
- **Dependency Inversion**: Dipendenze su astrazioni, non implementazioni concrete

### Pattern Utilizzati

- **Repository Pattern**: Astrazione dell'accesso ai dati
- **Service Layer Pattern**: Logica business separata dai controller
- **Dependency Injection**: Gestione dipendenze tramite DI container
- **Factory Pattern**: IDbConnectionFactory per le connessioni
- **Middleware Pattern**: Gestione cross-cutting concerns

### Best Practices

- Tutti i campi di audit (DataInserimento, UtenteInserimento, etc.)
- Soft delete per preservare lo storico
- Validazione input sia client che server-side
- Logging strutturato con Serilog
- Gestione errori centralizzata
- Separazione layer (API, Service, Repository, Data)

---

**Versione**: 1.0.0
**Data**: Novembre 2025
**Autore**: Luca Parolin
