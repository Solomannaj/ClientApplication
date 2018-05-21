using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientApplication.BusinessLogic
{

    public static class CurvesData 
    {
       static CurvesData()
       {
            LSTCurvesData = new List<string>();
       }
        public static List<string> LSTCurvesData { get; set; }
    }
}