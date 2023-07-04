namespace MyTools.QueueСontroller.QueueСontroller.Scripts
{
    public class DataQueue
    {
        public IItemQueue Item;
        public int IndexPlace;

        public DataQueue(IItemQueue item, int indexPlace)
        {
            this.Item = item;
            this.IndexPlace = indexPlace;
        }
    }
}

