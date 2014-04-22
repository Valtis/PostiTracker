using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters;

using Windows.Foundation;

namespace SharedLibrary.Posti.Dao
{
    public sealed class StorageDao
    {
        private const string fileName = "items.json";
        private Dictionary<String, Domain.TrackedItem> items = new Dictionary<String, Domain.TrackedItem>();


        public IEnumerable<Domain.TrackedItem> Items
        {
            get { return items.Values; }
        }
        
        public StorageDao()
        {
           
        }
        
        public void AddItem(Domain.TrackedItem item)
        {
            if (items.ContainsKey(item.TrackingCode))
            {
                items[item.TrackingCode] = item;
            }
            else                
            {
                items.Add(item.TrackingCode, item);
            }
        }

        public void Clear()
        {
            items.Clear();
        }
    
        public async void Save()
        {

            string json = JsonConvert.SerializeObject(items, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            });

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile textFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (IRandomAccessStream textStream = await textFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (DataWriter textWriter = new DataWriter(textStream))
                {
                    textWriter.WriteString(json);
                    await textWriter.StoreAsync();
                }
            }
        }

        public IAsyncOperation<IList<Domain.TrackedItem>> Load()
        {
            return LoadHelper().AsAsyncOperation();
        }

        private async Task<IList<Domain.TrackedItem>> LoadHelper()
        {
            Clear();
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile textFile = await localFolder.GetFileAsync(fileName);

                using (IRandomAccessStream textStream = await textFile.OpenReadAsync())
                {
                    using (DataReader textReader = new DataReader(textStream))
                    {
                        uint textLength = (uint)textStream.Size;
                        await textReader.LoadAsync(textLength);
                        string json = textReader.ReadString(textLength);

                        items = JsonConvert.DeserializeObject<IDictionary<String, Domain.TrackedItem>>(json) as Dictionary<String, Domain.TrackedItem>;
                     

                    }
                }
            }
            catch (Exception ex)
            {
            }

            return items.Values.ToList<Domain.TrackedItem>();
        }
 
    }
}
