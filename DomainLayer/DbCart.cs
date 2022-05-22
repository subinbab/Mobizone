﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer
{
    public class DbCart
    {
        public int id { get; set; }
        public string sessionId { get; set; }
        public ICollection<CartDetails> cartDetails { get; set; }
    }
}
