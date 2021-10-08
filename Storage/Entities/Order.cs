using Storage.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace Storage.Entities
{
    [Table("order")]
    public class Order : Entity
    {
        [Column("Order_Number")]
        public int OrderNumber { get; set; }

        [Column("Dish_Id")]
        public int DishId { get; set; }

        [Column("Order_Date")]
        public DateTime OrderDate { get; set; }
        public int Qty { get; set; }

        [ForeignKey("DishId")]
        public Dish Dish { get; set; }
    }
}
