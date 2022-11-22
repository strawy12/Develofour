using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static Dictionary<int, Action<object[]>> eventDictionary = new Dictionary<int, Action<object[]>>();

    public static void StartListening<T>(T eventName, Action<object[]> listener) where T : Enum
    {
        Action<object> thisEvent;
        int eventNumber = (int)eventName;

        if (eventDictionary.TryGetValue(eventNumber, out thisEvent))
        {
            thisEvent += listener;
            eventDictionary[eventNumber] = thisEvent;
        }

        else
        {
            eventDictionary.Add(eventNumber, listener);
        }
    }


    public static void StopListening<T>(T eventName, Action<object[]> listener) where T : Enum
    {
        Action<object> thisEvent;
        int eventNumber = (int)eventName;

        if (eventDictionary.TryGetValue(eventNumber, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventNumber] = thisEvent;
        }

        else
        {
            eventDictionary.Remove(eventNumber);
        }
    }


    public static void TriggerEvent<T>(T eventName, object[] param = null) where T : Enum
    {
        Action<object> thisEvent;
        int eventNumber = (int)eventName;

        if (eventDictionary.TryGetValue(eventNumber, out thisEvent))
        {
            thisEvent?.Invoke(param);
        }
    }
}


