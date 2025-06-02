using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using System.Text;
using System.Threading.Tasks;

namespace OmniArt
{
    public class App : Application
    {
        // The IServiceProvider object retrieves a service object
        public static IServiceProvider Services { get; private set; }

        public App(IServiceProvider services)
        {

            Services = services;

            MainPage = new NavigationPage(new WelcomePage());
            
        }

    }
}
