using SLua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class LuaManager : IManager
{
    public string Entry = "main";
    public string[] WorkDirectorices = new string[] { string.Empty, "Game" };

    public LuaManager() { Instance = this; }
    public static LuaManager Instance { private set; get; }

    public LuaModuleManager ModuleManager
    {
        get { return mModuleManager; }
    }

    public LuaState MainState
    {
        get { return mLuaServer.luaState; }
    }

    private LuaSvr mLuaServer = new LuaSvr();
    private LuaModuleManager mModuleManager;

    private LuaFunction mOnCloseLua;

    public override bool Initialize()
    {
        mLuaServer.init(null, null);
        mModuleManager = new LuaModuleManager(mLuaServer.luaState);
        LuaState.loaderDelegate += LoadFile;
        mLuaServer.start(Entry);
        mOnCloseLua = (LuaFunction)MainState["OnCloseLua"];
        return MainState != null;
    }

    byte[] LoadFile(string filename)
    {
        foreach(var dir in WorkDirectorices)
        {
            string pathfile = Path.Combine(dir, filename).Replace('\\', '/');
            pathfile = AFun.EnsureNameWithOutExtenision(pathfile);
            pathfile.Replace('.', '/');
            TextAsset asset = null;
            try
            {
                asset = Resources.Load<TextAsset>(pathfile);
            }
            catch (System.Exception exc)
            {
                Debug.LogError(exc.Message);
            }
            if (asset)
            {
                return asset.bytes;
            }
        }
 
        return null;
    }
    public override void Update()
    {
    }

    public override void OnDestroy()
    {
        mModuleManager.Destroy();
        mOnCloseLua.call();
        mLuaServer = null;
    }
}
