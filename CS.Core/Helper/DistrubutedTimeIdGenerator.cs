namespace CS.Core.Helper
{
    public class DistributedTimeIdGenerator
    {
        private const int MaxNodeId = 9;
        private const int MaxSequence = 999;

        private readonly int _nodeId;
        private int _sequence = 0;
        private readonly object _lock = new();

        public DistributedTimeIdGenerator(int nodeId)
        {
            if (nodeId < 0 || nodeId > MaxNodeId)
            {
                throw new ArgumentException($"Node ID must be between 0 and {MaxNodeId}");
            }
            _nodeId = nodeId;
        }

        public int NextId()
        {
            lock (_lock)
            {
                var timePart = DateTimeOffset.UtcNow.ToUnixTimeSeconds() % 100;
                _sequence++;

                if (_sequence > MaxSequence)
                {
                    _sequence = 0;
                }

                return (int)(timePart * 10000 + _nodeId * 1000 + _sequence);
            }
        }
    }

}
