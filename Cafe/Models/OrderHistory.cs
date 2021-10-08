using Cafe.Models.Base;
using Storage.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Models
{
    class OrderHistory : NotifyPropertyChanged
    {
        public DateTime OrderDate { get; set; }
        public int OrderId { get; set; }
        public decimal OrderSum { get; set; }
    }
}
