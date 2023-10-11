﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressFood.FoodApp.Infrastructure.UI
{
    public class ListResponseDTO<T>
    {
        public int TotalCount { get; set; }
        public List<T>? Data { get; set; }
    }
}
