using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;
#nullable disable

namespace Restpelicula.Models
{
    public partial class Actore
    {
        public Actore()
        {
            ActoresPeliculas = new HashSet<ActoresPelicula>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Apellido es requerido")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "El campo Fecha de Nacimiento es requerido")]
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage = "El campo Pais es requerido")]
        public string Pais { get; set; }
        [Required(ErrorMessage = "El campo Ciudad es requerido")]
        public string Ciudad { get; set; }
        [Required(ErrorMessage = "El campo Identificacion es requerido")]
        public string Identificacion { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        [Required(ErrorMessage = "El campo Genero es requerido")]
        public string Genero { get; set; }
        [Required(ErrorMessage = "El campo EmailVerify es requerido")]
        [NotMapped]
        public string EmailVerify { get; set; }



        public virtual ICollection<ActoresPelicula> ActoresPeliculas { get; set; }
    }
}
