

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication.BusinessLogic
{
    public interface IDataSubscriber
    {
        void SubscribeData();
        void Dispose();
    }
    public class DataSubscriber : IDisposable, IDataSubscriber
    {
        private  IConnection connection; 
        private  IModel channel;
        private  ConnectionFactory factory;
        private EventingBasicConsumer consumer;

        public DataSubscriber()
        {
            factory = new ConnectionFactory()
            {
                HostName = ConfigurationManager.AppSettings["RabbitMQServer"],
                UserName = ConfigurationManager.AppSettings["RabbitMQUser"],
                Password = ConfigurationManager.AppSettings["RabbitMQPassword"],
                VirtualHost = ConfigurationManager.AppSettings["RabbitMQVirtualHost"] ,
                Port =Convert.ToInt32(ConfigurationManager.AppSettings["RabbitMQPort"])
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            consumer = new EventingBasicConsumer(channel);
        }

        public void Dispose()
        {
            try
            {
                connection.Dispose();
            }
            catch { }


            try
            {
                channel.Dispose();
            }
            catch { }

        }
            
        public  void SubscribeData()
        {
            channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
               
                if(!CurvesData.LSTCurvesData.Contains(message))
                    CurvesData.LSTCurvesData.Add(message);
            };
            channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);
        }

    }
}
