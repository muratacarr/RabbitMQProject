using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new("amqps://kxqqxjni:CMnzfY4D8EJPC2k4Nz5atz-SSTKGVic0@sparrow.rmq.cloudamqp.com/kxqqxjni");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
//channel.QueueDeclare("hello-queue", true, false, false);

channel.BasicQos(0, 1, true);

var consumer= new EventingBasicConsumer(channel);

var queueName = channel.QueueDeclare().QueueName;

var routeKey = "*.Error.*";
//var routeKey = "Error.#";
channel.QueueBind(queueName, "logs-topic", routeKey);

channel.BasicConsume(queueName, false, consumer);

Console.WriteLine("Loglar dinleniyor...");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Thread.Sleep(1000);
    Console.WriteLine("Gelen Mesaj : " + message);
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();