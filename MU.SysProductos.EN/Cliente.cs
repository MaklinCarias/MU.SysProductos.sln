using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MU.SysProductos.EN
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "El apellido del cliente es obligatorio.")]
        [StringLength(50, ErrorMessage = "El Apellido no puede tener más de 50 caracteres.")]
        public string? Apellido { get; set; }
        [Required(ErrorMessage = "El dui del cliente es obligatorio.")]
        [StringLength(9, ErrorMessage = "El dui no puede tener más de 9 caracteres.")]
        public string? DUI { get; set; }
        [Required(ErrorMessage = "El telefono del cliente es obligatorio.")]
        [StringLength(8, ErrorMessage = "El teléfono no puede tener más de 8 caracteres.")]
        public string? Telefono { get; set; }

    }
}
