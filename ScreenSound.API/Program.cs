using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
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
builder.Services.AddTransient<DAL<Musica>>();


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

app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] Artista artista) => {
    var artistaAAtualizar = dal.RecuperarPor(a => a.Id == artista.Id);

    if (artistaAAtualizar is null)
    {
        return Results.NotFound();
    }

    artistaAAtualizar.Nome = artista.Nome;
    artistaAAtualizar.Bio = artista.Bio;
    artistaAAtualizar.FotoPerfil = artista.FotoPerfil;

    dal.Atualizar(artistaAAtualizar);
    
    return Results.Ok();
});

app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
{
    return Results.Ok(dal.Listar());
});

app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> dal, string nome) =>
{
    var musica = dal.RecuperarPor(a => a.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)); 

    if (musica is null)
    {
        return Results.NotFound(new { Message = "Musica não encontrado" });
    }

    return Results.Ok(musica);
});

app.MapPost("/Musicas", ([FromServices] DAL<Musica> dal, [FromBody] Musica musica) => 
{
    dal.Adicionar(musica);
    
    return Results.Ok();
});

app.MapDelete("/Musicas/{id}", ([FromServices] DAL<Musica> dal, int id) => {
    var musica = dal.RecuperarPor(a => a.Id == id);
   
    if (musica is null)
    {
        return Results.NotFound();
    
    }
    
    dal.Deletar(musica);
    return Results.NoContent();

});

app.MapPut("/Musicas", ([FromServices] DAL<Musica> dal, [FromBody] Musica musica) => {
    var musicaAAtualizar = dal.RecuperarPor(a => a.Id == musica.Id);
    
    if (musicaAAtualizar is null)
    {
        return Results.NotFound();
    
    }
    musicaAAtualizar.Nome = musica.Nome;
    musicaAAtualizar.AnoLancamento = musica.AnoLancamento;

    dal.Atualizar(musicaAAtualizar);
    
    return Results.Ok();
});

app.Run();

/*
 Os códigos de retorno HTTP são códigos de três dígitos que o servidor web
 envia para o navegador indicando o resultado da solicitação feita por ele.
 Esses códigos são divididos em classes que representam categorias 
 diferentes. São elas:

    - Códigos informativos (100 a 199): indicam que a solicitação do 
      cliente foi recebida e está sendo processada;
    - Códigos de sucesso (200 a 299): indicam que o servidor concluiu com 
      sucesso a ação solicitada;
    - Códigos de redirecionamento (300 a 399): indicam que o cliente 
      precisa tomar ações adicionais para completar a solicitação;
    - Códigos de erro do cliente (400 a 499): indicam que ocorreu um erro 
      na solicitação do cliente;
    - Códigos de erro do servidor (500 a 599): indicam que ocorreu um erro
      no servidor ao processar a solicitação do cliente.

Veja os principais códigos HTTP e quando os utilizar:

200 (OK) => Em requisições GET, PUT e DELETE executadas com sucesso.
201 (Created) => Em requisições POST, quando um novo recurso é criado com sucesso.
206 (Partial Content) => Em requisições GET que devolvem apenas uma parte do conteúdo de um recurso.
302 (Found) => Em requisições feitas à URIs antigas, que foram alteradas.
400 (Bad Request) => Em requisições cujas informações enviadas pelo cliente sejam invalidas.
401 (Unauthorized) => Em requisições que exigem autenticação, mas seus dados não foram fornecidos.
403 (Forbidden) => Em requisições que o cliente não tem permissão de acesso ao recurso solicitado.
404 (Not Found) => Em requisições cuja URI de um determinado recurso seja inválida.
405 (Method Not Allowed) => Em requisições cujo método HTTP indicado pelo cliente não seja suportado.
406 (Not Acceptable) => Em requisições cujo formato da representação do recurso requisitado pelo cliente não seja suportado.
415 (Unsupported Media Type) => Em requisições cujo formato da representação do recurso enviado pelo cliente não seja suportado.
429 (Too Many Requests) => No caso do serviço ter um limite de requisições que pode ser feita por um cliente, e ele já tiver sido atingido.
500 (Internal Server Error) => Em requisições onde um erro tenha ocorrido no servidor.
503 (Service Unavailable) => Em requisições feitas a um serviço que esta fora do ar, para manutenção ou sobrecarga.
 */