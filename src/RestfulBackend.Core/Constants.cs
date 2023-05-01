using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulBackend.Core
{
    public static class Constants
    {
        // https://patorjk.com/software/taag/#p=display&f=Ogre&t=logo
        public static string Logo =
            @" _                    " + Environment.NewLine +
            @"| | ___   __ _  ___   " + Environment.NewLine +
            @"| |/ _ \ / _` |/ _ \  " + Environment.NewLine +
            @"| | (_) | (_| | (_) | " + Environment.NewLine +
            @"|_|\___/ \__, |\___/  " + Environment.NewLine +
            @"         |___/        " + Environment.NewLine +
            Environment.NewLine;

        public static string ProductName = "RESTful Backend Server";
        public static string InternalServerError = "An internal server error was encountered.";
        public static string BadRequestError = "Your request was invalid.  Please refer to the API documentation.";
        public static string NotFoundError = "The requested object was not found.";
    }
}
