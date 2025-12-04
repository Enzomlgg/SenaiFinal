using BarraByTechAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- ADICIONA O SERVIÇO CORS (Necessário para comunicação entre portas diferentes) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            // Permite requisições de qualquer origem, método e cabeçalho.
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
// ----------------------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// O UseRouting é implicitamente executado, mas é um bom costume posicioná-lo
// para garantir que o UseCors venha no momento certo.
app.UseRouting();
app.UseCors();
// --- ATIVA O MIDDLEWARE CORS (Deve vir depois de UseRouting e antes de UseAuthorization) ---
app.UseCors("AllowFrontend");
// ---------------------------------------------------------------------------------------

app.UseAuthorization();

app.MapControllers();

app.Run();