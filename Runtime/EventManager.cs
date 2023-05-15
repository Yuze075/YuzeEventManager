using System;
using System.Collections.Generic;
using UnityEngine;

namespace YuzeToolkit.Framework.EventManager
{
    public static class EventManager
    {
        private class IsNotIEventInfoException : Exception
        {
            public IsNotIEventInfoException(string message) : base(message)
            {
            }
        }

        private static readonly Dictionary<Type, List<Action<IEventInfo>>> EventDictionary = new();

        /// <typeparam name="T">继承自<see cref="IEventInfo"/>的对象</typeparam>
        public static void AddListener<T>(Action<IEventInfo> action) where T : IEventInfo
        {
            AddListener(typeof(T), action);
        }

        public static void AddListener(IEventInfo eventInfo, Action<IEventInfo> action)
        {
            AddListener(eventInfo.GetType(), action);
        }

        public static void AddListener(Type type, Action<IEventInfo> action)
        {
            try
            {
                if (!typeof(IEventInfo).IsAssignableFrom(type))
                    throw new IsNotIEventInfoException($"[EventManager.AddListener]: {type} is not IEventInfo");
                if (!EventDictionary.ContainsKey(type))
                    EventDictionary.Add(type, new List<Action<IEventInfo>>());

                if (!EventDictionary[type].Contains(action))
                    EventDictionary[type].Add(action);
                else
                    Logger.Warning($"[EventManager.AddListener]: {action} already in EventManager");
            }
            catch (IsNotIEventInfoException e)
            {
                Logger.Exception(e);
                throw;
            }
        }

        /// <typeparam name="T">继承自<see cref="IEventInfo"/>的对象</typeparam>
        public static void RemoveListener<T>(Action<IEventInfo> action) where T : IEventInfo
        {
            RemoveListener(typeof(T), action);
        }

        public static void RemoveListener(IEventInfo eventInfo, Action<IEventInfo> action)
        {
            RemoveListener(eventInfo.GetType(), action);
        }

        public static void RemoveListener(Type type, Action<IEventInfo> action)
        {
            try
            {
                if (!typeof(IEventInfo).IsAssignableFrom(type))
                    throw new IsNotIEventInfoException($"[EventManager.RemoveListener]: {type} is not IEventInfo");
                if (EventDictionary.ContainsKey(type))
                    if (EventDictionary[type].Contains(action))
                        EventDictionary[type].Remove(action);
                    else
                        Logger.Warning($"[EventManager.RemoveListener]: {action} is not in EventManager");
                else
                    Logger.Warning($"[EventManager.RemoveListener]: {type} is not in EventManager");
            }
            catch (IsNotIEventInfoException e)
            {
                Logger.Exception(e);
                throw;
            }
        }

        /// <typeparam name="T">继承自<see cref="IEventInfo"/>的对象，并且存在空构造函数<code>new()</code></typeparam>
        public static void TriggerEvent<T>() where T : IEventInfo, new()
        {
            TriggerEvent(new T());
        }

        public static void TriggerEvent(IEventInfo eventInfo)
        {
            var type = eventInfo.GetType();
            if (EventDictionary.ContainsKey(type))
            {
                var actionList = EventDictionary[type];
                if (actionList.Count == 0)
                {
                    Logger.Log($"[EventManager.TriggerEvent]: {eventInfo.GetType()} is null");
                    return;
                }

                foreach (var action in actionList)
                {
                    action.Invoke(eventInfo);
                }
            }
            else
                Logger.Log($"[EventManager.TriggerEvent]: {eventInfo.GetType()} is not in EventManager");
        }

        public static void TriggerEvent(Type type)
        {
            try
            {
                if (!typeof(IEventInfo).IsAssignableFrom(type))
                    throw new IsNotIEventInfoException($"[EventManager.TriggerEvent]: {type} is not IEventInfo");
                TriggerEvent(Activator.CreateInstance(type) as IEventInfo);
            }
            catch (IsNotIEventInfoException e)
            {
                Logger.Exception(e);
                throw;
            }
            catch (MissingMethodException e)
            {
                Logger.Exception(e);
                throw;
            }
        }

        public static void ClearAll()
        {
            foreach (var keyValuePair in EventDictionary)
            {
                keyValuePair.Value.Clear();
            }

            EventDictionary.Clear();
            Logger.Log($"[EventManager.ClearAll]: clear all");
        }
    }
}