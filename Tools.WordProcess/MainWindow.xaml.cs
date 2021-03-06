﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Tools.WordProcess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Process_Click(object sender, RoutedEventArgs e)
        {
            ProcessWord();
        }


        private void ProcessWord()
        {
            // get source text
            var source = this.textSource.Text;

            var result = WordProcessTool.RemoveEmptyLine(source);

            // clear target box
            this.textTarget.Clear();

            // put result in the target box
            this.textTarget.Text = result;

            // also copy result to clipborder
            Clipboard.SetText(result);

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.textSource.Clear();
            this.textTarget.Clear();
        }

        private void GetUrl_Click(object sender, RoutedEventArgs e)
        {
            var text = WordProcessTool.GetTextFromUrl(textUrl.Text);
            this.textSource.Text = text; 
        }

        private void ImportFiles_Click(object sender, RoutedEventArgs e)
        {
            var win = new Windows.ReadMultipleFiles();
            win.Owner = this;
            if (win.ShowDialog() == true)
            {
                this.textSource.Text = win.Text;
            }
        }

        private void ProcessSingleFile_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            var result = dlg.ShowDialog();
            if (result == true)
            {
                var filename = dlg.FileName;
                if (System.IO.File.Exists(filename))
                {
                    var win = new Windows.ImportFromFile(filename);
                    win.Owner = this;
                    win.ShowDialog();
                }
            }
        }
    }
}
