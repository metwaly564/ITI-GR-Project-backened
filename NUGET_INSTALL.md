# NuGet Package Installation Guide

## Install Health Checks Package

In **NuGet Package Manager Console**, run:

```powershell
Install-Package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore -Version 8.0.0
```

Or if you want the latest version:

```powershell
Install-Package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
```

---

## Alternative: Using dotnet CLI

If you prefer using command line:

```powershell
cd "c:\Users\hp\Documents\GitHub\ITI-GR-Project-backened\ITI Pharmacy API"
dotnet add package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore --version 8.0.0
```

---

## After Installation

After installing the package, you can:

1. **Restore packages:**
   ```powershell
   dotnet restore
   ```

2. **Build the project:**
   ```powershell
   dotnet build
   ```

---

## Note

The package is already added to `.csproj` file, but you need to restore/install it using NuGet.
