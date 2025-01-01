using Microsoft.Data.SqlClient;

namespace ScreenSound.Exercicios;

internal class ScreenSoundComAdoNet
{
   private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ScreenSound;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";


   public SqlConnection ObterConexao()
   {
      return new SqlConnection(connectionString);
   }

}
