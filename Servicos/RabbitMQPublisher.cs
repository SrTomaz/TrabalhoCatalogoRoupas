using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Threading.Channels;

namespace CatalogoRoupas.Servicos
{
    public class RabbitMQPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;      
        private readonly string _responseQueue = "roupasResponseQueue";

        public RabbitMQPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _responseQueue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void SendMessage(object message)
        {
            try
            {
                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(exchange: "",
                                      routingKey: _responseQueue,
                                      basicProperties: null,
                                      body: body);

                Console.WriteLine($"Mensagem enviada: {json}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
            }
        }
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }



    }
}
