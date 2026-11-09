using AgentesIA.Api.Configurations;
using AgentesIA.Api.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddOpenApi();

// registrando a classe de conexão com o PostgreSQL
builder.Services.AddPostgreSql(
builder.Configuration
);

// registrando o serviço de geração de Embeddings
builder.Services.AddSingleton<EmbeddingService>();

// registrando o serviço que grava conteúdo na base de conhecimento
builder.Services.AddScoped<ConhecimentoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
