using System;
using System.Collections.Generic;
using UnityEngine;
 
public static class EventManage
{
    private static Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();

    public static void AddListener(string eventName, Action listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] += listener;
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    public static void RemoveListener(string eventName, Action Listener)
    {
        if(eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= Listener; 
        }
    }

    public static void TriggerEvent(string eventName)
    {
         if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName]?.Invoke();
        }
    }

}