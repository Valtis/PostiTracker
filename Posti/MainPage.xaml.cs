using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Background;
using Windows.System;

namespace Posti
{

    public sealed partial class MainPage : Page
    {
        private SharedLibrary.Posti.Dao.StorageDao storageDao = new SharedLibrary.Posti.Dao.StorageDao();
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;


            Posti.Tasks.RegisterTask.RegisterBackgroundTask(
            "BackgroundTask.TrackedItemUpdater",
            "TrackedItemUpdater",
            new TimeTrigger(15, false),
            null,
            new BackgroundTaskCompletedEventHandler(BackgroundTask.TrackedItemUpdater.OnComplete)
            );
            
        
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
         //   var refresh = new Posti.Actions.RefreshItems();
          //  refresh.Refresh(storageDao);
            

        }

        private async void PostCodeTextBox_OnKeyDown(Object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                TextBox textBox = sender as TextBox;
                
                BindingExpression bindingExpr = textBox.GetBindingExpression( TextBox.TextProperty );
                bindingExpr.UpdateSource();

                var webDao = bindingExpr.DataItem as SharedLibrary.Posti.Dao.WebDao;
                storageDao.AddItem(await webDao.LoadItem());
                storageDao.Save();
            }

        }

    }
}
