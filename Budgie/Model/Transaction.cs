using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgie.Model
{
    // Transaction object, has 5 member variables, overridden ToString method for displaying content
    // and a ToFile method for storing to a storagefile. Promotes Object Orientated Design.
    public class Transaction
    {
        public String transactionType { get; set; }
        public String transactionName { get; set; }
        public String transactionDesc { get; set; }
        public double transactionAmount { get; set; }
        public DateTime transactionDate { get; set; }

        public override string ToString()
        {
            return "Type: " + transactionType + "\nAmount: " + transactionAmount + "\nName: " + transactionName + "\nDesc: " + transactionDesc + "\nDate: " + transactionDate;
        }

        public string ToFile()
        {
            return "" + transactionType + "\n" + transactionAmount + "\n" + transactionName + "\n" + transactionDesc + "\n" + transactionDate + "\n$\n";
        }
    }
}
