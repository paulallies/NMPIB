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
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Browser;
using System.Runtime.Serialization.Json;

namespace ImageBrowser
{
    public class Magazine
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Image
    {
        public int Magid { get; set; }
        public string MagIssue { get; set; }
        public string Shoot { get; set; }
        public DateTime ShootDate { get; set; }
        public string description { get; set; }
        public string photographer { get; set; }
        public string keywords { get; set; }
    }

    public partial class MainPage : UserControl
    {            
        OpenFileDialog dlg;
        public MainPage()
        {
            Repository db = new Repository();
            InitializeComponent();
            GetMagList();
            GetCurrentUser();
        }

        private void GetMagList()
        {
            WebClient c = new WebClient();
            c.OpenReadCompleted += (sender, e) =>
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<Magazine>));
                List<Magazine> magList = (List<Magazine>)json.ReadObject(e.Result);
                MagList.DisplayMemberPath = "name";
                MagList.DataContext = magList;
                MagList.SelectedIndex = 0;
            };
            c.OpenReadAsync(new Uri(App.Current.Host.Source, "../Mag/List"));
        }

        private void GetCurrentUser()
        {
            WebClient c = new WebClient();
            c.OpenReadCompleted += (sender, e) =>
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(string));
                lblPhotographer.Text = (string)json.ReadObject(e.Result);
                if (lblPhotographer.Text.Trim().Length == 0)
                {
                    HtmlPage.Window.Navigate(new Uri(App.Current.Host.Source, "../Account/logon"));
                }

            };
            c.OpenReadAsync(new Uri(App.Current.Host.Source, "../Account/CurrentUser"));
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            myImage.Source = null;

            dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "Text Files (*.jpg)|*.jpg";

            bool? retval = dlg.ShowDialog();

            if (retval != null && retval == true) // open/uploaded
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(dlg.File.OpenRead());
                myImage.Source = image;


            }
            else //cancel
            {
                MessageBox.Show("Please Select File...");
            }
        }

        private void UploadFile(string fileName, FileStream fileStream, Image myImage)
        {
            UriBuilder ub = new UriBuilder(new Uri(App.Current.Host.Source,"../Image/SLUpload"));
            
            ub.Query = string.Format("filename={0}&Mag_id={1}&Mag_Issue={2}&Shoot={3}&ShootDate={4}&keywords={5}&description={6}&photographer={7}", 
                fileName,
                myImage.Magid.ToString(),
                myImage.MagIssue,
                myImage.Shoot,
                myImage.ShootDate.ToString("dd-MMM-yyyy"),                
                myImage.keywords,
                myImage.description,
                myImage.photographer
                );
            
            WebClient c = new WebClient();
            c.OpenWriteCompleted += (sender, e) =>
            {
                PushData(fileStream, e.Result);
                e.Result.Close();
                fileStream.Close();
              
            };
            c.OpenWriteAsync(ub.Uri);
            c.WriteStreamClosed += (sender, e) =>
            {
                //MessageBox.Show(e.Error.Message);
                HtmlPage.Window.Navigate(new Uri(App.Current.Host.Source, "../"));
            
            };

        }

        void c_WriteStreamClosed(object sender, WriteStreamClosedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PushData(Stream input, Stream output)
        {
            byte[] buffer = new byte[input.Length];
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        public void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Valid().Length == 0)
            {
                AddImage(dlg.File.Name, dlg.File.OpenRead());
            }
            else
            {
                MessageBox.Show(Valid());
            }
            
        }



        private void AddImage(string fileName, FileStream fileStream)
        {
            Image myImage = new Image() { 
             Magid = ((Magazine)(MagList.SelectedItem)).id,
             MagIssue = txtMagazineIssue.Text,
             Shoot = txtShoot.Text,
             ShootDate = (DateTime)ShootDate.SelectedDate,
             description = txtDescription.Text,
             keywords = txtKeywords.Text,
             photographer = lblPhotographer.Text
            };

            UploadFile(dlg.File.Name , dlg.File.OpenRead(), myImage);
        }

        private string Valid()
        {
            string errorMessage = "";
            if (txtMagazineIssue.Text.Trim().Length == 0)
            {
                errorMessage += "Magazine Issue missing!\n";
            }

            if (txtShoot.Text.Trim().Length == 0)
            {
                errorMessage += "Shoot missing!\n";
            }

            try
            {
                DateTime testShootDate = (DateTime)ShootDate.SelectedDate;
            }
            catch
            {
                errorMessage += "Shoot Date invalid!\n";
            }

            if (txtDescription.Text.Trim().Length == 0)
            {
                errorMessage += "Description missing!\n";
            }

            if (txtKeywords.Text.Trim().Length == 0)
            {
                errorMessage += "Keywords missing!\n";
            }

            if (myImage.Source == null)
            {
                errorMessage += "Image missing!";
            }



            return errorMessage;
        }
    }
}
