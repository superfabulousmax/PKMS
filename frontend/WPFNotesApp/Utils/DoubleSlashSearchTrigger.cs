using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFNotesApp.Events;

namespace WPFNotesApp.Utils
{
    public static class DoubleSlashSearchTrigger
    {
        public static readonly DependencyProperty EnableSlashTriggerProperty =
        DependencyProperty.RegisterAttached(
            "EnableSlashTrigger",
            typeof(bool),
            typeof(DoubleSlashSearchTrigger),
            new PropertyMetadata(false, OnEnableSlashTriggerChanged));

        public static void SetEnableSlashTrigger(DependencyObject obj, bool value) =>
            obj.SetValue(EnableSlashTriggerProperty, value);

        public static bool GetEnableSlashTrigger(DependencyObject obj) =>
            (bool)obj.GetValue(EnableSlashTriggerProperty);

        private static void OnEnableSlashTriggerChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is TextBox textBox && eventArgs.NewValue is bool enable && enable)
            {
                textBox.TextChanged += (s, args) =>
                {
                    var tb = (TextBox)s;
                    var caretIndex = tb.CaretIndex;
                    if (caretIndex < 2) return;

                    string beforeCaret = tb.Text.Substring(0, caretIndex);
                    int triggerIndex = beforeCaret.LastIndexOf("//");
                    if (triggerIndex == -1) return;

                    string query = beforeCaret.Substring(triggerIndex + 2);
                    if (string.IsNullOrWhiteSpace(query)) return;

                    var charRect = tb.GetRectFromCharacterIndex(caretIndex);
                    var screenPos = tb.PointToScreen(new Point(charRect.X, charRect.Bottom));

                    var eventAggregator = ((PrismApplication)Application.Current).Container.Resolve<IEventAggregator>();
                    eventAggregator.GetEvent<NoteSearchRequestedEvent>().Publish(new NoteSearchRequest
                    {
                        Query = query,
                        Position = screenPos
                    });
                };
            }
        }
    }
}
