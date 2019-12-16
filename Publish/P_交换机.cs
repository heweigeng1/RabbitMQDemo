using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Publish
{
    public class P_交换机
    {
        /// <summary>
        /// 开启2个client 客户端,每次发送消息,客户端都会收到相同的信息.
        /// </summary>
        public static void 订阅发布模式()
        {
            Console.WriteLine("Exchange Start");
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
            Console.WriteLine("fanout_订阅发布,direct_路由模式,topic_通配符模式");
            string type = "fanout";
            using (IConnection con = conFactory.CreateConnection())//创建连接对象
            {
                using (IModel channel = con.CreateModel())//创建连接会话对象
                {
                    //声明一个队列
                    channel.ExchangeDeclare(
                      exchange: exchangeName,//消息队列名称
                      type: type);
                    while (true)
                    {
                        Console.WriteLine("消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }

        /// <summary>
        /// 其实就是相对于把上面的的方法 把 routingKey指定了,并且把 type 指定为 direct 路由模式
        /// </summary>
        public static void 路由模式()
        {
            Console.WriteLine("Exchange Start");
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
            Console.WriteLine("fanout_订阅发布,direct_路由模式,topic_通配符模式");
            string type = "direct";
            Console.WriteLine("routingKey name:");
            string routingKey = Console.ReadLine();
            using (IConnection con = conFactory.CreateConnection())//创建连接对象
            {
                using (IModel channel = con.CreateModel())//创建连接会话对象
                {
                    //声明一个队列
                    channel.ExchangeDeclare(
                      exchange: exchangeName,//消息队列名称
                      type: type);
                    while (true)
                    {
                        Console.WriteLine("消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }

        public static void 通配符模式()
        {
            Console.WriteLine("Exchange Start");
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
            Console.WriteLine("fanout_订阅发布,direct_路由模式,topic_通配符模式");
            string type = "topic";
            Console.WriteLine("routingKey name:");
            string routingKey = Console.ReadLine();
            using (IConnection con = conFactory.CreateConnection())//创建连接对象
            {
                using (IModel channel = con.CreateModel())//创建连接会话对象
                {
                    //声明一个队列
                    channel.ExchangeDeclare(
                      exchange: exchangeName,//消息队列名称
                      type: type);
                    while (true)
                    {
                        Console.WriteLine("消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }
    }
}
