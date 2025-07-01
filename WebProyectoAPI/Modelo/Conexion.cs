using MySql.Data.MySqlClient;

namespace WebProyectoAPI.Modelo
{
    public class Conexion
    {
        protected MySqlConnection _cnx = null;

        string cadena = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        public Conexion()
        {
            _cnx = new MySqlConnection(cadena);
        }
    }
}
