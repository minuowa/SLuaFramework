using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum AppEventID
{
    OnNetData,
    OnNetEvent,
    OnMapLoaded,
}


public class NetDataArg
{
    public uint CmdID;
    public byte[] Data;
}

//public class NetEventArg 
//{
//    public NetEventID ID;
//}
