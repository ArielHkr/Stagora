using System.Threading.RateLimiting;
using Microsoft.AspNetCore.HttpOverrides;
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
        "La clé Groq:ApiKey est manquante. Ajoute-la dans les secrets utilisateur (développement) " +
        "ou dans la variable d'environnement Groq__ApiKey (production).");
}

// 3c. Créer le client Groq et l'enregistrer sous l'interface IChatClient
builder.Services.AddSingleton<IChatClient>(
    new OpenAIClient(
            new ApiKeyCredential(groqApiKey),
            new OpenAIClientOptions { Endpoint = new Uri(botOptions.Endpoint) })
        .GetChatClient(botOptions.Model)
        .AsIChatClient());
builder.Services.AddScoped<BotService>();

// CORS : autorise le front-end (Vue) à appeler l'API depuis une autre adresse.
// Les adresses autorisées sont dans appsettings.json, section "Cors:AllowedOrigins".
const string PolitiqueFrontend = "Frontend";
var originesAutorisees = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
    options.AddPolicy(PolitiqueFrontend, politique => politique
        .WithOrigins(originesAutorisees)
        .WithMethods("POST")
        .AllowAnyHeader()));

// Limite de requêtes : protège le quota Groq contre l'abus et le spam.
// Chaque adresse IP a droit à N messages par minute (appsettings.json, "RateLimit").
const string PolitiqueChat = "Chat";
var requetesParMinute = builder.Configuration.GetValue("RateLimit:RequetesParMinute", 10);

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy(PolitiqueChat, contexte =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: contexte.Connection.RemoteIpAddress?.ToString() ?? "inconnue",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = requetesParMinute,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
            }));
});

// Derrière l'hébergeur (Render), les requêtes passent par un proxy :
// on lit la vraie IP du visiteur et le protocole (https) dans les en-têtes X-Forwarded-*.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // L'adresse du proxy de l'hébergeur n'est pas connue à l'avance : on fait confiance à l'en-tête.
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Route /health : l'hébergeur l'appelle pour vérifier que l'API répond.
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// En développement, le front passe par le proxy de Vite en HTTP :
// une redirection vers HTTPS casserait ces appels.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors(PolitiqueFrontend);

// Après CORS : une réponse 429 garde ses en-têtes CORS, et le front peut l'afficher.
app.UseRateLimiter();

app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
