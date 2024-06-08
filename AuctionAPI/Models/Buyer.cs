using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionAPI.Models
{
    [Table("buyers")]  
    public class Buyer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
