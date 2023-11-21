

using RabbitMQ.Client;
using System.Text;

var factory= new ConnectionFactory();
factory.Uri = new("amqps://kxqqxjni:CMnzfY4D8EJPC2k4Nz5atz-SSTKGVic0@sparrow.rmq.cloudamqp.com/kxqqxjni");

using var connection = factory.CreateConnection();
var channel=connection.CreateModel();
channel.QueueDeclare("hello-queue",true,false,false);

Enumerable.Range(1, 30).ToList().ForEach(x =>
{
    string message = $"Merhaba {x}";
    var messageBody = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);
    Console.WriteLine($"Mesaj gönderilmiştir : {message}");
});



Console.ReadLine();