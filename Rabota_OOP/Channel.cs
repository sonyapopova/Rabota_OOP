namespace classiki
{
    public class Channel
    {
        private static int _channelId = 0;

        public int ID { get; }
        public string Name { get; set; }

        public Channel(string name)
        {
            _channelId++;
            ID = _channelId;
            Name = name;
        }
    }
}
