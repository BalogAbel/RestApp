using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;
using RestApp.UserService;
using RestApp.Util;

namespace RestApp.Register
{
    class RegisterViewModel : Screen
    {
        private PasswordBox _passwordBox2;
        private PasswordBox _passwordBox;

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; NotifyOfPropertyChange(() => Message);}
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; NotifyOfPropertyChange(() => Username);}
        }



        public RegisterViewModel()
        {
            DisplayName = "RestApp - Registration";
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _passwordBox = ((RegisterView)view).Password;
            _passwordBox2 = ((RegisterView)view).Password2;
        }

        public async void Register()
        {
            if (_passwordBox.Password != _passwordBox2.Password)
            {
                Message = "Password and its confirmation does not match";
                return;
            }
            using (var svc = new UserServiceClient())
            {
                try
                {
                    await svc.RegisterUserAsync(Username, Helper.Hash(_passwordBox.Password));
                    TryClose();
                }
                catch (FaultException<AlreadyRegisteredException>)
                {
                    Message = "A user has been already registered with this username";
                }
            }
        }

        public void Cancel()
        {
            TryClose();
        }
    }
}
