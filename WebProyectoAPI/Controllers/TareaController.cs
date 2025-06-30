using Microsoft.AspNetCore.Mvc;
using WebProyectoAPI;

namespace WebProyectoAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TareaController : ControllerBase
	{
		// ? Listar tareas
		[HttpGet("ListarTarea")]



		public IEnumerable<Tarea> ListarTarea()
		{
			return new Tarea().Listar();
		}

		// ? Agregar tarea y notificar a miembros y líder
		[HttpPost("AgregarTarea")]
		public IActionResult AgregarTarea([FromBody] Tarea tarea)
		{
			if (tarea.Agregar())
			{
				// ? Usa correctamente el nombre real de la propiedad
				var noti = new Notificacion();
				noti.NotificarMiembrosYLider(tarea.IdProyecto, $"Se ha asignado una nueva tarea: {tarea.TituloTarea}");

				return Ok(new { mensaje = "Tarea y notificación registrada correctamente" });
			}
			else
			{
				return BadRequest(new { mensaje = "Error al registrar tarea" });
			}
		}

        [HttpGet("ObtenerTarea/{id}")]
        public IActionResult ObtenerTarea(int id)
        {
            try
            {
                var tarea = new Tarea().ObtenerPorId(id);
                return Ok(tarea);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al obtener tarea", error = ex.Message });
            }
        }


        [HttpGet("ListarTareasPorUsuario/{idUsuario}")]
        public IActionResult ListarPorUsuario(int idUsuario)
        {
            var modelo = new Tarea();
            var lista = modelo.ListarPorUsuario(idUsuario);
            return Ok(lista);
        }



        // ? Actualizar tarea
        [HttpPut("ActualizarTarea")]
		public IActionResult ActualizarTarea([FromBody] Tarea tarea)
		{
			if (tarea.Actualizar())
				return Ok(new { mensaje = "Tarea actualizada correctamente" });
			else
				return BadRequest(new { mensaje = "Error al actualizar tarea" });
		}

		// ? Eliminar tarea
		[HttpDelete("EliminarTarea/{id}")]
		public IActionResult EliminarTarea(int id)
		{
			Tarea tarea = new Tarea();
			if (tarea.Eliminar(id))
				return Ok(new { mensaje = "Tarea eliminada correctamente" });
			else
				return BadRequest(new { mensaje = "Error al eliminar tarea" });
		}

		[HttpGet("ListarTareasPorProyecto/{idProyecto}")]
		public IActionResult ListarTareasPorProyecto(int idProyecto)
		{
			var tareas = new Tarea().ListarPorProyecto(idProyecto);
			return Ok(tareas);
		}

	}
}
