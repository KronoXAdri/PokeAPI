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


//Agregar Automapper
builder.Services.AddAutoMapper(typeof(PokemonMapper));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
