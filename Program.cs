using Microsoft.EntityFrameworkCore;
using SavalAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrar AppDbContext con la cadena de conexi�n
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar soporte para controladores
builder.Services.AddControllers();

// Agregar soporte para Swagger (Documentaci�n de API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar Swagger en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configurar middleware
app.UseHttpsRedirection();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

// Log para verificar que el servidor est� corriendo
Console.WriteLine("Servidor iniciado en: " + string.Join(", ", app.Urls));

// Iniciar la aplicaci�n
app.Run();
