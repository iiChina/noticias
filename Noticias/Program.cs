using Noticias.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();


var client = new MongoClient("mongodb://localhost:27017");
var db = client.GetDatabase("jornal");
var collection = db.GetCollection<Noticia>("noticias");

app.MapGet("noticia", async () =>
{
    var filter = Builders<Noticia>.Filter.Empty;
    var cursor = await collection.FindAsync(filter);
    var noticias = await cursor.ToListAsync();
    return noticias.Any() ? Results.Ok(noticias) : Results.NoContent();
});

app.MapGet("noticia/{id}", async (string id) =>
{
    var filter = Builders<Noticia>.Filter.Eq(n => n.Id, id);
    var cursor = await collection.FindAsync(filter);
    var noticia = await cursor.FirstOrDefaultAsync();
    return noticia == null ? Results.NoContent() : Results.Ok(noticia);

});

app.MapPost("noticia", async (Noticia noticia) =>
{
    if (noticia == null)
        return Results.BadRequest();
    await collection.InsertOneAsync(noticia);
    return Results.Created($"https://localhost:7019/noticia/{noticia.Id}", noticia);
});

app.MapPut("noticia/{id}", async (string id, Noticia noticia) =>
{
    if (noticia == null)
        return Results.BadRequest();
    var filter = Builders<Noticia>.Filter.Eq(n => n.Id, id);
    var update = Builders<Noticia>.Update
        .Set(n => n.Titulo, noticia.Titulo)
        .Set(n => n.Autor, noticia.Resumo)
        .Set(n => n.Texto, noticia.Texto)
        .Set(n => n.Autor, noticia.Autor);
    var result = await collection.UpdateOneAsync(filter, update);
    return Results.Ok();
});

app.MapDelete("noticia/{id}", async (string id) =>
{
    var filter = Builders<Noticia>.Filter.Eq(n => n.Id, id);
    await collection.DeleteOneAsync(filter);
    return Results.Ok();
});

app.Run();
