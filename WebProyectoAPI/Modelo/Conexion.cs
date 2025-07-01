using MySql.Data.MySqlClient;

namespace WebProyectoAPI.Modelo
{
    public class Conexion
    {
        protected MySqlConnection _cnx = null;

        public Conexion()
        {
            // Aquí lee del entorno
            string cadena = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            _cnx = new MySqlConnection(cadena);
        }
    }
}
