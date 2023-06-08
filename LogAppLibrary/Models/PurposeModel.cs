using System.Collections.Generic;

namespace LogAppLibrary
{
    public class PurposeModel
    {
        public string timeInOut { get; set; }

        public PurposeModel() { }
        public PurposeModel(string Time_InOut) 
        { 
            timeInOut = Time_InOut;
        }
    }
}
