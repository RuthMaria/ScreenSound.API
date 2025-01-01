using Microsoft.Data.SqlClient;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSound.Exercicios;

internal class ArtistaDALComAdoNet
{
    // Quando declaramos uma variável local como using, ela é descartada no
    // final do escopo em que ela foi declarada. Com isso conseguimos aplicar
    // uma boa prática e gerenciar melhor os recursos que estão sendo
    // utilizados e mantê-los somente quando estiverem sendo utilizados

    public IEnumerable<Artista> Listar()
    {
        var lista = new List<Artista>();
        using var connection = new ScreenSoundComAdoNet().ObterConexao();
        connection.Open();

        string sql = "SELECT * FROM Artistas";
        SqlCommand command = new SqlCommand(sql, connection); //  representa a instrução que será executada no banco de dados
        using SqlDataReader dataReader = command.ExecuteReader(); // responsável por ler as informações do banco

        while (dataReader.Read())
        {
            string nomeArtista = Convert.ToString(dataReader["Nome"]);
            string bioArtista = Convert.ToString(dataReader["Bio"]);
            int idArtista = Convert.ToInt32(dataReader["Id"]);

            Artista artista = new(nomeArtista, bioArtista) { Id = idArtista };

            lista.Add(artista);
        }

        return lista;
    }


    public void Adicionar(Artista artista)
    {
        using var connection = new ScreenSoundComAdoNet().ObterConexao();
        connection.Open();

        string sql = "INSERT INTO Artistas (Nome, FotoPerfil, Bio) VALUES (@nome, @perfilPadrao, @bio)";
        SqlCommand command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@nome", artista.Nome);
        command.Parameters.AddWithValue("@perfilPadrao", artista.FotoPerfil);
        command.Parameters.AddWithValue("@bio", artista.Bio);

        int retorno = command.ExecuteNonQuery();
        Console.WriteLine($"Linhas afetadas: {retorno}");
    }

    public void AtualizarUsandoAdoNet(Artista artista)
    {
        using var connection = new ScreenSoundComAdoNet().ObterConexao();
        connection.Open();

        string sql = "UPDATE Artistas SET Nome = @nome, Bio = @bio WHERE Id = @id";
        SqlCommand command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@nome", artista.Nome);
        command.Parameters.AddWithValue("@bio", artista.Bio);
        command.Parameters.AddWithValue("@id", artista.Id);

        int retorno = command.ExecuteNonQuery();
        Console.WriteLine($"Linhas afetadas: {retorno}");
    }

    public void DeletarUsandoAdoNet(Artista artista)
    {
        using var connection = new ScreenSoundComAdoNet().ObterConexao();
        connection.Open();

        string sql = "DELETE FROM Artistas WHERE Id = @id";
        SqlCommand command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", artista.Id);

        int retorno = command.ExecuteNonQuery();
        Console.WriteLine($"Linhas afetadas: {retorno}");
    }
}
