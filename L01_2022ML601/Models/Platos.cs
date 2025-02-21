using System.ComponentModel.DataAnnotations;

namespace L01_2022ML601.Models
{
    public class Platos
    {
        [Key]
        public int platoId { get; set; }
        public string nombrePlato { get; set; }
        public double precio { get; set; }
    }
}
