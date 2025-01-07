using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.NovaPasta;

public static class ArtistasExtensions
{

    public static void AddEndPointsArtistas(this WebApplication app)
    {
        // FromServices recupera o objeto criado pelo Asp.NET
        app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
        {
            var listaDeArtistas = dal.Listar();

            if (listaDeArtistas is null)
            {
                return Results.NotFound();
            }

            var listaDeArtistaResponse = EntityListToResponseList(listaDeArtistas);
            
            return Results.Ok(listaDeArtistaResponse);
        });

        app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
        {
            var artista = dal.RecuperarPor(a => a.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)); // "StringComparison.OrdinalIgnoreCase" ignora o case sensitive

            if (artista is null)
            {
                return Results.NotFound(new { Message = "Artista não encontrado" });
            }

            return Results.Ok(EntityToResponse(artista));
        });

        app.MapPost("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody]  ArtistaRequest artistaRequest) => // "FromBody" indica que os dados vem no corpo da requisição
        {
            var artista = new Artista(artistaRequest.nome, artistaRequest.bio);
            
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

        app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequestEdit artistaRequestEdit) => {

            var artistaAAtualizar = dal.RecuperarPor(a => a.Id == artistaRequestEdit.Id);

            if (artistaAAtualizar is null)
            {
                return Results.NotFound();
            }

            artistaAAtualizar.Nome = artistaRequestEdit.nome;
            artistaAAtualizar.Bio = artistaRequestEdit.bio;

            dal.Atualizar(artistaAAtualizar);

            return Results.Ok();
        });
    }

    private static ICollection<ArtistaResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
    {
        return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
    }

    private static ArtistaResponse EntityToResponse(Artista artista)
    {
        return new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
    }
}

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