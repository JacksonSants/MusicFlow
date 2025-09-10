using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MongoDB.Driver;
using MusicFlow.Data.ContextApp;
using MusicFlow.Data.Service.UserService;
using MusicFlow.Model.User;
using MusicFlow.Data.Service.Authentication;
using MusicFlow.Data.Service.ArtistService;
using MusicFlow.Model.Artists;

var builder = WebApplication.CreateBuilder(args);

//adiciona serviços de banco de dados
builder.Services.AddSingleton<MongoContextApp>();

builder.Services.AddScoped<IMongoCollection<User>>(provider =>
{
    var context = provider.GetRequiredService<MongoContextApp>();
    return context.Users;
});

builder.Services.AddScoped<IMongoCollection<Artist>>(provider =>
{
    var context = provider.GetRequiredService<MongoContextApp>();
    return context.Artists;
});


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<Authentication>();
builder.Services.AddScoped<IArtistService, ArtistService>();



// Configuração do JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

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
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Adiciona os serviços para controllers
builder.Services.AddControllers().AddNewtonsoftJson() ;

// **Registra os serviços do Swagger antes do builder.Build()**
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Middleware para Swagger em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
