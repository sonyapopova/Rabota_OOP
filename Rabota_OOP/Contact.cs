using System.Threading;

namespace classiki
{
    public class Contact
    {
        private static int _contactId = 0;
        private int _chanelId;
        private string _value;

        public int ChanelID
        {
            get => _chanelId; set => _chanelId = value;
        }
        public string Value
        {
            get => _value; set => _value = value;
        }
        public int ContactId { get; }

        public Contact(int contactId, string value, int chanelID)
        {
            ChanelID = chanelID;
            Value = value;
            ContactId = Interlocked.Increment(ref _contactId);
        }

    }
}
