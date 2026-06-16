using AccessControl.Web.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessControl.Web.API.Models
{
    [Table("OrderItem")]
   
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }
        public int OrderId { get; set; } 
        public int ProductId { get; set; } 
        public int Quantity { get; set; } 
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }=string.Empty;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
      }
}



