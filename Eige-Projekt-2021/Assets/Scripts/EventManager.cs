using System;
using System.Collections.Generic;
using System.Linq;

public class EventManager
{
    private static readonly EventManager Instance = new EventManager();

    public delegate void EventConsumer<T>(in T element) where T : Event;

    private readonly Dictionary<Type, List<EventConsumer<Event>>> _eventMap = new Dictionary<Type, List<EventConsumer<Event>>>();

    public static void subscribe(EventConsumer<Event> consumer)
    {
        var eventType = consumer.GetType().GetGenericArguments().Single();
        if (!Instance._eventMap.ContainsKey(eventType))
        {
            throw new InvalidOperationException("Tried to subscribe to unregistered Event type " + eventType);
        }

        var eventList = Instance._eventMap[eventType];
        eventList.Add(consumer);
    }

    public static void register(Type eventType)
    {
        if (eventType.IsSubclassOf(typeof(Event)))
        {
            Instance._eventMap[eventType] = new List<EventConsumer<Event>>();
        }
    }

    public static void dispatch<T>(T eventOccurence) where T : Event
    {
        var eventType = eventOccurence.GetType();
        if (!Instance._eventMap.ContainsKey(eventType))
        {
            throw new InvalidOperationException("Tried to dispatch unregistered Event type " + eventType);
        }

        var eventList = Instance._eventMap[eventType];
        foreach (var eventConsumer in eventList)
        {
            eventConsumer(eventOccurence);
        }
    }
}