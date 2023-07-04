namespace MyTools.BehaviourTree
{
    public class Invertor : Node
    {
        private Node _decoratorNode;

        public Invertor(Node decoratorNode)
        {
            _decoratorNode = decoratorNode;
        }

        public override NodeState Evaluate()
        {
            switch (_decoratorNode.Evaluate())
            {
                case NodeState.FAILURE:
                    _state = NodeState.SUCCESS;
                    return _state;
                case NodeState.SUCCESS:
                    _state = NodeState.FAILURE;
                    return _state;
                case NodeState.RUNNING:
                    _state = NodeState.RUNNING;
                    return _state;
            }

            _state = NodeState.SUCCESS;
            return _state;
        }
    }
}