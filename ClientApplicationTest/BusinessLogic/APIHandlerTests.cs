using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientApplication.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using ClientApplication.Models;

namespace ClientApplication.BusinessLogic.Tests
{
    [TestClass()]
    public class APIHandlerTests
    {
        IAPIHandler appiHandler;

        [TestInitialize()]
        public void Initialize()
        {
            CurvesData.LSTIndexInfo = new List<CurveInfo>();
        }

        [TestMethod()]
        public void APIHandlerTest()
        {
            try
            {
                var api = new APIHandler();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void GetCurveHeadersAsyncTest()
        {
            try
            {
                appiHandler = new APIHandler();
                var t = appiHandler.GetCurveHeadersAsync().Result;
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void InvokeDataTransfferAsyncTest()
        {
            
            try
            {
                string curves = "A,B,C";
                appiHandler = new APIHandler();
                appiHandler.GetCurveHeadersAsync();
                var t = appiHandler.InvokeDataTransfferAsync(curves).Result;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void StopDataTransfferAsyncTest()
        {
            try
            {
                appiHandler = new APIHandler();
                var t = appiHandler.StopDataTransfferAsync().Result;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void ExportXMLDataAsyncTest()
        {
            try
            {
                appiHandler = new APIHandler();
                CurvesData.LSTIndexInfo.Clear();
                CurvesData.LSTIndexInfo.Add(new CurveInfo() { Name = "A", MaxIndex = 123, MinIndex = 112 });
                var t = appiHandler.ExportXMLDataAsync().Result;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}