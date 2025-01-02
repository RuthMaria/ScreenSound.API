using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Data.SqlTypes;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
/*
 Durante a serialização, se houver ciclos de referência (por exemplo, um objeto A referenciando um objeto B, 
 que por sua vez referencia o objeto A), esses ciclos serão ignorados em vez de causar uma exceção.
 */
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


/*
 O Asp.NET vai criar esses objetos para utilizarmos na aplicação.
 Isso é chamado de injeção de dependência e evitou a duplicação do código abaixo em cada método.

 var dal = new DAL<Artista>(new ScreenSoundContext());

 Injeção de Dependência é um padrão de design que permite que objetos recebam suas dependências de 
 uma fonte externa ao invés de criarem-nas diretamente. Isso facilita o gerenciamento das dependências
 e promove um código mais modular, testável e fácil de manter.
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
        return Results.NotFound(new { Message = "Artista não encontrado" });
    }

    return Results.Ok(artista);
});

app.MapPost("/Artistas", ([FromServices] DAL < Artista > dal, [FromBody] Artista artista) => // "FromBody" indica que os dados vem no corpo da requisição
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
 Os códigos de retorno HTTP são códigos de três dígitos que o servidor web envia para o navegador indicando o resultado da 
 solicitação feita por ele. Esses códigos são divididos em classes que representam categorias diferentes. São elas:

    - Códigos informativos (100 a 199): indicam que a solicitação do cliente foi recebida e está sendo processada;
    - Códigos de sucesso (200 a 299): indicam que o servidor concluiu com sucesso a ação solicitada;
    - Códigos de redirecionamento (300 a 399): indicam que o cliente precisa tomar ações adicionais para completar a solicitação;
    - Códigos de erro do cliente (400 a 499): indicam que ocorreu um erro na solicitação do cliente;
    - Códigos de erro do servidor (500 a 599): indicam que ocorreu um erro no servidor ao processar a solicitação do cliente.
 */