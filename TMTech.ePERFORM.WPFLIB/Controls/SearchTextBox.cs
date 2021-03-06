﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TMTech.Shared.WPFLIB.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TMTech.ePERFORM.WPFLIB.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TMTech.ePERFORM.WPFLIB.Controls;assembly=TMTech.ePERFORM.WPFLIB.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:SearchTextBox/>
    ///
    /// </summary>
    public class SearchTextBox : TextBox
    {
        #region Dependency Properties

        public static DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(string), typeof(SearchTextBox));

        public static DependencyProperty LabelTextColorProperty = DependencyProperty.Register("LabelTextColor", typeof(Brush), typeof(SearchTextBox));

        public static DependencyProperty SearchModeProperty = DependencyProperty.Register("SearchMode", typeof(SearchMode), typeof(SearchTextBox), new PropertyMetadata(SearchMode.Instant));

        private static DependencyPropertyKey HasTextPropertyKey = DependencyProperty.RegisterReadOnly("HasText", typeof(bool), typeof(SearchTextBox), new PropertyMetadata());

        public static DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        public static DependencyProperty UseWildcardsProperty = DependencyProperty.Register("UseWildcards", typeof(bool), typeof(SearchTextBox), new PropertyMetadata(false));

        public static DependencyProperty MatchCaseProperty = DependencyProperty.Register("MatchCase", typeof(bool), typeof(SearchTextBox), new PropertyMetadata(false));

        public static DependencyProperty MatchPrefixProperty = DependencyProperty.Register("MatchPrefix", typeof(bool), typeof(SearchTextBox), new PropertyMetadata(false));

        public static DependencyProperty MatchSuffixProperty = DependencyProperty.Register("MatchSuffix", typeof(bool), typeof(SearchTextBox), new PropertyMetadata(false));

        public static DependencyProperty SearchEventTimeDelayProperty = DependencyProperty.Register("SearchEventTimeDelay", typeof(Duration), typeof(SearchTextBox),
            new FrameworkPropertyMetadata(
            new Duration(new TimeSpan(0, 0, 0, 0, 500)),
            new PropertyChangedCallback(OnSearchEventTimeDelayChanged))
        );

        private DispatcherTimer mSearchEventDelayTimer;



        private static DependencyPropertyKey IsMouseLeftButtonDownPropertyKey = DependencyProperty.RegisterReadOnly("IsMouseLeftButtonDown", typeof(bool), typeof(SearchTextBox), new PropertyMetadata());
        public static DependencyProperty IsMouseLeftButtonDownProperty = IsMouseLeftButtonDownPropertyKey.DependencyProperty;
        public bool IsMouseLeftButtonDown
        {
            get { return (bool)GetValue(IsMouseLeftButtonDownProperty); }
            private set { SetValue(IsMouseLeftButtonDownPropertyKey, value); }
        }


        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public Brush LabelTextColor
        {
            get { return (Brush)GetValue(LabelTextColorProperty); }
            set { SetValue(LabelTextColorProperty, value); }
        }

        public SearchMode SearchMode
        {
            get { return (SearchMode)GetValue(SearchModeProperty); }
            set { SetValue(SearchModeProperty, value); }
        }

        public bool UseWildcards
        {
            get { return (bool)GetValue(UseWildcardsProperty); }
            set { SetValue(UseWildcardsProperty, value); }
        }

        public bool MatchCase
        {
            get { return (bool)GetValue(MatchCaseProperty); }
            set { SetValue(MatchCaseProperty, value); }
        }

        public bool MatchPrefix
        {
            get { return (bool)GetValue(MatchPrefixProperty); }
            set { SetValue(MatchPrefixProperty, value); }
        }


        public bool MatchSuffix
        {
            get { return (bool)GetValue(MatchSuffixProperty); }
            set { SetValue(MatchSuffixProperty, value); }
        }




        public bool HasText
        {
            get { return (bool)GetValue(HasTextProperty); }
            private set { SetValue(HasTextPropertyKey, value); }
        }


        public string ReqularExpression
        {
            get
            {
                return Text;
            }
        }

        #endregion


        static SearchTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchTextBox), new FrameworkPropertyMetadata(typeof(SearchTextBox)));
        }


        /// <summary>
        /// Event handler for TextBox text changes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            HasText = Text.Length != 0;


            if (SearchMode == SearchMode.Instant)
            {
                mSearchEventDelayTimer.Stop();
                mSearchEventDelayTimer.Start();
            }
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Border iconBorder = GetTemplateChild("PART_SearchIconBorder") as Border;
            if (iconBorder != null)
            {
                iconBorder.MouseLeftButtonDown += new MouseButtonEventHandler(IconBorder_MouseLeftButtonDown);
                iconBorder.MouseLeftButtonUp += new MouseButtonEventHandler(IconBorder_MouseLeftButtonUp);
                iconBorder.MouseLeave += new MouseEventHandler(IconBorder_MouseLeave);
            }
        }


        private void IconBorder_MouseLeftButtonDown(object obj, MouseButtonEventArgs e)
        {
            IsMouseLeftButtonDown = true;
        }


     
        private void IconBorder_MouseLeftButtonUp(object obj, MouseButtonEventArgs e)
        {
            if (!IsMouseLeftButtonDown) return;

            if (HasText && SearchMode == SearchMode.Instant)
            {
                this.Text = "";
            }
            if (HasText && SearchMode == SearchMode.Delayed)
            {
                RaiseSearchEvent();
            }

            IsMouseLeftButtonDown = false;
        }

        private void IconBorder_MouseLeave(object obj, MouseEventArgs e)
        {

            IsMouseLeftButtonDown = false;
        }





        public Duration SearchEventTimeDelay
        {
            get { return (Duration)GetValue(SearchEventTimeDelayProperty); }
            set { SetValue(SearchEventTimeDelayProperty, value); }
        }

        static void OnSearchEventTimeDelayChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            SearchTextBox stb = o as SearchTextBox;
            if (stb != null)
            {
                stb.mSearchEventDelayTimer.Interval = ((Duration)e.NewValue).TimeSpan;
                stb.mSearchEventDelayTimer.Stop();
            }
        }



        public static readonly RoutedEvent SearchEvent = EventManager.RegisterRoutedEvent("Search", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchTextBox));


        public event RoutedEventHandler Search
        {
            add { AddHandler(SearchEvent, value); }
            remove { RemoveHandler(SearchEvent, value); }
        }


        private void RaiseSearchEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(SearchEvent);
            RaiseEvent(args);
        }



        /// <summary>
        /// Constructor
        /// </summary>
        public SearchTextBox()
            : base()
        {
            mSearchEventDelayTimer = new DispatcherTimer();
            mSearchEventDelayTimer.Interval = SearchEventTimeDelay.TimeSpan;
            mSearchEventDelayTimer.Tick += new EventHandler(OnSeachEventDelayTimerTick);
        }


        void OnSeachEventDelayTimerTick(object o, EventArgs e)
        {
            mSearchEventDelayTimer.Stop();
            RaiseSearchEvent();
        }


            

        protected override void OnKeyDown(KeyEventArgs e)
        {

            if (e.Key == Key.Escape && SearchMode == SearchMode.Instant)
            {
                this.Text = "";
            }
            else if ((e.Key == Key.Return || e.Key == Key.Enter) && SearchMode == SearchMode.Delayed)
            {
                RaiseSearchEvent();
            }
            else
            {
                base.OnKeyDown(e);
            }
        }


        public string GetRegularExpression()
        {
            string value = this.Text;

            bool hasQuestionChar = value.IndexOf('?') >= 0;
            bool hasStarChar = value.IndexOf('*') >= 0;

            if (hasQuestionChar && hasStarChar)
                return "^" + Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + "$";
            else if (hasStarChar)
                return "^" + Regex.Escape(value).Replace("\\*", ".*") + "$";
            else if (hasQuestionChar)
                return "^" + Regex.Escape(value).Replace("\\?", ".") + "$";

            return value;
        }
    }





    /// <summary>
    /// Defines the search for typing text in SearchTextBox.
    /// </summary>
    public enum SearchMode
    {
        Instant,
        Delayed,
    }
}
