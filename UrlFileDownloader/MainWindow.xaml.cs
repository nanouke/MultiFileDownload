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
using UrlFileDownloader.Vo;

using System.IO;
using System.Net;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using UrlFileDownloader.Core;
using System.Windows.Threading;

namespace UrlFileDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int nbDownloadSameTime = 5;
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
            this.progress.Dispatcher.Invoke(() => this.progress.Value = progress, DispatcherPriority.Background);
            Debug.WriteLine("Global progress");
            
        }

        private void btnBrowseInput_Click(object sender, RoutedEventArgs e)
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

                dgLinks.ItemsSource = this.downloaderManager;
            }
        }

        private void btnBrowseOutput_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();


                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.downloaderManager.Destination = dialog.SelectedPath;
                    this.tbOutputFolder.Text = this.downloaderManager.Destination;
                }
            }
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (this.downloaderManager.Destination != null && Directory.Exists(this.downloaderManager.Destination))
            {
                this.progress.Value = this.downloaderManager.Progress;
                this.downloaderManager.Start();
            }
        }
    }
}
