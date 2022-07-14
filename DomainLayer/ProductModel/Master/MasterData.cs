using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.ProductModel.Master
{
   
    public enum Master:int{

        Brand=1,
        SimType=2,
        ProductType=3,
        OsProcessor=4,
        OsCore=5,
        Ram=6,
        Storage=7,
        CamFeature=8,
        Color

    }
    public class masterModel
    {
        public Master master { get; set; }
        public int IsActive { get; set; }
    }
}
