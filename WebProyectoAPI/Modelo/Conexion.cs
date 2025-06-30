using MySql.Data.MySqlClient;

namespace WebProyectoAPI.Modelo
{
    public class Conexion
    {
        protected MySqlConnection _cnx = null;

        string cadena = "server=localhost;port=3306;database=gestorProyectoBD;user=root;password=;";

        public Conexion()
        {
            _cnx = new MySqlConnection(cadena);
        }
    }
}
