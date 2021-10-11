using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace Restpelicula.Models
{
    public class Change_rolcs
    {
        [Required(ErrorMessage = "El id del usuario es requerido")]
        public int IdUser { get; set; }
        [Required(ErrorMessage = "El id del rol es requerido")]
        public int IdRol { get; set; }
        [Required(ErrorMessage = "El id Email verificador es requerido")]
        public string EmailVerify { get; set; }






    }
}