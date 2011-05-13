using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Browser;
namespace SilverlightApplication1
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            HtmlPage.RegisterScriptableObject("SilverlightTest",this);
        }

        [ScriptableMember]
        public void btnUploadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Jpg|*.jpg"
            };

            if ((bool)ofd.ShowDialog())
            {
                UploadFile(ofd.File.Name, ofd.File.OpenRead());
            }

            else //cancel
            {
                StatusText.Text = "No files selected...";
            }

        }

        private void UploadFile(string fileName, Stream data)
        {
            UriBuilder ub = new UriBuilder("/Image/UploadFile");
            ub.Query = string.Format("filename={0}", fileName);
            WebClient c = new WebClient();
            c.OpenWriteCompleted += (sender, e) =>
                {
                    PushData(data, e.Result);
                    e.Result.Close();
                    data.Close();
                };
            c.OpenWriteAsync(ub.Uri);
        }

        private void PushData(Stream input, Stream output)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            while((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

    }
}
