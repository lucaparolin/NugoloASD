-- =============================================
-- TABELLE MODULO IMPIANTI E PRENOTAZIONI
-- =============================================

USE AISSURE_Pilot;
GO

-- =============================================
-- TABELLA: Impianti
-- Gestione strutture sportive
-- =============================================
CREATE TABLE Impianti (
    ImpiantoId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,

    Nome NVARCHAR(200) NOT NULL,
    Descrizione NVARCHAR(MAX),
    TipoImpianto NVARCHAR(100), -- Campo Calcio, Campo Tennis, Piscina, Palestra, Sala

    -- Indirizzo
    Indirizzo NVARCHAR(300),
    Citta NVARCHAR(100),
    CAP NVARCHAR(10),

    -- Caratteristiche
    Capienza INT,
    Superficie DECIMAL(10,2),
    UnitaMisura NVARCHAR(10), -- mq, ettari
    CopertoScoperto NVARCHAR(20), -- Coperto, Scoperto, Misto

    -- Prenotabilità
    Prenotabile BIT DEFAULT 1,
    PrenotabileOnline BIT DEFAULT 1,
    TempoMinimoPrenotazione INT DEFAULT 60, -- Minuti
    AnticipoPrevistaGiorni INT DEFAULT 0,
    MassimoCancellazioneOre INT DEFAULT 24,

    -- Prezzi
    TariffaOraria DECIMAL(10,2),
    TariffaGiornaliera DECIMAL(10,2),

    -- Immagini
    ImmagineUrl NVARCHAR(500),

    -- Stato
    Attivo BIT DEFAULT 1,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Impianti_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Impianti_AssociazioneId ON Impianti(AssociazioneId);
GO

-- =============================================
-- TABELLA: DisponibilitaImpianti
-- Orari di disponibilità impianti
-- =============================================
CREATE TABLE DisponibilitaImpianti (
    DisponibilitaId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    ImpiantoId INT NOT NULL,

    GiornoSettimana INT NOT NULL, -- 1=Lunedì, 7=Domenica
    OrarioInizio TIME NOT NULL,
    OrarioFine TIME NOT NULL,

    Disponibile BIT DEFAULT 1,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_DisponibilitaImpianti_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_DisponibilitaImpianti_Impianti FOREIGN KEY (ImpiantoId) REFERENCES Impianti(ImpiantoId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_DisponibilitaImpianti_ImpiantoId ON DisponibilitaImpianti(ImpiantoId);
GO

-- =============================================
-- TABELLA: Prenotazioni
-- Prenotazioni impianti
-- =============================================
CREATE TABLE Prenotazioni (
    PrenotazioneId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    ImpiantoId INT NOT NULL,
    SocioId INT NULL, -- NULL se prenotazione esterna

    DataPrenotazione DATE NOT NULL,
    OrarioInizio TIME NOT NULL,
    OrarioFine TIME NOT NULL,

    -- Richiedente (se non socio)
    NomeRichiedente NVARCHAR(200),
    EmailRichiedente NVARCHAR(100),
    TelefonoRichiedente NVARCHAR(20),

    -- Stato
    StatoPrenotazione NVARCHAR(50) DEFAULT 'Confermata', -- Confermata, In Attesa, Annullata, Completata

    -- Pagamento
    Importo DECIMAL(10,2),
    Pagato BIT DEFAULT 0,
    DataPagamento DATETIME NULL,

    Note NVARCHAR(500),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Prenotazioni_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Prenotazioni_Impianti FOREIGN KEY (ImpiantoId) REFERENCES Impianti(ImpiantoId),
    CONSTRAINT FK_Prenotazioni_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Prenotazioni_ImpiantoId ON Prenotazioni(ImpiantoId);
CREATE NONCLUSTERED INDEX IX_Prenotazioni_DataPrenotazione ON Prenotazioni(DataPrenotazione);
CREATE NONCLUSTERED INDEX IX_Prenotazioni_SocioId ON Prenotazioni(SocioId);
GO

-- =============================================
-- TABELLA: AbbonamentiImpianti
-- Abbonamenti per utilizzo impianti
-- =============================================
CREATE TABLE AbbonamentiImpianti (
    AbbonamentoId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    SocioId INT NOT NULL,
    ImpiantoId INT NULL, -- NULL = tutti gli impianti

    TipoAbbonamento NVARCHAR(100), -- Mensile, Trimestrale, Annuale, Carnet
    NumeroIngressi INT, -- per carnet
    IngressiUtilizzati INT DEFAULT 0,

    DataInizio DATE NOT NULL,
    DataScadenza DATE NOT NULL,

    Importo DECIMAL(10,2) NOT NULL,
    Pagato BIT DEFAULT 0,

    Attivo BIT DEFAULT 1,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_AbbonamentiImpianti_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_AbbonamentiImpianti_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId),
    CONSTRAINT FK_AbbonamentiImpianti_Impianti FOREIGN KEY (ImpiantoId) REFERENCES Impianti(ImpiantoId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_AbbonamentiImpianti_SocioId ON AbbonamentiImpianti(SocioId);
GO

PRINT 'Impianti tables created successfully';
GO
