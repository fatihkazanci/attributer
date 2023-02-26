using System.Reflection;

namespace AttributerLibrary
{
    public class AttributerContext
    {
        public MethodInfo? Method { get; set; }
        public object?[]? Arguments { get; set; }
        public bool Result { get; set; } = true;
        public AttributerError? Error { get; set; }
        public IServiceProvider? ServiceProvider { get; set; }
        public MethodBase[]? CallingParentMethods { get; set; }
    }
}
