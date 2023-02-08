using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drain.ViewModels
{
    public class EngineViewModel: BaseViewModel
    {
        private string _serial = "";
        public string Serial
        {
            get { return (_serial); }
            set { SetProperty(ref _serial, value); }
        }

        private string _hardwareCode = "";
        public string HardwareCode
        {
            get { return (_hardwareCode); }
            set { SetProperty(ref _hardwareCode, value); }
        }

        public void Generate(object param)
        {
            Random random = new Random();
            string sample = $"{random.Next()}";
            
            Serial = sample;
        }
    }
}
