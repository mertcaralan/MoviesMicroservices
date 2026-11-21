using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the IoC container.
builder.Services.AddDbContext<DbContext, MoviesDb>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString(nameof(MoviesDb))));
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(MoviesDb).Assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
