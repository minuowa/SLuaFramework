using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AppEventManager:Singleton<AppEventManager>
{
    Dictionary<AppEventID, List<AppEventHandler>> mHandlers = new Dictionary<AppEventID, List<AppEventHandler>>();

    public void AddEvent(AppEventID eventid,AppEventHandler handler)
    {
        List<AppEventHandler> handlers = null;
        if (!mHandlers.TryGetValue(eventid,out handlers))
        {
            handlers = new List<AppEventHandler>();
            mHandlers.Add(eventid, handlers);
        }
        if(handlers.IndexOf(handler) == -1)
            handlers.Add(handler);
    }
    public void RemoveHandler(AppEventHandler hanlder)
    {
        foreach (var handlers in mHandlers)
            handlers.Value.Remove(hanlder);
    }
    public void RemoveHandler(AppEventID eventid, AppEventHandler handler)
    {
        List<AppEventHandler> handlers = null;
        if (mHandlers.TryGetValue(eventid, out handlers))
            handlers.Remove(handler);
    }
    public void Trigger(AppEventID eventid, object data = null)
    {
        List<AppEventHandler> handlers = null;
        if (mHandlers.TryGetValue(eventid, out handlers))
        {
            foreach (var handler in handlers)
                handler.OnAppEvent(eventid, data);
        }
    }
    public void Destroy()
    {
        mHandlers.Clear();
    }
}
