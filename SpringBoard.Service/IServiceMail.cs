using System;
using System.Collections.Generic;
using System.Text;

namespace SpringBoard.Service
{
    public interface IServiceMail
    {
        public string sendMail(string mail, string obj, string body);
    }
}
