-- =============================================
-- TABELLE MODULO COMUNICAZIONI E NOTIFICHE
-- =============================================

USE NugoloASD;
GO

-- =============================================
-- TABELLA: Comunicazioni
-- Messaggi e notifiche
-- =============================================
CREATE TABLE Comunicazioni (
    ComunicazioneId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,

    TipoComunicazione NVARCHAR(50) NOT NULL, -- Email, SMS, Notifica Push, Comunicazione Interna

    Oggetto NVARCHAR(300),
    Messaggio NVARCHAR(MAX) NOT NULL,
    MessaggioHTML NVARCHAR(MAX),

    -- Mittente
    MittenteId INT NULL, -- FK a Utenti

    -- Destinatari
    TipoDestinatari NVARCHAR(100), -- Singolo, Gruppo, Tutti, Corso, Ruolo
    DestinatariJSON NVARCHAR(MAX), -- JSON array con ID destinatari

    -- Allegati
    AllegatiJSON NVARCHAR(MAX), -- JSON array con URL allegati

    -- Programmazione
    Programmata BIT DEFAULT 0,
    DataProgrammazione DATETIME NULL,

    -- Stato
    Inviata BIT DEFAULT 0,
    DataInvio DATETIME NULL,
    NumeroDestinatari INT DEFAULT 0,
    NumeroInviatiSuccesso INT DEFAULT 0,
    NumeroInviatiFalliti INT DEFAULT 0,

    -- Priorità
    Priorita NVARCHAR(20) DEFAULT 'Normale', -- Bassa, Normale, Alta, Urgente

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Comunicazioni_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Comunicazioni_Mittente FOREIGN KEY (MittenteId) REFERENCES Utenti(UtenteId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Comunicazioni_AssociazioneId ON Comunicazioni(AssociazioneId);
CREATE NONCLUSTERED INDEX IX_Comunicazioni_DataInvio ON Comunicazioni(DataInvio);
GO

-- =============================================
-- TABELLA: ComunicazioniDestinatari
-- Tracking destinatari comunicazioni
-- =============================================
CREATE TABLE ComunicazioniDestinatari (
    ComunicazioneDestinatarioId INT IDENTITY(1,1) PRIMARY KEY,
    ComunicazioneId INT NOT NULL,
    DestinatarioId INT NOT NULL, -- FK a Soci o Utenti

    TipoDestinatario NVARCHAR(50), -- Socio, Utente

    -- Stato invio
    Inviato BIT DEFAULT 0,
    DataInvio DATETIME NULL,
    ErroreInvio NVARCHAR(500),

    -- Tracking
    Aperto BIT DEFAULT 0,
    DataApertura DATETIME NULL,
    NumeroAperture INT DEFAULT 0,

    Cliccato BIT DEFAULT 0,
    DataClick DATETIME NULL,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),

    CONSTRAINT FK_ComunicazioniDestinatari_Comunicazioni FOREIGN KEY (ComunicazioneId) REFERENCES Comunicazioni(ComunicazioneId) ON DELETE CASCADE
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_ComunicazioniDestinatari_ComunicazioneId ON ComunicazioniDestinatari(ComunicazioneId);
CREATE NONCLUSTERED INDEX IX_ComunicazioniDestinatari_DestinatarioId ON ComunicazioniDestinatari(DestinatarioId);
GO

-- =============================================
-- TABELLA: TemplatesComunicazioni
-- Template predefiniti per comunicazioni
-- =============================================
CREATE TABLE TemplatesComunicazioni (
    TemplateId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NULL, -- NULL = template di sistema

    Nome NVARCHAR(200) NOT NULL,
    Descrizione NVARCHAR(500),
    TipoComunicazione NVARCHAR(50), -- Email, SMS

    Categoria NVARCHAR(100), -- Iscrizione, Pagamento, Scadenza, Promemoria, etc.

    Oggetto NVARCHAR(300),
    CorpoMessaggio NVARCHAR(MAX),
    CorpoMessaggioHTML NVARCHAR(MAX),

    -- Placeholders disponibili (JSON)
    PlaceholdersJSON NVARCHAR(MAX),

    Attivo BIT DEFAULT 1,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_TemplatesComunicazioni_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_TemplatesComunicazioni_AssociazioneId ON TemplatesComunicazioni(AssociazioneId);
GO

-- =============================================
-- TABELLA: Notifiche
-- Notifiche in-app
-- =============================================
CREATE TABLE Notifiche (
    NotificaId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    UtenteId INT NOT NULL,

    Titolo NVARCHAR(200) NOT NULL,
    Messaggio NVARCHAR(500) NOT NULL,

    TipoNotifica NVARCHAR(100), -- Info, Warning, Success, Error
    Categoria NVARCHAR(100), -- Pagamento, Scadenza, Corso, Evento, Sistema

    -- Link azione
    LinkAzione NVARCHAR(500),

    -- Stato
    Letta BIT DEFAULT 0,
    DataLettura DATETIME NULL,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Notifiche_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Notifiche_Utenti FOREIGN KEY (UtenteId) REFERENCES Utenti(UtenteId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Notifiche_UtenteId ON Notifiche(UtenteId);
CREATE NONCLUSTERED INDEX IX_Notifiche_Letta ON Notifiche(Letta);
GO

-- =============================================
-- TABELLA: ScadenzeAutomatiche
-- Configurazione scadenziari e reminder automatici
-- =============================================
CREATE TABLE ScadenzeAutomatiche (
    ScadenzaAutomaticaId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,

    TipoScadenza NVARCHAR(100) NOT NULL, -- Certificato Medico, Tesseramento, Pagamento, Documento

    GiorniPrimaScadenza INT DEFAULT 30, -- Giorni prima per inviare reminder
    NumeroReminderTotali INT DEFAULT 3,
    IntervalloGiorniReminderSuccessivi INT DEFAULT 7,

    TemplateId INT NULL,

    Attivo BIT DEFAULT 1,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_ScadenzeAutomatiche_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_ScadenzeAutomatiche_Templates FOREIGN KEY (TemplateId) REFERENCES TemplatesComunicazioni(TemplateId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_ScadenzeAutomatiche_AssociazioneId ON ScadenzeAutomatiche(AssociazioneId);
GO

PRINT 'Comunicazioni tables created successfully';
GO
