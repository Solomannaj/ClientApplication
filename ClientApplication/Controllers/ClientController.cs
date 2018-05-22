using ClientApplication.BusinessLogic;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.Mvc;

namespace ClientApplication.Controllers
{

    public class ClientController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            Logger.WrieException(string.Format("Action method failed - {0}", filterContext.Exception.Message));
            base.OnException(filterContext);
        }

        private IAPIHandler handler;
        private IDataSubscriber dataSubscriber;

        public ClientController(IAPIHandler objHandler, IDataSubscriber objSubscriber)
        {
            handler = objHandler;
            dataSubscriber = objSubscriber;
        }

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

        [HttpGet]
        public ActionResult ExportData()
        {
            string document = handler.ExportXMLDataAsync().Result;
            byte[] bytes = Encoding.Default.GetBytes(document);
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, ConfigurationManager.AppSettings["XMLFileName"]);
        }

        [HttpPost]
        public JsonResult InvokeDataTransfer(string [] curves)
        {
          
            CurvesData.ClearPrevData();
            CurvesData.UpdateHeaders(curves);

            string requestedCurves = string.Join(",", curves);
            handler.InvokeDataTransfferAsync(requestedCurves).Wait();
            dataSubscriber.SubscribeData();

            return Json(true, JsonRequestBehavior.AllowGet);
          
        }

        [HttpPost]
        public JsonResult StopDataTransfer()
        {
            handler.StopDataTransfferAsync().Wait();
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