using System.Threading;

namespace classiki
{
    public class User
    {
        private string _firstName;
        private string _lastName;
        private string _patronymic;
        private string _login;
        private string _password;
        private static int _userId = 0;

        public string FirstName
        {
            get => _firstName; set => _firstName = value;
        }
        public string LastName
        {
            get => _lastName; set => _lastName = value;
        }
        public string Patronymic
        {
            get => _patronymic; set => _patronymic = value;
        }
        public string Login
        {
            get => _login; set => _login = value;
        }
        public string Password
        {
            set => _password = value;
        }
        public int UserId { get; }

        public User(string firstName, string lastName, string patronymic, string login, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            Login = login;
            Password = password;
            UserId = Interlocked.Increment(ref _userId); 
        }

        public void Deconstruct(out string fullName, out string login)
        {
            fullName = $"{LastName} {FirstName} {Patronymic}";
            login = Login;
        }
    }
}
