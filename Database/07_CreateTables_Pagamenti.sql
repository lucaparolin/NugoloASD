-- =============================================
-- TABELLE MODULO GESTIONE ECONOMICA
-- Pagamenti, Fatturazione, Contabilità
-- =============================================

USE AISSURE_Pilot;
GO

-- =============================================
-- TABELLA: Pagamenti
-- Gestione pagamenti
-- =============================================
CREATE TABLE Pagamenti (
    PagamentoId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    SocioId INT NOT NULL,

    -- Causale
    Causale NVARCHAR(300) NOT NULL,
    Descrizione NVARCHAR(500),
    TipoPagamento NVARCHAR(100), -- Iscrizione Corso, Tesseramento, Abbonamento, Prenotazione, Quota Sociale

    -- Riferimenti
    IscrizioneId INT NULL,
    TesseramentoId INT NULL,
    PrenotazioneId INT NULL,
    AbbonamentoId INT NULL,

    -- Importi
    Importo DECIMAL(10,2) NOT NULL,
    ImportoPagato DECIMAL(10,2) DEFAULT 0,
    ImportoResiduo AS (Importo - ImportoPagato) PERSISTED,

    -- Metodo pagamento
    MetodoPagamento NVARCHAR(50), -- Contanti, Bonifico, Carta, PayPal, Stripe, Satispay, POS
    RiferimentoTransazione NVARCHAR(200), -- ID transazione gateway

    -- Date
    DataScadenza DATE,
    DataPagamento DATETIME NULL,

    -- Stato
    StatoPagamento NVARCHAR(50) DEFAULT 'In Attesa', -- In Attesa, Pagato, Parziale, Scaduto, Annullato

    -- Rate
    PagamentoRateale BIT DEFAULT 0,
    NumeroRata INT NULL,
    TotaleRate INT NULL,

    Note NVARCHAR(500),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Pagamenti_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Pagamenti_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId),
    CONSTRAINT FK_Pagamenti_Iscrizioni FOREIGN KEY (IscrizioneId) REFERENCES Iscrizioni(IscrizioneId),
    CONSTRAINT FK_Pagamenti_Tesseramenti FOREIGN KEY (TesseramentoId) REFERENCES Tesseramenti(TesseramentoId),
    CONSTRAINT FK_Pagamenti_Prenotazioni FOREIGN KEY (PrenotazioneId) REFERENCES Prenotazioni(PrenotazioneId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Pagamenti_AssociazioneId ON Pagamenti(AssociazioneId);
CREATE NONCLUSTERED INDEX IX_Pagamenti_SocioId ON Pagamenti(SocioId);
CREATE NONCLUSTERED INDEX IX_Pagamenti_DataScadenza ON Pagamenti(DataScadenza);
CREATE NONCLUSTERED INDEX IX_Pagamenti_StatoPagamento ON Pagamenti(StatoPagamento);
GO

-- =============================================
-- TABELLA: Fatture
-- Gestione fatture elettroniche
-- =============================================
CREATE TABLE Fatture (
    FatturaId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,
    SocioId INT NULL,

    -- Numero fattura
    NumeroFattura NVARCHAR(50) NOT NULL,
    AnnoFattura INT NOT NULL,
    ProgressivoFattura INT NOT NULL,

    DataEmissione DATE NOT NULL,
    DataScadenza DATE,

    -- Tipo documento
    TipoDocumento NVARCHAR(50) DEFAULT 'Fattura', -- Fattura, Ricevuta, Nota Credito, Nota Debito

    -- Cliente (se non socio)
    RagioneSocialeCliente NVARCHAR(300),
    CodiceFiscaleCliente NVARCHAR(16),
    PartitaIVACliente NVARCHAR(20),
    IndirizzoCliente NVARCHAR(300),
    CittaCliente NVARCHAR(100),
    CAPCliente NVARCHAR(10),
    ProvinciaCliente NVARCHAR(2),

    -- Importi
    Imponibile DECIMAL(10,2) NOT NULL,
    AliquotaIVA DECIMAL(5,2) DEFAULT 0,
    ImportoIVA AS (Imponibile * AliquotaIVA / 100) PERSISTED,
    Totale AS (Imponibile + (Imponibile * AliquotaIVA / 100)) PERSISTED,

    -- Dettagli
    Descrizione NVARCHAR(MAX),

    -- Fatturazione elettronica
    CodiceDestinatario NVARCHAR(10),
    PECDestinatario NVARCHAR(100),
    XMLFattura NVARCHAR(MAX), -- XML fattura elettronica
    NomeFileXML NVARCHAR(200),
    IdSDI NVARCHAR(50), -- ID Sistema di Interscambio

    -- Stato
    Inviata BIT DEFAULT 0,
    DataInvio DATETIME NULL,
    Accettata BIT DEFAULT 0,
    DataAccettazione DATETIME NULL,

    -- Pagamento
    Pagata BIT DEFAULT 0,
    PagamentoId INT NULL,

    -- PDF
    PDFUrl NVARCHAR(500),

    Note NVARCHAR(500),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_Fatture_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_Fatture_Soci FOREIGN KEY (SocioId) REFERENCES Soci(SocioId),
    CONSTRAINT FK_Fatture_Pagamenti FOREIGN KEY (PagamentoId) REFERENCES Pagamenti(PagamentoId),
    CONSTRAINT UK_Fatture_Numero UNIQUE (AssociazioneId, NumeroFattura, AnnoFattura)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_Fatture_AssociazioneId ON Fatture(AssociazioneId);
CREATE NONCLUSTERED INDEX IX_Fatture_DataEmissione ON Fatture(DataEmissione);
GO

-- =============================================
-- TABELLA: RigheFattura
-- Righe dettaglio fattura
-- =============================================
CREATE TABLE RigheFattura (
    RigaFatturaId INT IDENTITY(1,1) PRIMARY KEY,
    FatturaId INT NOT NULL,

    NumeroRiga INT NOT NULL,
    Descrizione NVARCHAR(500) NOT NULL,

    Quantita DECIMAL(10,2) DEFAULT 1,
    PrezzoUnitario DECIMAL(10,2) NOT NULL,
    Imponibile AS (Quantita * PrezzoUnitario) PERSISTED,

    AliquotaIVA DECIMAL(5,2) DEFAULT 0,

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    UtenteInserimento NVARCHAR(100) NOT NULL,

    CONSTRAINT FK_RigheFattura_Fatture FOREIGN KEY (FatturaId) REFERENCES Fatture(FatturaId) ON DELETE CASCADE
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_RigheFattura_FatturaId ON RigheFattura(FatturaId);
GO

-- =============================================
-- TABELLA: MovimentiContabili
-- Contabilità gestionale (non fiscale)
-- =============================================
CREATE TABLE MovimentiContabili (
    MovimentoId INT IDENTITY(1,1) PRIMARY KEY,
    AssociazioneId INT NOT NULL,

    DataMovimento DATE NOT NULL,
    TipoMovimento NVARCHAR(50) NOT NULL, -- Entrata, Uscita

    Categoria NVARCHAR(100), -- Iscrizioni, Tesseramenti, Affitti, Stipendi, Utenze, etc.
    SottoCategoria NVARCHAR(100),

    Importo DECIMAL(10,2) NOT NULL,
    Descrizione NVARCHAR(500),

    -- Centro di costo
    CentroDiCosto NVARCHAR(100), -- Nome Corso, Nome Evento, etc.

    -- Riferimenti
    PagamentoId INT NULL,
    FatturaId INT NULL,

    -- Documenti
    DocumentoUrl NVARCHAR(500),

    Note NVARCHAR(500),

    -- Audit
    DataInserimento DATETIME DEFAULT GETUTCDATE(),
    DataModifica DATETIME NULL,
    UtenteInserimento NVARCHAR(100) NOT NULL,
    UtenteModifica NVARCHAR(100) NULL,

    CONSTRAINT FK_MovimentiContabili_Associazioni FOREIGN KEY (AssociazioneId) REFERENCES Associazioni(AssociazioneId),
    CONSTRAINT FK_MovimentiContabili_Pagamenti FOREIGN KEY (PagamentoId) REFERENCES Pagamenti(PagamentoId),
    CONSTRAINT FK_MovimentiContabili_Fatture FOREIGN KEY (FatturaId) REFERENCES Fatture(FatturaId)
);
GO

-- Index per performance
CREATE NONCLUSTERED INDEX IX_MovimentiContabili_AssociazioneId ON MovimentiContabili(AssociazioneId);
CREATE NONCLUSTERED INDEX IX_MovimentiContabili_DataMovimento ON MovimentiContabili(DataMovimento);
CREATE NONCLUSTERED INDEX IX_MovimentiContabili_Categoria ON MovimentiContabili(Categoria);
GO

PRINT 'Pagamenti tables created successfully';
GO
