using RabbitMQ.Client;
using System;
using System.Text;

namespace Publish
{
    class Program
    {
        static void Main(string[] args)
        {
            //Regiest();
            //P_交换机.订阅发布模式();
            //P_交换机.路由模式();
            P_交换机.通配符模式();
        }

        public static void Regiest()
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
            Console.WriteLine("queue name");
            string queueName = Console.ReadLine();
            using (IConnection con = conFactory.CreateConnection())//创建连接对象
            {
                using (IModel channel = con.CreateModel())//创建连接会话对象
                {
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    while (true)
                    {
                        Console.WriteLine("消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }
    }
}
