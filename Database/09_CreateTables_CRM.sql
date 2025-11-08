-- =============================================
-- TABELLE MODULO CRM E FIDELIZZAZIONE
-- =============================================

USE NugoloASD;
GO

-- =============================================
-- TABELLA: Contatti (Leads/Prospect)
-- Gestione contatti e prospect
-- =============================================
CREATE TABLE Contatti (
    ContattoId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,

    Nome NVARCHAR(100) NOT NULL,
    Cognome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Telefono NVARCHAR(20),
    TelefonoCellulare NVARCHAR(20),

    DataNascita DATE,

    -- Origine
    Origine NVARCHAR(100), -- Sito Web, Evento, Passaparola, Social, etc.
    DataPrimoContatto DATETIME DEFAULT GETUTCDATE(),

    -- Interesse
    InteressePer NVARCHAR(300), -- Corso specifico, Sport, etc.

    -- Stato
    Stato NVARCHAR(50) DEFAULT 'Nuovo', -- Nuovo, Contattato, Interessato, Iscritto, Non Interessato

    -- Follow-up
    DataUltimoContatto DATETIME,
    ProssimoFollowUp DATETIME,
    AssegnatoA NVARCHAR(100), -- Username responsabile

    Note NVARCHAR(MAX),

    -- Conversione
    ConvertitoinSocio BIT DEFAULT 0,
    SocioId INT NULL,
    DataConversione DATETIME NULL,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Contatti_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Contatti_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Contatti_AssociazioneId ON Contatti(AssociazioneId);
CREATE NONCLUSTERED INDEX IX_Contatti_Stato ON Contatti(Stato);
CREATE NONCLUSTERED INDEX IX_Contatti_Email ON Contatti(Email);
GO

-- =============================================
-- TABELLA: InterazioniContatti
-- Storico interazioni con i contatti
-- =============================================
CREATE TABLE InterazioniContatti (
    InterazioneId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    ContattoId INT NOT NULL,

    TipoInterazione NVARCHAR(100), -- Chiamata, Email, Incontro, WhatsApp, SMS
    DataInterazione DATETIME DEFAULT GETUTCDATE(),

    Descrizione NVARCHAR(MAX),
    Esito NVARCHAR(100), -- Positivo, Negativo, Da ricontattare

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    UtenteInserimento NVARCHAR(100) NOT NULL,

    CONSTRAINT FK_InterazioniContatti_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_InterazioniContatti_Contatti FOREIGN KEY (ContattoId) REFERENCES Contatti(ContattoId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_InterazioniContatti_ContattoId ON InterazioniContatti(ContattoId);
GO

-- =============================================
-- TABELLA: Campagne
-- Campagne marketing
-- =============================================
CREATE TABLE Campagne (
    CampagnaId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,

    Nome NVARCHAR(200) NOT NULL,
    Descrizione NVARCHAR(500),
    TipoCampagna NVARCHAR(100), -- Email, SMS, Newsletter, Social

    DataInizio DATE,
    DataFine DATE,

    -- Target
    TargetSegmento NVARCHAR(100), -- Soci Attivi, Prospect, Ex-Soci, etc.
    FiltriJSON NVARCHAR(MAX), -- JSON con filtri per segmentazione

    -- Contenuto
    OggettoMessaggio NVARCHAR(300),
    TestoMessaggio NVARCHAR(MAX),
    HTMLMessaggio NVARCHAR(MAX),

    -- Statistiche
    NumeroDestinatari INT DEFAULT 0,
    NumeroInviati INT DEFAULT 0,
    NumeroAperture INT DEFAULT 0,
    NumeroClick INT DEFAULT 0,

    -- Stato
    Stato NVARCHAR(50) DEFAULT 'Bozza', -- Bozza, Programmata, Inviata, Completata

    DataInvio DATETIME NULL,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Campagne_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Campagne_AssociazioneId ON Campagne(AssociazioneId);
GO

-- =============================================
-- TABELLA: Sondaggi
-- Feedback e sondaggi
-- =============================================
CREATE TABLE Sondaggi (
    SondaggioId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,

    Titolo NVARCHAR(300) NOT NULL,
    Descrizione NVARCHAR(MAX),

    DataInizio DATE,
    DataFine DATE,

    -- Target
    TargetUtenti NVARCHAR(100), -- Tutti, Soci, Istruttori, etc.

    -- Struttura domande (JSON)
    DomandeJSON NVARCHAR(MAX),

    Attivo BIT DEFAULT 1,
    Anonimo BIT DEFAULT 0,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Sondaggi_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Sondaggi_AssociazioneId ON Sondaggi(AssociazioneId);
GO

-- =============================================
-- TABELLA: RisposteSondaggi
-- Risposte ai sondaggi
-- =============================================
CREATE TABLE RisposteSondaggi (
    RispostaSondaggioId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    SondaggioId INT NOT NULL,
    SocioId INT NULL, -- NULL se anonimo

    DataRisposta DATETIME DEFAULT GETUTCDATE(),

    -- Risposte (JSON)
    RisposteJSON NVARCHAR(MAX),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),

    CONSTRAINT FK_RisposteSondaggi_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_RisposteSondaggi_Sondaggi FOREIGN KEY (SondaggioId) REFERENCES Sondaggi(SondaggioId),
    CONSTRAINT FK_RisposteSondaggi_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_RisposteSondaggi_SondaggioId ON RisposteSondaggi(SondaggioId);
GO

PRINT 'CRM tables created successfully';
GO
