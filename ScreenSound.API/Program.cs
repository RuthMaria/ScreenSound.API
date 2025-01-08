using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Endpoints;
using ScreenSound.API.NovaPasta;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Models.Modelos;
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
