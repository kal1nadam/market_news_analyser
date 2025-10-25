using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using NewsAnalyzer.Application.Common.Interfaces;

namespace NewsAnalyzer.Infrastructure.Bus;

public sealed class ChannelEventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, Channel<object>>> _subs = new();
    
    public Task PublishAsync<T>(T message, CancellationToken ct = default)
        {
            // Check if there are any subscribers for the given message type
            if (_subs.TryGetValue(typeof(T), out var subs) && !subs.IsEmpty)
            {
                foreach (var (_, channel) in subs)
                {
                    // Attempt to write the message to the channel synchronously
                    // If the channel buffer is full, fall back to asynchronous writing
                    if (!channel.Writer.TryWrite(message!))
                    {
                        _ = channel.Writer.WriteAsync(message!, ct).AsTask();
                    }
                }
            }
            // Return a completed task as the operation is fire-and-forget
            return Task.CompletedTask;
        }

    public IAsyncEnumerable<T> SubscribeAsync<T>(CancellationToken ct = default)
    {
        // Create an unbounded channel for the given message type
        var channel = Channel.CreateUnbounded<object>(new UnboundedChannelOptions()
        {
            SingleReader = true, // Only one reader is allowed
            SingleWriter = false, // Multiple writers are allowed
        });
        
        // Generate a unique identifier for the subscription
        var id = Guid.NewGuid();
        // Add the channel to the subscription map for the given message type
        var map = _subs.GetOrAdd(typeof(T), _ => new());
        map[id] = channel;
        
        // Return an asynchronous enumerable that reads messages from the channel
        return ReadAll<T>(map, id, channel, ct);
    
        static async IAsyncEnumerable<T> ReadAll<T>(ConcurrentDictionary<Guid, Channel<object>> map, Guid subscriptionId,
            Channel<object> channel, [EnumeratorCancellation] CancellationToken ct)
        {
            try
            {
                // Continuously read messages from the channel until cancellation is requested
                while (await channel.Reader.WaitToReadAsync(ct))
                {
                    while (channel.Reader.TryRead(out var obj))
                    {
                        // Ensure the message is of the expected type before yielding it
                        if (obj is T t)
                        {
                            yield return t;
                        }
                    }
                }
            }
            finally
            {
                // Remove the subscription from the map and complete the channel
                map.TryRemove(subscriptionId, out _);
                channel.Writer.TryComplete();
            }
        }
    }
}