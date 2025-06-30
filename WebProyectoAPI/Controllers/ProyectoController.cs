using Microsoft.AspNetCore.Mvc;
using WebProyectoAPI;

namespace WebProyectoAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProyectoController : ControllerBase
	{
        //PROYECTO

        [HttpPost("AsignarMiembros")]
        public IActionResult AsignarMiembros([FromBody] AsignacionRequest request)
        {
            Proyecto p = new Proyecto();
            bool resultado = p.AsignarMiembros(request.IdProyecto, request.Usuarios);
            return Ok(new { exito = resultado });
        }

        public class AsignacionRequest
        {
            public int IdProyecto { get; set; }
            public List<int> Usuarios { get; set; }
        }

        [HttpGet("ObtenerProyecto/{id}")]
        public IActionResult ObtenerProyecto(int id)
        {
            Proyecto proyecto = new Proyecto();
            var resultado = proyecto.ObtenerPorId(id);
            if (resultado != null)
            {
                return Ok(resultado);
            }
            else
            {
                return NotFound("Proyecto no encontrado.");
            }
        }


        [HttpGet("{id}/miembros")]
        public IActionResult ObtenerMiembros(int id)
        {
            try
            {
                Proyecto proyecto = new Proyecto();
                var miembros = proyecto.ObtenerMiembros(id);
                return Ok(miembros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener los miembros del proyecto", detalle = ex.Message });
            }
        }


        [HttpGet("ListarPorUsuario/{id}")]
        public IActionResult ListarPorUsuario(int id)
        {
            Proyecto p = new Proyecto();
            var proyectos = p.ListarPorUsuario(id);
            return Ok(proyectos);
        }


        [HttpPost("AgregarProyecto")]
		public IActionResult AgregarProyecto([FromBody] Proyecto proyecto)
		{
			if (proyecto.Agregar())
				return Ok(new { mensaje = "Proyecto agregado correctamente" });
			else
				return BadRequest(new { mensaje = "Error al agregar proyecto" });
		}

		[HttpPut("ActualizarProyecto")]
		public IActionResult ActualizarProyecto([FromBody] Proyecto proyecto)
		{
			if (proyecto.Actualizar())
				return Ok(new { mensaje = "Proyecto actualizado correctamente" });
			else
				return BadRequest(new { mensaje = "Error al actualizar Proyecto" });
		}

		[HttpDelete("EliminarProyecto/{id}")]
		public IActionResult EliminarProyecto(int id)
		{
			Proyecto proyecto = new Proyecto();
			if (proyecto.Eliminar(id))
				return Ok(new { mensaje = "Proyecto eliminado correctamente" });
			else
				return BadRequest(new { mensaje = "Error al eliminar Proyecto" });
		}
	}
}
