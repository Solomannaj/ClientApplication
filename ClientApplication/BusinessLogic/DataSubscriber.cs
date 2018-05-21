

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
    class DataSubscriber : IDisposable
    {
        private  IConnection connection; 
        private  IModel channel;
        ConnectionFactory factory;

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
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            
            channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                CurvesData.LSTCurvesData.Add(message);
            };
            channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);
           
        }

    }
}
