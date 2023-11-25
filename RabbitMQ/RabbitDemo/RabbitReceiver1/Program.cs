﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri(uriString: "amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Receiver 1 App";


IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queName = "DemoQue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queName, durable: false, exclusive: false, autoDelete: false, arguments: null);
channel.QueueBind(queName, exchangeName, routingKey, arguments: null);
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

var consummer = new EventingBasicConsumer(channel);
consummer.Received += (sender, args) =>
{
    Task.Delay(TimeSpan.FromSeconds(value: 5)).Wait();
    var body = args.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Message Received: {message}");

    channel.BasicAck(args.DeliveryTag, multiple: false);
};

string consumerTag = channel.BasicConsume(queName, autoAck: false, consummer);

Console.ReadLine();

channel.BasicCancel(consumerTag);

channel.Close();
connection.Close();