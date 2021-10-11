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
    [Authorize]
    public class RolController : ControllerBase
    {
        private readonly evaluacionContext _ctx;
        public RolController(evaluacionContext ctx)
        {
            this._ctx = ctx;
        }
        /// <summary>
        /// Metodo para listar los roles que hay, por lo generar solo deben ser dos: Autorizado y No Autorizado
        /// </summary>
        ///<returns></returns>
        [HttpGet("consultar/roles")]
        public async Task<IEnumerable<Rol>> Get_rol()
        {
            return await _ctx.Rols.ToListAsync();
        }
        /// <summary>
        /// Metodo para crear los roles por lo generar solo deben ser dos: Autorizado y No Autorizado
        /// </summary>
        ///<returns></returns>
        [HttpPost("registrar/roles")]
        public async Task<IActionResult> Post_rol(Rol data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Errorhelpers.GetModelStateErrors(ModelState));
            }
            _ctx.Add(data);
            await _ctx.SaveChangesAsync();
            return StatusCode(201, new { error = false, message = "Rol creado con existo" });

        }
        /// <summary>
        /// Metodo para borrar un rol
        /// </summary>
        ///<returns></returns>
        [HttpDelete("borrar/roles/{id}/{emailverify}")]
        public async Task<IActionResult> Delete_rol(int id, string emailverify)
        {
            var consul = await _ctx.Usuarios.Where(x => x.Email == emailverify).SingleOrDefaultAsync();
            if (consul == null)
            {
                return StatusCode(400, new { error = true, message = "Tiene que enviar un correo ya logeado y valido" });
            }
            var verify = await _ctx.Rols.FindAsync(consul.IdRol);
            if (verify.Nombre != "Autorizado")
            {
                return StatusCode(401, new { error = true, message = "No tiene autorizacion para esta consulta" });
            }
            var rol_user = await _ctx.Rols.Include(x => x.Usuarios).Where(x => x.Id == id).SingleOrDefaultAsync();
            if (rol_user == null)
            {
                return NotFound(new { error = true, message = "Este rol no existe" });
            }
            if (rol_user.Usuarios.Count > 0)
            {
                return BadRequest(new { error = true, message = "Este rol no puede ser eliminado" });
            }
            _ctx.Remove(rol_user);
            await _ctx.SaveChangesAsync();
            return StatusCode(200, new { error = false, message = "El rol se ha eliminado" });
        }
    }
}