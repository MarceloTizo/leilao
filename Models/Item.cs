using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionAPI.Models
{
    [Table("items")] // Especifica explicitamente o nome da tabela em minúsculas
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("Descricao")]
        public string Descricao { get; set; }

        [Required]
        [Column("LanceInicial")]
        public decimal LanceInicial { get; set; }

        [Required]
        [Column("DataFimLeilao")]
        public DateTime DataFimLeilao { get; set; }

        [Column("LanceAtual")]
        public decimal LanceAtual { get; set; }
    }
}
