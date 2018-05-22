using ClientApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientApplication.BusinessLogic
{

    public static class CurvesData 
    {
       static CurvesData()
       {
            LSTCurvesData = new List<string>();
            LSTIndexInfo = new List<CurveInfo>();
            LSTCurveHeaders = new List<string>();
       }

        public static List<string> LSTCurveHeaders { get; set; }
        public static List<string> LSTCurvesData { get; set; }

        public static List<CurveInfo> LSTIndexInfo { get; set; }

        public static void UpdateData(string curveData)
        {
            try
            {
                string[] curves = curveData.Split(',');
                long index;
                if (curves != null && curves.Length > 0)
                {
                    index = Convert.ToInt64(curves[0]);
                    if (!LSTCurvesData.Contains(curveData))
                        LSTCurvesData.Add(curveData);

                    foreach (string header in LSTCurveHeaders)
                    {
                        CurveInfo[] infos = LSTIndexInfo.Where(x => (x.Name.TrimEnd().TrimStart() == header.TrimEnd().TrimStart())).ToArray();
                        if (infos.Length > 0)
                        {
                            if (infos[0].MaxIndex < index)
                            {
                                infos[0].MaxIndex = index;
                            }

                            if (infos[0].MinIndex > index)
                            {
                                infos[0].MinIndex = index;
                            }
                        }
                        else
                        {
                            CurveInfo info = new CurveInfo() { Name = header, MaxIndex = index, MinIndex = index };
                            if (!LSTIndexInfo.Contains(info))
                                LSTIndexInfo.Add(info);
                        }
                    }

                }
                else
                {
                    throw new Exception("Empty or Invalid data received");
                }
            }
            catch (Exception ex)
            {
                Logger.WrieException(string.Format("Data Subscription failed - {0}", ex.Message));
                throw new Exception(string.Format("Data Subscription failed - {0}", ex.Message));
            }
        }

        public static void UpdateHeaders(string[] curveHeaders)
        {
            foreach(string header in curveHeaders)
            {
                LSTCurveHeaders.Add(header);
            }
        }

        public static void ClearPrevData()
        {
            LSTCurvesData.Clear();
            LSTIndexInfo.Clear();
            LSTCurveHeaders.Clear();
        }
    }
}