using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SupeaI.Models;

namespace Json.Models
{
    public class IncomeData
    {
        public List<Tra_Income> Income {get; set;}
    }

    public class SpendData
    {
        public List<Tra_Spending> Spend {get; set;}
    }

}

