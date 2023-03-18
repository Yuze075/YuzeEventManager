using System;
using System.Collections.Generic;
using UnityEngine;

namespace YuzeToolkit.Framework.EventManager
{
    /// <summary>
    /// 用于管理一个对象中的全部事件，方便统一删除
    /// </summary>
    public class EventGroup
    {
        private readonly Dictionary<Type, List<Action<IEventInfo>>> _cachedEventDictionary = new();

        /// <typeparam name="T">继承自<see cref="IEventInfo"/>的对象</typeparam>
        public void AddListener<T>(Action<IEventInfo> action) where T : IEventInfo
        {
            AddListener(typeof(T), action);
        }
        
        public void AddListener(IEventInfo eventInfo, Action<IEventInfo> action)
        {
            AddListener(eventInfo.GetType(), action);
        }

        private void AddListener(Type type, Action<IEventInfo> action)
        {
            if (!_cachedEventDictionary.ContainsKey(type))
                _cachedEventDictionary.Add(type, new List<Action<IEventInfo>>());

            if (!_cachedEventDictionary[type].Contains(action))
            {
                _cachedEventDictionary[type].Add(action);
                EventManager.AddListener(type, action);
            }
            else
                Logger.Log($"[EventGroup.AddListener]: {action} already in EventGroup");
        }

        /// <typeparam name="T">继承自<see cref="IEventInfo"/>的对象</typeparam>
        public void RemoveListener<T>(Action<IEventInfo> action) where T : IEventInfo
        {
            RemoveListener(typeof(T), action);
        }

        public void RemoveListener(IEventInfo eventInfo, Action<IEventInfo> action)
        {
            RemoveListener(eventInfo.GetType(), action);
        }

        private void RemoveListener(Type type, Action<IEventInfo> action)
        {
            if (_cachedEventDictionary.ContainsKey(type))
                if (_cachedEventDictionary[type].Contains(action))
                {
                    _cachedEventDictionary[type].Remove(action);
                    EventManager.RemoveListener(type, action);
                }
                else
                    Logger.Log($"[EventGroup.RemoveListener]: {action} is not in EventGroup");
            else
                Logger.Log($"[EventGroup.RemoveListener]: {type} is not in EventGroup");
        }

        public void ClearAll()
        {
            foreach (var (type, actionList) in _cachedEventDictionary)
            {
                foreach (var action in actionList)
                {
                    EventManager.RemoveListener(type, action);
                }

                actionList.Clear();
            }

            _cachedEventDictionary.Clear();
            Logger.Log($"[EventGroup.ClearAll]: clear all");
        }
    }
}