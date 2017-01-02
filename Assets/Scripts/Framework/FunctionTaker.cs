using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLua;

public class FuncWrapper
{
    LuaFunction mCallBack;

    static LuaState mState;
    static LuaState State
    { get { if (mState == null) mState = LuaManager.Instance.MainState; return mState; } }

    public static implicit operator FuncWrapper(LuaFunction fun)
    {
        FuncWrapper wrapper = new FuncWrapper();
        wrapper.mCallBack = fun;
        return wrapper;
    }

    public object call(LuaTable instance)
    {
        if (mCallBack != null)
            return mCallBack.call(instance);
        return null;
    }

    public object call(LuaTable instance, params object[] args)
    {
        if (mCallBack != null)
        {
            System.IntPtr L = instance.L;
            int error = LuaObject.pushTry(L);

            LuaObject.pushVar(L, instance);

            for (int n = 0; args != null && n < args.Length; n++)
            {
                LuaObject.pushVar(L, args[n]);
            }

            if (mCallBack.innerCall(args != null ? args.Length + 1 : 1, error))
            {
                return State.topObjects(error - 1);
            }

            return null;
        }

        return null;
    }

    public object call(LuaTable instance, object a1)
    {
        if (mCallBack != null)
            return mCallBack.call(a1);
        return null;
    }

    public object call(LuaTable instance, object a1, object a2)
    {
        if (mCallBack != null)
            return mCallBack.call(a1,a2);
        return null;
    }

    public object call(LuaTable instance, object a1, object a2, object a3)
    {
        if (mCallBack != null)
            return mCallBack.call(instance, a1, a2, a3);
        return null;
    }
    public void Dispose()
    {
        if (mCallBack!=null)
            mCallBack.Dispose();
    }
}

public class FunctionTaker : BoolObject
{
    public static Dictionary<string, FunctionTaker> ModuleFunctionContaners = new Dictionary<string, FunctionTaker>();

    public FuncWrapper LoadProperty;

    public FuncWrapper Awake;
    public FuncWrapper Start;
    public FuncWrapper Update;
    public FuncWrapper LateUpdate;
    public FuncWrapper OnShow;
    public FuncWrapper OnHide;
    public FuncWrapper OnEnable;
    public FuncWrapper OnDisable;
    public FuncWrapper OnDestroy;
    public FuncWrapper OnCollisionEnter;
    public FuncWrapper OnTriggerEnter;
    public FuncWrapper OnTriggerStay;
    public FuncWrapper OnTriggerExit;
    public FuncWrapper OnApplicationQuit;
    //itween
    public FuncWrapper OnTweenComplete;
    public FuncWrapper OnTweenUpdate;

    //cache
    public FuncWrapper OnItemReady;
    public FuncWrapper OnItemCompleted;

    //
    public FuncWrapper OnAnim0;
    public FuncWrapper OnAnim1;
    public FuncWrapper OnAnim2;
    public FuncWrapper OnAnim3;
    public FuncWrapper OnAnim4;
    public FuncWrapper OnAnim5;

    //UI
    public FuncWrapper OnToggle;
    public FuncWrapper OnCommand;
    public FuncWrapper OnOpenPage;
    public FuncWrapper OnPress;
    public FuncWrapper OnSubmit;
    public FuncWrapper OnValueChange;
    public FuncWrapper OnDragStart;
    public FuncWrapper OnDrag;
    public FuncWrapper OnDragEnd;
    public FuncWrapper OnDragDropRelease;
    public FuncWrapper OnCenterPage;

