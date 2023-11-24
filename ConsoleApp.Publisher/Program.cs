

using ConsoleApp.Publisher;
using RabbitMQ.Client;
using System.Text;

//Publisher
var factory = new ConnectionFactory();
factory.Uri = new("amqps://kxqqxjni:CMnzfY4D8EJPC2k4Nz5atz-SSTKGVic0@sparrow.rmq.cloudamqp.com/kxqqxjni");
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

Dictionary<string,object> headers= new Dictionary<string, object>();

headers.Add("format", "pdf");
headers.Add("shape", "a");

var properties=channel.CreateBasicProperties();
properties.Headers=headers;

channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes("header mesajım"));

Console.WriteLine("Mesaj Gönderilmiştir.");

Console.ReadLine();