using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressFood.FoodApp.Core.Entities
{
    public class Food : BaseEntity
    {
        public string Title { get; set; }
        public int NumberOfFood { get; set; }
        //private double myDiscount;
        //public double Discount
        //{
        //    get { return myDiscount; }
        //    set { myDiscount = value*0.01; }
        //}
        //private double myPrice;
        //public double Price
        //{
        //    get { return myPrice; }
        //    set { myPrice = value - (value * Discount); }
        //}
        public double Discount { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public Restaurant Restaurant { get; set; }
        public int RestaurantId { get; set; }
    }
}
