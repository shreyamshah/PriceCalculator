using System;
using System.Collections.Generic;
using System.Text;

namespace PriceCalculator.Data
{
    public class ActivityResult
    {
        public static string key = "failed";
        public int RequestCode { get; set; }
        public object ResultCode { get; set; }
        public object Data { get; set; }
    }

    public class Paging
    {
        int start, pageSize;
        public Paging()
        {
            start = 0;
            pageSize = 10;
        }
    }
}
