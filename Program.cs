using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
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

builder.Services.AddControllers(option =>
{
    //Cache profile. Un cache global para evitar ponerlo en todas partes 
    option.CacheProfiles.Add("PorDefecto20Segundos", new CacheProfile(){ Duration = 20 });
});

//Soporte para Caché
//builder.Services.AddResponseCaching();

// Para agregar los repositorios
builder.Services.AddScoped<IPokemonRepositorio, PokemonRepositorio>();
builder.Services.AddScoped<IEntrenadorRepositorio, EntrenadorRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

String key = builder.Configuration.GetValue<String>("AppiSettings:Secreta");

//Agregar Automapper
builder.Services.AddAutoMapper(typeof(PokemonMapper));

//Soporte para versiones
//var apiVersioningBuilder = builder.Services.AddApiVersioning(opcion => {
//    opcion.AssumeDefaultVersionWhenUnspecified = true;
//    opcion.DefaultApiVersion = new ApiVersion(1, 0);
//    opcion.ReportApiVersions = true;
//    opcion.ApiVersionReader = ApiVersionReader.Combine(
//        new QueryStringApiVersionReader("api-version") // api-version=1.0 || 2.0 || etc
//    //);
//});
//apiVersioningBuilder.AddApiExplorer(opciones => {
//    opciones.GroupNameFormat = "'v'VVV";
//    opciones.SubstituteApiVersionInUrl = true;
//});

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
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiPokemon", Version = "v1" });
        options.SwaggerDoc("v2", new OpenApiInfo { Title = "ApiPokemon", Version = "v2" });
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
