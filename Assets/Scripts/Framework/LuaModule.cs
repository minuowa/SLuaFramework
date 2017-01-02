using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SLua;

public class LuaModule: IDisposable
{
    public LuaTable Module { get { return mTable; } }
    public FunctionTaker FunctionTaker { get { return mFunctionContainer; } }

    private List<LuaScriptObject> mInstances = new List<LuaScriptObject>();
    FunctionTaker mFunctionContainer = new FunctionTaker();
    string mName = string.Empty;
    LuaTable mTable;

    public LuaModule(string name,LuaTable table)
    {
        this.mName = name;
        this.mTable = table;
        this.mFunctionContainer.ReloadFunctions(this.mTable);
    }

    public void Unload()
    {
        var lua = LuaManager.Instance.MainState;

        lua.doString(string.Format("_G['{0}']=nil", mName.Replace('.', '/')));
        lua.doString(string.Format("{0}=nil", mName.Replace('/', '.')));
        lua.doString(string.Format("package.loaded['{0}']=nil", mName.Replace('/', '.')));

        mFunctionContainer.Unload();

        if (mTable != null)
            mTable.Dispose();
    }

    public void AddReference(LuaScriptObject instance)
    {
        Debug.Assert(mInstances.Find((luainstance) => luainstance == instance) == null);
        mInstances.Add(instance);
        this.AddReference();
    }
    public void RemoveReference(LuaScriptObject instance)
    {
        ToLuaHelper.SetMetaTable(instance.LuaInstance, null);
        mInstances.Remove(instance);
        this.Dispose();
    }

    public void Destroy()
    {
        Debug.Assert(mInstances.Count == Reference);
        while (mInstances.Count > 0)
            mInstances[0].OnDestroy();
    }

    #region IDisposable Support
    private bool disposedValue = false; // 要检测冗余调用
    public int Reference
    { get { return mReference; } }
    private int mReference = 0;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Unload();
            }
            disposedValue = true;
        }
    }
    void AddReference()
    {
        ++mReference;
    }
    void DecRef()
    {
        --mReference;
        if (mReference > 0)
            return;
        Dispose(true);
    }

    public void Dispose()
    {
        this.DecRef();
    }
    #endregion
}
