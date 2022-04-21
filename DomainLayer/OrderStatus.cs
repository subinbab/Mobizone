using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer
{
    public enum OrderStatus
    {
        orderplaced = 1,
        dispatched,
        delivered,
        cancelled
    }
}
