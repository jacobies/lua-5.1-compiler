using LuaCompiler_Test;

var tests = new Dictionary<string, string>
{
    {"ScriptWare++", "a++"},
    {"CompoundOperators", @"
        a+=1
        a-=1
        a^=1
        a..=1
        a*=1
    "},
    {"ContinueStatement", @"
        for i = 1, 5 do 
            continue
        end
     "},
    {"JITSuffixes", @"
        local a = 1i
        local b = 1ULL
        local c = 1LL
    "},
    {"51HexEscapes", @"
        local a = ""\u{5000}""
        local b = ""\xFFFF""
    "},
    {"FiveMHashString", @"
        local a = `Hello!`
    "}
};

var defaultOptions = new LuaCompiler.CompileOptions(false, false);
var specialOptions = new LuaCompiler.CompileOptions(true, true);
foreach (var test in tests)
{
    try
    {
        LuaCompiler.Compile(test.Value, test.Key is "JITSuffixes" or "51HexEscapes" ? specialOptions : defaultOptions);
    }
    catch
    {
        throw new Exception($"Errored in {test.Key}");
    }
}

return 0;
