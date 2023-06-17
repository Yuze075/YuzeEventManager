using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YuzeToolkit.Framework.EventManager
{
#if UNITASK
    public static partial class EventManager
    {
        private class IsNotIAsyncEventInfoException : Exception
        {
            public IsNotIAsyncEventInfoException(string message) : base(message)
            {
            }
        }

        private static readonly Dictionary<Type, List<Func<IAsyncEventInfo, UniTask>>> AsyncEventDictionary = new();


        /// <typeparam name="T">继承自<see cref="IAsyncEventInfo"/>的对象</typeparam>
        public static void AddAsyncListener<T>(Func<IAsyncEventInfo, UniTask> func) where T : IAsyncEventInfo
        {
            AddAsyncListener(typeof(T), func);
        }

        public static void AddAsyncListener(IAsyncEventInfo asyncEventInfo, Func<IAsyncEventInfo, UniTask> func)
        {
            AddAsyncListener(asyncEventInfo.GetType(), func);
        }

        public static void AddAsyncListener(Type type, Func<IAsyncEventInfo, UniTask> func)
        {
            try
            {
                if (!typeof(IAsyncEventInfo).IsAssignableFrom(type))
                    throw new IsNotIAsyncEventInfoException(
                        $"[EventManager.AddAsyncListener]: {type} is not IAsyncEventInfo");
                if (!AsyncEventDictionary.ContainsKey(type))
                    AsyncEventDictionary.Add(type, new List<Func<IAsyncEventInfo, UniTask>>());

                if (!AsyncEventDictionary[type].Contains(func))
                    AsyncEventDictionary[type].Add(func);
                else
                    Logger.Warning($"[EventManager.AddAsyncListener]: {func} already in EventManager");
            }
            catch (IsNotIAsyncEventInfoException e)
            {
                Logger.Exception(e);
                throw;
            }
        }

        /// <typeparam name="T">继承自<see cref="IAsyncEventInfo"/>的对象</typeparam>
        public static void RemoveAsyncListener<T>(Func<IAsyncEventInfo, UniTask> func) where T : IAsyncEventInfo
        {
            RemoveAsyncListener(typeof(T), func);
        }

        public static void RemoveAsyncListener(IAsyncEventInfo asyncEventInfo, Func<IAsyncEventInfo, UniTask> func)
        {
            RemoveAsyncListener(asyncEventInfo.GetType(), func);
        }

        public static void RemoveAsyncListener(Type type, Func<IAsyncEventInfo, UniTask> func)
        {
            try
            {
                if (!typeof(IAsyncEventInfo).IsAssignableFrom(type))
                    throw new IsNotIAsyncEventInfoException(
                        $"[EventManager.RemoveAsyncListener]: {type} is not IAsyncEventInfo");
                if (AsyncEventDictionary.ContainsKey(type))
                    if (AsyncEventDictionary[type].Contains(func))
                        AsyncEventDictionary[type].Remove(func);
                    else
                        Logger.Warning($"[EventManager.RemoveAsyncListener]: {func} is not in EventManager");
                else
                    Logger.Warning($"[EventManager.RemoveAsyncListener]: {type} is not in EventManager");
            }
            catch (IsNotIAsyncEventInfoException e)
            {
                Logger.Exception(e);
                throw;
            }
        }

        /// <typeparam name="T">继承自<see cref="IAsyncEventInfo"/>的对象，并且存在空构造函数<code>new()</code></typeparam>
        public static async UniTask TriggerAsyncEvent<T>() where T : IAsyncEventInfo, new()
        {
            await TriggerAsyncEvent(new T());
        }

        public static async UniTask TriggerAsyncEvent(IAsyncEventInfo eventInfo)
        {
            var type = eventInfo.GetType();
            if (AsyncEventDictionary.TryGetValue(type, out var funcList))
            {
                if (funcList.Count == 0)
                {
                    Logger.Log($"[EventManager.TriggerAsyncEvent]: {eventInfo.GetType()} is null");
                    return;
                }

                foreach (var func in funcList)
                {
                    await func.Invoke(eventInfo);
                }
            }
            else
                Logger.Log($"[EventManager.TriggerAsyncEvent]: {eventInfo.GetType()} is not in EventManager");
        }

        public static async UniTask TriggerAsyncEvent(Type type)
        {
            try
            {
                if (!typeof(IAsyncEventInfo).IsAssignableFrom(type))
                    throw new IsNotIAsyncEventInfoException(
                        $"[EventManager.TriggerAsyncEvent]: {type} is not IAsyncEventInfo");
                await TriggerAsyncEvent(Activator.CreateInstance(type) as IAsyncEventInfo);
            }
            catch (IsNotIAsyncEventInfoException e)
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


        /// <typeparam name="T">继承自<see cref="IAsyncEventInfo"/>的对象，并且存在空构造函数<code>new()</code></typeparam>
        public static async UniTask TriggerAsyncEventParallel<T>() where T : IAsyncEventInfo, new()
        {
            await TriggerAsyncEventParallel(new T());
        }

        public static async UniTask TriggerAsyncEventParallel(IAsyncEventInfo asyncEventInfo)
        {
            var type = asyncEventInfo.GetType();
            if (AsyncEventDictionary.TryGetValue(type, out var funcList))
            {
                if (funcList.Count == 0)
                {
                    Logger.Log($"[EventManager.TriggerAsyncEvent]: {asyncEventInfo.GetType()} is null");
                    return;
                }

                await UniTask.WhenAll(funcList.Select(func => func.Invoke(asyncEventInfo)));
            }
            else
                Logger.Log($"[EventManager.TriggerAsyncEvent]: {asyncEventInfo.GetType()} is not in EventManager");
        }

        public static async UniTask TriggerAsyncEventParallel(Type type)
        {
            try
            {
                if (!typeof(IAsyncEventInfo).IsAssignableFrom(type))
                    throw new IsNotIAsyncEventInfoException(
                        $"[EventManager.TriggerAsyncEvent]: {type} is not IAsyncEventInfo");
                await TriggerAsyncEventParallel(Activator.CreateInstance(type) as IAsyncEventInfo);
            }
            catch (IsNotIAsyncEventInfoException e)
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
    }
#endif
}