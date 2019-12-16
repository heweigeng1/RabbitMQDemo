using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Timers;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //Regiest();
            //能者多劳模式.Go();
            //C_交换机.订阅模式();
            //C_交换机.路由模式();
            C_交换机.通配符模式();
        }

        public static void Regiest()
        {
            Console.WriteLine("Client Start");
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
            Console.WriteLine("queue name");
            string queueName = Console.ReadLine();
            using (IConnection conn = conFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //channel.BasicQos(0, 3, false);
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        byte[] message = ea.Body;//接收到的消息
                        int.TryParse(Encoding.UTF8.GetString(message), out int num);
                        if (num == 8)
                        {
                            Thread.Sleep(5000);
                        }
                        Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));
                    };
                    //消费者开启监听
                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }
    }
}
