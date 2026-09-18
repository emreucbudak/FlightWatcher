using FlightWatcher.Application.Interfaces;

namespace FlightWatcher.Infrastructure.Facades
{
    public class FlyingWatcherFacade : IFlyingWatcherFacade
    {
        public void StartScan()
        {
        }

        public string HowToUse()
        {
            return "Düzenli fiyat taramasını başlatmak için /taramayibaslat komutunu kullanmalısınız.";
        }

        public string WhatDoesItDo()
        {
            return "İlk kontrol yapıldıktan sonra, belirlediğiniz aralıklarla fiyatlar yeniden kontrol edilir. "
                + "Belirlediğiniz maksimum fiyata eşit veya daha düşük fiyatlı bir bilet bulunduğunda size bildirim gönderilir.";
        }
    }
}
