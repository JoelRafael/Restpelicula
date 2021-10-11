using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;
namespace Restpelicula.Models
{
    public class Login
    {
        [Required(ErrorMessage = "El campo Email es requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Password { get; set; }



    }
}