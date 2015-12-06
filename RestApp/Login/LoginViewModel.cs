using System.ServiceModel;
using System.Windows;
using Caliburn.Micro;
using RestApp.UserService;
using RestApp.Util;

namespace RestApp.Login
{
    public class LoginViewModel : Screen
    {
        private string _password;


        private string _username;

        public LoginViewModel()
        {
            Success = false;
            DisplayName = "RestApp - Login";
            Username = "abel";
            Password = "pass";
        }

        public bool Success { get; set; }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        public async void Login()
        {
            using (var service = new UserServiceClient())
            {
                try
                {
                    var result = await service.LoginAsync(Username, Password);
                    AppData.User = result;
                    Success = true;
                    TryClose();
                }
                catch (FaultException<BadLoginCredentialsException>)
                {
                    Password = null;
                    MessageBox.Show($"Unsuccessfull login with username: {Username}");
                }
            }
        }
    }
}