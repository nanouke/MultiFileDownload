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
using System.Windows.Shapes;

namespace UrlFileDownloader
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {

        public ConfigWindow()
        {
            InitializeComponent();

            this.tbDelimiter.Text = Properties.Settings.Default.delimiter.ToString();
            this.tbSimultaneousDownload.Text = Properties.Settings.Default.SimultaneousDownload.ToString();

        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            this.spError.Visibility = Visibility.Hidden;
            if (this.validate()) {

                Properties.Settings.Default.delimiter = this.tbDelimiter.Text[0];
                Properties.Settings.Default.SimultaneousDownload = int.Parse(this.tbSimultaneousDownload.Text);

                this.DialogResult = true;
                this.Close();
            }
            else
            { 
                this.tbError.Text = "One or more fields are invalid.";
                this.spError.Visibility = Visibility.Visible;
            }
            
        }

        private bool validate()
        {
            bool valid = true;
            int result;
            if (!int.TryParse(this.tbSimultaneousDownload.Text, out result))
            {
                valid = false;
            }

            if(this.tbDelimiter.Text == string.Empty && this.tbDelimiter.Text.Length == 1)
            {
                valid = false;
            }

            return valid;
        }

    }
}
