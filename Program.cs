//using CatalogoRoupas.Data;
using CatalogoRoupas.Servicos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient("BFFCatalogoRoupas", client =>
{
    client.BaseAddress = new Uri("https://localhost:7122/mensageria/RoupaMensageria/"); // URL do BFF
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<RabbitMQConsumerService>();
builder.Services.AddSingleton<RabbitMQPublisher>();

builder.Services.AddSingleton<RoupasService>();
// Adicionar o contexto do banco de dados
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

public partial class Program { }