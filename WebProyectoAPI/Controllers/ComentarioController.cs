using Microsoft.AspNetCore.Mvc;
using WebProyectoAPI;
using WebProyectoAPI.Modelo;

namespace WebProyectoAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ComentarioController : ControllerBase
	{

		//USUARIO

		[HttpGet("ListarComentario")]
		public IEnumerable<Comentario> ListarComentario()
		{
			return new Comentario().Listar();
		}

		[HttpPost("AgregarComentario")]
		public IActionResult AgregarComentario([FromBody] Comentario comentario)
		{
			if (comentario.Agregar())
				return Ok(new { mensaje = "Comentario agregado correctamente" });
			else
				return BadRequest(new { mensaje = "Error al agregar Comentario" });
		}

		[HttpPut("ActualizarComentario")]
		public IActionResult ActualizarComentario([FromBody] Comentario comentario)
		{
			if (comentario.Actualizar())
				return Ok(new { mensaje = "Comentario actualizado correctamente" });
			else
				return BadRequest(new { mensaje = "Error al actualizar Comentario" });
		}

		[HttpDelete("EliminarComentario/{id}")]
		public IActionResult EliminarComentario(int id)
		{
            Comentario comentario = new Comentario();
			if (comentario.Eliminar(id))
				return Ok(new { mensaje = "Comentario eliminado correctamente" });
			else
				return BadRequest(new { mensaje = "Error al eliminar Comentario" });
		}
	}
}
