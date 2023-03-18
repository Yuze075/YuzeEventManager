using System;

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

        public static void RemoveListener(this IEventInfo eventInfo, EventGroup eventGroup, Action<IEventInfo> action)
        {
            eventGroup.RemoveListener(eventInfo, action);
        }
    }
}