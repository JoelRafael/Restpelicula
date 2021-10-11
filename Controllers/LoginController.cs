using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Restpelicula.Models;
using Restpelicula.Helpers;
using Microsoft.Extensions.Configuration;
namespace Restpelicula.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly evaluacionContext _ctx;
        private readonly IConfiguration conf;
        public LoginController(evaluacionContext ctx, IConfiguration config)
        {
            this._ctx = ctx;
            this.conf = config;
        }
        /// <summary>
        ///Metodo para hacer login
        /// </summary>
        ///<returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        //Esta funcion es para hacer el login 
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(Errorhelpers.GetModelStateErrors(ModelState));
            }
            var user = await _ctx.Usuarios.Include(x => x.IdRolNavigation).Where(x => x.Email == login.Email).SingleOrDefaultAsync();
            if (user == null)
            {
                return NotFound(new { eror = true, message = "Este usuario no existe" });
            }
            string verify_password = Hashhelpers.ConvertToDescrypt(user.Password);
            if (verify_password != login.Password)
            {
                return NotFound(new { error = true, message = "El usuario o el password es incorrecto" });
            }
            //Despues de verificar los datos del usuario y que sean validos, pasamos a crear el token//

            string secret = this.conf.GetValue<string>("SecretKey");
            var jwtHelpers = new Jwthelpers(secret);
            string token = jwtHelpers.CreateToken(user.Nombre);
            var response = new Responseuser
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Email = user.Email,
                IdRol = user.IdRol
            };
            //El token tendra una vigencia de 1 hora//
            return StatusCode(200, new { error = false, message = "Logeado con exito", tokens = token, users = response });
        }
    }
}