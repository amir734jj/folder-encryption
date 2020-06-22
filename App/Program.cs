using System.Reflection;
using App.Windows;
using LightInject;
using Logic;
using Logic.Interfaces;
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

            // Application.Run(container.GetInstance<RootWindow>());

            var amir = new DecryptLogic();

            var key =
                "@WQa9I5u@[GbMz$**4/jZ04f~%P=]a2e!0qz&t#F0u6%RDF%Clc$~21TzKk$z5y68wchv$p%vqOiucKbV*tZ%3Qp@F[ySH!RUH*hc$v=Ql~y~*ivRT/*$t/Y@G$]UVSwzNaU]6&+wXW&!d0$eYf+82#D]WfdeB*7$TBAq76R069v=!Ta75Z7==&&XVv9r@p2L6djPVno%Qf=59h3oid4&nn]F8viV6!hiM7ScQ%8S9]26F=Wx8%vQZp24cE4mui=";
            
            amir.AesDecrypt("/home/amir-pc/Downloads/hashcons.c.aes", key, 128);
        }
    }
}