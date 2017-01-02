using LuaInterface;
using SLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LuaModuleManager
{
    private Dictionary<string, LuaModule> Modules = new Dictionary<string, LuaModule>();

    LuaState mState;

    public LuaModuleManager(LuaState state)
    { mState = state; }

    public void Unload(string moduleName)
    {
        LuaModule module = null;
        if (Modules.TryGetValue(moduleName, out module))
            module.Unload();
        Modules.Remove(moduleName);
    }

    public void ReleaseInstance(LuaScriptObject instance)
    {
        LuaModule module = null;

        if (Modules.TryGetValue(instance.ModuleName, out module))
        {
            module.RemoveReference(instance);
            if (module.Reference == 0)
                Modules.Remove(instance.ModuleName);
        }
    }

    public LuaModule Load(string moduleName)
    {
        if (string.IsNullOrEmpty(moduleName))
            return null;

        LuaModule module = null;
        if (Modules.TryGetValue(moduleName, out module))
            return module;

        ToLuaHelper.Require(moduleName);

        LuaTable moduleTable = (LuaTable)mState.getTable(moduleName);
        if (moduleTable == null)
            return null;

        moduleTable["__index"] = (LuaFunction)mState["ScriptGet"];
        ToLuaHelper.SetMetaTable(moduleTable, moduleTable);

        module = new LuaModule(moduleName, moduleTable);

        Modules.Add(moduleName, module);

        return module;
    }
    public FunctionTaker GetFunctionTaker(string moduleName)
    {
        if (string.IsNullOrEmpty(moduleName))
            return null;

        LuaModule module = null;
        if (Modules.TryGetValue(moduleName, out module))
            return module.FunctionTaker;
        return null;
    }

    public LuaTable CreateInstance(LuaScriptObject script)
    {
        LuaTable instance = null;

        LuaModule luaModule = Load(script.ModuleName);

        if (luaModule == null)
            return null;

        LuaTable module = luaModule.Module;

        instance = ToLuaHelper.CreateTable();

        if (instance != null)
        {
            luaModule.AddReference(script);

            ToLuaHelper.SetMetaTable(instance, module);
        }

        return instance;
    }

    public void Destroy()
    {
        List<LuaModule> modules = new List<LuaModule>();
        modules.AddRange(Modules.Values);

        foreach (var module in modules)
            module.Destroy();

        modules.Clear();
        Modules.Clear();
    }

    #region LUA
    static string InstanceGetFunc = @"
    function ScriptGet(tab,key)
	    local v = rawget(tab,key)
	    if v~=nil then return v end
	    
        local meta = getmetatable(tab)
	    if meta~=nil then  return rawget(meta,key) end
	    return nil
    end";
    #endregion

}
