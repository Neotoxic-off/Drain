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

        public enum Format
        {
            png,
            jpg,
            gif,
            mp4,
            mp3,
            pdf,
            unknown
        }

        private List<(int, byte[], Format)> _formats { get; set; }

        public ManagerViewModel()
        {
            Client = new WebClient();
            _formats = new List<(int, byte[], Format)>()
            {
                (0, new byte[] { 0x89, 0x50, 0x4E, 0x47 }, Format.png),
                (0, new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46 }, Format.jpg ),
                (0, new byte[] { 0x47, 0x49, 0x46 }, Format.gif ),
                (4, new byte[] { 0x66, 0x74, 0x79, 0x70 }, Format.mp4 ),
                (0, new byte[] { 0x49, 0x44, 0x33 }, Format.mp3 ),
                (0, new byte[] { 0x25, 0x50, 0x44, 0x45 }, Format.pdf )
            };
        }

        public void Download(string url, string path)
        {
            byte[] content = null;
            int size = 10;
            byte[] buffer = new byte[size];
            Format extension = Format.unknown;
            int bytes_read = 0;

            if (File.Exists(path) == true)
            {
                path = $"{path}_{path.GetHashCode()}";
            }
            Client.DownloadFile(url, path);
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                bytes_read = fs.Read(buffer, 0, size);
                fs.Close();
            }
            extension = SearchFormat(buffer);
            if (extension != Format.unknown)
            {
                Rename(path, $"{path}.{extension}");
            }
        }

        private void Rename(string path, string name)
        {
            File.Move(path, name);
        }

        private Format SearchFormat(byte[] bytes)
        {
            int valid = 0;

            foreach ((int, byte[], Format) format in _formats)
            {
                for (int i = format.Item1; i < format.Item2.Length; i++)
                {
                    if (bytes[i] == format.Item2[i - format.Item1])
                    {
                        valid++;
                    }
                }
                if (valid == format.Item2.Length)
                    return (format.Item3);
                valid = 0;
            }

            return (Format.unknown);
        }
    }
}
