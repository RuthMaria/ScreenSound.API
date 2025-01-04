using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints;

public static class MusicasExtensions
{

    public static void AddEndPointsMusicas(this WebApplication app)
    {

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

        app.MapPost("/Musicas", ([FromServices] DAL<Musica> dal, [FromBody] MusicaRequest musicaRequest) =>
        {
            var musica = new Musica(musicaRequest.nome, musicaRequest.anoLancamento);
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

        app.MapPut("/Musicas", ([FromServices] DAL<Musica> dal, [FromBody] MusicaRequestEdit MusicaRequestEdit) => {
            var musicaAAtualizar = dal.RecuperarPor(a => a.Id == MusicaRequestEdit.Id);

            if (musicaAAtualizar is null)
            {
                return Results.NotFound();

            }
            musicaAAtualizar.Nome = MusicaRequestEdit.nome;
            musicaAAtualizar.AnoLancamento = MusicaRequestEdit.anoDeLancamento;

            dal.Atualizar(musicaAAtualizar);

            return Results.Ok();
        });

    }

}

