using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Windows;
using RestApp.UserService;

namespace RestApp
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell
    {
        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value; 
                NotifyOfPropertyChange(() => Message);
            }
        }



        public void Login()
        {
            using (var client = new UserServiceClient())
            {
                try
                {
                    string token = client.Login(UserName, HashPassword(Password));
                }
                catch (FaultException<BadLoginCredentialsException>)
                {
                    Message = "Username or password is invalid";
                    Password = null;
                }
            }
        }

        private string HashPassword(string value)
        {
            using (var hash = SHA256.Create())
            {
                return string.Join("", hash
                    .ComputeHash(Encoding.UTF8.GetBytes(value))
                    .Select(item => item.ToString("x2")));
            }
        }
    }
}