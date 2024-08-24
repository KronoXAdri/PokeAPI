using Microsoft.EntityFrameworkCore;
using PokeAPI.Data;
using PokeAPI.PokemonsMapper;
using PokeAPI.Repositorio;
using PokeAPI.Repositorio.IRepositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Para agregar los repositorios
builder.Services.AddScoped<IPokemonRepositorio, PokemonRepositorio>();
builder.Services.AddScoped<IEntrenadorRepositorio, EntrenadorRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();


//Agregar Automapper
builder.Services.AddAutoMapper(typeof(PokemonMapper));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
