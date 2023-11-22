using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new("amqps://kxqqxjni:CMnzfY4D8EJPC2k4Nz5atz-SSTKGVic0@sparrow.rmq.cloudamqp.com/kxqqxjni");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

//var randomQueueName = channel.QueueDeclare().QueueName; //rastgele kuyruk ismi verilmesi için
var randomQueueName = "log-database-save-queue";

channel.QueueDeclare(randomQueueName, true, false, false); //son kısmı false olduğu için bu sub silinse bile kalıcı olacaktır. Kalıcı olacak bir kuyruk tanımlamak                                                                istersek bu şekilde olacak

channel.QueueBind(randomQueueName, "logs-fanout",string.Empty,null);

channel.BasicQos(0, 1, false);
var consumer= new EventingBasicConsumer(channel);

channel.BasicConsume(randomQueueName, false,consumer);

Console.WriteLine("Loglar dinleniyor....");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    Thread.Sleep(1000);
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine("Gelen Mesaj : " + message);
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();