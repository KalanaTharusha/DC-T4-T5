using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;
using System.Reflection;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using System.Drawing;
using System.Windows.Interop;
using ServerInterface;
using BusinessTierInterface;
using System.IO;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusinessServerInterface foob;
        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<BusinessServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8101/BusinessService";
            foobFactory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            ItemCountBlock.Text = "Total Items: " + foob.GetNumEntries().ToString();

        }

        private void goBtn_Click(object sender, RoutedEventArgs e)
        {

            int index = 0;
            string fName = "", lName = "";
            int bal = 0;
            uint acct = 0, pin = 0;
            String bitmapString = null;

            try
            {
                index = Int32.Parse(indexBox.Text);

                if (index >= 0 && index < foob.GetNumEntries())
                {

                    foob.GetValuesForEntry(index, out acct, out pin, out bal, out fName, out lName, out bitmapString);

                    fNameBox.Text = fName;
                    lNameBox.Text = lName;
                    accNoBox.Text = acct.ToString();
                    pinBox.Text = pin.ToString("D4");
                    balBox.Text = bal.ToString("C");

                    if (bitmapString != null)
                    {
                        displayProfilePic(bitmapString);
                    }

                }
                else
                {
                    throw new Exception("Invalid Index");
                }
            }
            catch (FaultException<IndexFault> ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (FaultException<BitmapFault> ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        // convert base64 string to bitmap and then display it through the UI thread
        private void displayProfilePic(String bitmapString)
        {
            Bitmap bitmap;

            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(bitmapString);

                    using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                    {
                        bitmap = new Bitmap(memoryStream);
                    }

                    ImageSourceConverter converter = new ImageSourceConverter();
                    var pic = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                    profilePic.Source = pic;
                }
                catch
                {
                    throw new Exception("Error while decoding the bitmap");
                }
            });
        }

    }
}
