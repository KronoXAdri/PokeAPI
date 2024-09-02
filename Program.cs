using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PokeAPI.Data;
using PokeAPI.PokemonsMapper;
using PokeAPI.Repositorio;
using PokeAPI.Repositorio.IRepositorio;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Para agregar los repositorios
builder.Services.AddScoped<IPokemonRepositorio, PokemonRepositorio>();
builder.Services.AddScoped<IEntrenadorRepositorio, EntrenadorRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

String key = builder.Configuration.GetValue<String>("AppiSettings:Secreta");

//Agregar Automapper
builder.Services.AddAutoMapper(typeof(PokemonMapper));

//Aquí se configura la autenticación
builder.Services.AddAuthentication(
        x => 
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    ).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false  
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Autenticación JWT usando el esquema Bearer. \r\n\r\n " +
            "Ingresa la palabra 'Bearer' seguido de un [espacio] y después su token en el campo de abajo. \r\n\r\n" +
            "Ejemplo: \"Bearer tklkhjhkiu12\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<String>()
            }
        });
    }
);

//Soporte para CORS
//Se puede habilitar: 1-Un dominio, 2-Multiples dominios, 3-Cualquier dominio (Tener en cuenta seguridad)
//Uso de ejemplo el dominio: http://localhost:8888, se debe cambiar por el correcto 
//Se usa (*) para todos los dominios
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("PoliticaCors",
//        build =>
//        {
//            //build.WithOrigins("http://localhost:7172").AllowAnyMethod().AllowAnyHeader();
//            build.WithOrigins("http://prueba").AllowAnyMethod().AllowAnyHeader();
//        });
//});

builder.Services.AddDbContext<PokemonContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PokemonConection"));

});

    var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<PokemonContext>();

    dataContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//Soporte para CORS
//app.UseRouting();
//app.UseCors("PoliticaCors");
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers().RequireCors("PoliticaCors");
//});

//Sirve para la autenticación
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
