# Build Fix Guide

## Problem
When running `dotnet build` from inside the `ITI Pharmacy API` folder, you get:
```
error MSB3202: The project file "c:\Users\hp\Documents\GitHub\ITI-GR-Project-backened\ITI Pharmacy API\ITI Pharmacy API\ITI Pharmacy API.csproj" was not found.
```

## Solution

### Option 1: Use Solution Root (Recommended)
Run commands from the **solution root** directory:

```powershell
# Navigate to solution root
cd "c:\Users\hp\Documents\GitHub\ITI-GR-Project-backened"

# Build
dotnet build --project "ITI Pharmacy API\ITI Pharmacy API.csproj"

# Run
dotnet run --project "ITI Pharmacy API\ITI Pharmacy API.csproj"
```

### Option 2: Use PowerShell Script
Run the provided script:
```powershell
.\build-and-run.ps1
```

### Option 3: Use Solution File
```powershell
cd "c:\Users\hp\Documents\GitHub\ITI-GR-Project-backened"
dotnet build "ITI Pharmacy API.sln"
dotnet run --project "ITI Pharmacy API\ITI Pharmacy API.csproj"
```

### Option 4: Use Full Path
```powershell
dotnet build "c:\Users\hp\Documents\GitHub\ITI-GR-Project-backened\ITI Pharmacy API\ITI Pharmacy API.csproj"
```

---

## Quick Commands

**Build:**
```powershell
cd "c:\Users\hp\Documents\GitHub\ITI-GR-Project-backened"
dotnet build --project "ITI Pharmacy API\ITI Pharmacy API.csproj"
```

**Run:**
```powershell
cd "c:\Users\hp\Documents\GitHub\ITI-GR-Project-backened"
dotnet run --project "ITI Pharmacy API\ITI Pharmacy API.csproj"
```

**Build & Run (one command):**
```powershell
cd "c:\Users\hp\Documents\GitHub\ITI-GR-Project-backened"
dotnet run --project "ITI Pharmacy API\ITI Pharmacy API.csproj"
```

---

## Why This Happens

The `.sln` file references the project as:
```
"ITI Pharmacy API\ITI Pharmacy API.csproj"
```

When you're inside the `ITI Pharmacy API` folder, dotnet tries to find:
```
"ITI Pharmacy API\ITI Pharmacy API\ITI Pharmacy API.csproj"  ❌ (wrong path)
```

But the actual path is:
```
"ITI Pharmacy API\ITI Pharmacy API.csproj"  ✅ (correct path)
```

**Solution:** Always run commands from the solution root directory, or use the `--project` flag with the relative path.

---

**Last Updated:** January 2025
