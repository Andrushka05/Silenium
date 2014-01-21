using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParserHelpers
{
    public interface IAd<T>
    {
        void Login(string email, string pass);
        void LogOut();
        List<T> GetAdList(string url, string proxy);
        bool PlaceAd(List<T> adList);
        List<Link> CategoryList(string link);
        List<Link> CityList();
    }
}
