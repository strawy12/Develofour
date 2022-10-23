using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();

    public static void StartListening(string eventName, Action listener)
    {
        Action thisEvent;

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

    public static void StopListening(string eventName, Action listener)
    {
        Action thisEvent;

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

    public static void TriggerEvent(string eventName)
    {
        Action thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke();
        }
    }
}


public class PEventManager
{
    private static Dictionary<string, Action<Param>> eventDictionary = new Dictionary<string, Action<Param>>();

    public static void StartListening(string eventName, Action<Param> listener)
    {
        Action<Param> thisEvent;

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

    public static void StopListening(string eventName, Action<Param> listener)
    {
        Action<Param> thisEvent;

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

    public static void TriggerEvent(string eventName, Param param)
    {
        Action<Param> thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke(param);
            EventManager.TriggerEvent(eventName);
        }
    }
}
