namespace UrlFileDownloader.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;


    public class DownloaderManager : ObservableCollection<Download>
    {

        private int simultaniusDownload;
        private Thread balancer;
        private bool completed;
        private double progress;
        private string destination;


        public bool Completed { get { return this.completed; } }

        public int SimultaniusDownload{
            get { return this.simultaniusDownload; }
            set {
                this.simultaniusDownload = (value > 0) ? value : 1;
            }
        }

        public string Destination { get => destination; set
            {
                this.destination = value;
                foreach(Download download in this)
                {
                    download.Destination = this.destination;
                }
            }
        }
        public double Progress { get => progress; set {
                if(value < 0)
                {
                    value = 0.0;
                }
                if(value > 100.0)
                {
                    value = 100.0;
                }
                progress = value;
                this.RaisePogressEventHandler();
            }
        }

        public DownloaderManager(int simultaniusDownload) : base()
        {
            this.SimultaniusDownload = simultaniusDownload;
            this.progress = 0.0;
            this.balancer = new Thread(runBalancer);
            this.completed = false;
           
        }

        public void AddDowload(string url, string name)
        {
            Download download = new Download(url, destination);
            if (this.Destination != null)
            {
                download.Destination = this.Destination;
            }
            this.Add(download);

        }

        

        public void Start()
        {
            this.completed = false;
            if (this.balancer == null)
            {
                this.balancer = new Thread(runBalancer);
            }
            this.balancer.Start();

        }

        private void runBalancer()
        {
            while (!this.completed)
            {

                int inProgressCount = this.Where(x => x.DownloadStatus == Download.Status.InProgress).Count();
                int waiting = this.Where(x => x.DownloadStatus == Download.Status.Waiting).Count();
                Debug.WriteLine("En progression : " + inProgressCount);

                if ( inProgressCount < this.simultaniusDownload && inProgressCount < this.Count)
                {
                    for(int i = inProgressCount; i < this.simultaniusDownload &&  i < waiting; i++)
                    {
                        
                        this.First(x => x.DownloadStatus == Download.Status.Waiting).StartDownload();
                    }

                }

                int completed = this.Where(x => x.DownloadStatus == Download.Status.Completed || x.DownloadStatus == Download.Status.Error).Count();


                this.Progress = (double)completed / (double)this.Count() * 100.0;

                if (completed == this.Count)
                {
                    this.completed = true;
                    break;
                }

            }

            this.balancer = null;
        }

        public delegate void ProgressEventHandler(object sender, double progression);

        // Declare the event.
        public event ProgressEventHandler progressEventHandler;

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        protected virtual void RaisePogressEventHandler()
        {
            Debug.WriteLine("progress...");
            // Raise the event by using the () operator.
            if (progressEventHandler != null)
                progressEventHandler(this, this.Progress);
        }
    }
}
