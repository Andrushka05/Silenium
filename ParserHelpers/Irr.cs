using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using java.util.concurrent;

namespace ParserHelpers
{
    public class Irr:Ad<Ir>
    {
        public Irr()
        {
            _driver = InitWebDriver();
            _driver.get("http://m.avito.ru");
            _driver.manage().timeouts().implicitlyWait(30, TimeUnit.SECONDS);
        }
        public override void Login(string email, string pass)
        {
            throw new NotImplementedException();
        }
        public override void LogOut()
        {
            throw new NotImplementedException();
        }

        public override List<Ir> GetAdList(string url, ProgressBar progress, ref string error)
        {
            //количество товаров на странице //ul class same_adds_paging/li/a[text()='60']  click()

            //http://pskov.irr.ru/sports-tourism/page_len60/page2/ к каталогу добавляется page_len60/page1/

            //ссылка на товар //div class add_title_wrap/a

            //телефон  //a id show_contact_phones click()

            //фото телефона  //p id contact_phones/img src

            //продавец  //ul class form_info/li[text()=constain Продавец:

            //link продавец  //ul class form_info/li[text()=constain  Объявление на сайте компании:

            //описание  //p class text

            //название //h1 class title3

            //артикул //div class grey_info /span class number убрать №

            //дата  //div class grey_info /span class data

            //просмотров  //div class grey_info /span id advCountViewsButton

            //цена //div class credit_cost

            //фото  //ul class slider_pagination/li/a/img  заменяем small на view

            throw new NotImplementedException();
        }

        public override bool PlaceAd(List<Ir> adList)
        {
            throw new NotImplementedException();
        }

        public override List<Link> CategoryList(string link)
        {
            ////div class popubList popubUl breadcrumbsPopupList popupHideable/ul/li/a   a/span class black-количество объявлений в разделе
            throw new NotImplementedException();
        }

        public override List<Link> CityList()
        {
            throw new NotImplementedException();
        }
    }
}
