using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LN_WEB.Entities
{
    public class UpdateQuantityRequest
    {
        public long DishCartId { get; set; }
        public int Quantity { get; set; }
    }
}