# 🚀 ITI Pharmacy API - Deployment Guide

Complete guide for deploying the application to production environments.

---

## 📋 Pre-Deployment Checklist

- [ ] All tests passing
- [ ] Security review completed
- [ ] Environment variables configured
- [ ] Database backup strategy in place
- [ ] SSL certificates ready
- [ ] CORS configured for production domain
- [ ] Logging configured
- [ ] Performance testing completed

---

## 🔐 Security Configuration

### **1. Update JWT Secret**

**❌ NEVER use the default JWT key in production!**

```json
{
  "Jwt": {
    "Key": "GENERATE-A-STRONG-RANDOM-KEY-AT-LEAST-64-CHARACTERS-LONG",
    "Issuer": "YourProductionAPI",
    "Audience": "YourProductionApp",
    "ExpireDays": 7
  }
}
```

**Generate a strong key:**
```bash
# PowerShell
[Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(64))

# Linux/Mac
openssl rand -base64 64
```

### **2. Update CORS**

In `Program.cs`, replace `AllowAll` policy:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins(
            "https://yourdomain.com",
            "https://www.yourdomain.com"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

app.UseCors("Production");
```

### **3. Enable HTTPS Redirection**

Ensure this is in `Program.cs`:

```csharp
app.UseHttpsRedirection();
```

### **4. Disable Swagger in Production** (Optional)

Update `Program.cs`:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

---

## ☁️ Deploy to Azure App Service

### **Step 1: Create Azure Resources**

```bash
# Login to Azure
az login

# Create resource group
az group create --name pharmacy-rg --location eastus

# Create App Service plan
az appservice plan create \
  --name pharmacy-plan \
  --resource-group pharmacy-rg \
  --sku B1 \
  --is-linux

# Create web app
az webapp create \
  --resource-group pharmacy-rg \
  --plan pharmacy-plan \
  --name iti-pharmacy-api \
  --runtime "DOTNETCORE:8.0"
```

### **Step 2: Create Azure SQL Database**

```bash
# Create SQL Server
az sql server create \
  --name pharmacy-sql-server \
  --resource-group pharmacy-rg \
  --location eastus \
  --admin-user sqladmin \
  --admin-password 'YourSecurePassword123!'

# Create database
az sql db create \
  --resource-group pharmacy-rg \
  --server pharmacy-sql-server \
  --name PharmacyDB \
  --service-objective S0

# Configure firewall (allow Azure services)
az sql server firewall-rule create \
  --resource-group pharmacy-rg \
  --server pharmacy-sql-server \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0
```

### **Step 3: Configure Connection String**

```bash
# Set connection string in App Service
az webapp config connection-string set \
  --resource-group pharmacy-rg \
  --name iti-pharmacy-api \
  --settings DefaultConnection="Server=tcp:pharmacy-sql-server.database.windows.net,1433;Database=PharmacyDB;User ID=sqladmin;Password=YourSecurePassword123!;Encrypt=true;Connection Timeout=30;" \
  --connection-string-type SQLAzure
```

### **Step 4: Configure App Settings**

```bash
# Set JWT configuration
az webapp config appsettings set \
  --resource-group pharmacy-rg \
  --name iti-pharmacy-api \
  --settings \
    Jwt__Key="YOUR_PRODUCTION_JWT_KEY" \
    Jwt__Issuer="ITI.Pharmacy.API" \
    Jwt__Audience="PharmacyUsers" \
    ASPNETCORE_ENVIRONMENT="Production"
```

### **Step 5: Deploy Code**

**Option A: Using Visual Studio**
1. Right-click project → Publish
2. Choose Azure → Azure App Service (Linux)
3. Select your app service
4. Click Publish

**Option B: Using Azure CLI**
```bash
# Build and publish
dotnet publish -c Release -o ./publish

# Create deployment package
cd publish
zip -r ../deploy.zip .

# Deploy to Azure
az webapp deployment source config-zip \
  --resource-group pharmacy-rg \
  --name iti-pharmacy-api \
  --src ../deploy.zip
```

**Option C: Using GitHub Actions** (see below)

### **Step 6: Run Migrations**

```bash
# From your local machine
dotnet ef database update --connection "Server=tcp:pharmacy-sql-server.database.windows.net,1433;Database=PharmacyDB;User ID=sqladmin;Password=YourSecurePassword123!;Encrypt=true;Connection Timeout=30;"
```

---

## 🐳 Deploy with Docker

### **Step 1: Create Dockerfile**

Create `Dockerfile` in project root:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ITI Pharmacy API.csproj", "./"]
RUN dotnet restore "ITI Pharmacy API.csproj"
COPY . .
RUN dotnet build "ITI Pharmacy API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ITI Pharmacy API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ITI Pharmacy API.dll"]
```

### **Step 2: Create docker-compose.yml**

```yaml
version: '3.8'

services:
  api:
    build: .
    ports:
      - "8080:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=PharmacyDB;User Id=sa;Password=YourStrong@Password;TrustServerCertificate=True
      - Jwt__Key=${JWT_KEY}
    depends_on:
      - sqlserver
    networks:
      - pharmacy-network

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong@Password
      - MSSQL_PID=Express
    ports:
      - "1433:1433"
    volumes:
      - sqlserver-data:/var/opt/mssql
    networks:
      - pharmacy-network

volumes:
  sqlserver-data:

networks:
  pharmacy-network:
    driver: bridge
```

### **Step 3: Build and Run**

```bash
# Build image
docker build -t pharmacy-api .

# Run with docker-compose
docker-compose up -d

# View logs
docker-compose logs -f api

# Run migrations
docker exec pharmacy-api dotnet ef database update
```

---

## 🖥️ Deploy to IIS (Windows Server)

### **Step 1: Prerequisites**

1. Install .NET 8.0 Hosting Bundle: https://dotnet.microsoft.com/download/dotnet/8.0
2. Enable IIS on Windows Server
3. Install SQL Server

### **Step 2: Publish Application**

```bash
dotnet publish -c Release -o C:\inetpub\wwwroot\PharmacyAPI
```

### **Step 3: Configure IIS**

1. Open IIS Manager
2. Create new Application Pool:
   - Name: PharmacyAPIPool
   - .NET CLR Version: No Managed Code
   - Managed Pipeline Mode: Integrated
3. Create new Website:
   - Site name: PharmacyAPI
   - Application Pool: PharmacyAPIPool
   - Physical path: C:\inetpub\wwwroot\PharmacyAPI
   - Port: 80 (or 443 for HTTPS)

### **Step 4: Configure web.config**

IIS needs `web.config`. It should be auto-generated, but verify:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" 
                  arguments=".\ITI Pharmacy API.dll" 
                  stdoutLogEnabled="false" 
                  stdoutLogFile=".\logs\stdout" 
                  hostingModel="inprocess" />
    </system.webServer>
  </location>
</configuration>
```

### **Step 5: Set Permissions**

```powershell
# Grant permissions to IIS user
icacls "C:\inetpub\wwwroot\PharmacyAPI" /grant "IIS AppPool\PharmacyAPIPool:(OI)(CI)F" /T
```

---

## 🔄 GitHub Actions CI/CD

Create `.github/workflows/deploy.yml`:

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v3

    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --configuration Release --no-restore

    - name: Test
      run: dotnet test --no-restore --verbosity normal

    - name: Publish
      run: dotnet publish -c Release -o ${{env.DOTNET_ROOT}}/pharmacy-api

    - name: Deploy to Azure Web App
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'iti-pharmacy-api'
        slot-name: 'production'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ${{env.DOTNET_ROOT}}/pharmacy-api
```

**Setup secrets in GitHub:**
1. Go to repository Settings → Secrets → Actions
2. Add `AZURE_WEBAPP_PUBLISH_PROFILE` (download from Azure Portal)

---

## 🔧 Environment-Specific Configuration

### **Use appsettings.Production.json**

Create `appsettings.Production.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_PRODUCTION_CONNECTION_STRING"
  },
  "Jwt": {
    "Key": "USE_ENVIRONMENT_VARIABLE_INSTEAD",
    "Issuer": "ITI.Pharmacy.API",
    "Audience": "PharmacyUsers",
    "ExpireDays": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "yourdomain.com"
}
```

### **Use Environment Variables**

Set in production:

```bash
# Azure
export ConnectionStrings__DefaultConnection="..."
export Jwt__Key="..."

# Docker
-e ConnectionStrings__DefaultConnection="..."
-e Jwt__Key="..."

# IIS (web.config)
<environmentVariables>
  <environmentVariable name="ConnectionStrings__DefaultConnection" value="..." />
  <environmentVariable name="Jwt__Key" value="..." />
</environmentVariables>
```

---

## 📊 Monitoring & Logging

### **Add Application Insights** (Azure)

```bash
# Install package
dotnet add package Microsoft.ApplicationInsights.AspNetCore

# Add to Program.cs
builder.Services.AddApplicationInsightsTelemetry();
```

### **Configure Logging**

Update `appsettings.Production.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    },
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Information"
      }
    }
  }
}
```

---

## 🔄 Database Migrations in Production

### **Option 1: Manual (Recommended)**

```bash
# From deployment machine with access to production DB
dotnet ef database update --connection "PRODUCTION_CONNECTION_STRING"
```

### **Option 2: Automatic (Startup)**

Already configured in `Program.cs`:

```csharp
await db.Database.MigrateAsync();
```

**⚠️ Warning**: May cause delays on startup. Consider manual migrations for production.

---

## 📦 Production Checklist

### **Pre-Deployment**
- [ ] All tests passing
- [ ] Code reviewed
- [ ] Security audit completed
- [ ] Performance tested
- [ ] Database backup created

### **Configuration**
- [ ] Strong JWT secret key
- [ ] CORS configured for production domain
- [ ] HTTPS enabled
- [ ] Connection string secured (not in source code)
- [ ] Environment set to "Production"
- [ ] Logging configured
- [ ] Error handling tested

### **Database**
- [ ] Production database created
- [ ] Migrations applied
- [ ] Backup strategy in place
- [ ] Data seeding configured
- [ ] Connection pooling optimized

### **Infrastructure**
- [ ] SSL certificate installed
- [ ] Firewall rules configured
- [ ] Health checks enabled
- [ ] Auto-scaling configured (if applicable)
- [ ] CDN configured (for static files)

### **Post-Deployment**
- [ ] Smoke tests passed
- [ ] API responding correctly
- [ ] Database accessible
- [ ] Monitoring active
- [ ] Alerts configured

---

## 🆘 Rollback Plan

If deployment fails:

```bash
# Azure
az webapp deployment slot swap \
  --resource-group pharmacy-rg \
  --name iti-pharmacy-api \
  --slot staging \
  --target-slot production

# Docker
docker-compose down
docker-compose up -d --build

# IIS
# Restore previous published folder from backup
```

---

## 📞 Support

For production issues:
1. Check application logs
2. Review Azure Application Insights (if enabled)
3. Check database connectivity
4. Verify environment variables
5. Review recent deployments

---

**🎉 Your production deployment is complete!**

Remember to monitor your application and set up alerts for critical issues.

