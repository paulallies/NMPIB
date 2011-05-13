using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;


namespace ImageBrowser
{
    public class Repository
    {
        public string CurrentUser()
        {
            string name = "";
            WebClient mvc = new WebClient();
            mvc.OpenReadCompleted +=  (sender, e) =>
            {
                name = e.Result.ToString();
            };
            mvc.OpenReadAsync(new Uri("http://localhost:33/Account/CurrentUser"));

            return name;
        }

    }
}
