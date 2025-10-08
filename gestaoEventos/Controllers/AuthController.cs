using Microsoft.AspNetCore.Mvc;  // Necessário para o uso de controladores e construção de APIs RESTful
using Microsoft.IdentityModel.Tokens;  // Necessário para trabalhar com tokens de segurança, incluindo JWT
using System;  // Inclui funcionalidades básicas como DateTime
using System.IdentityModel.Tokens.Jwt;  // Inclui classes para manipulação de tokens JWT
using System.Security.Claims;  // Inclui classes para manipulação de declarações de segurança (claims)
using System.Text;  // Necessário para manipulação de strings, como a codificação de chaves

// Define a rota base para o controlador como "api/[controller]", que será "api/auth" devido ao nome do controlador
[Route("api/[controller]")]
[ApiController]  // Indica que este controlador é uma API e adiciona funcionalidades automáticas, como validação de modelo
public class AuthController : ControllerBase  // Define o controlador AuthController, que herda de ControllerBase
{
    // Define um endpoint HTTP POST com a rota "api/auth/login"
    [HttpPost("login")]
    public IActionResult Login()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(
            HttpContext.RequestServices.GetRequiredService<IConfiguration>()["Jwt:Key"]
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "user") }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }
}