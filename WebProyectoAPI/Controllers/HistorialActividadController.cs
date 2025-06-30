using Microsoft.AspNetCore.Mvc;
using WebProyectoAPI;
using WebProyectoAPI.Modelo;

namespace WebProyectoAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class HistorialActividadController : ControllerBase
	{

		//USUARIO

		[HttpGet("ListarHistorialActividad")]
		public IEnumerable<HistorialActividad> ListarHistorialActividad()
		{
			return new HistorialActividad().Listar();
		}

		[HttpPost("AgregarHistorialActividad")]
		public IActionResult AgregarHistorialActividad([FromBody] HistorialActividad historialActividad)
		{
			if (historialActividad.Agregar())
				return Ok(new { mensaje = "Historial de Actividad agregado correctamente" });
			else
				return BadRequest(new { mensaje = "Error al agregar Historial de Actividad" });
		}

		[HttpPut("ActualizarHistorialActividad")]
		public IActionResult ActualizarHistorialActividad([FromBody] HistorialActividad historialActividad)
		{
			if (historialActividad.Actualizar())
				return Ok(new { mensaje = "Historial de Actividad actualizado correctamente" });
			else
				return BadRequest(new { mensaje = "Error al actualizar Historial de Actividad" });
		}

		[HttpDelete("EliminarHistorialActividad/{id}")]
		public IActionResult EliminarHistorialActividad(int id)
		{
            HistorialActividad historialActividad = new HistorialActividad();
			if (historialActividad.Eliminar(id))
				return Ok(new { mensaje = "HistorialActividad eliminado correctamente" });
			else
				return BadRequest(new { mensaje = "Error al eliminar HistorialActividad" });
		}
	}
}
