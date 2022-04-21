using DomainLayer;
using DomainLayer.ProductModel.Master;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface IPrivacyOperation
    {
        Task Add(PrivacyPolicy data);
        Task Edit(PrivacyPolicy data);
        Task Get(PrivacyPolicy data);
    }
}
