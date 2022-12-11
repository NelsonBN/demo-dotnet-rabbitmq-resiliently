using System.Collections.Generic;
using RabbitMQ.Client;

namespace Demo.BuildingBlocks.MessageBus;

public static class MessageBusFactory
{
    public static void CreateUnroutedExchange(this IModel channel, string exchangeName)
    {
        // Unrouted exchange
        var unroutedName = $"{exchangeName}-Unrouted";

        channel.ExchangeDeclare(
            exchange: unroutedName,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            arguments: null);

        channel.QueueDeclare(
            queue: unroutedName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        channel.QueueBind(
            queue: unroutedName,
            exchange: unroutedName,
            routingKey: "",
            arguments: null);



        // Main exchange
        channel.ExchangeDeclare(
            exchange: exchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            arguments: new Dictionary<string, object>
            {
                ["alternate-exchange"] = unroutedName,
            });
    }

    public static void SubscribeExchangeWithDeadletter(this IModel channel, string queueName, string exchangeName, params string[] routes)
    {
        channel.CreateQueueWithDeadletter(queueName, exchangeName);

        foreach(var route in routes)
        {
            channel.QueueBind(
                queue: queueName,
                exchange: exchangeName,
                routingKey: route);
        }
    }

    public static void CreateQueueWithDeadletter(this IModel channel, string queueName, string exchangeName)
    {
        // Deadletter
        var queueDeadletterName = $"{queueName}-Deadletter";
        var exchangeQeadletterName = $"{exchangeName}-{queueName}-Deadletter";

        channel.ExchangeDeclare(
            exchange: exchangeQeadletterName,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            arguments: null);

        channel.QueueDeclare(
            queue: queueDeadletterName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        channel.QueueBind(
            queue: queueDeadletterName,
            exchange: exchangeQeadletterName,
            routingKey: "",
            arguments: null);

        // Create queues
        channel.QueueDeclare(
            queue: queueName,
            durable: true, // Persiste queue
            exclusive: false, // Allow multi connection
            autoDelete: false, // Don't delete when all cosumer close connection
            arguments: new Dictionary<string, object>
            {
                ["x-dead-letter-exchange"] = exchangeQeadletterName,
                ["x-dead-letter-routing-key"] = ""
            });
    }
}
