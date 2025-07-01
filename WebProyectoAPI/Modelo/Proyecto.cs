using MySql.Data.MySqlClient;
using System.Data;
using WebProyectoAPI.Modelo;

namespace WebProyectoAPI
{
	public class Proyecto : Conexion
	{
		public Proyecto() { }

		public int IdProyecto { get; set; } = 0;
		public string NombreProyecto { get; set; } = "";
		public string Descripcion { get; set; } = "";
		public DateTime FechaInicio { get; set; } = DateTime.Now;
		public DateTime FechaFin { get; set; } = DateTime.Now;
		public int IdUsuario { get; set; } = 0;
		public string EstadoProyecto { get; set; } = "Activo";
		public int TotalTareas { get; set; } = 0;
		public string Jefe { get; set; } = ""; // ✅ importante

		// ✅ Obtener proyecto por ID con nombre del jefe incluido
		public Proyecto ObtenerPorId(int id)
		{
			try
			{
				using (MySqlCommand comando = new MySqlCommand("sp_ObtenerProyecto", _cnx))
				{
					comando.CommandType = CommandType.StoredProcedure;
					comando.Parameters.AddWithValue("@p_IdProyecto", id);

					_cnx.Open();
					using (MySqlDataReader leer = comando.ExecuteReader())
					{
						if (leer.Read())
						{
							return new Proyecto
							{
								IdProyecto = Convert.ToInt32(leer["IdProyecto"]),
								NombreProyecto = leer["NombreProyecto"].ToString(),
								Descripcion = leer["Descripcion"].ToString(),
								FechaInicio = Convert.ToDateTime(leer["FechaInicio"]),
								FechaFin = Convert.ToDateTime(leer["FechaFin"]),
								EstadoProyecto = leer["EstadoProyecto"].ToString(),
								IdUsuario = Convert.ToInt32(leer["IdUsuario"]),
								Jefe = leer["Jefe"].ToString() // ✅ ahora sí incluido
							};
						}
						return null!;
					}
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

		public List<Proyecto> ListarPorUsuario(int idUsuario)
		{
			try
			{
				MySqlCommand comando = new MySqlCommand("sp_ListarProyectosPorUsuario", _cnx);
				comando.CommandType = CommandType.StoredProcedure;
				comando.Parameters.AddWithValue("@p_IdUsuario", idUsuario);

				_cnx.Open();
				List<Proyecto> lista = new List<Proyecto>();
				var leer = comando.ExecuteReader();

				while (leer.Read())
				{
					Proyecto p = new Proyecto
					{
						IdProyecto = Convert.ToInt32(leer["IdProyecto"]),
						NombreProyecto = leer["NombreProyecto"].ToString(),
						Descripcion = leer["Descripcion"].ToString(),
						FechaInicio = Convert.ToDateTime(leer["FechaInicio"]),
						FechaFin = leer["FechaFin"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(leer["FechaFin"]),
						EstadoProyecto = leer["EstadoProyecto"].ToString(),
						IdUsuario = Convert.ToInt32(leer["IdUsuario"]),
						TotalTareas = leer["TotalTareas"] is DBNull ? 0 : Convert.ToInt32(leer["TotalTareas"])
					};
					lista.Add(p);
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
				using (MySqlCommand comando = new MySqlCommand("sp_AgregarProyecto", _cnx))
				{
					comando.CommandType = CommandType.StoredProcedure;

					comando.Parameters.AddWithValue("@p_NombreProyecto", this.NombreProyecto);
					comando.Parameters.AddWithValue("@p_Descripcion", this.Descripcion);
					comando.Parameters.AddWithValue("@p_FechaInicio", this.FechaInicio);
					comando.Parameters.AddWithValue("@p_FechaFin", this.FechaFin);
					comando.Parameters.AddWithValue("@p_IdUsuario", this.IdUsuario);
					comando.Parameters.AddWithValue("@p_EstadoProyecto", this.EstadoProyecto);

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
				using (MySqlCommand comando = new MySqlCommand("sp_ActualizarProyecto", _cnx))
				{
					comando.CommandType = CommandType.StoredProcedure;

					comando.Parameters.AddWithValue("@p_IdProyecto", this.IdProyecto);
					comando.Parameters.AddWithValue("@p_NombreProyecto", this.NombreProyecto);
					comando.Parameters.AddWithValue("@p_Descripcion", this.Descripcion);
					comando.Parameters.AddWithValue("@p_FechaInicio", this.FechaInicio);
					comando.Parameters.AddWithValue("@p_FechaFin", this.FechaFin);
					comando.Parameters.AddWithValue("@p_IdUsuario", this.IdUsuario);
					comando.Parameters.AddWithValue("@p_EstadoProyecto", this.EstadoProyecto);

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
				using (MySqlCommand comando = new MySqlCommand("sp_EliminarProyectoLogico", _cnx))
				{
					comando.CommandType = CommandType.StoredProcedure;
					comando.Parameters.AddWithValue("@p_IdProyecto", id);
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

		public bool AsignarMiembros(int idProyecto, List<int> idsUsuarios)
		{
			try
			{
				using (MySqlCommand comando = new MySqlCommand("sp_AsignarMiembros", _cnx))
				{
					comando.CommandType = CommandType.StoredProcedure;
					_cnx.Open();

					string ids = string.Join(";", idsUsuarios);
					comando.Parameters.AddWithValue("@p_IdProyecto", idProyecto);
					comando.Parameters.AddWithValue("@p_IdsUsuarios", ids);

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

		public List<Usuario> ObtenerMiembros(int idProyecto)
		{
			List<Usuario> lista = new List<Usuario>();
			try
			{
				MySqlCommand cmd = new MySqlCommand("sp_ObtenerMiembrosProyecto", _cnx);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@p_IdProyecto", idProyecto);
				_cnx.Open();

				using var reader = cmd.ExecuteReader();
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


	}
}
