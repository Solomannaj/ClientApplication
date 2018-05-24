
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Configuration;
using System.Text;

namespace ClientApplication.BusinessLogic
{
    public interface IDataSubscriber
    {
        void SubscribeData();
        void ConnectToRabbitMQ();


    }
    public class DataSubscriber : IDataSubscriber
    {
        private  IConnection connection; 
        private  IModel channel;
        private  ConnectionFactory factory;
        private EventingBasicConsumer consumer;

        public void ConnectToRabbitMQ()
        {
            try
            {
                factory = new ConnectionFactory()
                {
                    HostName = ConfigurationManager.AppSettings["RabbitMQServer"],
                    UserName = ConfigurationManager.AppSettings["RabbitMQUser"],
                    Password = ConfigurationManager.AppSettings["RabbitMQPassword"],
                    VirtualHost = ConfigurationManager.AppSettings["RabbitMQVirtualHost"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["RabbitMQPort"])
                };

                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                consumer = new EventingBasicConsumer(channel);
            }
            catch (Exception ex)
            {
                Logger.WrieException(string.Format("Failed to connect RabbitMQ channel - {0}", ex.Message));
                throw new Exception(string.Format("Failed to connect RabbitMQ channel - {0}", ex.Message));
            }
        }
            
        public  void SubscribeData()
        {
            try
            {
                channel.QueueDeclare(queue: ConfigurationManager.AppSettings["QueueName"], durable: false, exclusive: false, autoDelete: false, arguments: null);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    CurvesData.UpdateData(message);
                };
                channel.BasicConsume(queue: ConfigurationManager.AppSettings["QueueName"], autoAck: true, consumer: consumer);
            }
            catch (Exception ex)
            {
                Logger.WrieException(string.Format("Exception occured with RabbitMQ channel - {0}", ex.Message));
                throw new Exception(string.Format("Exception occured with RabbitMQ channel - {0}", ex.Message));
            }
           
        }
        
    }
}
