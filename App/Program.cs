using System.Reflection;
using App.Windows;
using LightInject;
using Terminal.Gui;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new ServiceContainer();

            container.RegisterAssembly("Models")
                .RegisterAssembly("Logic")
                .RegisterAssembly(Assembly.GetExecutingAssembly());

            Application.Run(container.GetInstance<RootWindow>());
        }
    }
}