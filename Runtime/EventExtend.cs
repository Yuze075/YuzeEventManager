using System;
using Cysharp.Threading.Tasks;

namespace YuzeToolkit.Framework.EventManager
{
    public static class EventExtend
    {
        public static void TriggerEvent(this IEventInfo eventInfo)
        {
            EventManager.TriggerEvent(eventInfo);
        }

        public static void AddListener(this IEventInfo eventInfo, Action<IEventInfo> action)
        {
            EventManager.AddListener(eventInfo, action);
        }

        public static void RemoveListener(this IEventInfo eventInfo, Action<IEventInfo> action)
        {
            EventManager.RemoveListener(eventInfo, action);
        }

        public static void AddListener(this IEventInfo eventInfo, EventGroup eventGroup, Action<IEventInfo> action)
        {
            eventGroup.AddListener(eventInfo, action);
        }

        public static void AddListener(this IEventInfo eventInfo, Action<IEventInfo> action, EventGroup eventGroup)
        {
            eventGroup.AddListener(eventInfo, action);
        }

        public static void RemoveListener(this IEventInfo eventInfo, EventGroup eventGroup, Action<IEventInfo> action)
        {
            eventGroup.RemoveListener(eventInfo, action);
        }

        public static void RemoveListener(this IEventInfo eventInfo, Action<IEventInfo> action, EventGroup eventGroup)
        {
            eventGroup.RemoveListener(eventInfo, action);
        }

#if UNITASK
        public static async UniTask TriggerAsyncEvent(this IAsyncEventInfo asyncEventInfo)
        {
            await EventManager.TriggerAsyncEvent(asyncEventInfo);
        }
        
        public static async UniTask TriggerAsyncEventParallel(this IAsyncEventInfo asyncEventInfo)
        {
            await EventManager.TriggerAsyncEventParallel(asyncEventInfo);
        }

        public static void AddAsyncListener(this IAsyncEventInfo asyncEventInfo, Func<IAsyncEventInfo, UniTask> func)
        {
            EventManager.AddAsyncListener(asyncEventInfo, func);
        }

        public static void RemoveAsyncListener(this IAsyncEventInfo asyncEventInfo, Func<IAsyncEventInfo, UniTask> func)
        {
            EventManager.RemoveAsyncListener(asyncEventInfo, func);
        }

        public static void AddAsyncListener(this IAsyncEventInfo asyncEventInfo, EventGroup eventGroup,
            Func<IAsyncEventInfo, UniTask> func)
        {
            eventGroup.AddAsyncListener(asyncEventInfo, func);
        }

        public static void AddAsyncListener(this IAsyncEventInfo asyncEventInfo, Func<IAsyncEventInfo, UniTask> func,
            EventGroup eventGroup)
        {
            eventGroup.AddAsyncListener(asyncEventInfo, func);
        }

        public static void RemoveAsyncListener(this IAsyncEventInfo asyncEventInfo, EventGroup eventGroup,
            Func<IAsyncEventInfo, UniTask> func)
        {
            eventGroup.RemoveAsyncListener(asyncEventInfo, func);
        }

        public static void RemoveAsyncListener(this IAsyncEventInfo asyncEventInfo, Func<IAsyncEventInfo, UniTask> func,
            EventGroup eventGroup)
        {
            eventGroup.RemoveAsyncListener(asyncEventInfo, func);
        }
#endif
    }
}