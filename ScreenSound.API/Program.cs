using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Data.SqlTypes;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
/*
 Durante a serializa��o, se houver ciclos de refer�ncia (por exemplo, um objeto A referenciando um objeto B, 
 que por sua vez referencia o objeto A), esses ciclos ser�o ignorados em vez de causar uma exce��o.
 */
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


/*
 O Asp.NET vai criar esses objetos para utilizarmos na aplica��o.
 Isso � chamado de inje��o de depend�ncia e evitou a duplica��o do c�digo abaixo em cada m�todo.

 var dal = new DAL<Artista>(new ScreenSoundContext());

 Inje��o de Depend�ncia � um padr�o de design que permite que objetos recebam suas depend�ncias de 
 uma fonte externa ao inv�s de criarem-nas diretamente. Isso facilita o gerenciamento das depend�ncias
 e promove um c�digo mais modular, test�vel e f�cil de manter.
*/
builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();

var app = builder.Build();

// FromServices recupera o objeto criado pelo Asp.NET
app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
{
    return Results.Ok(dal.Listar());
});

app.MapGet("/Artistas/{nome}", ([FromServices] DAL < Artista > dal, string nome) =>
{
    var artista = dal.RecuperarPor(a => a.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)); // "StringComparison.OrdinalIgnoreCase" ignora o case sensitive

    if (artista is null)
    {
        return Results.NotFound(new { Message = "Artista n�o encontrado" });
    }

    return Results.Ok(artista);
});

app.MapPost("/Artistas", ([FromServices] DAL < Artista > dal, [FromBody] Artista artista) => // "FromBody" indica que os dados vem no corpo da requisi��o
{
    dal.Adicionar(artista);
    return Results.Ok();
});

app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> dal, int id) => {
    var artista = dal.RecuperarPor(a => a.Id == id);

    if (artista is null)
    {
        return Results.NotFound();
    }

    dal.Deletar(artista);
    
    return Results.NoContent();

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