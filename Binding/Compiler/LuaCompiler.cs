using System.Runtime.InteropServices;
using System.Text;
using Compiler.Binding;
using Compiler.Binding.Objects;

namespace LuaCompiler_Test;

public class LuaCompiler
{
    private static readonly Encoding LuaEncoding = Encoding.GetEncoding(28591);

    [StructLayout(LayoutKind.Sequential)]
    public record struct CompileOptions(
        bool IsLua51, // Lua 5.1
        // ReSharper disable once InconsistentNaming
        bool IsLuaJIT // LuaJIT
    );
    
    public static IEnumerable<byte> Compile(string source, CompileOptions compileOptions)
    {
        // ReSharper disable once InconsistentNaming
        var Compiler = new Binding();

        var status = Compiler.LoadString(source);

        if (!status.Item1)
            throw new Exception("Syntax error or similar");

        var function = (LuaFunction) status.Item2!;
        
        if (Compiler["string"] is not LuaTable stringTable || stringTable.Dictionary["dump"] is not LuaFunction dumpFunction)
            throw new Exception("Lua state error");
        
        var dumpedFunction = dumpFunction.Call(function);
        
        if (dumpedFunction.Length != 2 || dumpedFunction[0] is not string bytecode)
            throw new Exception("Couldn't dump function");
        
        return LuaEncoding.GetBytes(bytecode);
    }
}