using System.ComponentModel.Composition;

namespace RestApp
{
    [Export(typeof (IShell))]
    public class ShellViewModel : IShell
    {
        [ImportingConstructor]
        public ShellViewModel()
        {
        }
    }
}