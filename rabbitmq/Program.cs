using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MongoDB.Driver;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQConsumer
{
    class Program
    {
        private static string rabbitMqHost = "localhost"; // RabbitMQ Host
        private static string queueName = "sensores"; // Nombre de la cola de RabbitMQ
        private static string mongoConnectionString = "mongodb://localhost:27017"; // URL de MongoDB
        private static string mongoDatabaseName = "SensorDB"; // Nombre de la base de datos de MongoDB
        private static string mongoCollectionName = "Sensores"; // Nombre de la colección en MongoDB

        static void Main(string[] args)
        {
            // Establecer conexión a RabbitMQ
            var factory = new ConnectionFactory() { HostName = rabbitMqHost };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declarar la cola
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // Configuración del consumidor
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                // Procesar el mensaje recibido
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                bool messageProcessed = false;
                int retryCount = 0;
                const int maxRetries = 5;

                while (!messageProcessed && retryCount < maxRetries)
                {
                    try
                    {
                        // Intentar procesar el mensaje
                        Console.WriteLine($"Recibido mensaje: {message}");

                        // Simulación de procesamiento: Guardar mensaje en MongoDB
                        SaveMessageToMongoDB(message);
                        
                        // Si el procesamiento es exitoso, marcar como procesado
                        messageProcessed = true;
                        Console.WriteLine("Mensaje procesado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        retryCount++;
                        Console.WriteLine($"Error al procesar el mensaje: {ex.Message}. Reintento #{retryCount}.");
                        Thread.Sleep(2000); // Esperar 2 segundos antes de reintentar
                    }
                }

                if (!messageProcessed)
                {
                    Console.WriteLine("El mensaje no pudo ser procesado después de varios intentos.");
                }
                else
                {
                    // Acknowledge the message to confirm it was processed
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };

            // Consumir mensajes de la cola
            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            // Esperar por mensajes
            Console.WriteLine("Esperando mensajes de RabbitMQ...");
            Console.ReadLine();
        }

        // Función para guardar el mensaje en MongoDB
        private static void SaveMessageToMongoDB(string message)
        {
            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase(mongoDatabaseName);
            var collection = database.GetCollection<dynamic>(mongoCollectionName);

            // Guardamos el mensaje como un documento en MongoDB
            var document = new { Message = message, Timestamp = DateTime.Now };
            collection.InsertOne(document);

            Console.WriteLine("Mensaje guardado en MongoDB.");
        }
    }
}
