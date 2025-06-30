using Microsoft.AspNetCore.Mvc;
using WebProyectoAPI;

namespace WebProyectoAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class NotificacionController : ControllerBase
	{
		// ? GET: Listar notificaciones
		[HttpGet("ListarNotificacion")]
		public IEnumerable<Notificacion> ListarNotificacion()
		{
			return new Notificacion().Listar();
		}

		// ? POST: Agregar una notificación individual
		[HttpPost("AgregarNotificacion")]
		public IActionResult AgregarNotificacion([FromBody] Notificacion notificacion)
		{
			if (notificacion.Agregar())
				return Ok(new { mensaje = "Notificación agregada correctamente" });
			else
				return BadRequest(new { mensaje = "Error al agregar la notificación" });
		}

		// ? PUT: Actualizar notificación
		[HttpPut("ActualizarNotificacion")]
		public IActionResult ActualizarNotificacion([FromBody] Notificacion notificacion)
		{
			if (notificacion.Actualizar())
				return Ok(new { mensaje = "Notificación actualizada correctamente" });
			else
				return BadRequest(new { mensaje = "Error al actualizar la notificación" });
		}

		// ? DELETE: Eliminar notificación
		[HttpDelete("EliminarNotificacion/{id}")]
		public IActionResult EliminarNotificacion(int id)
		{
			Notificacion notificacion = new Notificacion();
			if (notificacion.Eliminar(id))
				return Ok(new { mensaje = "Notificación eliminada correctamente" });
			else
				return BadRequest(new { mensaje = "Error al eliminar la notificación" });
		}

		// ? POST: Notificar a todos los miembros y líder de un proyecto
		[HttpPost("NotificarMiembrosYLider")]
		public IActionResult NotificarMiembrosYLider([FromBody] NotificacionProyectoDTO data)
		{
			try
			{
				Notificacion noti = new Notificacion();
				bool resultado = noti.NotificarMiembrosYLider(data.IdProyecto, data.Mensaje);

				if (resultado)
					return Ok(new { mensaje = "Notificaciones enviadas a miembros y líder." });
				else
					return BadRequest(new { mensaje = "No se pudieron enviar las notificaciones." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { mensaje = "Error interno", detalle = ex.Message });
			}
		}
	}

	// ? DTO para notificar a múltiples usuarios por proyecto
	public class NotificacionProyectoDTO
	{
		public int IdProyecto { get; set; }
		public string Mensaje { get; set; } = "";
	}
}
