# Stagora

« Accueil », l'assistant IA d'une église, développé en C# / .NET.

Il répond aux questions des visiteurs (heures des cultes, événements, groupes),
cherche dans une base de connaissances (RAG avec pgvector), inscrit les personnes
aux événements et transfère à un responsable humain tout ce qui est personnel ou délicat.

## Stack
- .NET 8+, ASP.NET Core Minimal API
- Claude (SDK C# officiel `Anthropic`)
- PostgreSQL + pgvector (Npgsql, Pgvector)
- Embeddings Voyage AI
- Tests d'évaluation avec xUnit
