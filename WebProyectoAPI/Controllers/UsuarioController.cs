using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebProyectoAPI.Modelo;

namespace WebProyectoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // ✅ Ruta clara: /api/usuario
    public class UsuarioController : ControllerBase
    {
        // Modelo para cambiar contraseña
        public class CambioClaveRequest
        {
            public int IdUsuario { get; set; }
            public string NuevaClave { get; set; } = "";
        }

        // ✅ Test de conexión a Railway
[HttpGet("test-connection")]
public IActionResult TestConnection()
{
    try
    {
        string cadena = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        if (string.IsNullOrEmpty(cadena))
            return BadRequest("❌ No se encontró la variable CONNECTION_STRING.");

        return Ok($"Cadena que estoy usando:\n{cadena}");
    }
    catch (Exception ex)
    {
        return BadRequest($"❌ Error al conectar: {ex.Message}");
    }
}


        [HttpGet("BuscarUsuarios")]
        public IActionResult BuscarUsuarios([FromQuery] string filtro)
        {
            Usuario usuario = new Usuario();
            var resultado = usuario.BuscarUsuarios(filtro);
            return Ok(resultado);
        }

        [HttpPut("CambiarClave")]
        public IActionResult CambiarClave([FromBody] CambioClaveRequest datos)
        {
            try
            {
                var usuario = new Usuario();
                bool actualizado = usuario.CambiarClave(datos.IdUsuario, datos.NuevaClave);

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
            var usuarios = new Usuario().Listar();

            foreach (var u in usuarios)
            {
                if (u.Correo == loginRequest.Correo)
                {
                    bool contraseñaOk = BCrypt.Net.BCrypt.Verify(loginRequest.Password, u.Password);
                    if (contraseñaOk)
                    {
                        return Ok(new
                        {
                            mensaje = "Login exitoso",
                            idUsuario = u.IdUsuario,
                            nombre = u.Nombre,
                            correo = u.Correo,
                            rol = u.Rol,
                            fotoPerfil = string.IsNullOrEmpty(u.FotoPerfil)
                                ? null
                                : $"https://TU_DOMINIO/uploads/{u.FotoPerfil}" // 🔁 cambia esto por tu dominio
                        });
                    }
                }
            }

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

            return BadRequest(new { mensaje = "No se pudo actualizar la foto" });
        }

        [HttpDelete("EliminarFoto/{idUsuario}")]
        public IActionResult EliminarFoto(int idUsuario)
        {
            try
            {
                var usuario = new Usuario();
                bool eliminado = usuario.EliminarFotoPerfil(idUsuario);

                if (eliminado)
                    return Ok(new { mensaje = "Foto eliminada correctamente" });
                else
                    return BadRequest(new { mensaje = "No se pudo eliminar la foto" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al eliminar la foto", error = ex.Message });
            }
        }
    }
}
