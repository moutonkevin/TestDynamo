# Push events to a channel

1. Add publisher to DI

`services.AddTransient<IPublisher, AblyPublisher>();`

2. Publish to channel

`var result = await _publisher.PublishAsync(new TestEvent
{
    Content = "helloooo"
});`

The TestEvent class will have to inherit from the Event class. The name of the channel is defined inside

# Listen to events on a channel

1. Add listener to DI

`services.AddTransient<IListener, AblyListener>();`

2. Add the class that will handle the event to DI

`services.AddTransient<IHandler<TestEvent>, TestHandler>();`

3. Create the Handler class

``public class TestHandler : IHandler<TestEvent>
{
    public async Task<bool> HandleAsync(TestEvent message)
    {
        ...
    }
}``

3. Start listening to events

`...Services.GetService<IListener>()
.ListenTo<TestEvent>();`


