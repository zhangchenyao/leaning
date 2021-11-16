using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvmtest
{
    class ObservableUser: ObservableObject
    {
        private readonly User user;

        public ObservableUser(User user) => this.user = user;

        public string Name
        {
            get => user.Name;
            set => SetProperty(user.Name, value, user, (u, n) => u.Name = n);
        }
    }
}
