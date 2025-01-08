using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Endpoints;
using ScreenSound.API.NovaPasta;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Models.Modelos;
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
builder.Services.AddTransient<DAL<Genero>>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.AddEndPointsArtistas();
app.AddEndPointsMusicas();
app.AddEndPointGeneros();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
