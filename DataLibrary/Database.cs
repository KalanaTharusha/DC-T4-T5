using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class Database
    {
        readonly List<DataStruct> dataList;



        public Database()
        {
            dataList = new List<DataStruct>();

            for (int i = 0; i < 10; i++)
            {
                DatabaseGenerator databaseGenerator = new DatabaseGenerator();
                databaseGenerator.GetNextAccount(out uint pin, out uint accNo, out string firstName, out string lastName, out int balance, out Bitmap bitmap);

                DataStruct data = new DataStruct(pin, accNo, firstName, lastName, balance, bitmap);
                dataList.Add(data);
            }

        }

        public uint GetAcctNoByIndex(int index)
        {
            try
            {
                return dataList[index].acctNo;
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException();
            }
        }
        public uint GetPINByIndex(int index)
        {
            try
            {
                return dataList[index].pin;
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException();
            }
        }
        public string GetFirstNameByIndex(int index)
        {
            try
            {
                return dataList[index].firstName;
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException();
            }
        }
        public string GetLastNameByIndex(int index)
        {
            try
            {
                return dataList[index].lastName;
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException();
            }
        }
        public int GetBalanceByIndex(int index)
        {
            try
            {
                return dataList[index].balance;
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException();
            }
        }
        public Bitmap GetBitmapByIndex(int index)
        {
            try
            {
                return dataList[index].bitmap;
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException();
            }
        }
        public int GetNumRecords()
        {
            return dataList.Count;
        }
    }
}