    public virtual void ReloadFunctions(LuaTable luamodule)
    {
        LoadProperty = (LuaFunction)luamodule["LoadProperty"];
        Awake = (LuaFunction)luamodule["Awake"];
        Start = (LuaFunction)luamodule["Start"];
        Update = (LuaFunction)luamodule["Update"];
        LateUpdate = (LuaFunction)luamodule["LateUpdate"];

        OnShow = (LuaFunction)luamodule["OnShow"];
        OnHide = (LuaFunction)luamodule["OnHide"];

        OnEnable = (LuaFunction)luamodule["OnEnable"];
        OnDisable = (LuaFunction)luamodule["OnDisable"];

        OnDestroy = (LuaFunction)luamodule["OnDestroy"];

        OnCollisionEnter = (LuaFunction)luamodule["OnCollisionEnter"];
        OnTriggerEnter = (LuaFunction)luamodule["OnTriggerEnter"];
        OnTriggerStay = (LuaFunction)luamodule["OnTriggerStay"];
        OnTriggerExit = (LuaFunction)luamodule["OnTriggerExit"];
        OnApplicationQuit = (LuaFunction)luamodule["OnApplicationQuit"];

        OnTweenComplete = (LuaFunction)luamodule["OnTweenComplete"];
        OnTweenUpdate = (LuaFunction)luamodule["OnTweenUpdate"];

        OnItemReady = (LuaFunction)luamodule["OnItemReady"];
        OnItemCompleted = (LuaFunction)luamodule["OnItemCompleted"];

        OnAnim0 = (LuaFunction)luamodule["OnAnim0"];
        OnAnim1 = (LuaFunction)luamodule["OnAnim1"];
        OnAnim2 = (LuaFunction)luamodule["OnAnim2"];
        OnAnim3 = (LuaFunction)luamodule["OnAnim3"];
        OnAnim4 = (LuaFunction)luamodule["OnAnim4"];
        OnAnim5 = (LuaFunction)luamodule["OnAnim5"];

        OnToggle = (LuaFunction)luamodule["OnToggle"];
        OnCommand = (LuaFunction)luamodule["OnCommand"];
        OnOpenPage = (LuaFunction)luamodule["OnOpenPage"];
        OnPress = (LuaFunction)luamodule["OnPress"];
        OnSubmit = (LuaFunction)luamodule["OnSubmit"];
        OnValueChange = (LuaFunction)luamodule["OnValueChange"];
        OnDragStart = (LuaFunction)luamodule["OnDragStart"];
        OnDrag = (LuaFunction)luamodule["OnDrag"];
        OnDragEnd = (LuaFunction)luamodule["OnDragEnd"];
        OnDragDropRelease = (LuaFunction)luamodule["OnDragDropRelease"];
        OnCenterPage = (LuaFunction)luamodule["OnCenterPage"];

    }

    public void Unload()
    {
        if (LoadProperty != null) LoadProperty.Dispose(); LoadProperty = null;
        if (Awake != null) Awake.Dispose(); Awake = null;
        if (Start != null) Start.Dispose(); Start = null;
        if (Update != null) Update.Dispose(); Update = null;
        if (LateUpdate != null) LateUpdate.Dispose(); LateUpdate = null;

        if (OnShow != null) OnShow.Dispose(); OnShow = null;
        if (OnHide != null) OnHide.Dispose(); OnHide = null;
        if (OnEnable != null) OnEnable.Dispose(); OnEnable = null;
        if (OnDisable != null) OnDisable.Dispose(); OnDisable = null;
        if (OnCollisionEnter != null) OnCollisionEnter.Dispose(); OnCollisionEnter = null;

        if (OnTriggerEnter != null) OnTriggerEnter.Dispose(); OnTriggerEnter = null;
        if (OnTriggerStay != null) OnTriggerStay.Dispose(); OnTriggerStay = null;
        if (OnTriggerExit != null) OnTriggerExit.Dispose(); OnTriggerExit = null;

        if (OnApplicationQuit != null) OnApplicationQuit.Dispose(); OnApplicationQuit = null;
        if (OnDestroy != null) OnDestroy.Dispose(); OnDestroy = null;

        if (OnTweenUpdate != null) OnTweenUpdate.Dispose(); OnTweenUpdate = null;
        if (OnTweenComplete != null) OnTweenComplete.Dispose(); OnTweenComplete = null;

        if (OnItemReady != null) OnItemReady.Dispose(); OnItemReady = null;
        if (OnItemCompleted != null) OnItemCompleted.Dispose(); OnItemCompleted = null;

        if (OnAnim0 != null) OnAnim0.Dispose(); OnAnim0 = null;
        if (OnAnim1 != null) OnAnim1.Dispose(); OnAnim1 = null;
        if (OnAnim2 != null) OnAnim2.Dispose(); OnAnim2 = null;
        if (OnAnim3 != null) OnAnim3.Dispose(); OnAnim3 = null;
        if (OnAnim4 != null) OnAnim4.Dispose(); OnAnim4 = null;
        if (OnAnim5 != null) OnAnim5.Dispose(); OnAnim5 = null;

        if (OnToggle != null) OnToggle.Dispose(); OnToggle = null;
        if (OnCommand != null) OnCommand.Dispose(); OnCommand = null;
        if (OnOpenPage != null) OnOpenPage.Dispose(); OnOpenPage = null;
        if (OnPress != null) OnPress.Dispose(); OnPress = null;
        if (OnSubmit != null) OnSubmit.Dispose(); OnSubmit = null;
        if (OnValueChange != null) OnValueChange.Dispose(); OnValueChange = null;
        if (OnDragStart != null) OnDragStart.Dispose(); OnDragStart = null;
        if (OnDrag != null) OnDrag.Dispose(); OnDrag = null;
        if (OnDragEnd != null) OnDragEnd.Dispose(); OnDragEnd = null;
        if (OnDragDropRelease != null) OnDragDropRelease.Dispose(); OnDragDropRelease = null;
        if (OnCenterPage != null) OnCenterPage.Dispose(); OnCenterPage = null;
    }
}
