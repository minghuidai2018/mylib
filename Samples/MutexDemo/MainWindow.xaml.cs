﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace MutexDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static Mutex mMutex = new Mutex(true, "{851C4EBF-46B5-4298-88FF-01B4B8D8AE6C}");

        public MainWindow()
        {
            InitializeComponent();

            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            //Shared._PipeServer.Stop();
        }



        public void OpenSubmission(string sid)
        {
            var tabItem = new TabItem() { Header = sid, };

            tabControl.Items.Add(tabItem);

            this.Activate();
        }

    }
}
