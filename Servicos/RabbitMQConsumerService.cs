using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using CatalogoRoupas.Models;
using CatalogoRoupas.Controllers;
using System.Threading.Channels;
using System.Runtime.Intrinsics.X86;

namespace CatalogoRoupas.Servicos
{
    public class RabbitMQConsumerService : BackgroundService
    {

        //private readonly string _hostName = "localhost";
        private readonly string _hostName = "localhost";
        private readonly string _requestQueue = "roupasQueue";
        //private readonly string _responseQueue = "roupasResponseQueue";

        private IConnection? _connection;
        private IModel? _channel;
        private readonly RoupasService _roupasService;
        private readonly RabbitMQPublisher _publisher;

        string response = "";

        public RabbitMQConsumerService(RoupasService roupasService, RabbitMQPublisher publisher)
        {
            _roupasService = roupasService;
            _publisher = publisher;
            //var factory = new ConnectionFactory() { HostName = _hostName };
            //_connection = factory.CreateConnection();
            //_channel = _connection.CreateModel();
            //_channel.QueueDeclare(queue: _requestQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            //_channel.QueueDeclare(queue: _responseQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }



        //_channel.QueueDeclare(queue: _responseQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

        //public override Task StartAsync(CancellationToken cancellationToken)
        //{
        //    var factory = new ConnectionFactory() { HostName = _hostName };
        //    _connection = factory.CreateConnection();
        //    _channel = _connection.CreateModel();

        //    _channel.QueueDeclare(queue: _requestQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

        //    return base.StartAsync(cancellationToken);
        //}


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName
                //Port = 15672,
                //UserName = "guest",  // Nome de usuário padrão
                //Password = "guest",
                //RequestedHeartbeat = TimeSpan.FromSeconds(10),
                //Ssl = new SslOption()  // Habilitar SSL, se necessário
                //{
                //    Enabled = true,
                //    ServerName = "localhost"
                //}
            };   // Senha padrão };

            //factory.setRequestedHeartbeat(60);
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _requestQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            //_channel.QueueDeclare(queue: _responseQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            return base.StartAsync(cancellationToken);
        }




        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {


            //var factory = new ConnectionFactory() { HostName = _hostName };
            //using var connection = factory.CreateConnection();
            //using var channel = connection.CreateModel();

            //channel.QueueDeclare(queue: _requestQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            //channel.QueueDeclare(queue: _responseQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            //var result = _channel.BasicGet("roupasQueue", true);
            //if (result != null)
            //{
            //    Console.WriteLine($"🔵 Mensagem recebida manualmente: {message}");
            //}


            //var message = Encoding.UTF8.GetString(result.Body.ToArray());
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
                try
                {
                    var request = JsonSerializer.Deserialize<RabbitMQMessage>(message);

                    if (request != null)
                    {
                        switch (request.Tipo.ToLower())
                        {
                            case "create":
                                _roupasService.Create(request.Dados);
                                //response = $"Roupa {JsonSerializer.Serialize(request.Dados.nomePeca)} criada com sucesso";
                                break;
                            case "read":
                                var roupa = _roupasService.Read(request.Dados.idRoupa);

                                //response = $"Detalhes da roupa ID {request.Dados.idRoupa}: Nome=Camisa Preta, Tamanho=M, Preço=99.99";
                                Console.WriteLine($"Roupa encontrada: {JsonSerializer.Serialize(roupa)}");                               
                                break;
                            case "update":
                                _roupasService.Update(request.Dados);
                                //response = "Roupa atualizada com sucesso!";
                                break;
                            case "delete":
                                _roupasService.Delete(request.Dados.idRoupa);
                                //response = "Roupa deletada com sucesso!";
                                break;
                            default:
                                Console.WriteLine($"Tipo de operação inválido: {request.Tipo}");
                                //response = $"Tipo de operação inválido: {request.Tipo}";
                                break;
                        }

                        //if (_channel == null || !_channel.IsOpen)
                        //{
                        //    Console.WriteLine("Canal RabbitMQ não está aberto. Mensagem não foi enviada.");
                        //}
                        //_publisher.SendMessage(response);


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                }
            };
            //var responseBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new { status = "ok", message = response }));
            //_channel.BasicPublish(exchange: "", routingKey: _responseQueue, basicProperties: null, body: responseBody);

            _channel.BasicConsume(queue: _requestQueue, autoAck: true, consumer: consumer);



            //_channel.BasicConsume(queue: _requestQueue, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        //public override Task StopAsync(CancellationToken cancellationToken)
        //{
        //    _channel?.Close();
        //    _connection?.Close();
        //    return base.StopAsync(cancellationToken);
        //}

    }
}
