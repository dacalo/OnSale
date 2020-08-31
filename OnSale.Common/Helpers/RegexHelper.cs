﻿using System;
using System.Net.Mail;

namespace OnSale.Common.Helpers
{
    public class RegexHelper : IRegexHelper
    {
        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
