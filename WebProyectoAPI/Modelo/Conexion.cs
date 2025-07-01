using MySql.Data.MySqlClient;
using System;

namespace WebProyectoAPI.Modelo
{
    public class Conexion
    {
        protected MySqlConnection _cnx = null;

        public Conexion()
        {
            string url = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            // Convierte la URL a una cadena de conexión
            var uri = new Uri(url);
            string server = uri.Host;
            int port = uri.Port;
            string database = uri.AbsolutePath.TrimStart('/');
            string user = uri.UserInfo.Split(':')[0];
            string password = uri.UserInfo.Split(':')[1];

            string cadena = $"Server={server};Port={port};Database={database};Uid={user};Pwd={password};";

            _cnx = new MySqlConnection(cadena);
        }
    }
}
