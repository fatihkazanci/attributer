namespace AttributerLibrary
{
    public abstract class Attributer : Attribute, IAttributer
    {
        public virtual void OnAfterExecuted(AttributerContext context) { }

        public virtual void OnBeforeExecute(AttributerContext context) { }
    }
}
