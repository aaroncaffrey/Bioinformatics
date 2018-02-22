//-----------------------------------------------------------------------
// <copyright file="UserProteinInterfaceOperations.cs" company="Aaron Caffrey">
//     Copyright (c) 2013 Aaron Caffrey. All rights reserved.
// </copyright>
// <author>Aaron Caffrey</author>
//-----------------------------------------------------------------------

using System;
using System.Linq;
using System.Windows.Forms;

namespace BioinformaticsHelperLibrary.UserProteinInterface
{
    public static class UserProteinInterfaceOperations
    {
        private static void LabelEstimatedTimeRemainingUpdateProc(Label estimatedTimeRemainingLabel, long startTicks, long itemsCompleted, long itemsTotal)
        {
            if ((estimatedTimeRemainingLabel == null) || (!estimatedTimeRemainingLabel.Created))
            {
                return;
            }

            TimeSpan estimatedTimeRemaining = EstimatedTimeRemainingTimeSpan(startTicks, itemsCompleted, itemsTotal);

            //lock (estimatedTimeRemainingLabel)
            {
                estimatedTimeRemainingLabel.Text = estimatedTimeRemaining.ToString(@"dd\:hh\:mm\:ss");
            }
        }

        /// <summary>
        ///     LabelEstimatedTimeRemainingUpdate.
        /// </summary>
        /// <param name="estimatedTimeRemainingLabel"></param>
        /// <param name="startTicks"></param>
        /// <param name="itemsCompleted"></param>
        /// <param name="itemsTotal"></param>
        public static void LabelEstimatedTimeRemainingUpdate(Label estimatedTimeRemainingLabel, long startTicks, long itemsCompleted, long itemsTotal)
        {
            if ((estimatedTimeRemainingLabel == null) || (!estimatedTimeRemainingLabel.Created))
            {
                return;
            }

            if (estimatedTimeRemainingLabel.InvokeRequired)
            {
                estimatedTimeRemainingLabel.Invoke(new MethodInvoker(delegate { LabelEstimatedTimeRemainingUpdateProc(estimatedTimeRemainingLabel, startTicks, itemsCompleted, itemsTotal); }));
            }
            else
            {
                LabelEstimatedTimeRemainingUpdateProc(estimatedTimeRemainingLabel, startTicks, itemsCompleted, itemsTotal);
            }
        }

        private static void ProgressBarIncrementProc(ProgressBar progressBar, int incrementValue = -1)
        {
            if ((progressBar == null) || (!progressBar.Created))
            {
                return;
            }

            if (incrementValue > -1)
            {
                //lock (progressBar)
                {
                    progressBar.Increment(incrementValue);
                }
            }
        }

