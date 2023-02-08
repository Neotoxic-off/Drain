using Drain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Drain.ViewModels
{
    public class MainViewModel: BaseViewModel
    {
        public SettingsViewModel Settings { get; set; }
        public EngineViewModel Engine { get; set; }

        private DelegateCommand Loader { get; set; }
        public DelegateCommand GenerateCommand { get; set; }

        public MainViewModel()
        {
            Settings = new SettingsViewModel();
            Engine = new EngineViewModel();
            Loader = new DelegateCommand(Load);
            GenerateCommand = new DelegateCommand(Engine.Generate);

            Loader.Execute(null);
        }

        private void Load(object data)
        {
            Settings.LoadVersion();
        }
    }
}
