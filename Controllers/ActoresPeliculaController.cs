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
    public class ActoresPeliculaController : ControllerBase
    {
        private readonly evaluacionContext _ctx;
        public ActoresPeliculaController(evaluacionContext ctx)
        {
            this._ctx = ctx;
        }
        /// <summary>
        /// Metodo para relacionar los actores que han participado en una pelicula
        /// </summary>
        ///<returns></returns>
        [HttpPost("agregar/actores/peliculas")]
        public async Task<IActionResult> Add_actors_films(ActoresPelicula data)
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
            return StatusCode(201, new { error = false, message = "Se ha registrado un actor a una pelicula" });
        }
        /// <summary>
        /// Metodo para eliminar una relacion que haya entre una pelicula y un actor
        /// </summary>
        ///<returns></returns>
        [HttpDelete("borrar/actores/peliculas/{IdActor}/{IdFilms}/{emailverify}")]
        public async Task<IActionResult> Delete_actors_films(int IdActor, int IdFilms, string emailverify)
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
            var objet = await _ctx.ActoresPeliculas.Where(x => x.IdActores == IdActor && x.IdPeliculas == IdFilms).SingleOrDefaultAsync();
            if (objet == null)
            {
                return NotFound(new { error = true, message = "La consulta no tuvo resultado" });
            }
            _ctx.ActoresPeliculas.Remove(objet);
            await _ctx.SaveChangesAsync();
            return StatusCode(200, new { error = false, message = "Actor Eliminado" });
        }
    }
}