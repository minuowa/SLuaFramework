using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

public class AFun
{
    internal class Watcher
    {
        public string tag;
        public DateTime time;
        public override string ToString()
        {
            TimeSpan t = DateTime.Now - time;
            return string.Format("=>{0} {1:00}:{2:00}:{3:00}:{4:000}", tag, t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
        }
    }
    static Stack<Watcher> mWatchers = new Stack<Watcher>();
    public static void BeginWatch(string tag)
    {
        Watcher w = new Watcher();
        w.tag = tag;
        w.time = DateTime.Now;
        mWatchers.Push(w);
    }
    public static void EndWatch()
    {
        if (mWatchers.Count == 0)
            return;
        Watcher w = mWatchers.Pop();
        //ALog.Warning(w.ToString());
    }
    public static void Watch(Action act, string tag = "")
    {
        if (act == null)
            return;
        if (string.IsNullOrEmpty(tag))
            tag = act.Method.Name;
        
        Watcher w= new Watcher();
        w.tag = tag;
        w.time = DateTime.Now;
        act();
        //ALog.Warning(w.ToString());
    }
    public static string EnsureNameWithExtenision(string name, string ext)
    {
        if (name.IndexOf(ext) == -1)
            name += ext;
        return name;
    }
    public static string EnsureNameWithXML(string name)
    {
        return EnsureNameWithExtenision(name, ".xml");
    }

    public static bool HasFlag<T>(T e, T flag)
    {
        return (Convert.ToInt32(e) & Convert.ToInt32(flag)) != 0;
    }

    public static string EnsureNameWithOutExtenision(string name)
    {
        int pos1 = name.LastIndexOf('.');
        if (pos1 == -1)
            return name;
        return name.Substring(0, pos1);
    }
    public static string BytesToString(byte[] arr)
    {
        if (arr == null || arr.Length == 0)
            return string.Empty;

        int i = arr.Length - 1;

        for (; i >= 0; --i)
        {
            if (arr[i] != 0)
                break;
        }
        return new string(Encoding.UTF8.GetChars(arr, 0, i + 1));
    }


    public static string ClampByteIn2(byte b)
    {
        string s = Convert.ToString(b, 16);
        if (s.Length == 1)
        {
            s = s.Insert(0, "0");
        }
        return s;
    }
};
