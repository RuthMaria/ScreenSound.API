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
        return Results.NotFound(new { Message = "Artista n�o encontrado" });
    }

    return Results.Ok(artista);
});

app.MapPost("/Artistas", ([FromBody] Artista artista) => // "FromBody" indica que os dados vem no corpo da requisi��o
{
    var dal = new DAL<Artista>(new ScreenSoundContext());
    dal.Adicionar(artista);
    return Results.Ok();
});

app.Run();

/*
 Os c�digos de retorno HTTP s�o c�digos de tr�s d�gitos que o servidor web envia para o navegador indicando o resultado da 
 solicita��o feita por ele. Esses c�digos s�o divididos em classes que representam categorias diferentes. S�o elas:

    - C�digos informativos (100 a 199): indicam que a solicita��o do cliente foi recebida e est� sendo processada;
    - C�digos de sucesso (200 a 299): indicam que o servidor concluiu com sucesso a a��o solicitada;
    - C�digos de redirecionamento (300 a 399): indicam que o cliente precisa tomar a��es adicionais para completar a solicita��o;
    - C�digos de erro do cliente (400 a 499): indicam que ocorreu um erro na solicita��o do cliente;
    - C�digos de erro do servidor (500 a 599): indicam que ocorreu um erro no servidor ao processar a solicita��o do cliente.
 */