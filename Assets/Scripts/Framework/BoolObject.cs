using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class BoolObject
{
    public static implicit operator bool(BoolObject u) { return u != null; }
}
