using System;
using Terminal.Gui;

namespace App.Windows
{
    public sealed class Navigation : View
    {
        public EventHandler OnGoToEncryption { get; set; }
        
        public EventHandler OnGoToDecryption { get; set; }
        
        public Navigation()
        {
            ColorScheme = new ColorScheme();

            var actionLabel = new Label("Select an action");

            var goToEncryption = new Button("Encryption")
            {
                Clicked = () =>
                {
                    OnGoToEncryption(this, EventArgs.Empty);
                },
                X = Pos.Right(actionLabel),
                Y = Pos.Bottom(actionLabel)
            };

            var goToDecryption = new Button("Decryption")
            {
                Clicked = () => OnGoToDecryption(this, EventArgs.Empty),
                X = Pos.Right(actionLabel),
                Y = Pos.Bottom(actionLabel) + 1
            };

            Add(actionLabel, goToEncryption, goToDecryption);
        }
    }
}