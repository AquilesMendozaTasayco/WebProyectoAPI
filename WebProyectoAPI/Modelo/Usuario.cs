using MySql.Data.MySqlClient;
using System.Data;
using WebProyectoAPI.Modelo;
using BCrypt.Net;

namespace WebProyectoAPI
{
    public class Usuario : Conexion
    {
        public Usuario() { }

        public int IdUsuario { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Password { get; set; } = "";
        public string Rol { get; set; } = "";
		public string FotoPerfil { get; set; } = "";


		public bool ActualizarFotoPerfil(int idUsuario, string nombreArchivo)
		{
			try
			{
				using (MySqlCommand cmd = new MySqlCommand("UPDATE Usuario SET FotoPerfil = @foto WHERE IdUsuario = @id", _cnx))
				{
					cmd.Parameters.AddWithValue("@foto", nombreArchivo);
					cmd.Parameters.AddWithValue("@id", idUsuario);
					_cnx.Open();
					return cmd.ExecuteNonQuery() > 0;
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


		public List<Usuario> Listar()
        {
            try
            {
                MySqlCommand comando = new MySqlCommand("sp_ListarUsuario", _cnx);
                comando.CommandType = CommandType.StoredProcedure;
                _cnx.Open();

                List<Usuario> lista = new List<Usuario>();
                MySqlDataReader leer = comando.ExecuteReader();

                while (leer.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(leer["IdUsuario"]),
                        Nombre = leer["Nombre"].ToString(),
                        Correo = leer["Correo"].ToString(),
                        Password = leer["Password"].ToString(),
                        Rol = leer["Rol"].ToString()
                    };
                    lista.Add(usuario);
                }

                return lista;
            }
            catch (Exception)
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
                using (MySqlCommand comando = new MySqlCommand("sp_AgregarUsuario", _cnx))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    string hash = BCrypt.Net.BCrypt.HashPassword(this.Password);

                    comando.Parameters.AddWithValue("@p_Nombre", this.Nombre);
                    comando.Parameters.AddWithValue("@p_Correo", this.Correo);
                    comando.Parameters.AddWithValue("@p_Password", hash);
                    comando.Parameters.AddWithValue("@p_Rol", this.Rol);

                    _cnx.Open();
                    return comando.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
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
                using (MySqlCommand comando = new MySqlCommand("sp_ActualizarUsuario", _cnx))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@p_IdUsuario", this.IdUsuario);
                    comando.Parameters.AddWithValue("@p_Nombre", this.Nombre);
                    comando.Parameters.AddWithValue("@p_Correo", this.Correo);
                    comando.Parameters.AddWithValue("@p_Password", this.Password);
                    comando.Parameters.AddWithValue("@p_Rol", this.Rol);

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
                using (MySqlCommand comando = new MySqlCommand("sp_EliminarUsuario", _cnx))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@p_IdUsuario", id);

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

        public List<Usuario> BuscarUsuarios(string filtro)
        {
            List<Usuario> lista = new List<Usuario>();
            try
            {
                using (MySqlCommand comando = new MySqlCommand("sp_BuscarUsuarios", this._cnx))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@p_busqueda", filtro);
                    this._cnx.Open();

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Usuario
                            {
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                Nombre = reader["Nombre"].ToString(),
                                Correo = reader["Correo"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                this._cnx.Close();
            }
            return lista;
        }
        public bool CambiarClave(int idUsuario, string nuevaClave)
        {
            try
            {
                using (MySqlCommand comando = new MySqlCommand("sp_CambiarClaveUsuario", _cnx))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    string hash = BCrypt.Net.BCrypt.HashPassword(nuevaClave);
                    comando.Parameters.AddWithValue("@p_IdUsuario", idUsuario);
                    comando.Parameters.AddWithValue("@p_NuevaPassword", hash);

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
