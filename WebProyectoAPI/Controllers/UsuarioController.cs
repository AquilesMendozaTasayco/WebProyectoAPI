using Microsoft.AspNetCore.Mvc;
using WebProyectoAPI;

namespace WebProyectoAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UsuarioController : ControllerBase
	{

        //USUARIO

        [HttpGet("BuscarUsuarios")]
        public IActionResult BuscarUsuarios([FromQuery] string filtro)
        {
            Usuario usuario = new Usuario();
            var resultado = usuario.BuscarUsuarios(filtro);
            return Ok(resultado);
        }

        [HttpPut("CambiarClave")]
        public IActionResult CambiarClave([FromBody] dynamic datos)
        {
            try
            {
                int idUsuario = datos.idUsuario;
                string nuevaClave = datos.nuevaClave;

                var usuario = new Usuario();
                bool actualizado = usuario.CambiarClave(idUsuario, nuevaClave);

                if (actualizado)
                    return Ok(new { mensaje = "Contraseña actualizada correctamente" });
                else
                    return BadRequest(new { mensaje = "No se pudo actualizar la contraseña" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al cambiar la contraseña", error = ex.Message });
            }
        }


        [HttpPost("Login")]
		public IActionResult Login([FromBody] Usuario loginRequest)
		{
			Console.WriteLine("🟡 Intentando login...");
			Console.WriteLine("Correo recibido: " + loginRequest.Correo);
			Console.WriteLine("Password recibido: " + loginRequest.Password);

			var usuarios = new Usuario().Listar();

			bool correoEncontrado = false;
			bool passwordCorrecta = false;

			foreach (var u in usuarios)
			{
				Console.WriteLine($"→ Usuario en BD: {u.Correo}");

				if (u.Correo == loginRequest.Correo)
				{
					correoEncontrado = true;
					Console.WriteLine("✓ Correo coincide");

					// Verificamos la contraseña encriptada
					bool contraseñaOk = BCrypt.Net.BCrypt.Verify(loginRequest.Password, u.Password);
					Console.WriteLine($"✓ ¿Contraseña correcta? → {contraseñaOk}");

					if (contraseñaOk)
					{
						passwordCorrecta = true;

						Console.WriteLine("✅ Login exitoso");
						return Ok(new
						{
							mensaje = "Login exitoso",
							idUsuario = u.IdUsuario,
							nombre = u.Nombre,
							correo = u.Correo,
							rol = u.Rol,
							fotoPerfil = string.IsNullOrEmpty(u.FotoPerfil)
								? null
								: $"http://localhost:5155/uploads/{u.FotoPerfil}"
						});


					}
				}
			}

			Console.WriteLine("❌ Login fallido");
			if (!correoEncontrado)
				Console.WriteLine("✗ El correo no existe en la base de datos.");
			else if (!passwordCorrecta)
				Console.WriteLine("✗ La contraseña no coincide.");

			return Unauthorized(new { mensaje = "Correo o contraseña incorrectos" });
		}


		[HttpGet("ExisteCorreo")]
		public IActionResult ExisteCorreo(string correo)
		{
			var usuarios = new Usuario().Listar();
			var existe = usuarios.Any(u => u.Correo == correo);
			return Ok(new { existe });
		}


		[HttpGet("ListarUsuario")]
		public IEnumerable<Usuario> ListarUsuario()
		{
			return new Usuario().Listar();
		}

		[HttpPost("AgregarUsuario")]
		public IActionResult AgregarUsuario([FromBody] Usuario usuario)
		{
			if (usuario.Agregar())
				return Ok(new { mensaje = "Usuario agregado correctamente" });
			else
				return BadRequest(new { mensaje = "Error al agregar usuario" });
		}

		[HttpPut("ActualizarUsuario")]
		public IActionResult ActualizarUsuario([FromBody] Usuario usuario)
		{
			if (usuario.Actualizar())
				return Ok(new { mensaje = "Usuario actualizado correctamente" });
			else
				return BadRequest(new { mensaje = "Error al actualizar usuario" });
		}

		[HttpDelete("EliminarUsuario/{id}")]
		public IActionResult EliminarUsuario(int id)
		{
			Usuario usuario = new Usuario();
			if (usuario.Eliminar(id))
				return Ok(new { mensaje = "Usuario eliminado correctamente" });
			else
				return BadRequest(new { mensaje = "Error al eliminar usuario" });
		}

		[HttpPost("SubirFoto")]
		public async Task<IActionResult> SubirFoto([FromForm] IFormFile archivo, [FromForm] int idUsuario)
		{
			if (archivo == null || archivo.Length == 0)
				return BadRequest(new { mensaje = "Archivo inválido" });

			string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
			if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

			string nombreArchivo = Guid.NewGuid() + Path.GetExtension(archivo.FileName);
			string ruta = Path.Combine(carpeta, nombreArchivo);

			using (var stream = new FileStream(ruta, FileMode.Create))
			{
				await archivo.CopyToAsync(stream);
			}

			Usuario usuario = new Usuario();
			if (usuario.ActualizarFotoPerfil(idUsuario, nombreArchivo))
				return Ok(new { mensaje = "Foto actualizada", nombreArchivo });

			return BadRequest(new { mensaje = "No se pudo actualizar" });
		}

	}
}
