using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Posti.Tasks
{
    class RegisterTask
    {

        private async static Task<bool> RequestAccess()
        {
            BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();

            switch (status)
            {
                case BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity:
                    return true;
                default:
                    return false;
            }

        }
            

        public async static Task<BackgroundTaskRegistration> RegisterBackgroundTask(
                                                string taskEntryPoint,
                                                string name,
                                                IBackgroundTrigger trigger,
                                                IBackgroundCondition condition,
                                                BackgroundTaskCompletedEventHandler completionHandler
                                                )
        {
            if (!await RequestAccess())
            {
                return null;
            }
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {

                if (cur.Value.Name == name)
                {
                    return (BackgroundTaskRegistration)(cur.Value);
                }
            }

            var builder = new BackgroundTaskBuilder();

            builder.Name = name;
            builder.TaskEntryPoint = taskEntryPoint;
            builder.SetTrigger(trigger);

            if (condition != null)
            {

                builder.AddCondition(condition);
            }

            BackgroundTaskRegistration task = builder.Register();
            task.Completed += completionHandler;
            return task;
        }

    }
}
