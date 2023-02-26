using AttributerLibrary;

namespace BusinessLibrary.Attribute
{
    internal class LogAttribute : Attributer
    {
        public override void OnAfterExecuted(AttributerContext context)
        {
            //This is Example Attribute
            base.OnAfterExecuted(context);
        }
    }
}
