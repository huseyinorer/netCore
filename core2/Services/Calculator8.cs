﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core2.Services
{
    public class Calculator8 : ICalculate
    {
        public decimal Calculate(decimal amount)
        {
            return amount * 8 / 100;
        }
    }
}
