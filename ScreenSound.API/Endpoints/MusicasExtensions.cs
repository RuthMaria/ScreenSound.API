﻿using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Models.Modelos;

namespace ScreenSound.API.Endpoints;

public static class MusicasExtensions
{

    public static void AddEndPointsMusicas(this WebApplication app)
    {

        app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
        {
            var musicaList = dal.Listar();

            if (musicaList is null)
            {
                return Results.NotFound();
            }
            
            var musicaListResponse = EntityListToResponseList(musicaList);
            
            return Results.Ok(musicaListResponse);
        });

        app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> dal, string nome) =>
        {
            var musica = dal.RecuperarPor(a => a.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

            if (musica is null)
            {
                return Results.NotFound(new { Message = "Musica não encontrado" });
            }

            return Results.Ok(EntityToResponse(musica));
        });

        app.MapPost("/Musicas", ([FromServices] DAL<Musica> dal, [FromBody] MusicaRequest musicaRequest) =>
        {
            var musica = new Musica(musicaRequest.nome)
            {
                ArtistaId = musicaRequest.ArtistaId,
                AnoLancamento = musicaRequest.anoLancamento,
                Generos = musicaRequest.Generos is not null ? 
                GeneroRequestConverter(musicaRequest.Generos) :
                new List<Genero>()
            };

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

    private static ICollection<MusicaResponse> EntityListToResponseList(IEnumerable<Musica> musicaList)
    {
        return musicaList.Select(a => EntityToResponse(a)).ToList();
    }

    private static MusicaResponse EntityToResponse(Musica musica)
    {
        return new MusicaResponse(musica.Id, musica.Nome!, musica.Artista!.Id, musica.Artista.Nome);
    }
    private static ICollection<Genero> GeneroRequestConverter(ICollection<GeneroRequest> generos)
    {
        return generos.Select(a => RequestToEntity(a)).ToList();
    }

    private static Genero RequestToEntity(GeneroRequest genero)
    {
        return new Genero() { Nome = genero.Nome, Descricao = genero.Descricao };
    }


}

