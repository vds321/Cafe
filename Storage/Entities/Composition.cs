using Storage.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;


namespace Storage.Entities
{
    [Table("composition")]
    public class Composition : Entity
    {
        [Column("Dish_Id")]
        public int DishId { get; set; }

        [Column("Product_Id")]
        public int ProductId { get; set; }

        public virtual Dish Dish { get; set; }
        public virtual Product Product { get; set; }
    }
}
