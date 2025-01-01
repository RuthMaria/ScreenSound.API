using Microsoft.EntityFrameworkCore.Migrations;


/*
As migrations são um recurso do Entity que nos permite gerenciar tanto a 
estrutura do nosso banco quanto as diferentes versões que ele terá durante
o projeto, sem precisar mexer nos scripts SQL.

No contexto de banco de dados, utilizamos as migrations para gerenciar 
alterações no banco de dados. E no contexto em que estamos trabalhando com
o Entity Framework, as migrations são uma maneira de controlar as 
alterações no banco de dados com base nas modificações do projeto de 
maneira controlada e automatizada.

Através delas conseguimos fazer inclusão e exclusão de tabelas, alterações
de colunas e mudanças de informações, tudo isso atrelado à evolução e 
crescimento do projeto de forma estruturada.

Além disso, utilizando as migrations, é possível ter um histórico 
estruturado das alterações que ocorreram no banco de dados, facilitando o 
trabalho em equipe e também as atualizações em diversos ambientes existentes
 */

#nullable disable

namespace ScreenSound.Migrations
{
    /// <inheritdoc />
    public partial class projetoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artistas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FotoPerfil = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artistas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Musicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musicas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artistas");

            migrationBuilder.DropTable(
                name: "Musicas");
        }
    }
}
