using MySql.Data.MySqlClient;

namespace WebProyectoAPI.Modelo
{
    public class Conexion
    {
        protected MySqlConnection _cnx = null;

        public Conexion()
        {
            string cadena = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            if (string.IsNullOrEmpty(cadena))
            {
                throw new Exception("La variable CONNECTION_STRING no está configurada.");
            }

            _cnx = new MySqlConnection(cadena);
        }
    }
}
