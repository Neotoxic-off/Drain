using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Drain.ViewModels
{
    public class ManagerViewModel: BaseViewModel
    {
        private WebClient _client;
        private WebClient Client
        {
            get { return (_client); }
            set { SetProperty(ref _client, value); }
        }

        public ManagerViewModel()
        {
            Client = new WebClient();
        }

        public void Download(string url, string path)
        {
            if (File.Exists(path) == true)
            {
                path = $"{path}_{path.GetHashCode()}";
            }

            Client.DownloadFile(url, path);
        }
    }
}
