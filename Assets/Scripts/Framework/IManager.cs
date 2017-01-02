using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class IManager : AppEventHandler
{
    public abstract bool Initialize();
    public abstract void Update();
    public virtual void OnDestroy() { }
    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
    public virtual void Start() { }

    public string GetName()
    {
        return this.GetType().Name;
    }

    public virtual void OnAppEvent(AppEventID id, object arg = null)
    {
    }
    public void Trigger(AppEventID id, object arg = null)
    {
        AppEventManager.Instance.Trigger(id, arg);
    }
    protected void RegistEvent(AppEventID eventid)
    {
        AppEventManager.Instance.AddEvent(eventid, this);
    }
    protected void RemoveEvent(AppEventID eventid)
    {
        AppEventManager.Instance.RemoveHandler(eventid, this);
    }
    protected void RemoveAllEvent()
    {
        AppEventManager.Instance.RemoveHandler(this);
    }

}
