using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace Mailer.Behaviours
{
    public class VisibilityTransitionBehaviour : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Visibility), typeof(VisibilityTransitionBehaviour),
                new PropertyMetadata(default(Visibility), PropertyChangedCallback));


        public static readonly DependencyProperty AnimationOutProperty =
            DependencyProperty.Register("AnimationOut", typeof(Storyboard), typeof(VisibilityTransitionBehaviour),
                new PropertyMetadata(default(Storyboard)));

        public static readonly DependencyProperty AnimationInProperty =
            DependencyProperty.Register("AnimationIn", typeof(Storyboard), typeof(VisibilityTransitionBehaviour),
                new PropertyMetadata(default(Storyboard)));

        public Visibility Value
        {
            get => (Visibility) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public Storyboard AnimationOut
        {
            get => (Storyboard) GetValue(AnimationOutProperty);
            set => SetValue(AnimationOutProperty, value);
        }

        public Storyboard AnimationIn
        {
            get => (Storyboard) GetValue(AnimationInProperty);
            set => SetValue(AnimationInProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = (VisibilityTransitionBehaviour) d;

            b.TransitionOut((Visibility) e.OldValue);
        }

        protected override void OnAttached()
        {
            AssociatedObject.Visibility = Value;

            base.OnAttached();
        }

        private void TransitionOut(Visibility oldValue)
        {
            if (AssociatedObject == null)
                return;

            if (AnimationOut == null || oldValue == Visibility.Collapsed)
            {
                TransitionIn();
            }
            else
            {
                AnimationOut.Completed += AnimationOutCompleted;
                AnimationOut.Begin(AssociatedObject);
            }
        }

        private void TransitionIn()
        {
            if (AssociatedObject == null)
                return;

            AssociatedObject.Visibility = Value;
            if (AnimationIn != null) AnimationIn.Begin(AssociatedObject);
        }

        private void AnimationOutCompleted(object sender, object e)
        {
            AnimationOut.Completed -= AnimationOutCompleted;
            TransitionIn();
        }
    }
}