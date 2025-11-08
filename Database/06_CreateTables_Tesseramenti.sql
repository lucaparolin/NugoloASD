-- =============================================
-- TABELLE MODULO TESSERAMENTI E AFFILIAZIONI
-- =============================================

USE NugoloASD;
GO

-- =============================================
-- TABELLA: Federazioni
-- Elenco federazioni sportive
-- =============================================
CREATE TABLE Federazioni (
    FederazioneId INT IDENTITY(1,1) PRIMARY KEY,

    Nome NVARCHAR(200) NOT NULL,
    Sigla NVARCHAR(20) NOT NULL,
    Descrizione NVARCHAR(500),

    SitoWeb NVARCHAR(200),
    Email NVARCHAR(100),
    Telefono NVARCHAR(20),

    -- API Integration
    APIEndpoint NVARCHAR(300),
    APIKey NVARCHAR(500),
    APIAbilitata BIT DEFAULT 0,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT UK_Federazioni_Sigla UNIQUE (Sigla)
);
GO

-- =============================================
-- TABELLA: Affiliazioni
-- Affiliazioni associazioni alle federazioni
-- =============================================
CREATE TABLE Affiliazioni (
    AffiliazioneId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    FederazioneId INT NOT NULL,

    NumeroAffiliazione NVARCHAR(100) NOT NULL,
    AnnoSportivo NVARCHAR(20) NOT NULL,

    DataAffiliazione DATE NOT NULL,
    DataScadenza DATE NOT NULL,

    Importo DECIMAL(10,2),
    Pagato BIT DEFAULT 0,
    DataPagamento DATETIME NULL,

    Attivo BIT DEFAULT 1,

    Note NVARCHAR(500),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Affiliazioni_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Affiliazioni_Federazioni FOREIGN KEY (FederazioneId) REFERENCES Federazioni(FederazioneId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Affiliazioni_AssociazioneId ON Affiliazioni(AssociazioneId);
GO

-- =============================================
-- TABELLA: Tesseramenti
-- Tesseramenti federali dei soci
-- =============================================
CREATE TABLE Tesseramenti (
    TesseramentoId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    SocioId INT NOT NULL,
    FederazioneId INT NOT NULL,

    NumeroTessera NVARCHAR(100) NOT NULL,
    AnnoSportivo NVARCHAR(20) NOT NULL,

    TipoTessera NVARCHAR(100), -- Atleta, Tecnico, Dirigente, Arbitro
    Categoria NVARCHAR(100),
    Qualifica NVARCHAR(100),

    DataEmissione DATE NOT NULL,
    DataScadenza DATE NOT NULL,

    Importo DECIMAL(10,2),
    Pagato BIT DEFAULT 0,
    DataPagamento DATETIME NULL,

    -- Stato
    StatoTesseramento NVARCHAR(50) DEFAULT 'Attivo', -- Attivo, Sospeso, Scaduto, Annullato

    -- Documenti
    DocumentoTesseraUrl NVARCHAR(500),

    Note NVARCHAR(500),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Tesseramenti_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Tesseramenti_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId),
    CONSTRAINT FK_Tesseramenti_Federazioni FOREIGN KEY (FederazioneId) REFERENCES Federazioni(FederazioneId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Tesseramenti_AssociazioneId ON Tesseramenti(AssociazioneId);
CREATE NONCLUSTERED INDEX IX_Tesseramenti_SocioId ON Tesseramenti(SocioId);
CREATE NONCLUSTERED INDEX IX_Tesseramenti_DataScadenza ON Tesseramenti(DataScadenza);
GO

PRINT 'Tesseramenti tables created successfully';
GO
