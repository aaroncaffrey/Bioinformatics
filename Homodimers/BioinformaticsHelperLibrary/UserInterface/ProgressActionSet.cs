using System;

namespace BioinformaticsHelperLibrary.UserProteinInterface
{
    public class ProgressActionSet
    {
        public static void Report(string text, ProgressActionSet progressActionSet)
        {
            if (progressActionSet != null)
            {
                progressActionSet.Report(text);
            }
        }

        public static void ReportFilesSaved(string[] filesSaved, ProgressActionSet progressActionSet)
        {
            foreach (string savedFile in filesSaved)
            {
                Report("Saved: " + savedFile, progressActionSet);
            }
        }

        public static void EstimatedTimeRemainingAction(long startTicks, long itemsCompleted, long itemsTotal, ProgressActionSet progressActionSet)
        {
            if (progressActionSet != null)
            {
                progressActionSet.EstimatedTimeRemainingAction(startTicks, itemsCompleted, itemsTotal);
            }
        }

        public static void FinishAction(bool successful, ProgressActionSet progressActionSet)
        {
            if (progressActionSet != null)
            {
                progressActionSet.FinishAction(successful);
            }
        }

        public static void StartAction(int totalOperationsRequired, ProgressActionSet progressActionSet)
        {
            if (progressActionSet != null)
            {
                progressActionSet.StartAction(totalOperationsRequired);
            }
        }

        public static void ProgressAction(int totalOperationsProgressed, ProgressActionSet progressActionSet)
        {
            if (progressActionSet != null)
            {
                progressActionSet.ProgressAction(totalOperationsProgressed);
            }
        }

        /// <summary>
        ///     long startTicks, long itemsCompleted, long itemsTotal
        /// </summary>
        private readonly Action<long, long, long> _estimatedTimeRemainingAction;

        /// <summary>
        ///     Boolean value is whether or not the overall operation was successful
        /// </summary>
        private readonly Action<bool> _finishAction;

        /// <summary>
        ///     int value is Total Operations Progressed
        /// </summary>
        private readonly Action<int> _progressAction;

        /// <summary>
        ///     int value is Total Operations Required
        /// </summary>
        private readonly Action<int> _startAction;

        /// <summary>
        ///     string value is the new operation status to be forwarded to the user proteinInterface
        /// </summary>
        private readonly Action<string> _reportAction;

        public ProgressActionSet(Action<int> startAction, Action<bool> finishAction, Action<int> progressAction, Action<string> reportAction, Action<long, long, long> estimatedTimeRemainingAction)
        {
            _startAction = startAction;
            _finishAction = finishAction;
            _progressAction = progressAction;
            _reportAction = reportAction;
            _estimatedTimeRemainingAction = estimatedTimeRemainingAction;
        }

        public void EstimatedTimeRemainingAction(long startTicks, long itemsCompleted, long itemsTotal)
        {
            if (_estimatedTimeRemainingAction != null) _estimatedTimeRemainingAction(startTicks, itemsCompleted, itemsTotal);
        }

        public void FinishAction(bool successful)
        {
            if (_finishAction != null) _finishAction(successful);
        }

        public void StartAction(int totalOperationsRequired)
        {
            if (_startAction != null) _startAction(totalOperationsRequired);
        }

        public void ProgressAction(int totalOperationsProgressed)
        {
            if (_progressAction != null) _progressAction(totalOperationsProgressed);
        }

        public void Report(string statusText)
        {
            if (_reportAction != null) _reportAction(statusText);
        }
    }
}