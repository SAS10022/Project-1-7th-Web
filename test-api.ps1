# =====================================================
# Course Management API - Automated Test Suite
# =====================================================
# Save as: test-api.ps1
# Run: .\test-api.ps1

Write-Host "`n╔════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║   Course Management API - Test Suite      ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════╝`n" -ForegroundColor Cyan

# Configuration
$API_URL = "http://localhost:5035/api"
$USERNAME = "testdemo" + (Get-Random 10000)
$EMAIL = "test$((Get-Random 10000))@example.com"
$PASSWORD = "TestPass123!"

Write-Host "Configuration:" -ForegroundColor Yellow
Write-Host "  - API URL: $API_URL"
Write-Host "  - Test User: $USERNAME"
Write-Host "  - Test Email: $EMAIL`n"

# ============================================================
# TEST 1: REGISTER
# ============================================================
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Green
Write-Host "TEST 1: User Registration" -ForegroundColor Green
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━`n" -ForegroundColor Green

$registerBody = @{
    username = $USERNAME
    email = $EMAIL
    password = $PASSWORD
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$API_URL/auth/register" `
        -Method Post `
        -Headers @{"Content-Type"="application/json"} `
        -Body $registerBody `
        -ErrorAction Stop

    Write-Host "✅ Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "✅ Message:" (($response.Content | ConvertFrom-Json).message) -ForegroundColor Green
} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    exit 1
}

# ============================================================
# TEST 2: LOGIN
# ============================================================
Write-Host "`n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Green
Write-Host "TEST 2: User Login (Token in Header)" -ForegroundColor Green
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━`n" -ForegroundColor Green

$loginBody = @{
    username = $USERNAME
    password = $PASSWORD
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$API_URL/auth/login" `
        -Method Post `
        -Headers @{"Content-Type"="application/json"} `
        -Body $loginBody `
        -ErrorAction Stop

    Write-Host "✅ Status: $($response.StatusCode)" -ForegroundColor Green
    
    # Extract and display token from header
    $token = $response.Headers["X-Access-Token"]
    Write-Host "`n🔐 TOKEN IN HEADER:" -ForegroundColor Cyan
    Write-Host "Header Name: X-Access-Token" -ForegroundColor Cyan
    Write-Host "Token Preview: $($token.Substring(0, 50))..." -ForegroundColor Cyan
    Write-Host "Token Length: $($token.Length) characters`n" -ForegroundColor Cyan
    
    # Display response body
    Write-Host "👤 USER DATA IN BODY:" -ForegroundColor Yellow
    $userData = $response.Content | ConvertFrom-Json
    Write-Host "  - User ID: $($userData.userId)" -ForegroundColor Yellow
    Write-Host "  - Username: $($userData.username)" -ForegroundColor Yellow
    Write-Host "  - Email: $($userData.email)" -ForegroundColor Yellow
    Write-Host "  - Role: $($userData.role)" -ForegroundColor Yellow
    
    # Check if token is in body
    if ($response.Content -like "*token*" -or $response.Content -like "*Token*") {
        Write-Host "`n❌ ERROR: Token found in response body!" -ForegroundColor Red
        exit 1
    } else {
        Write-Host "`n✅ VERIFIED: Token NOT in response body (Secure!)" -ForegroundColor Green
    }

} catch {
    Write-Host "❌ Error: $_" -ForegroundColor Red
    exit 1
}

# ============================================================
# SUMMARY
# ============================================================
Write-Host "`n╔════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║  ✅ ALL TESTS PASSED - APIS WORKING!      ║" -ForegroundColor Green
Write-Host "║                                            ║" -ForegroundColor Green
Write-Host "║  ✓ Registration endpoint working           ║" -ForegroundColor Green
Write-Host "║  ✓ Login endpoint working                  ║" -ForegroundColor Green
Write-Host "║  ✓ Token in header (X-Access-Token)       ║" -ForegroundColor Green
Write-Host "║  ✓ Token NOT in response body              ║" -ForegroundColor Green
Write-Host "║  ✓ Security implementation verified        ║" -ForegroundColor Green
Write-Host "║                                            ║" -ForegroundColor Green
Write-Host "║  📋 Documentation files created:          ║" -ForegroundColor Green
Write-Host "║     - API_TEST_RESULTS.md                 ║" -ForegroundColor Green
Write-Host "║     - EVIDENCE_WORKING_APIS.md             ║" -ForegroundColor Green
Write-Host "║     - QUICK_REFERENCE.md                   ║" -ForegroundColor Green
Write-Host "║     - test-api.ps1 (This file)            ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════╝`n" -ForegroundColor Green

Write-Host "Next Steps:" -ForegroundColor Cyan
Write-Host "1. Open Swagger UI: http://localhost:5035/swagger/index.html" -ForegroundColor Cyan
Write-Host "2. Test the endpoints manually in Swagger" -ForegroundColor Cyan
Write-Host "3. Review the documentation files for detailed info" -ForegroundColor Cyan
