using System;
using System.Security.Cryptography;
using System.Threading;

namespace classiki
{
    public class User
    {
        private const int MinPasswordLength = 6;
        private const int PasswordHashSize = 32;
        private const int PasswordSaltSize = 16;
        private const int PasswordIterations = 100_000;

        private static int _userId = 0;

        private byte[] _passwordHash = Array.Empty<byte>();
        private byte[] _passwordSalt = Array.Empty<byte>();

        public string FirstName { get; }
        public string LastName { get; }
        public string Patronymic { get; }
        public string Login { get; }
        public int UserId { get; }

        /// <summary>
        /// Пароль хранится только в виде соли и хеша (PBKDF2), открытый текст не сохраняется.
        /// </summary>
        public string Password
        {
            set => SetPassword(value);
        }

        public User(string firstName, string lastName, string patronymic, string login, string password)
        {
            FirstName = RequireText(firstName, nameof(firstName), "Имя");
            LastName = RequireText(lastName, nameof(lastName), "Фамилия");
            Patronymic = RequireText(patronymic, nameof(patronymic), "Отчество");
            Login = RequireText(login, nameof(login), "Логин");
            SetPassword(password);
            UserId = Interlocked.Increment(ref _userId);
        }

        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password, _passwordSalt, PasswordIterations, HashAlgorithmName.SHA256, PasswordHashSize);

            return CryptographicOperations.FixedTimeEquals(hash, _passwordHash);
        }

        public void Deconstruct(out string fullName, out string login)
        {
            fullName = $"{LastName} {FirstName} {Patronymic}";
            login = Login;
        }

        private void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Пароль не может быть пустым.", nameof(password));

            if (password.Length < MinPasswordLength)
                throw new ArgumentException(
                    $"Пароль должен содержать не менее {MinPasswordLength} символов.", nameof(password));

            _passwordSalt = RandomNumberGenerator.GetBytes(PasswordSaltSize);
            _passwordHash = Rfc2898DeriveBytes.Pbkdf2(
                password, _passwordSalt, PasswordIterations, HashAlgorithmName.SHA256, PasswordHashSize);
        }

        private static string RequireText(string value, string paramName, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} не может быть пустым.", paramName);

            return value.Trim();
        }
    }
}
