using MySql.Data.MySqlClient;

namespace WebProyectoAPI.Modelo
{
    public class Conexion
    {
        protected MySqlConnection _cnx = null;

        string cadena = "server=turntable.proxy.rlwy.net;port=42741;database=railway;user=root;password=DblCzMgeLDTsNnjQCpkJuLtMJizUwmvL;";

        public Conexion()
        {
            _cnx = new MySqlConnection(cadena);
        }
    }
}
