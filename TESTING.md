# Testing Guide - AISSURE Pilot

## Compilazione e Test

### Pre-requisiti
- .NET SDK 8.0
- SQL Server 2019+ o SQL Server Express
- Editor (Visual Studio 2022, VS Code, Rider)

### 1. Compilazione del Progetto

```bash
cd AISSURE.Pilot
dotnet restore
dotnet build
```

**Output atteso**: Compilazione riuscita senza errori

### 2. Esecuzione del Progetto

```bash
dotnet run
```

**Output atteso**:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
```

### 3. Test Endpoints

#### 3.1 Test HomePage
```
GET https://localhost:7000/
```
Dovrebbe mostrare la landing page con i moduli implementati.

#### 3.2 Test Swagger UI
```
GET https://localhost:7000/swagger
```
Dovrebbe mostrare la documentazione API interattiva.

#### 3.3 Test API - Registrazione Utente

```http
POST https://localhost:7000/api/auth/register
Content-Type: application/json

{
  "username": "admin",
  "email": "admin@aissure.local",
  "password": "Admin123!@#",
  "nome": "Admin",
  "cognome": "Sistema",
  "associazioneId": 1
}
```

**Risposta attesa (200 OK)**:
```json
{
  "message": "Registrazione completata con successo",
  "utenteId": 1
}
```

#### 3.4 Test API - Login

```http
POST https://localhost:7000/api/auth/login
Content-Type: application/json

{
  "usernameOrEmail": "admin",
  "password": "Admin123!@#"
}
```

**Risposta attesa (200 OK)**:
```json
{
  "message": "Login effettuato con successo",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "utenteId": 1,
    "username": "admin",
    "email": "admin@aissure.local",
    "nome": "Admin",
    "cognome": "Sistema",
    "associazioneId": 1
  }
}
```

#### 3.5 Test API - Get Soci (Autenticato)

```http
GET https://localhost:7000/api/soci
Authorization: Bearer YOUR_JWT_TOKEN
X-Tenant-Id: 1
```

### 4. Verifica Database

Dopo aver eseguito gli script SQL, verificare che le tabelle siano create:

```sql
USE AISSURE_Pilot;

-- Verifica tabelle core
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';

-- Verifica ruoli
SELECT * FROM Ruoli;

-- Verifica permessi
SELECT COUNT(*) AS NumeroPermessi FROM Permessi;

-- Verifica federazioni
SELECT * FROM Federazioni;
```

### 5. Test Compilazione Repository

Creare un test semplice per verificare che i repository compilino correttamente:

```csharp
// Test temporaneo in Program.cs (dopo var app = builder.Build())

using (var scope = app.Services.CreateScope())
{
    var associazioneRepo = scope.ServiceProvider.GetRequiredService<IAssociazioneRepository>();
    var utenteRepo = scope.ServiceProvider.GetRequiredService<IUtenteRepository>();

    Console.WriteLine("Repository istanziati correttamente");
}
```

### 6. Troubleshooting

#### Errore: "Cannot find compilation library location for package"
```bash
dotnet clean
dotnet restore
dotnet build
```

#### Errore: "A connection was successfully established with the server, but then an error occurred"
- Verificare che SQL Server sia in esecuzione
- Verificare la connection string in appsettings.json
- Verificare che il database AISSURE_Pilot esista

#### Errore: 401 Unauthorized su API protette
- Verificare di aver incluso l'header Authorization: Bearer TOKEN
- Verificare che il token JWT non sia scaduto
- Verificare l'header X-Tenant-Id se richiesto

### 7. Test con Postman/Insomnia

Importare questa collection per testare rapidamente:

**Auth - Register**
```
POST {{baseUrl}}/api/auth/register
```

**Auth - Login**
```
POST {{baseUrl}}/api/auth/login
```

**Soci - List**
```
GET {{baseUrl}}/api/soci
Headers:
  Authorization: Bearer {{token}}
  X-Tenant-Id: 1
```

### 8. Metriche di Successo

✅ **Build Success**: 0 errori, 0 warning
✅ **Startup Success**: Applicazione avviata su HTTPS e HTTP
✅ **Database Connection**: Connessione al database riuscita
✅ **API Auth**: Registrazione e login funzionanti
✅ **API Soci**: CRUD operations funzionanti con autenticazione
✅ **Swagger UI**: Documentazione accessibile e funzionante

### 9. Performance Baseline

Durante i primi test, raccogliere questi dati:

- **Startup Time**: < 5 secondi
- **First Request**: < 500ms
- **Subsequent Requests**: < 100ms
- **Database Query**: < 50ms per query semplici

### 10. Log Monitoring

I log sono disponibili in:
- **Console**: Output real-time
- **File**: `logs/aissure-pilot-{date}.txt`

Verificare che i log mostrino:
```
[INF] Application started successfully
[INF] Listening on https://localhost:7000
[INF] Database connection pool initialized
```

---

## Note Importanti

1. **Prima esecuzione**: Assicurarsi che il database sia stato creato con tutti gli script SQL in ordine
2. **Dati iniziali**: Gli script di inizializzazione creano ruoli, permessi e template
3. **Tenant**: Per testare il multi-tenant, creare più associazioni nel database
4. **JWT Secret**: Cambiare la chiave segreta in produzione (appsettings.json)

## Prossimi Test da Implementare

- [ ] Unit Tests per Repository
- [ ] Integration Tests per API
- [ ] End-to-End Tests per flussi completi
- [ ] Performance Tests con carico
- [ ] Security Tests (SQL Injection, XSS, etc.)
