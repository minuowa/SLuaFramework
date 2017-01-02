using UnityEngine;
using System.Collections;
using SLua;
using System.IO;

public class LuaScriptObject : MonoBehaviour {
    public TextAsset luaFile;
    public string ModuleName { get { return mModuleName; } }
    public LuaTable LuaInstance { get { return mInstance; } }

    private LuaTable mInstance;
    private string mModuleName;
    private FunctionTaker mFunctionTaker;

    void Awake()
    {
        this.Recreate();
        mFunctionTaker.Awake.call(mInstance);
    }

    public void Recreate()
    {
        this.Clear();
        this.CreateInstance();
    }

    void Start()
    {
        mFunctionTaker.Start.call(mInstance);
    }

    void Clear()
    {
        if (mInstance != null)
        {
            mInstance.Dispose();
            mInstance = null;
        }
    }

    void CreateInstance()
    {
        mInstance = LuaManager.Instance.ModuleManager.CreateInstance(this);
        mFunctionTaker = LuaManager.Instance.ModuleManager.GetFunctionTaker(mModuleName);
    }
    // Update is called once per frame
    void Update () {
	
	}
    public void OnDestroy()
    {
        if (mInstance != null)
        {
            mFunctionTaker.OnDestroy.call(mInstance);
            LuaManager.Instance.ModuleManager.ReleaseInstance(this);
            mInstance.Dispose();
            mInstance = null;
        }
    }
    #region Editor

#if LOAD_FROM_NET
    static string luaDir = "ExternResources/Game/";
#else
    static string luaDir = "Resources/Game/";
#endif

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!luaFile)
        {
            mModuleName = string.Empty;
            return;
        }

        string path = UnityEditor.AssetDatabase.GetAssetPath(luaFile);
        int pos = path.IndexOf(luaDir);
        if (pos == -1)
        {
            Debug.LogError("Path Must In :" + luaDir + "  Error Path:" + path);
            return;
        }

        string fileName = path.Substring(pos + luaDir.Length);
        mModuleName = Path.Combine(Path.GetDirectoryName(fileName)
            , Path.GetFileNameWithoutExtension(fileName)).Replace('\\', '.')
            .Replace('/', '.');
    }
#endif
    #endregion
}
