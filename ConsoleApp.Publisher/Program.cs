

using ConsoleApp.Publisher;
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new("amqps://kxqqxjni:CMnzfY4D8EJPC2k4Nz5atz-SSTKGVic0@sparrow.rmq.cloudamqp.com/kxqqxjni");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);

Random rnd = new Random();
Enumerable.Range(1, 30).ToList().ForEach(x =>
{
    LogNames log1 = (LogNames)rnd.Next(1, 5);
    LogNames log2 = (LogNames)rnd.Next(1, 5);
    LogNames log3 = (LogNames)rnd.Next(1, 5);

    var routeKey = $"{log1}.{log2}.{log3}";

    string message = $"log-type : {log1}-{log2}-{log3}";
    var messageBody = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish("logs-topic", routeKey, null, messageBody);

    Console.WriteLine($"Log gönderilmiştir : {message}");
});



Console.ReadLine();