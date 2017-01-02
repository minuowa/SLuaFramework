using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(LuaScriptObject), true)]
public class LuaScriptObjectInspector: Editor
{
    static string mName = string.Empty;
    static string mType = string.Empty;

    static List<string> TypeList
    {
        get
        {
            //if (mTypeList == null)
            //{
            //    mTypeList = new List<string>();
            //    mTypeList.AddRange(StringComponent.Types.Keys);
            //    mTypeList.Sort();
            //}
            return mTypeList;
        }
    }

    static List<string> mTypeList;

    LuaScriptObject Owner
    {
        get { return (LuaScriptObject)target; }
    }

    LuaScriptObject scriptObject
    {
        get
        {
            return (LuaScriptObject)target;
        }
    }


#if LOAD_FROM_NET
    static string luaDir = "ExternResources/Game/";
#else
    static string luaDir = "Resources/Game/";
#endif


    static string mTempText = "print('Hello world')";

    void DrawTool()
    {
        if (Application.isPlaying)
        {
            GUILayout.BeginVertical();

            if (GUILayout.Button("Reload"))
            {
                UnityEngine.Object luafile = scriptObject.luaFile;
                if (luafile)
                {
                    string path = AssetDatabase.GetAssetPath(luafile);
                    luafile = AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));
                }
                scriptObject.Recreate();
            }

            mTempText = EditorGUILayout.TextArea(mTempText, GUILayout.MinHeight(100));
            
            if (GUILayout.Button("Execute", GUILayout.MaxWidth(100)))
            {
                LuaManager.Instance.MainState.doString(mTempText);
            }
            GUILayout.EndVertical();
        }
    }

    LuaScriptObject mLastScriptObject = null;
    UnityEngine.Object mOldLuaFile;

    public override void OnInspectorGUI()
    {
        //DrawProps();
        base.DrawDefaultInspector();
        DrawTool();

        //if (GUI.changed)
        //{
        //    if (scriptObject == mLastScriptObject && mOldLuaFile != scriptObject.luaFile)
        //    {
        //        scriptObject.ClearScript();
        //        OnEnable();
        //    }
        //}

        //serializedObject.Update();
        //serializedObject.ApplyModifiedProperties();

        //mLastScriptObject = scriptObject;
        //mOldLuaFile = scriptObject.luaFile;
    }

    public void DrawPropertyTool()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            AddMember();
        }
        EditorGUILayout.LabelField("Type", GUILayout.Width(40));
        mType = EditorGUILayout.TextField(mType, GUILayout.MinWidth(70));
        EditorGUILayout.LabelField("Name", GUILayout.Width(40));
        mName = EditorGUILayout.TextField(mName, GUILayout.MinWidth(70));
        EditorGUILayout.EndHorizontal();
    }

    public void DrawProperties()
    {
        //EditorGUILayout.BeginVertical();

        //foreach (var keyvalue in Owner.Values)
        //{
        //    EditorGUILayout.BeginHorizontal();

        //    EditorGUILayout.LabelField(keyvalue.name, GUILayout.Width(100));

        //    string typename = keyvalue.typename;

        //    if (keyvalue.valuetype == LuaPropertyValueType.Value)
        //    {
        //        if (typename == typeof(int).Name)
        //        {
        //            keyvalue.value = EditorGUILayout.IntField(int.Parse(keyvalue.value)).ToString();
        //        }
        //        else if (typename == typeof(uint).Name)
        //        {
        //            keyvalue.value = EditorGUILayout.IntField(int.Parse(keyvalue.value)).ToString();
        //        }
        //        else if (typename == typeof(float).Name)
        //        {
        //            keyvalue.value = EditorGUILayout.FloatField(float.Parse(keyvalue.value)).ToString();
        //        }
        //        else if (typename == typeof(double).Name)
        //        {
        //            keyvalue.value = EditorGUILayout.DoubleField(double.Parse(keyvalue.value)).ToString();
        //        }
        //        else if (typename == typeof(string).Name)
        //        {
        //            keyvalue.value = EditorGUILayout.TextField(keyvalue.value);
        //        }
        //        else if (typename == typeof(bool).Name)
        //        {
        //            keyvalue.value = EditorGUILayout.Toggle(bool.Parse(keyvalue.value)).ToString();
        //        }
        //    }
        //    else if (keyvalue.valuetype == LuaPropertyValueType.GameObject)
        //    {
        //        //keyvalue.value = EditorGUILayout.ObjectField((Object)keyvalue.value);
        //    }

        //    EditorGUILayout.EndHorizontal();
        //}

        //EditorGUILayout.EndVertical();
    }

    void AddMember()
    {
        if (string.IsNullOrEmpty(mType))
            return;
        if (string.IsNullOrEmpty(mName))
            return;
        //Owner.AddProperty(mType, mName);
    }

    public void DrawProps()
    {
        //DrawPropertyTool();

        //EditorGUILayout.BeginHorizontal();
        //if (GUILayout.Button("Apply"))
        //    Owner.Apply();
        //if (GUILayout.Button("LoadProperty"))
        //{
        //    if (!Owner.ReloadProperty())
        //        Owner.LoadDefaultPropertyFromLua();
        //}
        //EditorGUILayout.EndHorizontal();
        //GUI.changed = false;
        DrawProperties();
        //if (GUI.changed)
        //    Owner.Apply();
    }
    void OnDisable()
    {
        //Owner.Apply();
    }
    void OnEnable()
    {
        //if (!Owner.ReloadProperty())
        //    Owner.LoadDefaultPropertyFromLua();
    }
}
