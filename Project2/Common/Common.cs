using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectTwo
{
    class Common
    {
        static void Main(string[] args)
        {
        }
    }
    public interface IPersistable
    {
        XElement toXML();
    }
}
