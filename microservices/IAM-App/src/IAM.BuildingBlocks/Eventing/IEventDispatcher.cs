namespace IAM.BuildingBlocks.Eventing;

public interface IEventDispatcher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default);
}
