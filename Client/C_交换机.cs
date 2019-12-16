using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Client
{
    public class C_交换机
    {
        public static void 订阅模式()
        {
            Console.WriteLine("Start");
            Console.WriteLine("host:");
            var host = Console.ReadLine();
            Console.WriteLine("port:");
            var portstr = Console.ReadLine();
            int.TryParse(portstr, out int port);
            IConnectionFactory conFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = host,//IP地址
                Port = port,//端口号
                UserName = "admin",//用户账号
                Password = "admin"//用户密码
            };
            Console.WriteLine("exchange name");
            string exchangeName = Console.ReadLine();
            Console.WriteLine("* fanout_订阅发布,direct_路由模式,topic_通配符模式");
            string type = "fanout";

            using (IConnection conn = conFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.BasicQos(0, 1, false);
                    //声明一个队列
                    channel.ExchangeDeclare(
                      exchange: exchangeName,//消息队列名称
                      type: type
                      );
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    //创建队列
                    string queueName = exchangeName + "_" + Guid.NewGuid().ToString("N");
                    channel.QueueDeclare(queueName, false, false, false, null);
                    //绑定队列与交换机
                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");
                    consumer.Received += (model, ea) =>
                    {
                        byte[] message = ea.Body;//接收到的消息
                        int.TryParse(Encoding.UTF8.GetString(message), out int num);
                        if (num == 8)
                        {
                            Thread.Sleep(10000);
                        }
                        Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));
                        //手动确认
                        channel.BasicAck(ea.DeliveryTag, true);
                    };
                    //消费者开启监听(autoAck=false) 关闭自动确认
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }

        public static void 路由模式()
        {
            Console.WriteLine("Start");
            Console.WriteLine("host:");
            var host = Console.ReadLine();
            Console.WriteLine("port:");
            var portstr = Console.ReadLine();
            int.TryParse(portstr, out int port);
            IConnectionFactory conFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = host,//IP地址
                Port = port,//端口号
                UserName = "admin",//用户账号
                Password = "admin"//用户密码
            };
            Console.WriteLine("exchange name");
            string exchangeName = Console.ReadLine();
            Console.WriteLine("fanout_订阅发布,* direct_路由模式,topic_通配符模式");
            string type = "direct";

            Console.WriteLine("routingKey name:");
            string routingKey = Console.ReadLine();
            using (IConnection conn = conFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.BasicQos(0, 1, false);
                    //声明一个队列
                    channel.ExchangeDeclare(
                      exchange: exchangeName,//消息队列名称
                      type: type
                      );
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    //创建队列
                    string queueName = exchangeName + "_" + Guid.NewGuid().ToString("N");
                    channel.QueueDeclare(queueName, false, false, false, null);
                    //绑定队列与交换机
                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);
                    consumer.Received += (model, ea) =>
                    {
                        byte[] message = ea.Body;//接收到的消息
                        int.TryParse(Encoding.UTF8.GetString(message), out int num);
                        Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));
                        //手动确认
                        channel.BasicAck(ea.DeliveryTag, true);
                    };
                    //消费者开启监听(autoAck=false) 关闭自动确认
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// 通配符模式与路由模式一致,但是可以模糊匹配
        /// </summary>
        public static void 通配符模式()
        {
            Console.WriteLine("Start");
            Console.WriteLine("host:");
            var host = Console.ReadLine();
            Console.WriteLine("port:");
            var portstr = Console.ReadLine();
            int.TryParse(portstr, out int port);
            IConnectionFactory conFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = host,//IP地址
                Port = port,//端口号
                UserName = "admin",//用户账号
                Password = "admin"//用户密码
            };
            Console.WriteLine("exchange name");
            string exchangeName = Console.ReadLine();
            Console.WriteLine("fanout_订阅发布,* direct_路由模式,topic_通配符模式");
            string type = "topic";

            Console.WriteLine("请输入要匹配的key,用'_'分割");
            string routingKeyStr = Console.ReadLine();
            string[] routingKeys = routingKeyStr.Split('_');
            using (IConnection conn = conFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.BasicQos(0, 1, false);
                    //声明一个队列
                    channel.ExchangeDeclare(
                      exchange: exchangeName,//消息队列名称
                      type: type
                      );
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    //创建队列
                    string queueName = exchangeName + "_" + Guid.NewGuid().ToString("N");
                    channel.QueueDeclare(queueName, false, false, false, null);
                    foreach (var routingKey in routingKeys)
                    {
                        //绑定队列与交换机
                        channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);
                    }
                    consumer.Received += (model, ea) =>
                    {
                        byte[] message = ea.Body;//接收到的消息
                        int.TryParse(Encoding.UTF8.GetString(message), out int num);
                        Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));
                        //手动确认
                        channel.BasicAck(ea.DeliveryTag, true);
                    };
                    //消费者开启监听(autoAck=false) 关闭自动确认
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }
    }
}
