using AccessControl.Web.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessControl.Web.API.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
         public decimal? TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }=DateTime.UtcNow;
        public virtual ICollection<OrderItem> OrderItems
        {
            get;
            set;
        } = new List<OrderItem>();

    }

}




