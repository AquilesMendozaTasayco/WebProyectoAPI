using MySql.Data.MySqlClient;
using System.Data;

namespace WebProyectoAPI.Modelo
{
    public class Comentario : Conexion
    {
        public Comentario() { }

        public int IdComentario { get; set; } = 0;
        public string Contenido { get; set; } = "";
        public DateTime FechaComentario { get; set; } = DateTime.Now;
        public int IdTarea { get; set; } = 0;
        public int IdUsuario { get; set; } = 0;

        public List<Comentario> Listar()
        {
            try
            {
                List<Comentario> lista = new List<Comentario>();
                MySqlCommand comando = new MySqlCommand("sp_ListarComentario", _cnx);
                comando.CommandType = CommandType.StoredProcedure;

                _cnx.Open();
                MySqlDataReader leer = comando.ExecuteReader();

                while (leer.Read())
                {
                    var objComentario = new Comentario
                    {
                        IdComentario = Convert.ToInt32(leer["IdComentario"]),
                        Contenido = leer["Contenido"].ToString(),
                        FechaComentario = Convert.ToDateTime(leer["FechaComentario"]),
                        IdTarea = Convert.ToInt32(leer["IdTarea"]),
                        IdUsuario = Convert.ToInt32(leer["IdUsuario"])
                    };
                    lista.Add(objComentario);
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
                using (MySqlCommand comando = new MySqlCommand("sp_AgregarComentario", _cnx))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@p_Contenido", this.Contenido);
                    comando.Parameters.AddWithValue("@p_FechaComentario", this.FechaComentario);
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
                using (MySqlCommand comando = new MySqlCommand("sp_ActualizarComentario", _cnx))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@p_IdComentario", this.IdComentario);
                    comando.Parameters.AddWithValue("@p_Contenido", this.Contenido);
                    comando.Parameters.AddWithValue("@p_FechaComentario", this.FechaComentario);
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
                using (MySqlCommand comando = new MySqlCommand("sp_EliminarComentario", _cnx))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@p_IdComentario", id);

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
