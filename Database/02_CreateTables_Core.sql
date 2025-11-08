-- =============================================
-- TABELLE CORE - Multi-Tenant e Autenticazione
-- =============================================

USE NugoloASD;
GO

-- =============================================
-- TABELLA: Associazioni (Tenant)
-- Gestione multi-tenant
-- =============================================
CREATE TABLE Associazioni (
    AssociazioneId INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(200) NOT NULL,
    RagioneSociale NVARCHAR(300) NOT NULL,
    PartitaIVA NVARCHAR(20),
    CodiceFiscale NVARCHAR(16) NOT NULL,

    -- Indirizzo
    Indirizzo NVARCHAR(300),
    Citta NVARCHAR(100),
    CAP NVARCHAR(10),
    Provincia NVARCHAR(2),
    Regione NVARCHAR(50),
    Nazione NVARCHAR(50) DEFAULT 'Italia',

    -- Contatti
    Telefono NVARCHAR(20),
    Email NVARCHAR(100) NOT NULL,
    PEC NVARCHAR(100),
    SitoWeb NVARCHAR(200),

    -- Branding
    Logo NVARCHAR(500), -- URL o path al logo
    ColoriPrimari NVARCHAR(50), -- JSON con colori hex
    DominioPersonalizzato NVARCHAR(100),

    -- Configurazione
    TipoSport NVARCHAR(100),
    NumeroMassimoSoci INT DEFAULT 1000,
    DataScadenzaAbbonamento DATE,
    PianoAbbonamento NVARCHAR(50) DEFAULT 'Basic', -- Basic, Professional, Enterprise

    -- Stato
    Attivo BIT DEFAULT 1,
    DataRegistrazione DATE DEFAULT GETDATE(),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT UK_Associazioni_Email UNIQUE (Email),
    CONSTRAINT UK_Associazioni_CodiceFiscale UNIQUE (CodiceFiscale)
);
GO

-- =============================================
-- TABELLA: Ruoli
-- Definizione dei ruoli utente
-- =============================================
CREATE TABLE Ruoli (
    RuoloId INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(50) NOT NULL,
    Descrizione NVARCHAR(200),
    Livello INT NOT NULL, -- 1=Amministratore, 2=Segretario, 3=Istruttore, 4=Tesserato, 5=Genitore

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT UK_Ruoli_Nome UNIQUE (Nome)
);
GO

-- =============================================
-- TABELLA: Utenti
-- Gestione utenti del sistema
-- =============================================
CREATE TABLE Utenti (
    UtenteId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,

    -- Credenziali
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,
    Salt NVARCHAR(200) NOT NULL,

    -- Dati personali
    Nome NVARCHAR(100) NOT NULL,
    Cognome NVARCHAR(100) NOT NULL,
    CodiceFiscale NVARCHAR(16),
    DataNascita DATE,
    LuogoNascita NVARCHAR(100),

    -- Contatti
    Telefono NVARCHAR(20),
    TelefonoCellulare NVARCHAR(20),

    -- Sicurezza
    TwoFactorEnabled BIT DEFAULT 0,
    TwoFactorSecret NVARCHAR(200),
    UltimoAccesso DATETIME,
    TentativiAccessoFalliti INT DEFAULT 0,
    AccountBloccato BIT DEFAULT 0,
    DataBlocco DATETIME NULL,

    -- Stato
    Attivo BIT DEFAULT 1,
    EmailConfermata BIT DEFAULT 0,
    TokenConfermaEmail NVARCHAR(200),
    TokenResetPassword NVARCHAR(200),
    DataScadenzaTokenReset DATETIME NULL,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Utenti_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT UK_Utenti_Username UNIQUE (Username),
    CONSTRAINT UK_Utenti_Email UNIQUE (Email)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Utenti_AssociazioneId ON Utenti(AssociazioneId);
CREATE NONCLUSTERED INDEX IX_Utenti_Email ON Utenti(Email);
GO

-- =============================================
-- TABELLA: UtentiRuoli (Many-to-Many)
-- Associazione utenti e ruoli
-- =============================================
CREATE TABLE UtentiRuoli (
    UtenteId INT NOT NULL,
    RuoloId INT NOT NULL,
    AssociazioneId INT NOT NULL,

    DataAssegnazione DATETIME DEFAULT GETUTCDATE(),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    UtenteInserimento NVARCHAR(100) NOT NULL,

    PRIMARY KEY (UtenteId, RuoloId, AssociazioneId),
    CONSTRAINT FK_UtentiRuoli_Utenti FOREIGN KEY (UtenteId) REFERENCES Utenti(UtenteId),
    CONSTRAINT FK_UtentiRuoli_Ruoli FOREIGN KEY (RuoloId) REFERENCES Ruoli(RuoloId),
    CONSTRAINT FK_UtentiRuoli_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId)
);
GO

-- =============================================
-- TABELLA: Permessi
-- Gestione granulare dei permessi
-- =============================================
CREATE TABLE Permessi (
    PermessoId INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Descrizione NVARCHAR(300),
    Risorsa NVARCHAR(100) NOT NULL, -- Es: Soci, Corsi, Pagamenti
    Azione NVARCHAR(50) NOT NULL, -- Es: Lettura, Scrittura, Modifica, Eliminazione

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT UK_Permessi_NomeRisorsaAzione UNIQUE (Nome, Risorsa, Azione)
);
GO

-- =============================================
-- TABELLA: RuoliPermessi (Many-to-Many)
-- Associazione ruoli e permessi
-- =============================================
CREATE TABLE RuoliPermessi (
    RuoloId INT NOT NULL,
    PermessoId INT NOT NULL,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    UtenteInserimento NVARCHAR(100) NOT NULL,

    PRIMARY KEY (RuoloId, PermessoId),
    CONSTRAINT FK_RuoliPermessi_Ruoli FOREIGN KEY (RuoloId) REFERENCES Ruoli(RuoloId),
    CONSTRAINT FK_RuoliPermessi_Permessi FOREIGN KEY (PermessoId) REFERENCES Permessi(PermessoId)
);
GO

-- =============================================
-- TABELLA: AuditLog
-- Log di tutte le operazioni critiche
-- =============================================
CREATE TABLE AuditLog (
    AuditLogId BIGINT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    UtenteId INT NULL,

    Azione NVARCHAR(100) NOT NULL, -- INSERT, UPDATE, DELETE, LOGIN, LOGOUT, etc.
    Entita NVARCHAR(100) NOT NULL, -- Nome della tabella/entity
    EntitaId INT NULL,
    ValoriPrecedenti NVARCHAR(MAX), -- JSON
    NuoviValori NVARCHAR(MAX), -- JSON

    IndirizzoIP NVARCHAR(50),
    UserAgent NVARCHAR(500),

    DataOperazione DATETIME DEFAULT GETUTCDATE(),

    CONSTRAINT FK_AuditLog_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_AuditLog_Utenti FOREIGN KEY (UtenteId) REFERENCES Utenti(UtenteId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_AuditLog_AssociazioneId ON AuditLog(AssociazioneId);
CREATE NONCLUSTERED INDEX IX_AuditLog_DataOperazione ON AuditLog(DataOperazione);
GO

PRINT 'Core tables created successfully';
GO
