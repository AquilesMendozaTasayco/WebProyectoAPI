using MySql.Data.MySqlClient;
using System.Data;
using WebProyectoAPI.Modelo;

namespace WebProyectoAPI
{
	public class Notificacion : Conexion
	{
		public Notificacion() { }

		public int IdNotificacion { get; set; } = 0;
		public string Mensaje { get; set; } = "";
		public bool Leida { get; set; } = false;
		public DateTime FechaNotificacion { get; set; } = DateTime.Now;
		public int IdUsuario { get; set; } = 0;

		public List<Notificacion> Listar()
		{
			try
			{
				List<Notificacion> lista = new List<Notificacion>();
				MySqlCommand comando = new MySqlCommand("sp_ListarNotificacion", _cnx);
				comando.CommandType = CommandType.StoredProcedure;

				_cnx.Open();
				MySqlDataReader leer = comando.ExecuteReader();

				while (leer.Read())
				{
					var objNotificacion = new Notificacion
					{
						IdNotificacion = Convert.ToInt32(leer["IdNotificacion"]),
						Mensaje = leer["Mensaje"].ToString(),
						Leida = Convert.ToBoolean(leer["Leida"]),
						FechaNotificacion = Convert.ToDateTime(leer["FechaNotificacion"]),
						IdUsuario = Convert.ToInt32(leer["IdUsuario"])
					};
					lista.Add(objNotificacion);
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
				using (MySqlCommand comando = new MySqlCommand("sp_AgregarNotificacion", _cnx))
				{
					comando.CommandType = CommandType.StoredProcedure;

					comando.Parameters.AddWithValue("@p_Mensaje", this.Mensaje);
					comando.Parameters.AddWithValue("@p_Leida", this.Leida);
					comando.Parameters.AddWithValue("@p_FechaNotificacion", this.FechaNotificacion);
					comando.Parameters.AddWithValue("@p_IdUsuario", this.IdUsuario);

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
				using (MySqlCommand comando = new MySqlCommand("sp_ActualizarNotificacion", _cnx))
				{
					comando.CommandType = CommandType.StoredProcedure;

					comando.Parameters.AddWithValue("@p_IdNotificacion", this.IdNotificacion);
					comando.Parameters.AddWithValue("@p_Mensaje", this.Mensaje);
					comando.Parameters.AddWithValue("@p_Leida", this.Leida);
					comando.Parameters.AddWithValue("@p_FechaNotificacion", this.FechaNotificacion);
					comando.Parameters.AddWithValue("@p_IdUsuario", this.IdUsuario);

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

		public bool Eliminar(int id)
		{
			try
			{
				using (MySqlCommand comando = new MySqlCommand("sp_EliminarNotificacion", _cnx))
				{
					comando.CommandType = CommandType.StoredProcedure;
					comando.Parameters.AddWithValue("@p_IdNotificacion", id);

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

		// ✅ NUEVO MÉTODO PARA NOTIFICAR A MIEMBROS Y LÍDER DEL PROYECTO
		public bool NotificarMiembrosYLider(int idProyecto, string mensaje)
		{
			try
			{
				_cnx.Open();

				// 1. Obtener Id del líder
				int idLider = 0;
				using (MySqlCommand cmdLider = new MySqlCommand("SELECT IdUsuario FROM Proyecto WHERE IdProyecto = @idProyecto", _cnx))
				{
					cmdLider.Parameters.AddWithValue("@idProyecto", idProyecto);
					object result = cmdLider.ExecuteScalar();
					if (result != null)
						idLider = Convert.ToInt32(result);
				}

				// 2. Obtener Ids de miembros
				List<int> idsUsuarios = new List<int>();
				using (MySqlCommand cmdMiembros = new MySqlCommand("SELECT IdUsuario FROM ProyectoUsuario WHERE IdProyecto = @idProyecto", _cnx))
				{
					cmdMiembros.Parameters.AddWithValue("@idProyecto", idProyecto);
					using (MySqlDataReader reader = cmdMiembros.ExecuteReader())
					{
						while (reader.Read())
						{
							int id = Convert.ToInt32(reader["IdUsuario"]);
							if (!idsUsuarios.Contains(id))
								idsUsuarios.Add(id);
						}
					}
				}

				// 3. Agregar líder si no está
				if (!idsUsuarios.Contains(idLider))
					idsUsuarios.Add(idLider);

				// 4. Enviar notificación a todos
				foreach (int idUsuario in idsUsuarios)
				{
					var noti = new Notificacion
					{
						Mensaje = mensaje,
						Leida = false,
						FechaNotificacion = DateTime.Now,
						IdUsuario = idUsuario
					};
					noti.Agregar();
				}

				return true;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al notificar miembros y líder", ex);
			}
			finally
			{
				_cnx.Close();
			}
		}

	}
}
