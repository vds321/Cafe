using Storage.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Storage.Entities
{
    [Table("dish")]
    public class Dish : Entity
    {
        [Column("Category_Id")]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}
