using System.Reflection;

namespace AttributerLibrary
{
    public class AttributerError
    {
        public Exception? Exception { get; set; }
        public int? ErrorLine { get; set; }
        public MethodBase? ErrorMethod { get; set; }
        public MethodBase[]? CallingParentMethods { get; set; }
    }
}
