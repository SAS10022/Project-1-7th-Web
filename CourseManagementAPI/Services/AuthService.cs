using CourseManagementAPI.Auth;
using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    Task<bool> RegisterAsync(RegisterDto registerDto);
    Task<bool> UserExistsAsync(string username, string email);
}

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        ApplicationDbContext context,
        IPasswordService passwordService,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _passwordService = passwordService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

        if (user == null || !_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
            return null;

        var token = _jwtTokenService.GenerateToken(user);

        return new LoginResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Token = token
        };
    }

    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        if (await UserExistsAsync(registerDto.Username, registerDto.Email))
            return false;

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = _passwordService.HashPassword(registerDto.Password),
            Role = "User"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UserExistsAsync(string username, string email)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Username == username || u.Email == email);
    }
}
