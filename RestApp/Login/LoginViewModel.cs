using System.Globalization;
using System.ServiceModel;
using System.Windows.Controls;
using Caliburn.Micro;
using RestApp.Properties;
using RestApp.Register;
using RestApp.UserService;
using RestApp.Util;

namespace RestApp.Login
{
    public class LoginViewModel : Screen
    {

        #region Properties
        private string _message;


        private string _username;
        private PasswordBox _passwordBox;


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

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }
        #endregion

        public LoginViewModel()
        {
            Success = false;
            DisplayName = LocalizationHelper.GetString("LoginTitle");
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _passwordBox = ((LoginView)view).Password;
            Username = "abel";
            _passwordBox.Password = "pass";
        }

        public async void Login()
        {
            using (var service = new UserServiceClient())
            {
                try
                {
                    var result = await service.LoginAsync(Username, Helper.Hash(_passwordBox.Password));
                    AppData.User = result;
                    Success = true;
                    TryClose();
                }
                catch (FaultException<BadLoginCredentialsException>)
                {
                    _passwordBox.Password = "";
                    Message = LocalizationHelper.GetString("BadLogin");
                }
            }
        }

        public void Register()
        {
            var windowManager = IoC.Get<IWindowManager>();
            var vm = new RegisterViewModel();
            windowManager.ShowDialog(vm);
        }
    }
}