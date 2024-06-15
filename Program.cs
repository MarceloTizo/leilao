using Microsoft.EntityFrameworkCore;
using AuctionAPI.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao container.
builder.Services.AddControllers(); // Adiciona suporte a controladores e API's

// Configurar DbContext com MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 21)))); // Ajuste a versão do MySQL conforme necessário

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar o pipeline de requisições HTTP

// Mover a configuração do Swagger para fora do bloco de desenvolvimento
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auction API V1");
    c.RoutePrefix = string.Empty; 
});

app.UseHttpsRedirection();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();
