using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace RestApp.Reserve
{
    [Export(typeof (ReserveViewModel))]
    public class ReserveViewModel : Screen
    {
        private string _name;


        public ReserveViewModel()
        {
            Name = "asdasdasd";
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }
    }
}