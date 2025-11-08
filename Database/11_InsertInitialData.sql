-- =============================================
-- INSERIMENTO DATI INIZIALI
-- Ruoli, Permessi, Federazioni, Template
-- =============================================

USE AISSURE_Pilot;
GO

-- =============================================
-- INSERIMENTO RUOLI
-- =============================================
INSERT INTO Ruoli (Nome, Descrizione, Livello, UtenteInserimento) VALUES
('Amministratore', 'Accesso completo a tutte le funzionalità del sistema', 1, 'SYSTEM'),
('Segretario', 'Gestione soci, corsi, iscrizioni, pagamenti', 2, 'SYSTEM'),
('Istruttore', 'Gestione presenze e valutazioni dei propri corsi', 3, 'SYSTEM'),
('Tesserato', 'Accesso area riservata, visualizzazione dati personali', 4, 'SYSTEM'),
('Genitore', 'Accesso dati figli minorenni', 5, 'SYSTEM');
GO

-- =============================================
-- INSERIMENTO PERMESSI
-- =============================================
INSERT INTO Permessi (Nome, Descrizione, Risorsa, Azione, UtenteInserimento) VALUES
-- Permessi Soci
('soci.lettura', 'Visualizzare elenco e dettagli soci', 'Soci', 'Lettura', 'SYSTEM'),
('soci.scrittura', 'Creare e modificare soci', 'Soci', 'Scrittura', 'SYSTEM'),
('soci.eliminazione', 'Eliminare soci', 'Soci', 'Eliminazione', 'SYSTEM'),

-- Permessi Corsi
('corsi.lettura', 'Visualizzare corsi', 'Corsi', 'Lettura', 'SYSTEM'),
('corsi.scrittura', 'Creare e modificare corsi', 'Corsi', 'Scrittura', 'SYSTEM'),
('corsi.eliminazione', 'Eliminare corsi', 'Corsi', 'Eliminazione', 'SYSTEM'),

-- Permessi Iscrizioni
('iscrizioni.lettura', 'Visualizzare iscrizioni', 'Iscrizioni', 'Lettura', 'SYSTEM'),
('iscrizioni.scrittura', 'Gestire iscrizioni', 'Iscrizioni', 'Scrittura', 'SYSTEM'),
('iscrizioni.eliminazione', 'Eliminare iscrizioni', 'Iscrizioni', 'Eliminazione', 'SYSTEM'),

-- Permessi Presenze
('presenze.lettura', 'Visualizzare presenze', 'Presenze', 'Lettura', 'SYSTEM'),
('presenze.scrittura', 'Registrare presenze', 'Presenze', 'Scrittura', 'SYSTEM'),

-- Permessi Pagamenti
('pagamenti.lettura', 'Visualizzare pagamenti', 'Pagamenti', 'Lettura', 'SYSTEM'),
('pagamenti.scrittura', 'Registrare pagamenti', 'Pagamenti', 'Scrittura', 'SYSTEM'),
('pagamenti.eliminazione', 'Eliminare pagamenti', 'Pagamenti', 'Eliminazione', 'SYSTEM'),

-- Permessi Fatture
('fatture.lettura', 'Visualizzare fatture', 'Fatture', 'Lettura', 'SYSTEM'),
('fatture.scrittura', 'Emettere fatture', 'Fatture', 'Scrittura', 'SYSTEM'),

-- Permessi Tesseramenti
('tesseramenti.lettura', 'Visualizzare tesseramenti', 'Tesseramenti', 'Lettura', 'SYSTEM'),
('tesseramenti.scrittura', 'Gestire tesseramenti', 'Tesseramenti', 'Scrittura', 'SYSTEM'),

-- Permessi Impianti
('impianti.lettura', 'Visualizzare impianti', 'Impianti', 'Lettura', 'SYSTEM'),
('impianti.scrittura', 'Gestire impianti', 'Impianti', 'Scrittura', 'SYSTEM'),
('prenotazioni.lettura', 'Visualizzare prenotazioni', 'Prenotazioni', 'Lettura', 'SYSTEM'),
('prenotazioni.scrittura', 'Gestire prenotazioni', 'Prenotazioni', 'Scrittura', 'SYSTEM'),

-- Permessi Eventi
('eventi.lettura', 'Visualizzare eventi', 'Eventi', 'Lettura', 'SYSTEM'),
('eventi.scrittura', 'Gestire eventi', 'Eventi', 'Scrittura', 'SYSTEM'),

-- Permessi CRM
('crm.lettura', 'Visualizzare contatti CRM', 'CRM', 'Lettura', 'SYSTEM'),
('crm.scrittura', 'Gestire contatti CRM', 'CRM', 'Scrittura', 'SYSTEM'),

