using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;

#nullable disable

namespace Restpelicula.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Email es requerido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo Password es requerido")]
        [MinLength(8, ErrorMessage = "El password debe tener un minimo de 8 caracteres ")]
        [MaxLength(16, ErrorMessage = "El password debe tener un maximo de 16 caracteres ")]
        public string Password { get; set; }

        public int? IdRol { get; set; }

        public virtual Rol IdRolNavigation { get; set; }
    }
}
