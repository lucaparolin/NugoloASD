-- =============================================
-- TABELLE MODULO CORSI E ATTIVITÀ
-- =============================================

USE AISSURE_Pilot;
GO

-- =============================================
-- TABELLA: Corsi
-- Gestione corsi e attività
-- =============================================
CREATE TABLE Corsi (
    CorsoId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,

    Nome NVARCHAR(200) NOT NULL,
    Descrizione NVARCHAR(MAX),
    Categoria NVARCHAR(100), -- Es: Calcio, Tennis, Nuoto, Fitness
    Livello NVARCHAR(50), -- Principiante, Intermedio, Avanzato, Agonistico

    -- Periodo
    DataInizio DATE NOT NULL,
    DataFine DATE NOT NULL,
    AnnoSportivo NVARCHAR(20), -- Es: 2024/2025

    -- Orari
    GiorniSettimana NVARCHAR(50), -- JSON array: ["Lunedì", "Mercoledì", "Venerdì"]
    OrarioInizio TIME,
    OrarioFine TIME,

    -- Capacità
    PostiDisponibili INT,
    PostiOccupati INT DEFAULT 0,
    ListaAttesaAttiva BIT DEFAULT 0,

    -- Istruttore
    IstruttoreId INT NULL, -- FK a Soci dove TipoSocio = 'Tecnico'

    -- Prezzi
    PrezzoPieno DECIMAL(10,2),
    PrezzoRidotto DECIMAL(10,2),
    DescrizioneRiduzione NVARCHAR(300),

    -- Immagine
    ImmagineUrl NVARCHAR(500),

    -- Stato
    Attivo BIT DEFAULT 1,
    PubblicatoOnline BIT DEFAULT 1, -- Visibile per iscrizioni online

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Corsi_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Corsi_Istruttore FOREIGN KEY (IstruttoreId) REFERENCES Soci(SocioId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Corsi_AssociazioneId ON Corsi(AssociazioneId);
CREATE NONCLUSTERED INDEX IX_Corsi_IstruttoreId ON Corsi(IstruttoreId);
CREATE NONCLUSTERED INDEX IX_Corsi_DataInizio ON Corsi(DataInizio);
GO

-- =============================================
-- TABELLA: Iscrizioni
-- Gestione iscrizioni ai corsi
-- =============================================
CREATE TABLE Iscrizioni (
    IscrizioneId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    CorsoId INT NOT NULL,
    SocioId INT NOT NULL,

    DataIscrizione DATETIME DEFAULT GETUTCDATE(),
    StatoIscrizione NVARCHAR(50) DEFAULT 'In Attesa', -- In Attesa, Confermata, Lista Attesa, Annullata

    -- Prezzi applicati
    Importo DECIMAL(10,2) NOT NULL,
    Scontistica NVARCHAR(200),
    ImportoScontato DECIMAL(10,2),

    -- Pagamento
    Pagato BIT DEFAULT 0,
    DataPagamento DATETIME NULL,
    MetodoPagamento NVARCHAR(50),

    -- Note
    Note NVARCHAR(500),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Iscrizioni_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Iscrizioni_Corsi FOREIGN KEY (CorsoId) REFERENCES Corsi(CorsoId),
    CONSTRAINT FK_Iscrizioni_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId),
    CONSTRAINT UK_Iscrizioni_CorsoSocio UNIQUE (CorsoId, SocioId, DataIscrizione)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Iscrizioni_CorsoId ON Iscrizioni(CorsoId);
CREATE NONCLUSTERED INDEX IX_Iscrizioni_SocioId ON Iscrizioni(SocioId);
GO

-- =============================================
-- TABELLA: Lezioni
-- Singole lezioni del corso
-- =============================================
CREATE TABLE Lezioni (
    LezioneId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    CorsoId INT NOT NULL,

    DataLezione DATE NOT NULL,
    OrarioInizio TIME NOT NULL,
    OrarioFine TIME NOT NULL,

    IstruttoreId INT NULL,
    Argomento NVARCHAR(300),
    Note NVARCHAR(MAX),

    Annullata BIT DEFAULT 0,
    MotivoAnnullamento NVARCHAR(500),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Lezioni_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Lezioni_Corsi FOREIGN KEY (CorsoId) REFERENCES Corsi(CorsoId),
    CONSTRAINT FK_Lezioni_Istruttore FOREIGN KEY (IstruttoreId) REFERENCES Soci(SocioId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Lezioni_CorsoId ON Lezioni(CorsoId);
CREATE NONCLUSTERED INDEX IX_Lezioni_DataLezione ON Lezioni(DataLezione);
GO

-- =============================================
-- TABELLA: Presenze
-- Registro presenze
-- =============================================
CREATE TABLE Presenze (
    PresenzaId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    LezioneId INT NOT NULL,
    SocioId INT NOT NULL,

    Presente BIT DEFAULT 1,
    Giustificato BIT DEFAULT 0,
    MotivoAssenza NVARCHAR(300),

    -- Firma digitale / QR Code
    MetodoRilevazione NVARCHAR(50), -- Manuale, QRCode, NFC, App
    OraRilevazione DATETIME,

    Note NVARCHAR(500),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Presenze_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Presenze_Lezioni FOREIGN KEY (LezioneId) REFERENCES Lezioni(LezioneId),
    CONSTRAINT FK_Presenze_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId),
    CONSTRAINT UK_Presenze_LezioneSocio UNIQUE (LezioneId, SocioId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Presenze_LezioneId ON Presenze(LezioneId);
CREATE NONCLUSTERED INDEX IX_Presenze_SocioId ON Presenze(SocioId);
GO

-- =============================================
-- TABELLA: ValutazioniAtleti
-- Note tecniche e valutazioni degli istruttori
-- =============================================
CREATE TABLE ValutazioniAtleti (
    ValutazioneId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    SocioId INT NOT NULL,
    IstruttoreId INT NOT NULL,
    CorsoId INT NULL,

    DataValutazione DATE NOT NULL,

    Voto INT, -- 1-10 o altro sistema
    Categoria NVARCHAR(100), -- Tecnica, Tattica, Fisica, Mentale
    Osservazioni NVARCHAR(MAX),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_ValutazioniAtleti_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_ValutazioniAtleti_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId),
    CONSTRAINT FK_ValutazioniAtleti_Istruttore FOREIGN KEY (IstruttoreId) REFERENCES Soci(SocioId),
    CONSTRAINT FK_ValutazioniAtleti_Corsi FOREIGN KEY (CorsoId) REFERENCES Corsi(CorsoId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_ValutazioniAtleti_SocioId ON ValutazioniAtleti(SocioId);
GO

PRINT 'Corsi tables created successfully';
GO
