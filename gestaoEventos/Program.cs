using APIMongoDB.Model;
using APIMongoDB.Services;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------
// CONFIGURAÇÃO DE AUTENTICAÇÃO JWT
// ------------------------------------------------------------

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,                     // Verifica se a assinatura do token é válida (obrigatório)
        IssuerSigningKey = new SymmetricSecurityKey(key),    // Define a chave usada para validar a assinatura
        ValidateIssuer = false,                              // Desativa a validação do emissor (pode ser true em produção)
        ValidateAudience = false                              // Desativa a validação da audiência (pode ser true em produção)
    };

});

// ------------------------------------------------------------
// CONFIGURAÇÃO DO MONGODB
// ------------------------------------------------------------

builder.Services.Configure<ConfiguracaoMongoDB>(
    builder.Configuration.GetSection("ConfiguracaoMongoDb"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var config = sp.GetRequiredService<IOptions<ConfiguracaoMongoDB>>().Value;
    return new MongoClient(config.StringConexao);
});

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IOptions<ConfiguracaoMongoDB>>().Value;
    var cliente = sp.GetRequiredService<IMongoClient>();
    var banco = cliente.GetDatabase(config.NomeBanco);
    return banco.GetCollection<Evento>(config.NomeColecaoAlunos);
});

builder.Services.AddSingleton<ServicoEventos>();

// ------------------------------------------------------------
// CONFIGURAÇÃO DO SWAGGER COM JWT
// ------------------------------------------------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MottuBracelet API com JWT",
        Version = "v1",
        Description = "API MongoDB + JWT Authentication"
    });

    // Adiciona autenticação JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira 'Bearer' [espaço] e o token JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ------------------------------------------------------------
// CONSTRUÇÃO E EXECUÇÃO DO APP
// ------------------------------------------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MottuBracelet API v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Deve vir antes do Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
