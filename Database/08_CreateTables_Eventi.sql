-- =============================================
-- TABELLE MODULO EVENTI, GARE E TORNEI
-- =============================================

USE NugoloASD;
GO

-- =============================================
-- TABELLA: Eventi
-- Gestione eventi, gare, tornei
-- =============================================
CREATE TABLE Eventi (
    EventoId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,

    Nome NVARCHAR(300) NOT NULL,
    Descrizione NVARCHAR(MAX),
    TipoEvento NVARCHAR(100), -- Gara, Torneo, Manifestazione, Corso Formazione, Evento Sociale

    DataInizio DATETIME NOT NULL,
    DataFine DATETIME NOT NULL,

    -- Luogo
    Luogo NVARCHAR(300),
    Indirizzo NVARCHAR(300),
    Citta NVARCHAR(100),

    -- Iscrizioni
    IscrizioniAperte BIT DEFAULT 1,
    DataAperturaIscrizioni DATETIME,
    DataChiusuraIscrizioni DATETIME,
    NumeroMassimoPartecipanti INT,
    NumeroPartecipantiIscritti INT DEFAULT 0,

    -- Categorie
    Categorie NVARCHAR(500), -- JSON array

    -- Costi
    QuotaIscrizione DECIMAL(10,2),
    QuotaRidotta DECIMAL(10,2),

    -- Premi
    Premi NVARCHAR(MAX), -- JSON

    -- Immagine
    ImmagineUrl NVARCHAR(500),
    LocandineUrl NVARCHAR(500),

    -- Documenti
    RegolamentoUrl NVARCHAR(500),

    -- Pubblicazione
    PubblicatoOnline BIT DEFAULT 1,

    -- Stato
    Attivo BIT DEFAULT 1,

    Note NVARCHAR(MAX),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Eventi_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Eventi_AssociazioneId ON Eventi(AssociazioneId);
CREATE NONCLUSTERED INDEX IX_Eventi_DataInizio ON Eventi(DataInizio);
GO

-- =============================================
-- TABELLA: IscrizioniEventi
-- Iscrizioni agli eventi
-- =============================================
CREATE TABLE IscrizioniEventi (
    IscrizioneEventoId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    EventoId INT NOT NULL,
    SocioId INT NULL,

    -- Partecipante (se non socio)
    NomePartecipante NVARCHAR(200),
    CognomePartecipante NVARCHAR(200),
    EmailPartecipante NVARCHAR(100),
    TelefonoPartecipante NVARCHAR(20),
    DataNascitaPartecipante DATE,

    Categoria NVARCHAR(100),

    DataIscrizione DATETIME DEFAULT GETUTCDATE(),
    StatoIscrizione NVARCHAR(50) DEFAULT 'Confermata', -- Confermata, In Attesa, Annullata

    -- Pagamento
    QuotaApplicata DECIMAL(10,2),
    Pagato BIT DEFAULT 0,
    DataPagamento DATETIME NULL,

    Note NVARCHAR(500),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_IscrizioniEventi_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_IscrizioniEventi_Eventi FOREIGN KEY (EventoId) REFERENCES Eventi(EventoId),
    CONSTRAINT FK_IscrizioniEventi_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_IscrizioniEventi_EventoId ON IscrizioniEventi(EventoId);
CREATE NONCLUSTERED INDEX IX_IscrizioniEventi_SocioId ON IscrizioniEventi(SocioId);
GO

-- =============================================
-- TABELLA: Risultati
-- Risultati gare/tornei
-- =============================================
CREATE TABLE Risultati (
    RisultatoId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    EventoId INT NOT NULL,
    IscrizioneEventoId INT NOT NULL,

    Posizione INT,
    Punteggio NVARCHAR(100),
    Tempo NVARCHAR(50),

    Categoria NVARCHAR(100),

    Note NVARCHAR(500),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Risultati_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Risultati_Eventi FOREIGN KEY (EventoId) REFERENCES Eventi(EventoId),
    CONSTRAINT FK_Risultati_IscrizioniEventi FOREIGN KEY (IscrizioneEventoId) REFERENCES IscrizioniEventi(IscrizioneEventoId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Risultati_EventoId ON Risultati(EventoId);
GO

PRINT 'Eventi tables created successfully';
GO
