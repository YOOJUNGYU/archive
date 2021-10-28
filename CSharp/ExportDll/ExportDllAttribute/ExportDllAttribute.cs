using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;


[AttributeUsage(AttributeTargets.Method)]
public class ExportDllAttribute : Attribute
{
    public ExportDllAttribute(string exportName) : this(exportName, CallingConvention.StdCall) { }
    public ExportDllAttribute() : this(null) { }
    public ExportDllAttribute(string exportName, CallingConvention CallingConvention) { }
}
