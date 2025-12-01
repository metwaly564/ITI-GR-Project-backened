# PowerShell script to apply migration
# Run this script from the solution root directory

Write-Host "Applying migration: AddRankAndMaxOrderQuantity" -ForegroundColor Green

# Navigate to solution root
$solutionRoot = "c:\Users\hp\Documents\GitHub\ITI-GR-Project-backened"
Set-Location $solutionRoot

# Apply migration
Write-Host "Running: dotnet ef database update" -ForegroundColor Yellow
dotnet ef database update --project "ITI Pharmacy API\ITI Pharmacy API.csproj" --startup-project "ITI Pharmacy API\ITI Pharmacy API.csproj"

if ($LASTEXITCODE -eq 0) {
    Write-Host "Migration applied successfully!" -ForegroundColor Green
} else {
    Write-Host "Migration failed. Check errors above." -ForegroundColor Red
    Write-Host "You can apply the migration manually using APPLY_MIGRATION.sql" -ForegroundColor Yellow
}
