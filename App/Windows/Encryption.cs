using Terminal.Gui;

namespace App.Windows
{
    public class Encryption : Window
    {
        public Encryption(): base("Encryption")
        {
            ColorScheme = new ColorScheme();
            
            // By using Dim.Fill(), it will automatically resize without manual intervention
            Width = Dim.Fill();
            Height = Dim.Fill();

            var login = new Label("Login: ") {X = 3, Y = 2};
            var password = new Label("Password: ")
            {
                X = Pos.Left(login),
                Y = Pos.Top(login) + 1
            };
            var loginText = new TextField("")
            {
                X = Pos.Right(password),
                Y = Pos.Top(login),
                Width = 40
            };
            var passText = new TextField("")
            {
                Secret = true,
                X = Pos.Left(loginText),
                Y = Pos.Top(password),
                Width = Dim.Width(loginText)
            };

            var back = new Button("Back")
            {
                X = Pos.Bottom(password),
                Y = Pos.Bottom(password),
                Clicked = () =>
                {
                    Application.RequestStop();
                    Application.RequestStop();
                    
                    Application.Refresh();
                },
            };

            Add(
                // The ones with my favorite layout system
                login, password, loginText, passText, back);
        }
    }
}