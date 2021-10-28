using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ExportDll
{
    /// <summary>
    /// c# 으로 만든 dll 을 Export 시켜 다른 곳에서 사용할 수 있도록 해주는 실행파일입니다.
    /// export 시키고자 하는 dll 을 ExportDll.exe 파일이 빌드되는 위치에 넣고,
    /// 속성 > 디버그 > 명령줄인수에 dll파일의 경로(FullPath)를 넣어주고 exe 를 실행시킵니다
    /// http://thermidor.tistory.com/1397
    /// https://www.codeproject.com/Articles/16310/How-to-Automate-Exporting-NET-Function-to-Unmanage
    /// </summary>
    internal enum ParserState
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

    internal class Program
    {
        static readonly Dictionary<CallingConvention, string> DicCallingConvention = new Dictionary<CallingConvention, string>();
        static bool _verbose;
        static Program()
        {
            DicCallingConvention[CallingConvention.Cdecl] = typeof(CallConvCdecl).FullName;
            DicCallingConvention[CallingConvention.FastCall] = typeof(CallConvFastcall).FullName;
            DicCallingConvention[CallingConvention.StdCall] = typeof(CallConvStdcall).FullName;
            DicCallingConvention[CallingConvention.ThisCall] = typeof(CallConvThiscall).FullName;
            DicCallingConvention[CallingConvention.Winapi] = typeof(CallConvStdcall).FullName;
        }

        private static void Log(bool forced, string message, params object[] param)
        {
            if (forced || _verbose)
            {
                Console.WriteLine(message, param);
            }
        }

        private static void Log(string message, params object[] param)
        {
            Log(false, message, param);
        }

        private static void Load()
        {
            var dic = new Dictionary<string, Dictionary<string, KeyValuePair<string, string>>>();
            var assembly = Assembly.ReflectionOnlyLoadFrom((string)AppDomain.CurrentDomain.GetData("filepath"));
            var types = assembly.GetTypes();
            var exportCount = 0;
            foreach (var type in types)
            {
                var mis = type.FindMembers(MemberTypes.All, BindingFlags.Public | BindingFlags.Static, (mi, obj) => true, null);
                foreach (var mi in mis)
                {
                    var attrs = CustomAttributeData.GetCustomAttributes(mi);
                    foreach (var attr in attrs)
                        if (attr.Constructor.ReflectedType != null && attr.Constructor.ReflectedType.FullName == "ExportDllAttribute")
                        {
                            if (!dic.ContainsKey(type.FullName ?? throw new InvalidOperationException()))
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
                                    dic[type.FullName][mi.Name] = new KeyValuePair<string, string>((string)attr.ConstructorArguments[0].Value, DicCallingConvention[(CallingConvention)attr.ConstructorArguments[1].Value]);
                                    break;
                            }
                            exportCount++;
                        }
                }
            }
            AppDomain.CurrentDomain.SetData("exportCount", exportCount);
            AppDomain.CurrentDomain.SetData("dic", dic);
        }


        private static int Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    Log(true, "Parameter error!");
                    return 1;
                }
                var debug = false;
                var argsList = new List<string>(args);
                argsList.RemoveAt(0);
                var deb = argsList.FindIndex(x => x.ToLower().Contains("/debug"));
                if (deb > -1)
                    debug = true;
                else
                {
                    var rel = argsList.FindIndex(x => x.ToLower().Contains("/release"));
                    if (rel > -1)
                    {
                        argsList.RemoveAt(rel);
                        argsList.Add("/optimize");
                    }
                }
                var any = argsList.FindIndex(x => x.ToLower().Contains("/anycpu"));
                if (any > -1)
                {
                    argsList.RemoveAt(any);
                }
                else
                {
                    var x64 = argsList.FindIndex(x => x.ToLower().Contains("/x64"));
                    if (x64 > -1)
                    {
                        argsList.Add("/PE64");
                    }
                }
                var verb = argsList.FindIndex(x => x.ToLower().Contains("/verbose"));
                if (verb > -1)
                {
                    _verbose = true;
                    argsList.RemoveAt(verb);
                }
                var filepath = args[0];
                var path = System.IO.Path.GetDirectoryName(filepath);
                if (path == string.Empty)
                {
                    Log(true, "Full path needed!");
                    return 1;
                }
                var ext = System.IO.Path.GetExtension(filepath);
                if (ext != ".dll")
                {
                    Log(true, "Target should be dll!");
                    return 1;
                }

                var domain = AppDomain.CreateDomain("ReflectionOnly");
                domain.ReflectionOnlyAssemblyResolve += CurrentDomain_ReflectionOnlyAssemblyResolve;
                domain.SetData("filepath", filepath);
                domain.DoCallBack(Load);
                var dic = (Dictionary<string, Dictionary<string, KeyValuePair<string, string>>>)domain.GetData("dic");
                var exportCount = (int)domain.GetData("exportCount");
                AppDomain.Unload(domain);
                if (exportCount > 0)
                {
                    var exportPos = 1;
                    var filename = System.IO.Path.GetFileNameWithoutExtension(filepath);
                    System.IO.Directory.SetCurrentDirectory(path ?? throw new InvalidOperationException());
                    var proc = new Process();
                    var arguments = string.Format("/nobar{1}/out:{0}.il {0}.dll", filename, debug ? " /linenum " : " ");
                    Log("Deassebly file with arguments '{0}'", arguments);
                    var info = new ProcessStartInfo(Properties.Settings.Default.ildasmpath, arguments)
                    {
                        UseShellExecute = false, CreateNoWindow = false, RedirectStandardOutput = true
                    };
                    proc.StartInfo = info;
                    proc.Start();
                    proc.WaitForExit();
                    Log(proc.ExitCode != 0, proc.StandardOutput.ReadToEnd());
                    if (proc.ExitCode != 0)
                        return proc.ExitCode;
                    var wholeIlFile = new List<string>();
                    var sr = new System.IO.StreamReader(System.IO.Path.Combine(path, filename + ".il"), Encoding.Default);
                    var methodDeclaration = "";
                    var methodName = "";
                    var classDeclaration = "";
                    var methodBefore = "";
                    var methodAfter = "";
                    var methodPos = 0;
                    var classNames = new Stack<string>();
                    var state = ParserState.Normal;
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        if (line == null) continue;
                        var trimmedLine = line.Trim();
                        var addLine = true;
                        switch (state)
                        {
                            case ParserState.Normal:
                                if (trimmedLine.StartsWith(".class"))
                                {
                                    state = ParserState.ClassDeclaration;
                                    addLine = false;
                                    classDeclaration = trimmedLine;
                                }
                                else if (trimmedLine.StartsWith(".assembly extern ExportDll"))
                                {
                                    addLine = false;
                                    state = ParserState.DeleteExportDependency;
                                    Log("Deleting ExportDllAttribute dependency.");
                                }
                                break;
                            case ParserState.DeleteExportDependency:
                                if (trimmedLine.StartsWith("}"))
                                {
                                    state = ParserState.Normal;
                                }
                                addLine = false;
                                break;
                            case ParserState.ClassDeclaration:
                                if (trimmedLine.StartsWith("{"))
                                {
                                    state = ParserState.Class;
                                    var classname = "";
                                    var reg = new System.Text.RegularExpressions.Regex(@".+\s+([^\s]+) extends \[.*");
                                    var m = reg.Match(classDeclaration);
                                    if (m.Groups.Count > 1)
                                        classname = m.Groups[1].Value;
                                    classname = classname.Replace("'", "");
                                    if (classNames.Count > 0)
                                        classname = classNames.Peek() + "+" + classname;
                                    classNames.Push(classname);
                                    Log("Found class: " + classname);
                                    wholeIlFile.Add(classDeclaration);
                                }
                                else
                                {
                                    classDeclaration += " " + trimmedLine;
                                    addLine = false;
                                }
                                break;
                            case ParserState.Class:
                                if (trimmedLine.StartsWith(".class"))
                                {
                                    state = ParserState.ClassDeclaration;
                                    addLine = false;
                                    classDeclaration = trimmedLine;
                                }
                                else if (trimmedLine.StartsWith(".method"))
                                {
                                    if (dic.ContainsKey(classNames.Peek()))
                                    {
                                        methodDeclaration = trimmedLine;
                                        addLine = false;
                                        state = ParserState.MethodDeclaration;
                                    }
                                }
                                else if (trimmedLine.StartsWith("} // end of class"))
                                {
                                    classNames.Pop();
                                    state = classNames.Count > 0 ? ParserState.Class : ParserState.Normal;
                                }
                                break;
                            case ParserState.MethodDeclaration:
                                if (trimmedLine.StartsWith("{"))
                                {
                                    var reg = new System.Text.RegularExpressions.Regex(@"(?<before>[^\(]+(\(\s[^\)]+\))*\s)(?<method>[^\(]+)(?<after>\(.*)");
                                    var m = reg.Match(methodDeclaration);
                                    if (m.Groups.Count > 3)
                                    {
                                        methodBefore = m.Groups["before"].Value;
                                        methodAfter = m.Groups["after"].Value;
                                        methodName = m.Groups["method"].Value;
                                    }
                                    Log("Found method: " + methodName);
                                    if (dic[classNames.Peek()].ContainsKey(methodName))
                                    {
                                        methodPos = wholeIlFile.Count;
                                        state = ParserState.MethodProperties;
                                    }
                                    else
                                    {
                                        wholeIlFile.Add(methodDeclaration);
                                        state = ParserState.Method;
                                        methodPos = 0;
                                    }
                                }
                                else
                                {
                                    methodDeclaration += " " + trimmedLine;
                                    addLine = false;
                                }
                                break;
                            case ParserState.Method:
                                if (trimmedLine.StartsWith("} // end of method"))
                                {
                                    state = ParserState.Class;
                                }
                                break;
                            case ParserState.MethodProperties:
                                if (trimmedLine.StartsWith(".custom instance void [ExportDll"))
                                {
                                    addLine = false;
                                    state = ParserState.DeleteExportAttribute;
                                }
                                else if (trimmedLine.StartsWith("// Code") || trimmedLine.StartsWith("// 코드 크기"))
                                {
                                    state = ParserState.Method;
                                    if (methodPos != 0)
                                        wholeIlFile.Insert(methodPos, methodDeclaration);
                                }
                                break;
                            case ParserState.DeleteExportAttribute:
                                if (trimmedLine.StartsWith(".custom") || trimmedLine.StartsWith("// Code") || trimmedLine.StartsWith("// 코드 크기"))
                                {
                                    var attr = dic[classNames.Peek()][methodName];
                                    if (methodBefore.Contains("marshal( "))
                                    {
                                        var pos = methodBefore.IndexOf("marshal( ", StringComparison.Ordinal);
                                        methodBefore = methodBefore.Insert(pos, "modopt([mscorlib]" + attr.Value + ") ");
                                        methodDeclaration = methodBefore + methodName + methodAfter;
                                    }
                                    else
                                        Log("\tChanging calling convention: " + attr.Value);
                                    if (methodPos != 0)
                                        wholeIlFile.Insert(methodPos, methodDeclaration);
                                    if (methodName == "DllMain")
                                        wholeIlFile.Add(" .entrypoint");
                                    wholeIlFile.Add($".export [{exportPos}] as {dic[classNames.Peek()][methodName].Key}");
                                    Log("\tAdding .vtentry:{0} .export:{1}", exportPos, dic[classNames.Peek()][methodName].Key);
                                    exportPos++;
                                    state = ParserState.Method;
                                }
                                else
                                    addLine = false;
                                break;
                        }
                        if (addLine)
                            wholeIlFile.Add(line);
                    }
                    sr.Close();
                    var sw = new System.IO.StreamWriter(System.IO.File.Open(System.IO.Path.Combine(path, filename + ".il"), System.IO.FileMode.Create), Encoding.Default);
                    foreach (var line in wholeIlFile)
                    {
                        sw.WriteLine(line);
                    }
                    sw.Close();
                    var res = filename + ".res";
                    if (System.IO.File.Exists(filename + ".res"))
                        res = " /resource=" + res;
                    else
                        res = "";
                    proc = new Process();
                    arguments = string.Format("/nologo /quiet /out:{0}.dll {0}.il /DLL{1} {2}", filename, res, string.Join(" ", argsList.ToArray()));
                    Log("Compiling file with arguments '{0}'", arguments);
                    info = new ProcessStartInfo(Properties.Settings.Default.ildasmpath2, arguments)
                    {
                        UseShellExecute = false, CreateNoWindow = false, RedirectStandardOutput = true
                    };
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
