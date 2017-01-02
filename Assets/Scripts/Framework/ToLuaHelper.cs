using LuaInterface;
using SLua;

public class ToLuaHelper
{
    static LuaState Lua
    {
        get
        {
            return LuaManager.Instance.MainState;
        }
    }

    public static void ClearStack()
    {
        var L = Lua.L;
        LuaDLL.lua_pop(L, LuaDLL.lua_gettop(L));
    }

    public static void SetMetaTable(LuaTable owner, LuaTable b)
    {
        LuaObject.pushVar(Lua.L,owner);
        LuaDLL.luaL_getmetatable(Lua.L, "__index");
        LuaObject.pushVar(Lua.L, b);
        LuaDLL.lua_setmetatable(Lua.L, -2);
        ClearStack();
    }

    public static void Require(string moduleName)
    {
        var str = string.Format("require('{0}')", moduleName);
        Lua.doString(str);
    }

    public static LuaTable CreateTable()
    {
        LuaDLL.lua_newtable(Lua.L);
        int valueref = LuaDLL.luaL_ref(Lua.L, LuaIndexes.LUA_REGISTRYINDEX);
        LuaTable tab = new LuaTable(Lua.L, valueref);
        return tab;
    }


    public static T Call<T>(LuaTable owner, string fun, params object[] args)
    {
        LuaFunction func = (LuaFunction)owner[fun];
        if (func != null)
        {
            var ret = (object[])func.call(args);
            if (ret != null && ret.Length > 0)
                return (T)ret[0];
        }
        return default(T);
    }

    public static object[] Call(LuaTable owner,string fun, params object[] args)
    {
        LuaFunction func = (LuaFunction)owner[fun];
        if (func != null)
            return (object[])func.call(args);
        return null;
    }
}