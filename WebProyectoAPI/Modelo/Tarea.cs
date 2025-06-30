using MySql.Data.MySqlClient;
using System.Data;
using WebProyectoAPI.Modelo;

namespace WebProyectoAPI
{
    public class Tarea : Conexion
    {
        public Tarea() { }

        public int IdTarea { get; set; } = 0;
        public string TituloTarea { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public string Prioridad { get; set; } = "";
        public DateTime FechaLimite { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "";
        public int IdProyecto { get; set; } = 0;
        public int IdUsuario { get; set; } = 0;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
        public int Progreso { get; set; } = 0;

        public string NombreProyecto { get; set; } = "";
        public string NombreUsuarioAsignado { get; set; } = "";


		public List<Tarea> ListarPorProyecto(int idProyecto)
		{
			List<Tarea> lista = new List<Tarea>();
			try
			{
				MySqlCommand cmd = new MySqlCommand("sp_ListarTareasPorProyecto", _cnx);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@p_IdProyecto", idProyecto);
				_cnx.Open();

				using var reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					lista.Add(new Tarea
					{
						IdTarea = Convert.ToInt32(reader["IdTarea"]),
						TituloTarea = reader["TituloTarea"].ToString(),
						Descripcion = reader["Descripcion"].ToString(),
						Prioridad = reader["Prioridad"].ToString(),
						FechaLimite = Convert.ToDateTime(reader["FechaLimite"]),
						Estado = reader["Estado"].ToString(),
						IdProyecto = Convert.ToInt32(reader["IdProyecto"]),
						IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
						NombreUsuarioAsignado = reader["NombreUsuarioAsignado"].ToString()
					});
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

			return lista;
		}

		public List<Tarea> Listar()
        {
            try
            {
                MySqlCommand comando = new MySqlCommand("sp_ListarTarea", _cnx);
                comando.CommandType = CommandType.StoredProcedure;
                _cnx.Open();

                List<Tarea> lista = new List<Tarea>();
                MySqlDataReader leer = comando.ExecuteReader();

                while (leer.Read())
                {
                    lista.Add(new Tarea
                    {
                        IdTarea = Convert.ToInt32(leer["IdTarea"]),
                        TituloTarea = leer["TituloTarea"].ToString(),
                        Descripcion = leer["Descripcion"].ToString(),
                        Prioridad = leer["Prioridad"].ToString(),
                        FechaLimite = Convert.ToDateTime(leer["FechaLimite"]),
                        Estado = leer["Estado"].ToString(),
                        IdProyecto = Convert.ToInt32(leer["IdProyecto"]),
                        IdUsuario = Convert.ToInt32(leer["IdUsuario"]),
                        FechaCreacion = leer["FechaCreacion"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(leer["FechaCreacion"]),
                        FechaActualizacion = leer["FechaActualizacion"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(leer["FechaActualizacion"]),
                        Progreso = leer["Progreso"] is DBNull ? 0 : Convert.ToInt32(leer["Progreso"]),
                        NombreProyecto = leer["NombreProyecto"]?.ToString() ?? "",
                        NombreUsuarioAsignado = leer["NombreUsuarioAsignado"]?.ToString() ?? ""
                    });
                }

                return lista;
            }
            finally
            {
                _cnx.Close();
            }
        }

        public List<Tarea> ListarPorUsuario(int idUsuario)
        {
            try
            {
                MySqlCommand comando = new MySqlCommand("sp_ListarTareasPorUsuario", _cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@p_IdUsuario", idUsuario);
                _cnx.Open();

                List<Tarea> lista = new List<Tarea>();
                MySqlDataReader leer = comando.ExecuteReader();

                while (leer.Read())
                {
                    lista.Add(new Tarea
                    {
                        IdTarea = Convert.ToInt32(leer["IdTarea"]),
                        TituloTarea = leer["TituloTarea"].ToString(),
                        Descripcion = leer["Descripcion"].ToString(),
                        Prioridad = leer["Prioridad"].ToString(),
                        FechaLimite = Convert.ToDateTime(leer["FechaLimite"]),
                        Estado = leer["Estado"].ToString(),
                        IdProyecto = Convert.ToInt32(leer["IdProyecto"]),
                        IdUsuario = Convert.ToInt32(leer["IdUsuario"]),
                        FechaCreacion = leer["FechaCreacion"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(leer["FechaCreacion"]),
                        FechaActualizacion = leer["FechaActualizacion"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(leer["FechaActualizacion"]),
                        Progreso = leer["Progreso"] is DBNull ? 0 : Convert.ToInt32(leer["Progreso"]),
                        NombreProyecto = leer["NombreProyecto"]?.ToString() ?? "",
                        NombreUsuarioAsignado = leer["NombreUsuarioAsignado"]?.ToString() ?? ""
                    });
                }

                return lista;
            }
            finally
            {
                _cnx.Close();
            }
        }

        public Tarea ObtenerPorId(int id)
        {
            try
            {
                MySqlCommand comando = new MySqlCommand("sp_ObtenerTarea", _cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@p_IdTarea", id);
                _cnx.Open();

                MySqlDataReader leer = comando.ExecuteReader();
                if (leer.Read())
                {
                    return new Tarea
                    {
                        IdTarea = Convert.ToInt32(leer["IdTarea"]),
                        TituloTarea = leer["TituloTarea"].ToString(),
                        Descripcion = leer["Descripcion"].ToString(),
                        Prioridad = leer["Prioridad"].ToString(),
                        FechaLimite = Convert.ToDateTime(leer["FechaLimite"]),
                        Estado = leer["Estado"].ToString(),
                        IdProyecto = Convert.ToInt32(leer["IdProyecto"]),
                        IdUsuario = Convert.ToInt32(leer["IdUsuario"]),
                        FechaCreacion = leer["FechaCreacion"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(leer["FechaCreacion"]),
                        FechaActualizacion = leer["FechaActualizacion"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(leer["FechaActualizacion"]),
                        Progreso = leer["Progreso"] is DBNull ? 0 : Convert.ToInt32(leer["Progreso"]),
                        NombreProyecto = leer["NombreProyecto"]?.ToString() ?? "",
                        NombreUsuarioAsignado = leer["NombreUsuarioAsignado"]?.ToString() ?? ""
                    };
                }

                return null!;
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
                using var comando = new MySqlCommand("sp_AgregarTarea", _cnx)
                {
                    CommandType = CommandType.StoredProcedure
                };

                comando.Parameters.AddWithValue("@p_TituloTarea", TituloTarea);
                comando.Parameters.AddWithValue("@p_Descripcion", Descripcion);
                comando.Parameters.AddWithValue("@p_Prioridad", Prioridad);
                comando.Parameters.AddWithValue("@p_FechaLimite", FechaLimite);
                comando.Parameters.AddWithValue("@p_Estado", string.IsNullOrEmpty(Estado) ? "Pendiente" : Estado);
                comando.Parameters.AddWithValue("@p_IdProyecto", IdProyecto);
                comando.Parameters.AddWithValue("@p_IdUsuario", IdUsuario);
                comando.Parameters.AddWithValue("@p_Progreso", Progreso);

                _cnx.Open();
                return comando.ExecuteNonQuery() > 0;
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
                using var comando = new MySqlCommand("sp_ActualizarTarea", _cnx)
                {
                    CommandType = CommandType.StoredProcedure
                };

                comando.Parameters.AddWithValue("@p_IdTarea", IdTarea);
                comando.Parameters.AddWithValue("@p_TituloTarea", TituloTarea);
                comando.Parameters.AddWithValue("@p_Descripcion", Descripcion);
                comando.Parameters.AddWithValue("@p_Prioridad", Prioridad);
                comando.Parameters.AddWithValue("@p_FechaLimite", FechaLimite);
                comando.Parameters.AddWithValue("@p_Estado", Estado);
                comando.Parameters.AddWithValue("@p_IdProyecto", IdProyecto);
                comando.Parameters.AddWithValue("@p_IdUsuario", IdUsuario);
                comando.Parameters.AddWithValue("@p_Progreso", Progreso);

                _cnx.Open();
                return comando.ExecuteNonQuery() > 0;
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
                using var comando = new MySqlCommand("sp_EliminarTarea", _cnx)
                {
                    CommandType = CommandType.StoredProcedure
                };
                comando.Parameters.AddWithValue("@p_IdTarea", id);

                _cnx.Open();
                return comando.ExecuteNonQuery() > 0;
            }
            finally
            {
                _cnx.Close();
            }
        }
    }
}
