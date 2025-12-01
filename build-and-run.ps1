# PowerShell script to build and run the project
# Run this from the solution root directory

Write-Host "Building ITI Pharmacy API..." -ForegroundColor Green

# Navigate to solution root
$solutionRoot = "c:\Users\hp\Documents\GitHub\ITI-GR-Project-backened"
Set-Location $solutionRoot

# Build the project
Write-Host "Running: dotnet build --project 'ITI Pharmacy API\ITI Pharmacy API.csproj'" -ForegroundColor Yellow
dotnet build --project "ITI Pharmacy API\ITI Pharmacy API.csproj"

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nBuild succeeded! Starting the application..." -ForegroundColor Green
    Write-Host "Press Ctrl+C to stop the server" -ForegroundColor Yellow
    Write-Host "`n" -NoNewline
    
    # Run the project
    dotnet run --project "ITI Pharmacy API\ITI Pharmacy API.csproj"
} else {
    Write-Host "`nBuild failed. Please check the errors above." -ForegroundColor Red
}
