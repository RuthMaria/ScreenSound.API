using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.MapGet("/Artistas", () =>
{
    var dal = new DAL<Artista>(new ScreenSoundContext());
    return Results.Ok(dal.Listar());
});

app.MapGet("/Artistas/{nome}", (string nome) =>
{
    var dal = new DAL<Artista>(new ScreenSoundContext());
    var artista = dal.RecuperarPor(a => a.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)); // "StringComparison.OrdinalIgnoreCase" ignora o case sensitive

    if (artista is null)
    {
        return Results.NotFound(new { Message = "Artista não encontrado" });
    }

    return Results.Ok(artista);
});

app.MapPost("/Artistas", ([FromBody] Artista artista) => // "FromBody" indica que os dados vem no corpo da requisição
{
    var dal = new DAL<Artista>(new ScreenSoundContext());
    dal.Adicionar(artista);
    return Results.Ok();
});

app.Run();

/*
 Os códigos de retorno HTTP são códigos de três dígitos que o servidor web envia para o navegador indicando o resultado da 
 solicitação feita por ele. Esses códigos são divididos em classes que representam categorias diferentes. São elas:

    - Códigos informativos (100 a 199): indicam que a solicitação do cliente foi recebida e está sendo processada;
    - Códigos de sucesso (200 a 299): indicam que o servidor concluiu com sucesso a ação solicitada;
    - Códigos de redirecionamento (300 a 399): indicam que o cliente precisa tomar ações adicionais para completar a solicitação;
    - Códigos de erro do cliente (400 a 499): indicam que ocorreu um erro na solicitação do cliente;
    - Códigos de erro do servidor (500 a 599): indicam que ocorreu um erro no servidor ao processar a solicitação do cliente.
 */