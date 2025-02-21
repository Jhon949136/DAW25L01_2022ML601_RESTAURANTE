using System.ComponentModel.DataAnnotations;

namespace L01_2022ML601.Models
{
    public class Pedidos
    {
        [Key]
        public int pedidoId { get; set; }
        public int motoristaId { get; set; }
        public int clienteId { get; set; }
        public int platoId { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
    }
}
