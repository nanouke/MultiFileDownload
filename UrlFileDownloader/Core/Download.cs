

namespace UrlFileDownloader.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class Download : INotifyPropertyChanged
    {
        public enum Status
        {
            InProgress,
            Waiting,
            Error,
            Pause,
            Completed
        };

        private Thread thread;
        private Stopwatch startAt;

        private double progress;
        private string error;
        private Status status;
        private string url;
        private string destination;
        private double speed;
        private string name;

        public double Progress { get => progress; set { progress = value; OnPropertyChanged("Progress"); } }
        public string Error { get => error; set { error = value; OnPropertyChanged("Error"); } }
        public Status DownloadStatus { get => status; private set { this.status = value; OnPropertyChanged("DownloadStatus"); } }
        public string Url { get => url; set { url = value; OnPropertyChanged("Url"); } }
        public string Destination { get => destination; set { this.destination = value; OnPropertyChanged("Destination"); } }
        public double Speed { get => speed; set { speed = value; OnPropertyChanged("Speed"); } }
        public string Name { get => name; set { name = value; OnPropertyChanged("Name"); } }

        public Download(string url, string name)
        {
            this.url = url;
            this.status = Status.Waiting;
            this.progress = 0.0;
            this.name = name;
        }



        public void StartDownload()
        {
            if(this.destination == null || !Directory.Exists(this.destination))
            {
                throw new IOException();
            }

            this.thread = new Thread(Downloading);
            this.startAt = new Stopwatch();
            this.DownloadStatus = Status.InProgress;
            Debug.WriteLine("Sart Dwonaload of " + this.name);
            this.thread.Start();

        }

        private async void Downloading()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadProgressChanged += FileDownloadProgressChanged;
            webClient.DownloadFileCompleted += FileDownloadCompleted;
            webClient.DownloadFileAsync(new Uri(this.url), this.destination + "/" + this.name);
        }

        private void FileDownloadProgressChanged(Object sender, DownloadProgressChangedEventArgs e)
        {
            WebClient webClient = (WebClient)sender;

            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());

            this.Progress = bytesIn / totalBytes * 100;
            this.Speed = (e.BytesReceived / 1024d / this.startAt.Elapsed.TotalMilliseconds);

        }

        private void FileDownloadCompleted(Object sender, AsyncCompletedEventArgs e)
        {
            this.DownloadStatus = Status.Error;

            if(File.Exists(this.destination + "/" + this.name))
            {
                FileInfo fileInfo = new FileInfo(this.destination + "/" + this.name);
                if(fileInfo.Length > 0)
                {
                    this.DownloadStatus = Status.Completed;
                    Debug.WriteLine("Completed Dwonaload of " + this.name);
                }
            }

        }


        // Event

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
