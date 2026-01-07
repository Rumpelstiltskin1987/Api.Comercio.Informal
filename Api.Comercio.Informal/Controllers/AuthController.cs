using Api.Business;
using Api.Entities;
using Api.Entities.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<Usuario> _userManager;
    private readonly IConfiguration _config;

    public AuthController(UserManager<Usuario> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] DtoLoginRequest model)
    {
        //var user = await _userManager.FindByEmailAsync(model.Email);
        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            // Obtenemos los roles para incluirlos en el token
            var roles = await _userManager.GetRolesAsync(user);

            // Generamos el token
            var tokenString = GenerarJwtToken(user, roles);

            // Respondemos con el objeto que Android espera (LoginResponse.kt)
            return Ok(new
            {
                Token = tokenString,
                IdCobrador = user.Id,
                User = user.UserName,
                Rol = roles.FirstOrDefault() 
            });
        }

        return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });
    }

    private string GenerarJwtToken(Usuario user, IList<string> roles)
    {
        // 1. Definir los "Claims" (Aseveraciones de identidad)
        // Estos datos viajarán dentro del token y Android podrá leerlos.
        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Este es tu idCobrador
        new Claim(ClaimTypes.Email, user.Email!)
    };

        // Agregar los roles del usuario al token
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // 2. Obtener la llave secreta desde appsettings.json
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 3. Configurar el tiempo de expiración (Usamos los 480 minutos definidos)
        var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"]));

        // 4. Crear el Token
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}