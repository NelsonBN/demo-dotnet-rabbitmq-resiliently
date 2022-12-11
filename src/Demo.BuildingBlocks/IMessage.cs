using MediatR;

namespace Demo.BuildingBlocks;

public interface IMessage : INotification
{
    static abstract string MessageType { get; }
}
