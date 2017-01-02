using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum AppEnvironment
{
    Default = 1 << 0,
    Editor = 1 << 1,
    Game = 1 << 2,
    MapEditor = 1 << 3 | Editor,
    UIEdotor = 1 << 4 | Editor ,
    GameOnEditor = 1<<5 | Editor | Game,
    GameOnMachine = 1 << 6 | Game,
}

//[ExecuteInEditMode]
public class App : MonoBehaviour
{
    List<IManager> mManagers = new List<IManager>();

    public static App Instance
    {
        get { return mInstance; }
    }

    static App mInstance;

    public AppEnvironment environment = AppEnvironment.Default;

    void AddManager<T>() where T : IManager, new()
    {
        var typename = typeof(T).Name;
        if (null == mManagers.Find((mgr) => mgr.GetName() == typename))
            mManagers.Add(new T());
    }

    void Awake()
    {
        if (!CheckSingleton())
            return;
        this.CheckEnvironment();
        this.AddManagers();
    }


    void AddManagers()
    {
        if (Application.isPlaying) AddManager<ResourceManager>();
        if (Application.isPlaying) AddManager<NetManager>();
        AddManager<LuaManager>();

        foreach (var mgr in mManagers)
        {
            if (!mgr.Initialize())
            {
                Debug.LogError(string.Format(mgr.GetName()) + " Initialize Failed!");
                return;
            }
        }
    }

    bool InvalidInstance()
    {
        return mInstance == this;
    }

    bool CheckSingleton()
    {
        if (mInstance != null)
        {
            GameObject.Destroy(this.gameObject);
            return false;
        }

        mInstance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
        return true;
    }

    void CheckEnvironment()
    {
        var envcontroller = AppEnvironmentController.Instance;

        if (envcontroller)
        {
            environment = envcontroller.environment;

            Application.targetFrameRate = envcontroller.targetFPS;
            Application.runInBackground = envcontroller.runInBackground;
            Screen.sleepTimeout = envcontroller.sleepTimeout;
        }
        else
        {
            Application.targetFrameRate = 30;
            Application.runInBackground = false;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (environment == AppEnvironment.Default)
        {
            if(Application.isPlaying)
                environment = AppEnvironment.MapEditor;
            else
                environment = AppEnvironment.Editor;
        }

#if !UNITY_EDITOR
        environment = AppEnvironment.GameOnMachine;
#endif
    }

    void Start()
    {
        if (!this.InvalidInstance())
            return;

        if ((environment & AppEnvironment.Game) > 0)
        {
            //var scenebase = Gen.Instantiate("Prefabs/Objects/AGame");
            //scenebase.transform.parent = this.gameObject.transform;
            //LuaManager.Instance.ShowUI("HomePage", true, false);
        }
    }


    void Update()
    {
        foreach (var mgr in mManagers)
            mgr.Update();
    }

    void LateUpdate()
    {
        foreach (var mgr in mManagers)
            mgr.LateUpdate();
    }

    void FixedUpdate()
    {
        foreach (var mgr in mManagers)
            mgr.FixedUpdate();
    }

    void OnDestroy()
    {
        if (!this.InvalidInstance())
            return;

        mManagers.Reverse();
        foreach (var mgr in mManagers)
            mgr.OnDestroy();
    }

#if !LOAD_FROM_NET
    [RuntimeInitializeOnLoadMethod]
#endif
    public static void Initialize()
    {
        if (mInstance == null)
        {
            GameObject go = new GameObject();
            go.name = typeof(App).Name;
            GameObject.DontDestroyOnLoad(go);
            go.AddComponent<App>();

            //ATrigger.DataCenter.InstallStaticTriggers(typeof(App).Assembly);
            //Information.Instance.Initialize();
        }
    }
}