        /// <summary>
        ///     ProgressBarIncrement.
        /// </summary>
        /// <param name="progressBar"></param>
        /// <param name="incrementValue"></param>
        public static void ProgressBarIncrement(ProgressBar progressBar, int incrementValue = -1)
        {
            if ((progressBar == null) || (!progressBar.Created))
            {
                return;
            }

            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new MethodInvoker(delegate { ProgressBarIncrementProc(progressBar, incrementValue); }));
            }
            else
            {
                progressBar.Increment(incrementValue);
            }
        }

        /// <summary>
        ///     Enable or disable behaviour triggering controls on the user proteinInterface.
        /// </summary>
        /// <param name="enabledValue">True to enable.  False to disable.</param>
        public static void SetControlsEnabledProperty(Control control, Control[] controlsToSkip, bool enabledValue)
        {
            if (control == null || !control.Created)
            {
                return;
            }

            if (controlsToSkip == null || !controlsToSkip.Contains(control))
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(new MethodInvoker(delegate()
                    {
                        //lock (control)
                        {
                            control.Enabled = enabledValue;
                        }
                    }));
                }
                else
                {
                    //lock (control)
                    {
                        control.Enabled = enabledValue;
                    }
                }
            }

            foreach (Control childControl in control.Controls)
            {
                //if (!controlsToSkip.Contains(childControl))
                {
                    SetControlsEnabledProperty(childControl, controlsToSkip, enabledValue);
                }
            }
        }

        private static void TextBoxClearProc(TextBox textBox)
        {
            if ((textBox == null) || (!textBox.Created))
            {
                return;
            }

            //lock (textBox)
            {
                textBox.Clear();
            }
        }

        /// <summary>
        ///     TextBoxClear.
        /// </summary>
        /// <param name="textBox"></param>
        public static void TextBoxClear(TextBox textBox)
        {
            if ((textBox == null) || (!textBox.Created))
            {
                return;
            }

            if (textBox.InvokeRequired)
            {
                textBox.Invoke(new MethodInvoker(delegate { TextBoxClearProc(textBox); }));
            }
            else
            {
                TextBoxClearProc(textBox);
            }
        }

        /// <summary>
        ///     Calculates a TimeSpan of the Estimated Time Remaining for an operation where the total number of completed
        ///     workloads and the total number of workloads are known.
        /// </summary>
        /// <param name="startTicks">The ticks value before the workloads commenced.</param>
        /// <param name="itemsCompleted">The total number of workloads completed.</param>
        /// <param name="itemsTotal">The overall total number of workloads.</param>
        /// <returns></returns>
        public static TimeSpan EstimatedTimeRemainingTimeSpan(long startTicks, long itemsCompleted, long itemsTotal)
        {
            return new TimeSpan(((DateTime.Now.Ticks - startTicks) / (itemsCompleted > 0 ? itemsCompleted : 1)) * (itemsTotal - itemsCompleted));
        }

        private static void TextBoxAppendLineProc(TextBox textBox, string text, bool timestamp = true, bool linenumbers = true)
        {
            if ((textBox == null) || (text == null) || (!textBox.Created))
            {
                return;
            }

            //lock (textBox)
            {
                const string space = " ";
                string lineNumber = (linenumbers) ? ("" + ((textBox.Lines.Length == 0) ? 1 : textBox.Lines.Length) + space) : string.Empty;
                string date = timestamp ? ("" + DateTime.Now + space) : string.Empty;
                string newLine = Environment.NewLine;

                textBox.AppendText(lineNumber + date + text + newLine);
                textBox.SelectionStart = textBox.Text.Length;
                textBox.ScrollToCaret();
            }
        }

        /// <summary>
        ///     TextBoxAppendLine.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="text"></param>
        public static void TextBoxAppendLine(TextBox textBox, string text, bool timestamp = true, bool linenumbers = true)
        {
            if ((textBox == null) || (text == null) || (!textBox.Created))
            {
                return;
            }

            if (textBox.InvokeRequired)
            {
                textBox.Invoke(new MethodInvoker(delegate { TextBoxAppendLineProc(textBox, text, timestamp, linenumbers); }));
            }
            else
            {
                TextBoxAppendLineProc(textBox, text, timestamp, linenumbers);
            }
        }

        private static void ProgressBarResetProc(ProgressBar progressBar, int minimumValue = -1, int maximumValue = -1, int progressValue = -1)
        {
            if ((progressBar == null) || (!progressBar.Created))
            {
                return;
            }

            //lock (progressBar)
            {
                if (minimumValue > -1)
                {
                    progressBar.Minimum = minimumValue;
                }

                if (maximumValue > -1)
                {
                    progressBar.Maximum = maximumValue;
                }

                if (progressValue > -1)
                {
                    progressBar.Value = progressValue;
                }
            }
        }

        /// <summary>
        ///     ProgressBarReset.
        /// </summary>
        /// <param name="progressBar"></param>
        /// <param name="minimumValue"></param>
        /// <param name="maximumValue"></param>
        /// <param name="progressValue"></param>
        public static void ProgressBarReset(ProgressBar progressBar, int minimumValue = -1, int maximumValue = -1, int progressValue = -1)
        {
            if ((progressBar == null) || (!progressBar.Created))
            {
                return;
            }

            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new MethodInvoker(delegate { ProgressBarResetProc(progressBar, minimumValue, maximumValue, progressValue); }));
            }
            else
            {
                ProgressBarResetProc(progressBar, minimumValue, maximumValue, progressValue);
            }
        }
    }
}