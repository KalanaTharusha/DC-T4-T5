using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    internal class DatabaseGenerator
    {
        private static readonly Random random = new Random();

        private string GetFirstname()
        {
            string[] firstNames = { "Emily", "John", "Logan", "Smith", "Paul", "Jack", "Lily", "Olivia", "Mark", "Ava" };
            return firstNames[random.Next(firstNames.Length)];
        }

        private string GetLastname()
        {
            string[] lastNames = { "Wilson", "Walker", "Miller", "Shelby", "Williams", "Parker", "Turner", "Phillips", "Collins", "White" };
            return lastNames[random.Next(lastNames.Length)];
        }

        private uint GetPIN()
        {
            return (uint)random.Next(10000);
        }

        private uint GetAccNo()
        {
            return (uint)random.Next(1000000000);
        }

        private int GetBalance()
        {
            return random.Next(1000000);
        }

        private Bitmap GetBitmap()
        {
            int height = 32;
            int width = 32;

            Bitmap bitmap = new Bitmap(height, width);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color color = Color.FromArgb(
                        random.Next(256),
                        random.Next(256),
                        random.Next(256)
                        );

                    bitmap.SetPixel(x, y, color);
                }
            }

            return bitmap;
        }

        public void GetNextAccount(out uint pin, out uint acctNo, out string firstName, out string lastName, out int balance, out Bitmap bitmap)
        {
            pin = GetPIN();
            acctNo = GetAccNo();
            firstName = GetFirstname();
            lastName = GetLastname();
            balance = GetBalance();
            bitmap = GetBitmap();
        }
    }
}
