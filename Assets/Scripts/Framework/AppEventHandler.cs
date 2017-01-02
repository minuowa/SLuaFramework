using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface AppEventHandler
{
    void OnAppEvent(AppEventID id, object arg = null);
    void Trigger(AppEventID id, object arg = null);
}
