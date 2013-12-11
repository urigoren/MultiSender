using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiSender.Models
{
    public class Recipient
    {
        public string Email;
        public string Name;
        public string Regex;
        public override string ToString()
        {
            return Name;
        }
    }
}
