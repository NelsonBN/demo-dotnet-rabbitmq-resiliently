using System;
using System.Diagnostics;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Demo.BuildingBlocks.MessageBus;

public static class MessageBusExtensions
{
    public static IBasicProperties SetAppId(this IBasicProperties properties, string? appId = null)
    {
        if(string.IsNullOrWhiteSpace(appId))
        {
            appId = Assembly.GetEntryAssembly()?.GetName()?.Name;
        }

        properties.AppId = appId;

        return properties;
    }

    public static IBasicProperties SetMessageId(this IBasicProperties properties, string? messageId = null)
    {
        properties.MessageId = messageId ?? Guid.NewGuid().ToString();
        return properties;
    }

    public static IBasicProperties SetCorrelationId(this IBasicProperties properties, IBasicProperties originalProperties)
        => properties.SetCorrelationId(originalProperties.MessageId);

    public static IBasicProperties SetCorrelationId(this IBasicProperties properties, string? correlationId = null)
    {
        if(string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Activity.Current?.Id;

            if(string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = $"guid:{Guid.NewGuid()}";
            }
        }

        properties.CorrelationId = correlationId;

        return properties;
    }

    public static IBasicProperties SetContentTypeJson(this IBasicProperties properties)
    {
        properties.ContentType = MediaTypeNames.Application.Json;
        return properties;
    }

    public static IBasicProperties SetMessageType(this IBasicProperties properties, string messageType)
    {
        properties.Type = messageType;
        return properties;
    }

    public static string GetMessageType(this BasicDeliverEventArgs args)
        => args.BasicProperties.Type;

    public static IBasicProperties SetAsDurable(this IBasicProperties properties, bool durable = true)
    {
        properties.Persistent = durable;
        return properties;
    }

    public static IBasicProperties SetEncodingUTF8(this IBasicProperties properties)
    {
        properties.ContentEncoding = Encoding.UTF8.BodyName;
        return properties;
    }


    public static ReadOnlyMemory<byte> Serialize(this object message)
    {
        var payload = JsonSerializer.Serialize(message);
        return new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(payload));
    }

    public static Encoding GetEncoding(this BasicDeliverEventArgs args)
        => Encoding.GetEncoding(args.BasicProperties.ContentEncoding);

    public static object? Deserialize(this BasicDeliverEventArgs args, Type type)
    {
        var encoding = args.GetEncoding();

        var body = args.Body.ToArray();
        var payload = encoding.GetString(body);

        return JsonSerializer.Deserialize(payload, type);
    }

    public static void StopConsume(this IModel channel, params string[] consumerTags)
    {
        if(channel.IsClosed)
        {
            return;
        }

        foreach(var consumerTag in consumerTags)
        {
            channel.BasicCancelNoWait(consumerTag);
        }
    }
}
