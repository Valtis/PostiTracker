using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.Foundation;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SharedLibrary.Posti.Dao
{
    public sealed class WebDao : INotifyPropertyChanged
    {
        private string trackingCode;
        private string url;
        private Domain.TrackedItem item = null;


        public Domain.TrackedItem GetCachedItem()
        {
            return item;
        }
        
        private async void SetItem(IAsyncOperation<Domain.TrackedItem> op)
        {
            item = await op;
            OnPropertyChanged("Description");
        }

        public string Description
        {
            get
            {
                if (item != null)
                {
                    return item.ToString();
                }
                return "Empty description!";
            }

        }

        public string TrackingCode 
        { 
            get
            {
                return trackingCode;
            }
            set 
            {
                trackingCode = value;
                url = "http://www.posti.fi/itemtracking/posti/search_by_shipment_id?lang=fi&ShipmentId=" + value;
                OnPropertyChanged();
            } 
        }

        public WebDao()
        {
            
        }
        
        public WebDao(String code)
        {
            TrackingCode = code;
        }

        public IAsyncOperation<Domain.TrackedItem> LoadItem() 
        {
            var result = GetTrackedItemHelper().AsAsyncOperation();
            SetItem(result);
            return result;
        }

        private async Task<Domain.TrackedItem> GetTrackedItemHelper() 
        {
            var item = new Domain.TrackedItem();
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url))
            using (var content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                if (result != null)
                {
                    var parser = new TrackPageParser(result);

                    item =  parser.Parse();
                    item.TrackingCode = trackingCode;
                } 
            }

            return item;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName="")
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}