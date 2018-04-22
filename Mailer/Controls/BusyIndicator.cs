using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Mailer.Controls
{
    /// <summary>
    ///     A control to provide a visual indicator when an application is busy.
    /// </summary>
    [TemplateVisualState(Name = "Idle", GroupName = "BusyStates")]
    [TemplateVisualState(Name = "Busy", GroupName = "BusyStates")]
    [TemplateVisualState(Name = "Error", GroupName = "BusyStates")]
    [TemplateVisualState(Name = "Visible", GroupName = "VisibilityStates")]
    [TemplateVisualState(Name = "Hidden", GroupName = "VisibilityStates")]
    [StyleTypedProperty(Property = "BusyContentStyle", StyleTargetType = typeof(ContentPresenter))]
    public class BusyIndicator : ContentControl
    {
        /// <summary>
        ///     Identifies the IsBusy dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(false, OnIsBusyChanged));

        /// <summary>
        ///     Identifies the FocusAferBusy dependency property.
        /// </summary>
        public static readonly DependencyProperty FocusAferBusyProperty = DependencyProperty.Register(
            "FocusAferBusy",
            typeof(Control),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        ///     Identifies the BusyContent dependency property.
        /// </summary>
        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
            "BusyContent",
            typeof(object),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        ///     Identifies the BusyTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
            "BusyContentTemplate",
            typeof(DataTemplate),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ErrorProperty = DependencyProperty.Register(
            "Error", typeof(string), typeof(BusyIndicator),
            new PropertyMetadata(default(string), OnErrorPropertyChanged));

        /// <summary>
        ///     Identifies the ProgressBarStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty BusyContentStyleProperty = DependencyProperty.Register(
            "BusyContentStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));


        static BusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyIndicator),
                new FrameworkPropertyMetadata(typeof(BusyIndicator)));
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the busy indicator should show.
        /// </summary>
        public bool IsBusy
        {
            get => (bool) GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }

        /// <summary>
        ///     Gets or sets a Control that should get focus when the busy indicator disapears.
        /// </summary>
        public Control FocusAferBusy
        {
            get => (Control) GetValue(FocusAferBusyProperty);
            set => SetValue(FocusAferBusyProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating the busy content to display to the user.
        /// </summary>
        public object BusyContent
        {
            get => GetValue(BusyContentProperty);
            set => SetValue(BusyContentProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating the template to use for displaying the busy content to the user.
        /// </summary>
        public DataTemplate BusyContentTemplate
        {
            get => (DataTemplate) GetValue(BusyContentTemplateProperty);
            set => SetValue(BusyContentTemplateProperty, value);
        }

        public string Error
        {
            get => (string) GetValue(ErrorProperty);
            set => SetValue(ErrorProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating the style to use for the progress bar.
        /// </summary>
        public Style BusyContentStyle
        {
            get => (Style) GetValue(BusyContentStyleProperty);
            set => SetValue(BusyContentStyleProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the BusyContent is visible.
        /// </summary>
        protected bool IsContentVisible { get; set; }

        private static void OnErrorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator) d).ChangeVisualState(true);
        }

        /// <summary>
        ///     Overrides the OnApplyTemplate method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(false);
        }

        /// <summary>
        ///     Changes the control's visual state(s).
        /// </summary>
        /// <param name="useTransitions">True if state transitions should be used.</param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            if (string.IsNullOrEmpty(Error))
                VisualStateManager.GoToState(this, IsBusy ? "Busy" : "Idle", useTransitions);
            else
                VisualStateManager.GoToState(this, "Error", useTransitions);

            VisualStateManager.GoToState(this, IsContentVisible ? "Visible" : "Hidden", useTransitions);
        }

        /// <summary>
        ///     IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="d">BusyIndicator that changed its IsBusy.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator) d).OnIsBusyChanged(e);
        }

        /// <summary>
        ///     IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsBusy)
            {
                // Go visible now
                IsContentVisible = true;
            }
            else
            {
                // No longer visible
                if (string.IsNullOrEmpty(Error))
                {
                    IsContentVisible = false;

                    if (FocusAferBusy != null)
                        FocusAferBusy.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
                            {
                                FocusAferBusy.Focus();
                                FocusAferBusy = null;
                            }
                        ));
                }
                else
                {
                    IsContentVisible = true;
                }
            }

            ChangeVisualState(true);
        }
    }
}