using System;
using System.Collections.Generic;

namespace LogAppLibrary
{
    public class PurposeModel
    {
        public string timeInOut { get; set; }
        public int Quantity { get; set; }
        public string Borrow { get; set; }
        public string Return { get; set; }
        public string ItemName { get; set; }

        public PurposeModel() { }
        public PurposeModel(string Time_InOut) 
        { 
            timeInOut = Time_InOut;
        }
        public PurposeModel(decimal quantity, string Time_InOut, string item_name)
        {
            Quantity = Convert.ToInt32(quantity);
            timeInOut = Time_InOut;
            ItemName = item_name;
        }
        public PurposeModel(string borrow, string return_)
        {
            Borrow = borrow;
            Return = return_;
        }
    }
}
