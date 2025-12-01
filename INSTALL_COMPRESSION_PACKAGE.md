# Install Response Compression Package

## In NuGet Package Manager Console

Run this command:

```powershell
Install-Package Microsoft.AspNetCore.ResponseCompression -Version 2.2.2
```

Or for the latest version (compatible with .NET 8.0):

```powershell
Install-Package Microsoft.AspNetCore.ResponseCompression
```

---

## Alternative: Using dotnet CLI

```powershell
cd "c:\Users\hp\Documents\GitHub\ITI-GR-Project-backened\ITI Pharmacy API"
dotnet add package Microsoft.AspNetCore.ResponseCompression --version 2.2.2
```

---

## Note

In ASP.NET Core 8.0, Response Compression should be built-in, but if you're getting errors, install the package explicitly.

After installation, run:
```powershell
dotnet restore
dotnet build
```
