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
        private APIHandler handler;
        private DataSubscriber dataSubscriber;

        public ClientController()
        {
            handler = new APIHandler();
            dataSubscriber = new DataSubscriber();
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
        public JsonResult InvokeDataTransfer(string [] curves)
        {
            string requestedCurves = string.Join( ",",curves);

            handler.GetCurveHeadersAsync().GetAwaiter();
            handler.InvokeDataTransfferAsync(requestedCurves).GetAwaiter();
           
            Action actionSubscribe = () => { dataSubscriber.SubscribeData(); };
            Task.Run(actionSubscribe);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StopDataTransfer()
        {
            handler.StopDataTransfferAsync().GetAwaiter();
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