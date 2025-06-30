using MySql.Data.MySqlClient;
using System.Data;

namespace WebProyectoAPI.Modelo
{
    public class HistorialActividad : Conexion
    {
        public HistorialActividad() { }

        public int IdHistorial { get; set; } = 0;
        public string Accion { get; set; } = "";
        public DateTime FechaActividad { get; set; } = DateTime.Now;
        public int IdTarea { get; set; } = 0;
        public int IdUsuario { get; set; } = 0;

        public List<HistorialActividad> Listar()
        {
            try
            {
                List<HistorialActividad> lista = new List<HistorialActividad>();
                MySqlCommand comando = new MySqlCommand("sp_ListarHistorialDeActividad", _cnx);
                comando.CommandType = CommandType.StoredProcedure;

                _cnx.Open();
                MySqlDataReader leer = comando.ExecuteReader();

                while (leer.Read())
                {
                    var obj = new HistorialActividad
                    {
                        IdHistorial = Convert.ToInt32(leer["IdHistorial"]),
                        Accion = leer["Accion"].ToString(),
                        FechaActividad = Convert.ToDateTime(leer["FechaActividad"]),
                        IdTarea = Convert.ToInt32(leer["IdTarea"]),
                        IdUsuario = Convert.ToInt32(leer["IdUsuario"])
                    };
                    lista.Add(obj);
                }

                return lista;
            }
            catch
            {
                throw;
            }
            finally
            {
                _cnx.Close();
            }
        }

        public bool Agregar()
        {
            try
            {
                using (MySqlCommand comando = new MySqlCommand("sp_AgregarHistorialDeActividad", _cnx))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@p_Accion", this.Accion);
                    comando.Parameters.AddWithValue("@p_FechaActividad", this.FechaActividad);
                    comando.Parameters.AddWithValue("@p_IdTarea", this.IdTarea);
                    comando.Parameters.AddWithValue("@p_IdUsuario", this.IdUsuario);

                    _cnx.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                _cnx.Close();
            }
        }

        public bool Actualizar()
        {
            try
            {
                using (MySqlCommand comando = new MySqlCommand("sp_ActualizarHistorialDeActividad", _cnx))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@p_IdHistorial", this.IdHistorial);
                    comando.Parameters.AddWithValue("@p_Accion", this.Accion);
                    comando.Parameters.AddWithValue("@p_FechaActividad", this.FechaActividad);
                    comando.Parameters.AddWithValue("@p_IdTarea", this.IdTarea);
                    comando.Parameters.AddWithValue("@p_IdUsuario", this.IdUsuario);

                    _cnx.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                _cnx.Close();
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                using (MySqlCommand comando = new MySqlCommand("sp_EliminarHistorialDeActividad", _cnx))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@p_IdHistorial", id);

                    _cnx.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                _cnx.Close();
            }
        }
    }
}
