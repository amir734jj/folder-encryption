using Terminal.Gui;

namespace App.Windows
{
    public sealed class RootWindow : Toplevel
    {
        public RootWindow(Encryption encryption, Navigation navigation)
        {
            navigation.OnGoToEncryption += (sender, args) => Application.Run(encryption);
            
            navigation.OnGoToDecryption += (sender, args) => Application.Run(encryption);

            Add(navigation);
        }
    }
}