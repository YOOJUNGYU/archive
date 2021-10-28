using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ExportDll
{

    enum ParserState
    {
        Normal,
        ClassDeclaration,
        Class,
        DeleteExportDependency,
        MethodDeclaration,
        MethodProperties,
        Method,
        DeleteExportAttribute,
    }
    class Program
    {
        static Dictionary<System.Runtime.InteropServices.CallingConvention, string> diccc = new Dictionary<CallingConvention, string>();
        static bool verbose = false;
        static Program()
        {
            diccc[System.Runtime.InteropServices.CallingConvention.Cdecl] = typeof(CallConvCdecl).FullName;
            diccc[System.Runtime.InteropServices.CallingConvention.FastCall] = typeof(CallConvFastcall).FullName;
            diccc[System.Runtime.InteropServices.CallingConvention.StdCall] = typeof(CallConvStdcall).FullName;
            diccc[System.Runtime.InteropServices.CallingConvention.ThisCall] = typeof(CallConvThiscall).FullName;
            diccc[System.Runtime.InteropServices.CallingConvention.Winapi] = typeof(CallConvStdcall).FullName;
        }

        static void Log(bool forced, string message, params object[] param)
        {
            if (forced || verbose)
            {
                Console.WriteLine(message, param);
            }
        }

        static void Log(string message, params object[] param)
        {
            Log(false, message, param);
        }

        static void Load()
        {
            var dic = new Dictionary<string, Dictionary<string, KeyValuePair<string, string>>>();
            Assembly assembly = Assembly.ReflectionOnlyLoadFrom((string)AppDomain.CurrentDomain.GetData("filepath"));
            Type[] types = assembly.GetTypes();
            int exportscount = 0;
            foreach (Type type in types)
            {
                MemberInfo[] mis = type.FindMembers(MemberTypes.All, BindingFlags.Public | BindingFlags.Static, new MemberFilter((mi, obj) => true), null);
                foreach (MemberInfo mi in mis)
                {
                    var attrs = CustomAttributeData.GetCustomAttributes(mi);
                    foreach (var attr in attrs)
                        if (attr.Constructor.ReflectedType.FullName == "ExportDllAttribute")
                        {
                            if (!dic.ContainsKey(type.FullName))
                                dic[type.FullName] = new Dictionary<string, KeyValuePair<string, string>>();
                            switch (attr.ConstructorArguments.Count)
                            {
                                case 0:
                                    dic[type.FullName][mi.Name] = new KeyValuePair<string, string>(mi.Name, typeof(CallConvStdcall).FullName);
                                    break;
                                case 1:
                                    dic[type.FullName][mi.Name] = new KeyValuePair<string, string>((string)attr.ConstructorArguments[0].Value, typeof(CallConvStdcall).FullName);
                                    break;
                                case 2:
                                    dic[type.FullName][mi.Name] = new KeyValuePair<string, string>((string)attr.ConstructorArguments[0].Value, diccc[(CallingConvention)attr.ConstructorArguments[1].Value]);
                                    break;
                                default:
                                    break;
                            }
                            exportscount++;
                        }
                }
            }
            AppDomain.CurrentDomain.SetData("exportscount", exportscount);
            AppDomain.CurrentDomain.SetData("dic", dic);
        }


        static int Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    Log(true, "Parameter error!");
                    return 1;
                }
                bool debug = false;
                List<string> listargs = new List<string>(args);
                listargs.RemoveAt(0);
                int deb = listargs.FindIndex(new Predicate<string>(x => x.ToLower().Contains("/debug")));
                if (deb > -1)
                    debug = true;
                else
                {
                    int rel = listargs.FindIndex(new Predicate<string>(x => x.ToLower().Contains("/release")));
                    if (rel > -1)
                    {
                        listargs.RemoveAt(rel);
                        listargs.Add("/optimize");
                    }
                }
                int any = listargs.FindIndex(new Predicate<string>(x => x.ToLower().Contains("/anycpu")));
                if (any > -1)
                {
                    listargs.RemoveAt(any);
                }
                else
                {
                    int x64 = listargs.FindIndex(new Predicate<string>(x => x.ToLower().Contains("/x64")));
                    if (x64 > -1)
                    {
                        listargs.Add("/PE64");
                    }
                }
                int verb = listargs.FindIndex(new Predicate<string>(x => x.ToLower().Contains("/verbose")));
                if (verb > -1)
                {
                    verbose = true;
                    listargs.RemoveAt(verb);
                }
                string filepath = args[0];
                string path = System.IO.Path.GetDirectoryName(filepath);
                if (path == string.Empty)
                {
                    Log(true, "Full path needed!");
                    return 1;
                }
                string ext = System.IO.Path.GetExtension(filepath);
                if (ext != ".dll")
                {
                    Log(true, "Target should be dll!");
                    return 1;
                }

                AppDomain domain = AppDomain.CreateDomain("ReflectionOnly");
                domain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);
                domain.SetData("filepath", filepath);
                domain.DoCallBack(new CrossAppDomainDelegate(Program.Load));
                var dic = (Dictionary<string, Dictionary<string, KeyValuePair<string, string>>>)domain.GetData("dic");
                int exportscount = (int)domain.GetData("exportscount");
                AppDomain.Unload(domain);
                if (exportscount > 0)
                {
                    int exportpos = 1;
                    string filename = System.IO.Path.GetFileNameWithoutExtension(filepath);
                    System.IO.Directory.SetCurrentDirectory(path);
                    Process proc = new Process();
                    string arguments = string.Format("/nobar{1}/out:{0}.il {0}.dll", filename, debug ? " /linenum " : " ");
                    Log("Deassebly file with arguments '{0}'", arguments);
                    System.Diagnostics.ProcessStartInfo info = new ProcessStartInfo(Properties.Settings.Default.ildasmpath, arguments);
                    info.UseShellExecute = false;
                    info.CreateNoWindow = false;
                    info.RedirectStandardOutput = true;
                    proc.StartInfo = info;
                    proc.Start();
                    proc.WaitForExit();
                    Log(proc.ExitCode != 0, proc.StandardOutput.ReadToEnd());
                    if (proc.ExitCode != 0)
                        return proc.ExitCode;
                    List<string> wholeilfile = new List<string>();
                    System.IO.StreamReader sr = System.IO.File.OpenText(System.IO.Path.Combine(path, filename + ".il"));
                    string methoddeclaration = "";
                    string methodname = "";
                    string classdeclaration = "";
                    string methodbefore = "";
                    string methodafter = "";
                    int methodpos = 0;
                    Stack<string> classnames = new Stack<string>();
                    List<string> externassembly = new List<string>();
                    ParserState state = ParserState.Normal;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string trimedline = line.Trim();
                        bool addilne = true;
                        switch (state)
                        {
                            case ParserState.Normal:
                                if (trimedline.StartsWith(".class"))
                                {
                                    state = ParserState.ClassDeclaration;
                                    addilne = false;
                                    classdeclaration = trimedline;
                                }
                                else if (trimedline.StartsWith(".assembly extern ExportDll"))
                                {
                                    addilne = false;
                                    state = ParserState.DeleteExportDependency;
                                    Log("Deleting ExportDllAttribute dependency.");
                                }
                                break;
                            case ParserState.DeleteExportDependency:
                                if (trimedline.StartsWith("}"))
                                {
                                    state = ParserState.Normal;
                                }
                                addilne = false;
                                break;
                            case ParserState.ClassDeclaration:
                                if (trimedline.StartsWith("{"))
                                {
                                    state = ParserState.Class;
                                    string classname = "";
                                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@".+\s+([^\s]+) extends \[.*");
                                    System.Text.RegularExpressions.Match m = reg.Match(classdeclaration);
                                    if (m.Groups.Count > 1)
                                        classname = m.Groups[1].Value;
                                    classname = classname.Replace("'", "");
                                    if (classnames.Count > 0)
                                        classname = classnames.Peek() + "+" + classname;
                                    classnames.Push(classname);
                                    Log("Found class: " + classname);
                                    wholeilfile.Add(classdeclaration);
                                }
                                else
                                {
                                    classdeclaration += " " + trimedline;
                                    addilne = false;
                                }
                                break;
                            case ParserState.Class:
                                if (trimedline.StartsWith(".class"))
                                {
                                    state = ParserState.ClassDeclaration;
                                    addilne = false;
                                    classdeclaration = trimedline;
                                }
                                else if (trimedline.StartsWith(".method"))
                                {
                                    if (dic.ContainsKey(classnames.Peek()))
                                    {
                                        methoddeclaration = trimedline;
                                        addilne = false;
                                        state = ParserState.MethodDeclaration;
                                    }
                                }
                                else if (trimedline.StartsWith("} // end of class"))
                                {
                                    classnames.Pop();
                                    if (classnames.Count > 0)
                                        state = ParserState.Class;
                                    else
                                        state = ParserState.Normal;
                                }
                                break;
                            case ParserState.MethodDeclaration:
                                if (trimedline.StartsWith("{"))
                                {
                                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(?<before>[^\(]+(\(\s[^\)]+\))*\s)(?<method>[^\(]+)(?<after>\(.*)");
                                    System.Text.RegularExpressions.Match m = reg.Match(methoddeclaration);
                                    if (m.Groups.Count > 3)
                                    {
                                        methodbefore = m.Groups["before"].Value;
                                        methodafter = m.Groups["after"].Value;
                                        methodname = m.Groups["method"].Value;
                                    }
                                    Log("Found method: " + methodname);
                                    if (dic[classnames.Peek()].ContainsKey(methodname))
                                    {
                                        methodpos = wholeilfile.Count;
                                        state = ParserState.MethodProperties;
                                    }
                                    else
                                    {
                                        wholeilfile.Add(methoddeclaration);
                                        state = ParserState.Method;
                                        methodpos = 0;
                                    }
                                }
                                else
                                {
                                    methoddeclaration += " " + trimedline;
                                    addilne = false;
                                }
                                break;
                            case ParserState.Method:
                                if (trimedline.StartsWith("} // end of method"))
                                {
                                    state = ParserState.Class;
                                }
                                break;
                            case ParserState.MethodProperties:
                                if (trimedline.StartsWith(".custom instance void [ExportDll"))
                                {
                                    addilne = false;
                                    state = ParserState.DeleteExportAttribute;
                                }
                                else if (trimedline.StartsWith("// Code"))
                                {
                                    state = ParserState.Method;
                                    if (methodpos != 0)
                                        wholeilfile.Insert(methodpos, methoddeclaration);
                                }
                                break;
                            case ParserState.DeleteExportAttribute:
                                if (trimedline.StartsWith(".custom") || trimedline.StartsWith("// Code"))
                                {
                                    KeyValuePair<string, string> attr = dic[classnames.Peek()][methodname];
                                    if (methodbefore.Contains("marshal( "))
                                    {
                                        int pos = methodbefore.IndexOf("marshal( ");
                                        methodbefore = methodbefore.Insert(pos, "modopt([mscorlib]" + attr.Value + ") ");
                                        methoddeclaration = methodbefore + methodname + methodafter;
                                    }
                                    else
                                        Log("\tChanging calling convention: " + attr.Value);
                                    if (methodpos != 0)
                                        wholeilfile.Insert(methodpos, methoddeclaration);
                                    if (methodname == "DllMain")
                                        wholeilfile.Add(" .entrypoint");
                                    wholeilfile.Add(string.Format(".export [{0}] as {1}", exportpos, dic[classnames.Peek()][methodname].Key));

                                    Log("\tAdding .vtentry:{0} .export:{1}", exportpos, dic[classnames.Peek()][methodname].Key);
                                    exportpos++;
                                    state = ParserState.Method;
                                }
                                else
                                    addilne = false;
                                break;
                        }
                        if (addilne)
                            wholeilfile.Add(line);
                    }
                    sr.Close();
                    System.IO.StreamWriter sw = System.IO.File.CreateText(System.IO.Path.Combine(path, filename + ".il"));
                    foreach (string line in wholeilfile)
                    {
                        sw.WriteLine(line);
                    }
                    sw.Close();
                    string res = filename + ".res";
                    if (System.IO.File.Exists(filename + ".res"))
                        res = " /resource=" + res;
                    else
                        res = "";
                    proc = new Process();
                    arguments = string.Format("/nologo /quiet /out:{0}.dll {0}.il /DLL{1} {2}", filename, res, string.Join(" ", listargs.ToArray()));
                    Log("Compiling file with arguments '{0}'", arguments);
                    info = new ProcessStartInfo(Properties.Settings.Default.ilasmpath, arguments);
                    info.UseShellExecute = false;
                    info.CreateNoWindow = false;
                    info.RedirectStandardOutput = true;
                    proc.StartInfo = info;
                    proc.Start();
                    proc.WaitForExit();
                    Log(proc.ExitCode != 0, proc.StandardOutput.ReadToEnd());
                    if (proc.ExitCode != 0)
                        return proc.ExitCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            return 0;
        }

        static Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.ReflectionOnlyLoad(args.Name);
        }
    }
}
