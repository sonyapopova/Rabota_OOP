using System;
using System.Threading;

namespace classiki
{
    public class Channel
    {
        private static int _channelId = 0;

        public int ID { get; }
        public string Name { get; }

        public Channel(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название канала не может быть пустым.", nameof(name));

            ID = Interlocked.Increment(ref _channelId);
            Name = name.Trim();
        }
    }
}
