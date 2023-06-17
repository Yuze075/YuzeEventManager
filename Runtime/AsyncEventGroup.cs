using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YuzeToolkit.Framework.EventManager
{
#if UNITASK
    public partial class EventGroup
    {
        private readonly Dictionary<Type, List<Func<IAsyncEventInfo, UniTask>>> _cachedAsyncEventDictionary = new();

        /// <typeparam name="T">继承自<see cref="IAsyncEventInfo"/>的对象</typeparam>
        public void AddAsyncListener<T>(Func<IAsyncEventInfo, UniTask> func) where T : IEventInfo
        {
            AddAsyncListener(typeof(T), func);
        }

        public void AddAsyncListener(IAsyncEventInfo eventInfo, Func<IAsyncEventInfo, UniTask> func)
        {
            AddAsyncListener(eventInfo.GetType(), func);
        }

        public void AddAsyncListener(Type type, Func<IAsyncEventInfo, UniTask> func)
        {
            if (!_cachedAsyncEventDictionary.ContainsKey(type))
                _cachedAsyncEventDictionary.Add(type, new List<Func<IAsyncEventInfo, UniTask>>());

            if (!_cachedAsyncEventDictionary[type].Contains(func))
            {
                _cachedAsyncEventDictionary[type].Add(func);
                EventManager.AddAsyncListener(type, func);
            }
            else
                Logger.Warning($"[EventGroup.AddAsyncListener]: {func} already in EventGroup");
        }

        /// <typeparam name="T">继承自<see cref="IAsyncEventInfo"/>的对象</typeparam>
        public void RemoveAsyncListener<T>(Func<IAsyncEventInfo, UniTask> func) where T : IEventInfo
        {
            RemoveAsyncListener(typeof(T), func);
        }

        public void RemoveAsyncListener(IAsyncEventInfo eventInfo, Func<IAsyncEventInfo, UniTask> func)
        {
            RemoveAsyncListener(eventInfo.GetType(), func);
        }

        public void RemoveAsyncListener(Type type, Func<IAsyncEventInfo, UniTask> func)
        {
            if (_cachedAsyncEventDictionary.ContainsKey(type))
                if (_cachedAsyncEventDictionary[type].Contains(func))
                {
                    _cachedAsyncEventDictionary[type].Remove(func);
                    EventManager.RemoveAsyncListener(type, func);
                }
                else
                    Logger.Warning($"[EventGroup.RemoveAsyncListener]: {func} is not in EventGroup");
            else
                Logger.Warning($"[EventGroup.RemoveAsyncListener]: {type} is not in EventGroup");
        }
    }
#endif
}