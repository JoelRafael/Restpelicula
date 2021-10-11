using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Restpelicula.Models;
using Restpelicula.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
namespace Restpelicula.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]

    public class FilmsController : ControllerBase
    {
        private readonly evaluacionContext _ctx;
        public FilmsController(evaluacionContext ctx)
        {
            this._ctx = ctx;
        } /// <summary>
          ///Metodo para consulta la lista de pelicula
          /// </summary>
          ///<returns></returns>

        [HttpGet("consultar/peliculas/{emailverify}")]
        public async Task<IActionResult> Get_films(string emailverify)
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
            var fimls = await _ctx.Peliculas.ToListAsync();
            return Ok(fimls);
        }

        /// <summary>
        ///Metodo para ver los actores que han participado en una pelicula
        /// </summary>
        ///<returns></returns>

        [HttpGet("consultar/peliculas/actores/{titulo}/{emailverify}")]
        public async Task<IActionResult> films_actors(string titulo, string emailverify)
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
            var id_films = await _ctx.Peliculas.Where(x => x.Titulo == titulo).SingleOrDefaultAsync();
            if (id_films == null)
            {
                return NotFound(new { error = true, message = "Esta pelicula no existe" });
            }
            var films_actor = await _ctx.ActoresPeliculas.Include(x => x.IdActoresNavigation).Where(x => x.IdPeliculas == id_films.Id).ToListAsync();
            if (films_actor == null)
            {
                return NotFound(new { error = true, message = "Esta pelicula no existe" });
            }
            return Ok(new { error = false, rows = films_actor });
        }
        /// <summary>
        ///Metodo para registrar una nueva pelicula
        /// </summary>
        ///<returns></returns>
        [HttpPost("registrar/peliculas")]
        public async Task<IActionResult> Pos_films(Pelicula pelicula)
        {
            var consul = await _ctx.Usuarios.Where(x => x.Email == pelicula.EmailVerify).SingleOrDefaultAsync();
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
            _ctx.Add(pelicula);
            await _ctx.SaveChangesAsync();
            return StatusCode(201, new { error = false, message = "Pelicula registrada con existo" });

        }
        /// <summary>
        ///Metodo para actualizar una peliculas
        /// </summary>
        ///<returns></returns>
        [HttpPut("actualizar/peliculas/{id}")]
        public async Task<IActionResult> Put_films(int id, Pelicula data)
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
            if (id != data.Id)
            {
                return BadRequest(new { error = true, message = "El id no coinsiden" });
            }
            if (!await _ctx.Peliculas.Where(x => x.Id == id).AnyAsync())
            {
                return NotFound(new { error = true, message = "Esta pelicula no existe" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(Errorhelpers.GetModelStateErrors(ModelState));
            }
            _ctx.Entry(data).State = EntityState.Modified;
            await _ctx.SaveChangesAsync();
            return StatusCode(201, new { error = false, message = "Pelicula actualizado con existo" });
        }

        /// <summary>
        ///Metodo para borrar una pelicula
        /// </summary>
        ///<returns></returns>
        [HttpDelete("borrar/peliculas/{id}/{EmailVerify}")]
        public async Task<IActionResult> Delete_films(int id, string EmailVerify)
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
            var films = await _ctx.Peliculas.Include(x => x.ActoresPeliculas).Where(y => y.Id == id).SingleOrDefaultAsync();
            if (films == null)
            {
                return NotFound(new { error = true, message = "Esta pelicula no existe" });
            }
            if (films.ActoresPeliculas.Count > 0)
            {
                return BadRequest(new { error = true, message = "Esta pelicula no se puede borrar por que ya ha participado algunos actores en ella" });
            }
            _ctx.Peliculas.Remove(films);
            await _ctx.SaveChangesAsync();
            return StatusCode(200, new { error = false, message = "Pelicula Eliminado" });
        }
    }
}