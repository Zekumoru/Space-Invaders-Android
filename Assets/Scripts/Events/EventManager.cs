using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    #region Fields

    static Dictionary<EventName, List<UnityEvent<GameObject>>> events = new Dictionary<EventName, List<UnityEvent<GameObject>>>();
    static Dictionary<EventName, List<UnityAction<GameObject>>> listeners = new Dictionary<EventName, List<UnityAction<GameObject>>>();

    #endregion

    #region Public methods

    public static void Initialize()
    {
        foreach (EventName eventName in Enum.GetValues(typeof(EventName)))
        {
            if (!events.ContainsKey(eventName))
            {
                events.Add(eventName, new List<UnityEvent<GameObject>>());
                listeners.Add(eventName, new List<UnityAction<GameObject>>());
            }
            else
            {
                events[eventName].Clear();
                listeners[eventName].Clear();
            }
        }
    }

    public static void AddEventHandler(EventName eventName, UnityEvent<GameObject> eventHandler)
    {
        foreach (UnityAction<GameObject> listener in listeners[eventName])
        {
            eventHandler.AddListener(listener);
        }
        events[eventName].Add(eventHandler);
    }

    public static void AddEventListener(EventName eventName, UnityAction<GameObject> listener)
    {
        foreach (UnityEvent<GameObject> eventHandler in events[eventName])
        {
            eventHandler.AddListener(listener);
        }
        listeners[eventName].Add(listener);
    }

    public static void RemoveEventHandler(EventName eventName, UnityEvent<GameObject> eventHandler)
    {
        events[eventName].Remove(eventHandler);
    }

    #endregion
}
