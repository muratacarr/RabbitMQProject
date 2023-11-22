

using ConsoleApp.Publisher;
using RabbitMQ.Client;
using System.Text;

var factory= new ConnectionFactory();
factory.Uri = new("amqps://kxqqxjni:CMnzfY4D8EJPC2k4Nz5atz-SSTKGVic0@sparrow.rmq.cloudamqp.com/kxqqxjni");

using var connection = factory.CreateConnection();
var channel=connection.CreateModel();

channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
{
    var queueName = $"direct-queue-{x}";

    channel.QueueDeclare(queueName, true, false, false);
    channel.QueueBind(queueName,"logs-direct",$"route-{x}",null);
});


Enumerable.Range(1, 30).ToList().ForEach(x =>
{
    LogNames log=(LogNames)new Random().Next(1,5);
    string message = $"Log - {log}";
    var messageBody = Encoding.UTF8.GetBytes(message);
    var routeKey = $"route-{log}";
    channel.BasicPublish("logs-direct",routeKey,null,messageBody);
    Console.WriteLine($"Log gönderilmiştir : {message}");
});



Console.ReadLine();