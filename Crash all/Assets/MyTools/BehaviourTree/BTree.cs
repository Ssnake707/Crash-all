namespace MyTools.BehaviourTree
{
    public abstract class BTree
    {
        protected Node _root = null;

        public void Tick()
        {
            if (_root != null)
            {
                _root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
    }
}

