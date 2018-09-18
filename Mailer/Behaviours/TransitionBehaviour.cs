using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace Mailer.Behaviours
{
    public class TransitionBehaviour : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty TransitionProperty =
            DependencyProperty.Register("Transition", typeof(Storyboard), typeof(TransitionBehaviour),
                new PropertyMetadata(default(Storyboard)));

        public static readonly DependencyProperty TransitionDelayProperty =
            DependencyProperty.Register("TransitionDelay", typeof(int), typeof(TransitionBehaviour),
                new PropertyMetadata(300));

        public static readonly DependencyProperty TransitionIndexProperty =
            DependencyProperty.RegisterAttached("TransitionIndex", typeof(int), typeof(TransitionBehaviour),
                new PropertyMetadata(default(int)));

        public static readonly DependencyProperty IgnoreTransitionProperty =
            DependencyProperty.RegisterAttached("IgnoreTransition", typeof(bool), typeof(TransitionBehaviour),
                new PropertyMetadata(default(bool)));

        private List<FrameworkElement> _lastTargets;

        public Storyboard Transition
        {
            get => (Storyboard) GetValue(TransitionProperty);
            set => SetValue(TransitionProperty, value);
        }

        public int TransitionDelay
        {
            get => (int) GetValue(TransitionDelayProperty);
            set => SetValue(TransitionDelayProperty, value);
        }

        public static void SetTransitionIndex(UIElement element, int value)
        {
            element.SetValue(TransitionIndexProperty, value);
        }

        public static int GetTransitionIndex(UIElement element)
        {
            return (int) element.GetValue(TransitionIndexProperty);
        }

        public static void SetIgnoreTransition(UIElement element, bool value)
        {
            element.SetValue(IgnoreTransitionProperty, value);
        }

        public static bool GetIgnoreTransition(UIElement element)
        {
            return (bool) element.GetValue(IgnoreTransitionProperty);
        }

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            if (AssociatedObject is ListBox)
            {
                var listBox = (ListBox) AssociatedObject;
                listBox.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
            }

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            if (AssociatedObject is ListBox)
            {
                var listBox = (ListBox) AssociatedObject;
                listBox.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
            }

            base.OnDetaching();
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (((ItemContainerGenerator) sender).Status == GeneratorStatus.ContainersGenerated)
            {
                var targets = GetTargets();
                if (targets == null)
                    return;

                var newItems = targets.Where(t => !_lastTargets.Contains(t)).ToList();
                if (newItems.Count == 0) _lastTargets.Clear();

                for (var index = 0; index < newItems.Count; index++)
                {
                    if ((bool) targets[index].GetValue(IgnoreTransitionProperty))
                        continue;

                    var transitionIndex = Convert.ToInt32(targets[index].GetValue(TransitionIndexProperty));
                    for (var i = 1; i < Transition.Children.Count; i++)
                        Transition.Children[i].BeginTime =
                            TimeSpan.FromMilliseconds(
                                TransitionDelay * (transitionIndex != 0 ? transitionIndex : index));

                    Transition.Begin(newItems[index]);
                }
            }
            else if (((ItemContainerGenerator) sender).Status == GeneratorStatus.GeneratingContainers)
            {
                _lastTargets = GetTargets();
            }
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            var targets = GetTargets();
            if (targets == null)
                return;

            for (var index = 0; index < targets.Count; index++)
            {
                if ((bool) targets[index].GetValue(IgnoreTransitionProperty))
                    continue;

                var transitionIndex = Convert.ToInt32(targets[index].GetValue(TransitionIndexProperty));
                for (var i = 1; i < Transition.Children.Count; i++)
                    Transition.Children[i].BeginTime =
                        TimeSpan.FromMilliseconds(TransitionDelay * (transitionIndex != 0 ? transitionIndex : index));

                Transition.Begin(targets[index]);
            }
        }

        private List<FrameworkElement> GetTargets()
        {
            var result = new List<FrameworkElement>();

            if (AssociatedObject is ListBox)
            {
                var itemsControl = (ListBox) AssociatedObject;
                var count = itemsControl.Items.Count;

                for (var i = 0; i < count; i++)
                {
                    var container = (FrameworkElement) itemsControl.ItemContainerGenerator.ContainerFromIndex(i);
                    if (container != null)
                        result.Add(container);
                }
            }
            else if (AssociatedObject is ItemsControl)
            {
                var itemsControl = (ItemsControl) AssociatedObject;
                var count = itemsControl.Items.Count;

                for (var i = 0; i < count; i++)
                {
                    var container = (FrameworkElement) itemsControl.ItemContainerGenerator.ContainerFromIndex(i);
                    if (container != null)
                        result.Add(container);
                }
            }
            else if (AssociatedObject is Panel)
            {
                var panel = (Panel) AssociatedObject;
                var count = panel.Children.Count;
                for (var i = 0; i < count; i++) result.Add((FrameworkElement) panel.Children[i]);
            }

            return result;
        }
    }
}