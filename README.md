# YuzeEventManager
* 一个基于Type类型实现的事件管理器

## 使用
* 继承自`IEventInfo`的类都可以作为一个事件
* 通过`EventManager`可以聆听或者触发不同的事件

## `EventGroup`
* 用于管理单个对象中的事件
* 方便移除一个对象中全部事件
