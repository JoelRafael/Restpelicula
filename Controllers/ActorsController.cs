
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
    public class ActorsController : ControllerBase
    {
        private readonly evaluacionContext _ctx;
        public ActorsController(evaluacionContext ctx)
        {
            this._ctx = ctx;
        }

        /// <summary>
        /// Metodo para listar los actores
        /// </summary>
        ///<returns></returns>


        [HttpGet("consultar/actores/{emailverify}")]
        public async Task<IActionResult> Get_actors(string emailverify)
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
            var actors = await _ctx.Actores.ToListAsync();
            return Ok(actors);
        }
        /// <summary>
        ///Metodo para ver las peliculas que ha participado un actor
        /// </summary>
        ///<returns></returns>

        [HttpGet("consultar/actores/peliculas/{id}/{emailverify}")]
        public async Task<IActionResult> Actors_films(int id, string emailverify)
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

            var actor_film = await _ctx.ActoresPeliculas.Where(x => x.IdActores == id).Include(x => x.IdPeliculasNavigation).ToListAsync();
            if (actor_film == null)
            {
                return NotFound(new { error = true, message = "Este actor no existe" });
            }
            return Ok(new { error = false, rows = actor_film });
        }

        /// <summary>
        ///Metodo para Crea un nuevo actor
        /// </summary>
        ///<returns></returns>
        [HttpPost("Registrar/actores")]
        public async Task<IActionResult> post_actors(Actore data)
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
            if (!ModelState.IsValid)
            {
                return BadRequest(Errorhelpers.GetModelStateErrors(ModelState));
            }
            _ctx.Add(data);
            await _ctx.SaveChangesAsync();
            return StatusCode(201, new { error = false, message = "Actor creado con existo" });
        }
        /// <summary>
        ///Metodo para actualizar un actor 
        /// </summary>
        ///<returns></returns>
        [HttpPut("actualizar/actores/{id}")]
        public async Task<IActionResult> Put_actors(int id, Actore data)
        {//Hacemos la verificacion del permiso del usuario antes de realizar la accion
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
            //En esta parte verificamos si el id del json y el de la url coinciden para hacer el update//
            if (id != data.Id)
            {
                return BadRequest(new { error = true, message = "El id no coinsiden" });
            }
            //Aqui verificamos si el id que se esta solicitando para el update, existe.
            if (!await _ctx.Actores.Where(x => x.Id == id).AnyAsync())
            {
                return NotFound(new { error = true, message = "Este actor no existe" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(Errorhelpers.GetModelStateErrors(ModelState));
            }
            _ctx.Entry(data).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
            return StatusCode(201, new { error = false, message = "Actor actualizado" });
        }
        /// <summary>
        ///Metodo para eliminar un actor
        /// </summary>
        ///<returns></returns>
        [HttpDelete("borrar/actores/{id}/{EmailVerify}")]
        public async Task<IActionResult> Delete_actor(int id, string EmailVerify)
        {
            var consul = await _ctx.Usuarios.Where(x => x.Email == EmailVerify).SingleOrDefaultAsync();
            if (consul == null)
            {
                return StatusCode(400, new { error = true, message = "Tiene que enviar un correo ya logeado y valido" });
            }
            var verify = await _ctx.Rols.FindAsync(consul.IdRol);
            if (verify.Nombre != "Autorizado")
            {
                return StatusCode(401, new { error = true, message = "No tiene autorizacion para esta consulta" });
            }
            var actors = await _ctx.Actores.Include(x => x.ActoresPeliculas).Where(y => y.Id == id).SingleOrDefaultAsync();
            if (actors == null)
            {
                return NotFound(new { error = true, message = "Este actor no existe" });
            }
            if (actors.ActoresPeliculas.Count > 0)
            {
                return BadRequest(new { error = true, message = "Este actor no se puede borrar por que ya ha participado en algunas peliculas" });
            }
            _ctx.Actores.Remove(actors);
            await _ctx.SaveChangesAsync();
            return StatusCode(200, new { error = false, message = "Actor Eliminado" });
        }
    }
}