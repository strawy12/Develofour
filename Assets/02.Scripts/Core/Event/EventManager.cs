using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static Dictionary<EEvent, Action<object>> eventDictionary = new Dictionary<EEvent, Action<object>>();

    public static void StartListening(EEvent eventName, Action<object> listener)
    {
        Action<object> thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            eventDictionary[eventName] = thisEvent;
        }

        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(EEvent eventName, Action<object> listener)
    {
        Action<object> thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventName] = thisEvent;
        }

        else
        {
            eventDictionary.Remove(eventName);
        }
    }

    public static void TriggerEvent(EEvent eventName, object param = null)
    {
        Action<object> thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke(param);
        }
    }
}


