using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posti.Actions
{

    class RefreshItems
    {
        public async void Refresh(SharedLibrary.Posti.Dao.StorageDao storage)
        {
            var items = await storage.Load();

            foreach (var item in items)
            {
                var dao = new SharedLibrary.Posti.Dao.WebDao(item.TrackingCode);
                storage.AddItem(await dao.LoadItem());
            }

            storage.Save();
        }

    }
}
