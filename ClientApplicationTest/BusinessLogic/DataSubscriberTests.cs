using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientApplication.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using ClientApplication.BusinessLogic.Fakes;
using System.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Fakes;
using System.Configuration.Fakes;
using System.Collections.Specialized;
using System.Threading;

namespace ClientApplication.BusinessLogic.Tests
{
    [TestClass()]
    public class DataSubscriberTests
    {
        IDataSubscriber dataSubscriber;
        NameValueCollection config;

        [TestInitialize()]
        public void Initialize()
        {
            config = new NameValueCollection();
            config.Add("RabbitMQServer", "localhost");
            config.Add("RabbitMQUser", "soloman");
            config.Add("RabbitMQPassword", "May@8102");
            config.Add("QueueName", "hello");
            config.Add("RabbitMQPort", "5672");
            config.Add("RabbitMQVirtualHost", "/");
        }

        [TestMethod()]
        public void DataSubscriberTest()
        {
            try
            {
                using (ShimsContext.Create())
                {
                    ShimDataSubscriber.Constructor = (a) => { };
                    var k = new DataSubscriber();
                }
               
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ConnectToRabbitMQTest()
        {
            string p = string.Empty;
            try
            {
                using (ShimsContext.Create())
                {
                    ShimDataSubscriber.Constructor = (a) => { };
                    dataSubscriber = new DataSubscriber();
                    ShimConfigurationManager.AppSettingsGet = () => { return config; };
                    ShimLogger.WrieExceptionString = (a) => { p = a; };

                    dataSubscriber.ConnectToRabbitMQ();
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void SubscribeDataTest()
        {
            CurvesData.LSTCurvesData.Clear();
            APIHandlerTests api = new APIHandlerTests();
            api.InvokeDataTransfferAsyncTest();
            string p = string.Empty;
            try
            {
                using (ShimsContext.Create())
                {
                    ShimDataSubscriber.Constructor = (a) => { };
                    dataSubscriber = new DataSubscriber();
                    ShimConfigurationManager.AppSettingsGet=()=>{ return config; };
                    ShimLogger.WrieExceptionString = (a) => { p = a; };

                    dataSubscriber.ConnectToRabbitMQ();
                    dataSubscriber.SubscribeData();
                }

                Thread.Sleep(10000);
                var j = CurvesData.LSTCurvesData;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}