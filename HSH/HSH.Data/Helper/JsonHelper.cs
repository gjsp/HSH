using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSH.Data.Helper
{
    public class JsonHelper
    {
        public enum Status
        {
            Ok,
            Error,
            Invalid
        }

        public class JsonResponse
        {
            public Status Status { get; set; }
            public string Message { get; set; }
            public string ResultMessage { get; set; }
            public bool ModelState { get; set; }
            public List<string> Errors { get; set; }
        }
    }
}