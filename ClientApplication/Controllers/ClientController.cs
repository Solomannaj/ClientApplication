using ClientApplication.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace ClientApplication.Controllers
{
   
    public class ClientController : Controller
    {
        private IAPIHandler handler;
        private IDataSubscriber dataSubscriber;

        public ClientController(IAPIHandler objHandler, IDataSubscriber objSubscriber)
        {
            handler = objHandler;
            dataSubscriber = objSubscriber;
        }

        // GET: Client
        public ActionResult Client()
        {
            string headers= handler.GetCurveHeadersAsync().Result;
            ViewBag.CurveHeaders = GetCurveHeaders(headers);
            return View();
        }

        [HttpPost]
        public JsonResult SubscribeData()
        {
            string row = "";
           
            if(CurvesData.LSTCurvesData !=null && CurvesData.LSTCurvesData.Count > 0)
            {
                row = CurvesData.LSTCurvesData[0];
                CurvesData.LSTCurvesData.Remove(row);
            }

            string[] dataRow = row.Split(',');

           return Json(dataRow, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExportData()
        {
           
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InvokeDataTransfer(string [] curves)
        {
            CurvesData.LSTCurvesData.Clear();
            string requestedCurves = string.Join( ",",curves);

            handler.GetCurveHeadersAsync().GetAwaiter();
            handler.InvokeDataTransfferAsync(requestedCurves).GetAwaiter();

            dataSubscriber.SubscribeData();

            //Action actionSubscribe = () => { dataSubscriber.SubscribeData(); };
            //Task.Run(actionSubscribe);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StopDataTransfer()
        {
            handler.StopDataTransfferAsync().GetAwaiter();
            CurvesData.LSTCurvesData.Clear();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private IList<SelectListItem> GetCurveHeaders(string headers)
        {
            string[] arrHeaders = headers.Split(',');
            List<SelectListItem> lstheaders = new List<SelectListItem>();
            SelectListItem item;
            foreach (string header in arrHeaders)
            {
                item = new SelectListItem { Text = header, Value = header };
                lstheaders.Add(item);
            }
            return lstheaders;
        }
    }
}