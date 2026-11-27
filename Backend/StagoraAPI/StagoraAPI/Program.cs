using Microsoft.Extensions.AI;
using OpenAI;
using StagoraAPI.Models;
using System.ClientModel;
using StagoraAPI.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<BotOptions>(
    builder.Configuration.GetSection(BotOptions.Section));

// 3a. Lire les valeurs dont on a besoin MAINTENANT
var botOptions = builder.Configuration
    .GetSection(BotOptions.Section)
    .Get<BotOptions>() ?? new BotOptions();

var groqApiKey = builder.Configuration["Groq:ApiKey"];

// 3b. Échouer tout de suite si la clé manque
if (string.IsNullOrWhiteSpace(groqApiKey))
{
    throw new InvalidOperationException(
        "La clé Groq:ApiKey est manquante. Ajoute-la dans les secrets utilisateur.");
}

// 3c. Créer le client Groq et l'enregistrer sous l'interface IChatClient
builder.Services.AddSingleton<IChatClient>(
    new OpenAIClient(
            new ApiKeyCredential(groqApiKey),
            new OpenAIClientOptions { Endpoint = new Uri(botOptions.Endpoint) })
        .GetChatClient(botOptions.Model)
        .AsIChatClient());
builder.Services.AddScoped<BotService>();

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
