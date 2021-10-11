using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;

#nullable disable

namespace Restpelicula.Models
{
    public partial class ActoresPelicula
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El id de actor es requerdio")]
        public int? IdActores { get; set; }
        [Required(ErrorMessage = "El id de la pelicula es requerdio")]
        public int? IdPeliculas { get; set; }
        [Required(ErrorMessage = "El campo EmailVerify es requerido")]
        [NotMapped]
        public string EmailVerify { get; set; }
        public virtual Actore IdActoresNavigation { get; set; }
        public virtual Pelicula IdPeliculasNavigation { get; set; }
    }
}