-- Permessi Comunicazioni
('comunicazioni.lettura', 'Visualizzare comunicazioni', 'Comunicazioni', 'Lettura', 'SYSTEM'),
('comunicazioni.scrittura', 'Inviare comunicazioni', 'Comunicazioni', 'Scrittura', 'SYSTEM'),

-- Permessi Reportistica
('report.lettura', 'Visualizzare report e statistiche', 'Report', 'Lettura', 'SYSTEM'),

-- Permessi Configurazione
('configurazione.lettura', 'Visualizzare configurazioni', 'Configurazione', 'Lettura', 'SYSTEM'),
('configurazione.scrittura', 'Modificare configurazioni', 'Configurazione', 'Scrittura', 'SYSTEM');
GO

-- =============================================
-- ASSEGNAZIONE PERMESSI AI RUOLI
-- =============================================

-- Amministratore: tutti i permessi
INSERT INTO RuoliPermessi (RuoloId, PermessoId, UtenteInserimento)
SELECT 1, PermessoId, 'SYSTEM' FROM Permessi;

-- Segretario: quasi tutti i permessi tranne configurazione e eliminazioni critiche
INSERT INTO RuoliPermessi (RuoloId, PermessoId, UtenteInserimento)
SELECT 2, PermessoId, 'SYSTEM' FROM Permessi
WHERE Nome IN (
    'soci.lettura', 'soci.scrittura',
    'corsi.lettura', 'corsi.scrittura',
    'iscrizioni.lettura', 'iscrizioni.scrittura',
    'presenze.lettura', 'presenze.scrittura',
    'pagamenti.lettura', 'pagamenti.scrittura',
    'fatture.lettura', 'fatture.scrittura',
    'tesseramenti.lettura', 'tesseramenti.scrittura',
    'impianti.lettura', 'prenotazioni.lettura', 'prenotazioni.scrittura',
    'eventi.lettura', 'eventi.scrittura',
    'crm.lettura', 'crm.scrittura',
    'comunicazioni.lettura', 'comunicazioni.scrittura',
    'report.lettura'
);

-- Istruttore: permessi limitati ai propri corsi
INSERT INTO RuoliPermessi (RuoloId, PermessoId, UtenteInserimento)
SELECT 3, PermessoId, 'SYSTEM' FROM Permessi
WHERE Nome IN (
    'soci.lettura',
    'corsi.lettura',
    'iscrizioni.lettura',
    'presenze.lettura', 'presenze.scrittura',
    'comunicazioni.lettura'
);

-- Tesserato: solo lettura dati personali
INSERT INTO RuoliPermessi (RuoloId, PermessoId, UtenteInserimento)
SELECT 4, PermessoId, 'SYSTEM' FROM Permessi
WHERE Nome IN (
    'corsi.lettura',
    'iscrizioni.lettura',
    'presenze.lettura',
    'pagamenti.lettura',
    'eventi.lettura'
);

-- Genitore: stesso del tesserato
INSERT INTO RuoliPermessi (RuoloId, PermessoId, UtenteInserimento)
SELECT 5, PermessoId, 'SYSTEM' FROM Permessi
WHERE Nome IN (
    'corsi.lettura',
    'iscrizioni.lettura',
    'presenze.lettura',
    'pagamenti.lettura',
    'eventi.lettura'
);
GO

-- =============================================
-- INSERIMENTO FEDERAZIONI COMUNI
-- =============================================
INSERT INTO Federazioni (Nome, Sigla, Descrizione, SitoWeb, UtenteInserimento) VALUES
('Comitato Olimpico Nazionale Italiano', 'CONI', 'Ente pubblico a base associativa per l organizzazione e il potenziamento dello sport nazionale', 'https://www.coni.it', 'SYSTEM'),
('Federazione Italiana Giuoco Calcio', 'FIGC', 'Federazione sportiva nazionale italiana che governa lo sport del calcio', 'https://www.figc.it', 'SYSTEM'),
('Federazione Italiana Pallacanestro', 'FIP', 'Federazione sportiva nazionale italiana della pallacanestro', 'https://www.fip.it', 'SYSTEM'),
('Federazione Italiana Tennis e Padel', 'FITP', 'Federazione sportiva nazionale italiana del tennis e padel', 'https://www.federtennis.it', 'SYSTEM'),
('Federazione Italiana Nuoto', 'FIN', 'Federazione sportiva nazionale italiana del nuoto', 'https://www.federnuoto.it', 'SYSTEM'),
('Federazione Italiana Pallavolo', 'FIPAV', 'Federazione sportiva nazionale italiana della pallavolo', 'https://www.federvolley.it', 'SYSTEM'),
('Unione Italiana Sport Per tutti', 'UISP', 'Ente di promozione sportiva', 'https://www.uisp.it', 'SYSTEM'),
('Centro Sportivo Italiano', 'CSI', 'Ente di promozione sportiva di ispirazione cristiana', 'https://www.csi-net.it', 'SYSTEM'),
('Associazione Sportiva Italiana', 'ASI', 'Ente di promozione sportiva riconosciuto dal CONI', 'https://www.asiitalia.org', 'SYSTEM'),
('Ente Nazionale Democratico di Azione Sociale', 'ENDAS', 'Ente di promozione sportiva', 'https://www.endas.it', 'SYSTEM');
GO

