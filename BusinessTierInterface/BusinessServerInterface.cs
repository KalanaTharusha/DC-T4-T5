﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace BusinessTierInterface
{
    [ServiceContract]
    public interface BusinessServerInterface
    {
        [OperationContract]
        int GetNumEntries();

        [OperationContract]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out String bitmapString);

        [OperationContract]
        [FaultContract(typeof(SearchFault))]
        void Search(string term, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out String bitmapString);
    }
}
