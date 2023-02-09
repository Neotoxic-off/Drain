using Drain.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Drain.ViewModels
{
    public class MainViewModel: BaseViewModel
    {
        public SettingsViewModel Settings { get; set; }
        public LoggerViewModel Logger { get; set; }
        private OpenFileDialog openFileDialog { get; set; }
        private DelegateCommand Loader { get; set; }
        public DelegateCommand SelectCommand { get; set; }
        public DelegateCommand DrainCommand { get; set; }
        public ManagerViewModel ManagerViewModel { get; set; }
        private string Root { get; set; }

        public MainViewModel()
        {
            Settings = new SettingsViewModel();
            Logger = new LoggerViewModel();
            Loader = new DelegateCommand(Load);
            Root = "Drainned";
            ManagerViewModel = new ManagerViewModel();
            SelectCommand = new DelegateCommand(Select);
            DrainCommand = new DelegateCommand(Drain);

            Loader.Execute(null);
        }

        private void Load(object data)
        {
            Logger.Record("loading");

            Settings.LoadVersion();
            openFileDialog = new OpenFileDialog();
            if (Directory.Exists(Root) == false)
            {
                Directory.CreateDirectory(Root);
            }

            Logger.Record("loaded");
        }

        private void Select(object data)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                Logger.Record("content updated");
            } else
            {
                Logger.Record("content not updated");
            }
        }

        private void Drain(object data)
        {
            string[] lines = null;
            string name = null;

            if (Validate() == true)
            {
                lines = LoadContent();
                foreach (string line in lines)
                {
                    name = Clean(GetName(line));
                    Logger.Record($"downloading {name}");
                    ManagerViewModel.Download(line, $"{Root}/{name}");
                    Logger.Record("downloaded");
                }
                Logger.Record("all content drained");
            } else
            {
                Logger.Record("no content to drain");
            }
        }

        private string Clean(string path)
        {
            string cleanned = path;
            char[] specials = { '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '/', ':', ';', '<', '=', '>', '?', '[', '\\', ']', '^', '`', '{', '}', '|', '~' };
        
            foreach (char c in specials)
            {
                if (cleanned.Contains(c) == true)
                {
                    cleanned = cleanned.Replace(c, '_');
                }
            }

            return (cleanned);
        }

        private bool Validate()
        {
            if (openFileDialog.FileName != null)
                return (File.Exists(openFileDialog.FileName));
            return (false);
        }

        private string GetName(string url)
        {
            string[] content = url.Split('/');

            return (content[content.Length - 1]);
        }

        private string[] LoadContent()
        {
            return (File.ReadAllLines(openFileDialog.FileName));
        }
    }
}
