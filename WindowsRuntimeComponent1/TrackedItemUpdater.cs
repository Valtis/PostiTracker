using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Background;
using NotificationsExtensions;
using Windows.UI.Notifications;

namespace BackgroundTask
{
    public sealed class TrackedItemUpdater : IBackgroundTask
    {
        private SharedLibrary.Posti.Dao.StorageDao dao;


        public TrackedItemUpdater()
        {
            dao = new SharedLibrary.Posti.Dao.StorageDao();
        }
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();
            var items = await dao.Load();
            bool changes = false;
            foreach (var item in items)
            {
                var webDao = new SharedLibrary.Posti.Dao.WebDao(item.TrackingCode);
                var webItem = await webDao.LoadItem();
                if (webItem != null && !item.Equals(webItem))
                {
                    changes = true;
                    dao.AddItem(webItem);
                }
                
            }
            if (changes)
            {

                dao.Save();

                var toastTemplate = ToastTemplateType.ToastText02;
                var toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                var toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode("Tracked item status changed!"));

                var toast = new ToastNotification(toastXml);
                var toastNotifier = ToastNotificationManager.CreateToastNotifier();
                toastNotifier.Show(toast);
            }
           /* else
            {
                var toastTemplate = ToastTemplateType.ToastText02;
                var toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                var toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode("No changes, just toasting for fun and profit!"));

                var toast = new ToastNotification(toastXml);
                var toastNotifier = ToastNotificationManager.CreateToastNotifier();
                toastNotifier.Show(toast);

            }*/


            _deferral.Complete();
        }


        public static void OnComplete(IBackgroundTaskRegistration task, BackgroundTaskCompletedEventArgs args)
        {

        }
    }
}
