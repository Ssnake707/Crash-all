using System.Collections.Generic;

namespace MyTools.BehaviourTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
    public class Node
    {
        protected NodeState _state;
        private Node _parent;
        protected List<Node> _children = new List<Node>();
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node Parent { get => _parent; }

        public NodeState State { get => _state; }

        public Node() => 
            _parent = null;

        public Node(List<Node> children)
        {
            foreach (Node item in children) 
                Attach(item);
        }

        private void Attach(Node node)
        {
            node._parent = this;
            _children.Add(node);
        }

        public Node GetRootParent(Node node)
        {
            Node rootParent = node._parent;
            if (rootParent == null) return node;
            return GetRootParent(rootParent);
        }

        public void SetData(string key, object value) => 
            _dataContext[key] = value;

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            Node node = _parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node._parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = _parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node._parent;
            }
            return false;
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;
    }
}

