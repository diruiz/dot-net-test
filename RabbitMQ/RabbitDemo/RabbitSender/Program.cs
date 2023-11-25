using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri(uriString: "amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Sender App";


IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queName = "DemoQue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queName, durable:false, exclusive:false, autoDelete: false, arguments: null);
channel.QueueBind(queName, exchangeName, routingKey, arguments: null);

for (int i = 0; i < 60; i++) {

    Console.WriteLine($"Sending message {i}");

    byte[] messageBodyBytes = Encoding.UTF8.GetBytes(s: $"Message # {i}");
    channel.BasicPublish(exchangeName, routingKey, basicProperties: null, messageBodyBytes);
    Thread.Sleep(1000);
}



channel.Close();
connection.Close();