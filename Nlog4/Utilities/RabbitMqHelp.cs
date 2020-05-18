using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Nlog4.Utilities
{
    public static class RabbitMqHelp
    {
        private static ConnectionFactory factory = new ConnectionFactory() { HostName = "192.168.15.121", UserName = "dev", Password = "654321", VirtualHost = "my_vhost" };



        /// <summary>
        /// 创建交换机
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="routingKey"></param>
        /// <param name="Etype"></param>
        public static void ExchangeDeclare(string ExchangeName, string routingKey = null, string Etype = null)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    if (Etype == null)
                    {
                        Etype = ExchangeType.Fanout;
                    }

                    channel.ExchangeDeclare(ExchangeName, Etype);
                    string message = "Create Exchange";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: ExchangeName,
                                         routingKey: routingKey,
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" MQ Sent {0}", message);
                }
            }
        }

        /// <summary>
        /// 创建队列
        /// </summary>
        /// <param name="QueueNmae"></param>
        /// <param name="Etype"></param>
        public static void QueueDeclare(string QueueNmae, string Etype = null, string ExchangeName = null, string routingKey = null)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    if (Etype == null)
                    {
                        Etype = ExchangeType.Fanout;
                    }
                    if (routingKey == null)
                    {
                        routingKey = QueueNmae;
                    }

                    channel.QueueDeclare(queue: QueueNmae,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                    string message = "Create Queue";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: ExchangeName,
                                         routingKey: routingKey,
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" MQ Sent {0}", message);
                }
            }
        }

        public static void MqPublish(string msg, string ExchangeName, string routingKey)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(msg);

                    channel.BasicPublish(exchange: ExchangeName,
                                         routingKey: routingKey,
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" MQ Sent {0}", msg);
                }
            }
        }



        public static void MqRecive(string msg, string ExchangeName, string routingKey)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(msg);

                    channel.BasicPublish(exchange: ExchangeName,
                                         routingKey: routingKey,
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" MQ Sent {0}", msg);
                }
            }
        }
    }
}
