namespace IAM.BuildingBlocks.Eventing;

public sealed class InMemoryEventDispatcher : IEventDispatcher
{
    private readonly List<object> _events = new();

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        // For now just keep events in memory; future: hook to Kafka/Rabbit
        _events.Add(@event!);
        return Task.CompletedTask;
    }

    public IReadOnlyCollection<object> Events => _events.AsReadOnly();
}
