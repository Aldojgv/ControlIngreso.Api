using ControlIngresoApp.Repository;
using ControlIngresoApp.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting; 

var builder = WebApplication.CreateBuilder(args);

// Swagger y documentación
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Servicios personalizados
builder.Services.AddSingleton<IngresoControlConnection>();
builder.Services.AddSingleton<DbInitializer>();
builder.Services.AddScoped<IParametro, ParametroRepository>();
builder.Services.AddScoped<IEmpresa, EmpresaRepository>();
builder.Services.AddScoped<IRegistro, RegistroRepository>();
builder.Services.AddScoped<IPersona, PersonaRepository>();
builder.Services.AddScoped<IPais, PaisRepository>();
builder.Services.AddCors(options =>
{
options.AddPolicy("PermitirTodo", policy =>
{
    policy
        .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost") // acepta cualquier localhost
        .AllowAnyHeader()
        .AllowAnyMethod();
});
});

builder.Services.AddControllers();
var app = builder.Build();
 app.UseCors("PermitirTodo");
app.UseRouting();

// Middleware y entorno
if (app.Environment.IsDevelopment())
{
   
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseHttpsRedirection();

    // Inicializa la base de datos con las tablas y datos base
    using (var scope = app.Services.CreateScope())
    {
        var dbInit = scope.ServiceProvider.GetRequiredService<DbInitializer>();
        await dbInit.InicializarAsync();
    }

}
    app.MapControllers();
    app.Run();