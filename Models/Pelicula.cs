using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;
#nullable disable

namespace Restpelicula.Models
{
    public partial class Pelicula
    {
        public Pelicula()
        {
            ActoresPeliculas = new HashSet<ActoresPelicula>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Titulo es requerido")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "El campo Genero es requerido")]
        public string Genero { get; set; }
        [Required(ErrorMessage = "El campo Fecha de creacion es requerido")]
        public DateTime FechaCreacion { get; set; }
        [Required(ErrorMessage = "El campo Creada por es requerido")]
        public string CreadaPor { get; set; }
        [Required(ErrorMessage = "El campo Publico dirigido es requerido")]
        public string Publico { get; set; }
        [Required(ErrorMessage = "El campo EmailVerify es requerido")]
        [NotMapped]
        public string EmailVerify { get; set; }

        public virtual ICollection<ActoresPelicula> ActoresPeliculas { get; set; }
    }
}
