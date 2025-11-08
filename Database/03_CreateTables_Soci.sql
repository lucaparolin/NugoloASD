-- =============================================
-- TABELLE MODULO SOCI E ANAGRAFICHE
-- =============================================

USE NugoloASD;
GO

-- =============================================
-- TABELLA: Soci
-- Gestione soci, atleti, tecnici, dirigenti
-- =============================================
CREATE TABLE Soci (
    SocioId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    UtenteId INT NULL, -- Collegamento a Utenti se il socio ha accesso al sistema

    -- Dati anagrafici
    Nome NVARCHAR(100) NOT NULL,
    Cognome NVARCHAR(100) NOT NULL,
    CodiceFiscale NVARCHAR(16) NOT NULL,
    DataNascita DATE NOT NULL,
    LuogoNascita NVARCHAR(100),
    Sesso CHAR(1), -- M, F, A (Altro)

    -- Documenti identità
    TipoDocumento NVARCHAR(50), -- Carta identità, Patente, Passaporto
    NumeroDocumento NVARCHAR(50),
    DataRilascioDocumento DATE,
    DataScadenzaDocumento DATE,
    EnteRilascio NVARCHAR(100),

    -- Residenza
    IndirizzoResidenza NVARCHAR(300),
    CittaResidenza NVARCHAR(100),
    CAPResidenza NVARCHAR(10),
    ProvinciaResidenza NVARCHAR(2),

    -- Domicilio (se diverso da residenza)
    IndirizzoDomicilio NVARCHAR(300),
    CittaDomicilio NVARCHAR(100),
    CAPDomicilio NVARCHAR(10),
    ProvinciaDomicilio NVARCHAR(2),

    -- Contatti
    Email NVARCHAR(100),
    Telefono NVARCHAR(20),
    TelefonoCellulare NVARCHAR(20),

    -- Informazioni socio
    NumeroTessera NVARCHAR(50),
    TipoSocio NVARCHAR(50) NOT NULL, -- Atleta, Tecnico, Dirigente, Socio Ordinario
    DataPrimaIscrizione DATE,
    DataUltimoRinnovo DATE,
    StatoSocio NVARCHAR(50) DEFAULT 'Attivo', -- Attivo, Sospeso, Non Iscritto, Storico

    -- Minori - Gestione tutori
    Minorenne BIT DEFAULT 0,
    GenitoreId INT NULL, -- FK a Soci (se il genitore è anche socio)
    NomeGenitore1 NVARCHAR(100),
    CognomeGenitore1 NVARCHAR(100),
    TelefonoGenitore1 NVARCHAR(20),
    EmailGenitore1 NVARCHAR(100),
    NomeGenitore2 NVARCHAR(100),
    CognomeGenitore2 NVARCHAR(100),
    TelefonoGenitore2 NVARCHAR(20),
    EmailGenitore2 NVARCHAR(100),

    -- Privacy e consensi
    ConsensoPrivacy BIT DEFAULT 0,
    DataConsensoPrivacy DATETIME,
    ConsensoMarketing BIT DEFAULT 0,
    ConsensoImmagini BIT DEFAULT 0,

    -- Foto
    FotoUrl NVARCHAR(500),

    -- Note
    Note NVARCHAR(MAX),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Soci_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Soci_Utenti FOREIGN KEY (UtenteId) REFERENCES Utenti(UtenteId),
    CONSTRAINT FK_Soci_Genitore FOREIGN KEY (GenitoreId) REFERENCES Soci(SocioId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Soci_AssociazioneId ON Soci(AssociazioneId);
CREATE NONCLUSTERED INDEX IX_Soci_CodiceFiscale ON Soci(CodiceFiscale);
CREATE NONCLUSTERED INDEX IX_Soci_Email ON Soci(Email);
CREATE NONCLUSTERED INDEX IX_Soci_NumeroTessera ON Soci(NumeroTessera);
CREATE NONCLUSTERED INDEX IX_Soci_StatoSocio ON Soci(StatoSocio);
GO

-- =============================================
-- TABELLA: Documenti
-- Gestione documenti dei soci
-- =============================================
CREATE TABLE Documenti (
    DocumentoId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    SocioId INT NOT NULL,

    TipoDocumento NVARCHAR(100) NOT NULL, -- Certificato Medico, Liberatoria, Tesseramento, Privacy, etc.
    NomeFile NVARCHAR(300) NOT NULL,
    PercorsoFile NVARCHAR(500) NOT NULL,
    Estensione NVARCHAR(10),
    DimensioneKB INT,

    DataEmissione DATE,
    DataScadenza DATE,
    Obbligatorio BIT DEFAULT 0,
    Validato BIT DEFAULT 0,
    DataValidazione DATETIME NULL,
    ValidatoDa NVARCHAR(100) NULL,

    Note NVARCHAR(500),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Documenti_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Documenti_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Documenti_SocioId ON Documenti(SocioId);
CREATE NONCLUSTERED INDEX IX_Documenti_DataScadenza ON Documenti(DataScadenza);
GO

-- =============================================
-- TABELLA: CertificatiMedici
-- Tracciamento specifico certificati medici
-- =============================================
CREATE TABLE CertificatiMedici (
    CertificatoMedicoId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    SocioId INT NOT NULL,
    DocumentoId INT NULL,

    TipoCertificato NVARCHAR(100) NOT NULL, -- Agonistico, Non Agonistico, Ludico-motorio
    NumeroProtocollo NVARCHAR(100),
    DataEmissione DATE NOT NULL,
    DataScadenza DATE NOT NULL,

    NomeMedico NVARCHAR(200),
    CodiceFiscaleMedico NVARCHAR(16),

    Validato BIT DEFAULT 0,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_CertificatiMedici_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_CertificatiMedici_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId),
    CONSTRAINT FK_CertificatiMedici_Documenti FOREIGN KEY (DocumentoId) REFERENCES Documenti(DocumentoId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_CertificatiMedici_SocioId ON CertificatiMedici(SocioId);
CREATE NONCLUSTERED INDEX IX_CertificatiMedici_DataScadenza ON CertificatiMedici(DataScadenza);
GO

-- =============================================
-- TABELLA: StoricoSoci
-- Storico attività del socio
-- =============================================
CREATE TABLE StoricoSoci (
    StoricoId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    SocioId INT NOT NULL,

    TipoEvento NVARCHAR(100) NOT NULL, -- Iscrizione, Rinnovo, Sospensione, Riattivazione, Cancellazione
    Descrizione NVARCHAR(500),
    DataEvento DATETIME NOT NULL,

    -- Dettagli JSON per eventi complessi
    DettagliJSON NVARCHAR(MAX),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    UtenteInserimento NVARCHAR(100) NOT NULL,

    CONSTRAINT FK_StoricoSoci_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_StoricoSoci_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_StoricoSoci_SocioId ON StoricoSoci(SocioId);
CREATE NONCLUSTERED INDEX IX_StoricoSoci_DataEvento ON StoricoSoci(DataEvento);
GO

PRINT 'Soci tables created successfully';
GO
