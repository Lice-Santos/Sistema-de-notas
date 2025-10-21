using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SafeScribe_cp05.Services;
using SafeScribe_cp05.Interface;
using Microsoft.OpenApi.Models; // Necess�rio para a configura��o do Swagger/JWT

var builder = WebApplication.CreateBuilder(args);

// Note: A leitura de chaves aqui � opcional, pois voc� a usa diretamente abaixo.
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 1. CONFIGURA��O DO SWAGGER PARA AUTORIZA��O JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SafeScribe API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {token}"
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
            new string[] {}
        }
    });
});
// FIM da configura��o do Swagger

// 2. REGISTRO DE SERVI�OS
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<INoteService, NoteService>();

// 3. CONFIGURA��O DA AUTENTICA��O JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Define que o emissor ('iss') do token deve ser checado.
            ValidateIssuer = true,

            // Define que a audi�ncia ('aud') do token deve ser checada.
            ValidateAudience = true,

            // Garante que o token n�o est� expirado ('exp') e � v�lido no tempo atual.
            ValidateLifetime = true,

            // Regra cr�tica: Garante que a assinatura do token � v�lida.
            // Isso evita adultera��o do token.
            ValidateIssuerSigningKey = true,

            // --- Configura��es dos Valores ---

            // O valor exato esperado para o emissor (lido do appsettings).
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            // O valor exato esperado para a audi�ncia (lido do appsettings).
            ValidAudience = builder.Configuration["Jwt:Audience"],

            // A Chave Secreta usada para verificar a assinatura digital do token.
            // A chave � lida da config e convertida em bytes.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization(); // Adiciona o servi�o de autoriza��o

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 4. ORDEM CORRETA DO MIDDLEWARE � FUNDAMENTAL:
// Authentication DEVE vir antes de Authorization!

app.UseAuthentication(); // <-- ESSENCIAL! Adiciona a l�gica de valida��o do token

app.UseAuthorization();  // Adiciona a l�gica de verifica��o de permiss�o ([Authorize])

app.MapControllers();

app.Run();