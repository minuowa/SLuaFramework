using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AppEnvironmentController : MonoBehaviour
{
    public AppEnvironment environment;
    public bool runInBackground = false;
    public int targetFPS = 30;
    public int sleepTimeout = -1;

    public static AppEnvironmentController Instance;

    public void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            GameObject.DontDestroyOnLoad(this);
        }
        else
            Debug.LogError("GameEnvironment Only Need One!");
    }
}
