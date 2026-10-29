using System;
using System.Threading;

namespace classiki
{
    public class Contact
    {
        private static int _contactId = 0;

        public int ChannelID { get; }
        public string Value { get; }
        public int ContactId { get; }

        public Contact(string value, int channelID)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Значение контакта не может быть пустым.", nameof(value));

            if (channelID <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(channelID),
                    "Идентификатор канала должен быть положительным.");

            ChannelID = channelID;
            Value = value.Trim();
            ContactId = Interlocked.Increment(ref _contactId);
        }
    }
}
