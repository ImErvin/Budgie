using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgie.Model
{
    public class Transaction
    {
        public String transactionType { get; set; }
        public String trasnactionName { get; set; }
        public String transactionDesc { get; set; }
        public double transactionAmount { get; set; }

        public override string ToString()
        {
            return "Type: " + transactionType + "\nName: " + trasnactionName + "\nDesc: " + transactionDesc + "\nAmount: " + transactionAmount;
        }
    }
}
