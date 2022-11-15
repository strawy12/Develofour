using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static Dictionary<int, Action<object>> eventDictionary = new Dictionary<int, Action<object>>();

    public static void StartListening(EEvent eventName, Action<object> listener)
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

    public static void StartListening(EQuestEvent eventName, Action<object> listener)
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

    public static void StopListening(EEvent eventName, Action<object> listener)
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

    public static void StopListening(EQuestEvent eventName, Action<object> listener)
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

    public static void TriggerEvent(EEvent eventName, object param = null)
    {
        Action<object> thisEvent;
        int eventNumber = (int)eventName;

        if (eventDictionary.TryGetValue(eventNumber, out thisEvent))
        {
            thisEvent?.Invoke(param);
        }
    }
    public static void TriggerEvent(EQuestEvent eventName, object param = null)
    {
        Action<object> thisEvent;
        int eventNumber = (int)eventName;

        if (eventDictionary.TryGetValue(eventNumber, out thisEvent))
        {
            thisEvent?.Invoke(param);
        }
    }
}


