﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class ServiceIndexDto
    {
        public int ServiceIndexId { get; set; }
        public string ServiceIndexHash { get; set; }
        public string ServiceName { get; set; }
        public Decimal ServicePrice { get; set; }

        public ServiceIndexDto() { }
    }
}
