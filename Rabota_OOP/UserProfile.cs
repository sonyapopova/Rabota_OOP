using System;
using System.Collections.Generic;

namespace classiki
{
    public class UserProfile
    {
        public User User { get; set; }
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        
        //в качестве демонтрации прочих свойств мы решили добавить дату создания профиля (прим. Попова София)
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public UserProfile(User user)
        {
            User = user;
        }
    }
}
