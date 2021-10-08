using Storage.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Storage.Entities
{
    [Table("product")]
    public class Product : Entity
    {
        public string Name { get; set; }
        public double Remains { get; set; }

        public virtual ICollection<Composition> Composition { get; set; }
    }
}
