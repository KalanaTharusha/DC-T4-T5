﻿using System;
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
using BusinessTierInterface;
using System.IO;
using DataLibrary;
using ServerInterface;

namespace AsyncClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusinessServerInterface foob;
        string term;
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
            Bitmap bitmap = null;

            searchBox.Text = string.Empty;

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
                        // convert base64 string to bitmap and then display it through the UI thread
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

        private async void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            indexBox.Text = string.Empty;
            term = searchBox.Text;

            Task<DataStruct> task = new Task<DataStruct>(Search);
            task.Start();

            DataStruct ds = await task;

            UpdateGUI(ds);

        }

        private DataStruct Search()
        {
            string fName = "", lName = "";
            int bal = 0;
            uint acct = 0, pin = 0;
            String bitmapString = null;

            try
            {
                foob.Search(term, out acct, out pin, out bal, out fName, out lName, out bitmapString);

                if (!fName.Equals(""))
                {
                    DataStruct ds = new DataStruct();
                    ds.acctNo = acct;
                    ds.pin = pin;
                    ds.balance = bal;
                    ds.firstName = fName;
                    ds.lastName = lName;

                    // convert base64 string to bitmap and then assign to the DataStruct object
                    try
                    {
                        byte[] imageBytes = Convert.FromBase64String(bitmapString);

                        using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                        {
                            ds.bitmap = new Bitmap(memoryStream);
                        }

                    }
                    catch
                    {
                        throw new Exception("Error while decoding the bitmap");
                    }

                    return ds;

                }

            }
            catch (FaultException<SearchFault> ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private void UpdateGUI(DataStruct ds)
        {
            fNameBox.Text = ds.firstName;
            lNameBox.Text = ds.lastName;
            accNoBox.Text = ds.acctNo.ToString();
            pinBox.Text = ds.pin.ToString("D4");
            balBox.Text = ds.balance.ToString("C");

            // display the profile picture through the UI thread
            this.Dispatcher.Invoke(() =>
            {
                try
                {

                    ImageSourceConverter converter = new ImageSourceConverter();
                    var pic = Imaging.CreateBitmapSourceFromHBitmap(ds.bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

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