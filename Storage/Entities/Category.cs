using Storage.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Storage.Entities
{
    [Table("category")]
    public class Category : Entity
    {
        public string Name { get; set; }
        public ICollection<Dish> Dishes { get; set; }
    }
}
