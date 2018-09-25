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

using System.IO;
using System.Net;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using UrlFileDownloader.Core;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;

namespace UrlFileDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int nbDownloadSameTime = 1;
        private int defaultTryTodownload = 3;
        private string outputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        private DownloaderManager downloaderManager;

        public MainWindow()
        {
            InitializeComponent();
            this.downloaderManager = new DownloaderManager(this.nbDownloadSameTime);
            this.downloaderManager.progressEventHandler += this.GlobalProgress;
        }

        private async void GlobalProgress(object sender, double progress)
        {
            this.btnDownload.Dispatcher.Invoke(() => ButtonProgressAssist.SetValue(this.btnDownload, progress), DispatcherPriority.Background);
            Debug.WriteLine("Global progress : " + progress);
            
        }

        private void btnBrowseInput_Click(object sender, RoutedEventArgs e)
        {
            this.loadInput();
        }

        private void btnBrowseOutput_Click(object sender, RoutedEventArgs e)
        {
            this.loadOutput();
        }


        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (this.downloaderManager.Destination != null && Directory.Exists(this.downloaderManager.Destination))
            {
                ButtonProgressAssist.SetValue(this.btnDownload, 50);
                //this.btnDownload.IsEnabled = false;
                this.downloaderManager.Start();
            }
        }

        private void loadOutput()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.downloaderManager.Destination = dialog.SelectedPath;
                    this.tbOutputFolder.Text = this.downloaderManager.Destination;

                    this.checkActivateDownloadButton();
                }
            }
        }


        private void loadInput()
        {
            this.downloaderManager.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.";

            DialogResult result = openFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbSourceFile.Text = openFileDialog.FileName;


                String[] lines = File.ReadAllLines(this.tbSourceFile.Text);

                foreach (string line in lines)
                {
                    String[] split = line.Split('[');
                    Download download = new Download(split[0], split[1]);
                    this.downloaderManager.Add(download);
                }

                this.dgLinks.ItemsSource = this.downloaderManager;
                this.checkActivateDownloadButton();
               
            }
        }

        private void checkActivateDownloadButton()
        {
            if(Directory.Exists(this.downloaderManager.Destination) && this.dgLinks.Items.Count > 0 && !this.downloaderManager.Completed)
            {
                this.btnDownload.IsEnabled = true;
                ButtonProgressAssist.SetIsIndicatorVisible(this.btnDownload, true);
            }
            else
            {
                btnDownload.IsEnabled = false;
                ButtonProgressAssist.SetIsIndicatorVisible(this.btnDownload, false);
            }
        }
    }
}
