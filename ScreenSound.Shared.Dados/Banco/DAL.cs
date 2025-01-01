namespace ScreenSound.Banco;
using ScreenSound.Modelos;

/*
 
Diferença entre DAL (Data Access Object) e DAO (Data Access Layer)
é que o DAL é a camada de acesso a dados que promove a abstração desses dados e vai 
emitir todos os comandos de SELECT, INSERT, UPDATE E DELETE de maneira 
separada da lógica das classes do projeto e independente da fonte de dados, 
enquanto o DAO é um objeto do banco de dados que representa um banco aberto.

Basicamente, o DAL representa a estrutura de acesso aos dados, independente do
tipo de banco utilizado, e o DAO é o objeto que representa o acesso a uma 
fonte de dados específica.
 
---------------------------------------------------------------------

 Uma classe abstrata é uma classe que serve como modelo ou base para 
 outras classes, mas não pode ser instanciada diretamente. 
 Em vez disso, ela é projetada para ser estendida por outras classes 
 que implementarão (ou sobrescreverão) seus métodos.

Ela pode ter atributos, métodos concretos (com implementação), métodos 
abstratos (sem implementação) e construtores.

A diferença dela para uma interface é que ela possui a relação de 
"é um" (herança). Serve para compartilhar códigos semelhantes, já que a 
interface só possui assinaturas de métodos.

A interface é usada quando queremos apenas um contrato que será usado em
várias classes não relacionadas entre sim, mas possuem um determinado
comportamento semelhante. Interface não possui métodos concretos.

----------------------------------------------------------------------

 A parte 'where T : class' define uma restrição genérica. 
 Isso significa que o tipo T passado para a classe DAL<T> deve ser uma 
 classe (não pode ser um tipo primitivo como int, bool, etc.).
 Essa restrição é importante porque o código trabalha com entidades do 
 Entity Framework, que são classes.
 */
public class DAL<T> where T : class
{
    private readonly ScreenSoundContext context;

    public DAL(ScreenSoundContext context)
    {
        this.context = context;
    }

    /*
     o Set identifica qual tipo estamos utilizando (artista ou música, por exemplo).
     Em seguida, é feito o acesso ao conjunto de entidades do tipo T.

     Ele é equivalente ao 'context.Artistas.ToList();' da forma não genérica 
    */
    public IEnumerable<T> Listar()
    {
       return context.Set<T>().ToList(); 
    }

    public void Adicionar(T objeto)
    {
        context.Set<T>().Add(objeto);
        context.SaveChanges(); // salva as alterações feita no banco
    }

    public void Atualizar(T objeto)
    {
        context.Set<T>().Update(objeto);
        context.SaveChanges();
    }

    public void Deletar(T objeto)
    {
        context.Set<T>().Remove(objeto);
        context.SaveChanges();
    }

    /*
     Func => desempenha o papel de uma função capaz de fornecer um retorno
     e realizar uma verificação. É muito útil em situações em que
     precisamos representar um método sem ter que passar especificamente
     valores personalizados, deixando a correspondência para a assinatura 
     do método.
     */
    public T? RecuperarPor(Func<T, bool> condicao)
    {
        return context.Set<T>().FirstOrDefault(condicao);
    }

    public IEnumerable<T> ListarPor(Func<T, bool> condicao)
    {
        return context.Set<T>().Where(condicao);
    }
}
