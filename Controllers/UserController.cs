using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Restpelicula.Models;
using Restpelicula.Helpers;
using Microsoft.AspNetCore.Authorization;
namespace Restpelicula.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly evaluacionContext _ctx;
        public UserController(evaluacionContext ctx)
        {
            this._ctx = ctx;
        }
        /// <summary>
        ///Metodo para ver la lista de usuarios
        /// </summary>
        ///<returns></returns>
        [HttpGet("consulta/usuario")]
        public async Task<IEnumerable<Usuario>> get_user()
        {

            return await _ctx.Usuarios.Include(x => x.IdRolNavigation).ToListAsync();
        }
        /// <summary>
        ///Metodo para registrar un usuario y Asignarle un rol los roles son: Autorizado y No Autorizado
        /// </summary>
        ///<returns></returns>
        [HttpPost("registrar/usuario")]
        public async Task<IActionResult> pod_user(Usuario user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Errorhelpers.GetModelStateErrors(ModelState));
            }

            var users = new Usuario
            {
                Nombre = user.Nombre,
                Email = user.Email,
                Password = Hashhelpers.ConvertToEncrypt(user.Password),
                IdRol = user.IdRol
            };
            _ctx.Add(users);
            await _ctx.SaveChangesAsync();
            return StatusCode(201, new { error = false, message = "Usuario creado con existo" });

        }

        /// <summary>
        ///Metodo para cambiar la password a un usuario
        /// </summary>
        ///<returns></returns>
        [HttpPut("cambiar/password")]
        public async Task<IActionResult> update_password(Change_password data)
        {
            var user = await _ctx.Usuarios.Where(x => x.Email == data.Email).SingleOrDefaultAsync();
            if (user == null)
            {
                return StatusCode(400, new { error = true, message = "Tiene que enviar un correo ya logeado y valido" });
            }
            if (user == null)
            {
                return NotFound(new { error = true, message = "Este usuario no existe" });
            }
            user.Password = Hashhelpers.ConvertToEncrypt(data.Password);
            await _ctx.SaveChangesAsync();
            return StatusCode(201, new { error = false, message = "Se ha cambiado el password" });

        }
        /// <summary>
        ///Metodo para cambiar el rol al usuario
        /// </summary>
        ///<returns></returns>
        [Authorize]
        [HttpPut("cambiar/rol")]

        public async Task<IActionResult> Change_rol(Change_rolcs data)
        {
            var consul = await _ctx.Usuarios.Where(x => x.Email == data.EmailVerify).SingleOrDefaultAsync();
            if (consul == null)
            {
                return StatusCode(400, new { error = true, message = "Tiene que enviar un correo ya logeado y valido" });
            }
            var verify = await _ctx.Rols.FindAsync(consul.IdRol);
            if (verify.Nombre != "Autorizado")
            {
                return StatusCode(401, new { error = true, message = "No tiene autorizacion para esta consulta" });
            }
            var user = await _ctx.Usuarios.Where(x => x.Id == data.IdUser).SingleOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { error = true, message = "Este usuaro no existe" });
            }
            user.IdRol = data.IdRol;
            await _ctx.SaveChangesAsync();
            return StatusCode(201, new { error = false, message = "Se ha cambiado el rol" });
        }

        /// <summary>
        ///Metodo para eliminar un usuario
        /// </summary>
        ///<returns></returns>
        [Authorize]
        [HttpDelete("borrar/usuario/{id}")]
        public async Task<IActionResult> Delete_user(int id)
        {
            var user = await _ctx.Usuarios.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (user == null)
            {
                return NotFound(new { error = true, message = "Este usuaro no existe" });
            }
            _ctx.Remove(user);
            await _ctx.SaveChangesAsync();
            return StatusCode(201, new { error = false, message = "Usuario eliminado" });
        }
    }
}