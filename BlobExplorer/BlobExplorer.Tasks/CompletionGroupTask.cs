using System;
using Windows.ApplicationModel.Background;
using Windows.Networking.BackgroundTransfer;

namespace BlobExplorer.Tasks
{
    public sealed class CompletionGroupTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTransferCompletionGroupTriggerDetails details = taskInstance.TriggerDetails
                as BackgroundTransferCompletionGroupTriggerDetails;

            if (details == null)
            {
                // This task was not triggered by a completion group.
                return;
            }

        }
    }
}