-- =============================================
-- INSERIMENTO TEMPLATE COMUNICAZIONI DI SISTEMA
-- =============================================
INSERT INTO TemplatesComunicazioni (AssociazioneId, Nome, Descrizione, TipoComunicazione, Categoria, Oggetto, CorpoMessaggio, PlaceholdersJSON, UtenteInserimento) VALUES
-- Template Benvenuto
(NULL, 'Benvenuto Nuovo Socio', 'Email di benvenuto per nuovi soci', 'Email', 'Iscrizione',
'Benvenuto in {{NomeAssociazione}}!',
'Gentile {{NomeSocio}} {{CognomeSocio}},

Siamo lieti di darti il benvenuto in {{NomeAssociazione}}!

La tua iscrizione è stata registrata con successo. Di seguito trovi i tuoi dati:

Numero Tessera: {{NumeroTessera}}
Email: {{EmailSocio}}

Per accedere alla tua area riservata, visita: {{LinkAreaRiservata}}

Per qualsiasi informazione, non esitare a contattarci.

Cordiali saluti,
{{NomeAssociazione}}',
'["{{NomeAssociazione}}", "{{NomeSocio}}", "{{CognomeSocio}}", "{{NumeroTessera}}", "{{EmailSocio}}", "{{LinkAreaRiservata}}"]',
'SYSTEM'),

-- Template Scadenza Certificato Medico
(NULL, 'Promemoria Scadenza Certificato Medico', 'Reminder scadenza certificato medico', 'Email', 'Scadenza',
'Promemoria: Il tuo certificato medico sta per scadere',
'Gentile {{NomeSocio}} {{CognomeSocio}},

Ti ricordiamo che il tuo certificato medico scadrà il {{DataScadenzaCertificato}}.

Per continuare a partecipare alle attività sportive, è necessario rinnovare il certificato medico.

Ti preghiamo di provvedere al più presto e di caricare il nuovo certificato nella tua area riservata.

Cordiali saluti,
{{NomeAssociazione}}',
'["{{NomeAssociazione}}", "{{NomeSocio}}", "{{CognomeSocio}}", "{{DataScadenzaCertificato}}"]',
'SYSTEM'),

-- Template Conferma Pagamento
(NULL, 'Conferma Pagamento Ricevuto', 'Conferma ricezione pagamento', 'Email', 'Pagamento',
'Conferma pagamento ricevuto',
'Gentile {{NomeSocio}} {{CognomeSocio}},

Confermiamo di aver ricevuto il pagamento di € {{ImportoPagamento}} in data {{DataPagamento}}.

Causale: {{CausalePagamento}}
Metodo: {{MetodoPagamento}}

Trovi la ricevuta allegata a questa email.

Cordiali saluti,
{{NomeAssociazione}}',
'["{{NomeAssociazione}}", "{{NomeSocio}}", "{{CognomeSocio}}", "{{ImportoPagamento}}", "{{DataPagamento}}", "{{CausalePagamento}}", "{{MetodoPagamento}}"]',
'SYSTEM'),

-- Template Iscrizione Corso
(NULL, 'Conferma Iscrizione Corso', 'Conferma iscrizione a un corso', 'Email', 'Iscrizione',
'Iscrizione al corso {{NomeCorso}} confermata',
'Gentile {{NomeSocio}} {{CognomeSocio}},

La tua iscrizione al corso "{{NomeCorso}}" è stata confermata!

Dettagli corso:
- Inizio: {{DataInizioCorso}}
- Fine: {{DataFineCorso}}
- Giorni: {{GiorniCorso}}
- Orario: {{OrarioCorso}}
- Istruttore: {{NomeIstruttore}}

Ti aspettiamo!

Cordiali saluti,
{{NomeAssociazione}}',
'["{{NomeAssociazione}}", "{{NomeSocio}}", "{{CognomeSocio}}", "{{NomeCorso}}", "{{DataInizioCorso}}", "{{DataFineCorso}}", "{{GiorniCorso}}", "{{OrarioCorso}}", "{{NomeIstruttore}}"]',
'SYSTEM'),

-- Template Promemoria Lezione
(NULL, 'Promemoria Lezione', 'Promemoria lezione del giorno', 'SMS', 'Promemoria',
'',
'Ciao {{NomeSocio}}, ti ricordiamo la lezione di {{NomeCorso}} oggi alle {{OrarioLezione}}. A presto!',
'["{{NomeSocio}}", "{{NomeCorso}}", "{{OrarioLezione}}"]',
'SYSTEM');
GO

PRINT 'Initial data inserted successfully';
GO
