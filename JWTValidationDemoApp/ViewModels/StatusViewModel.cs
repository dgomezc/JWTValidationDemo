using JWTValidationDemoApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTValidationDemoApp.ViewModels
{
    public class StatusViewModel : Observable
    {
        private bool _showPanel;

        public bool ShowPanel
        {
            get { return _showPanel; }
            set { Set(ref _showPanel, value); }
        }

        private bool _showProgressBar;

        public bool ShowProgressBar
        {
            get { return _showProgressBar; }
            set { Set(ref _showProgressBar, value); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { Set(ref _message, value); }
        }
    }
}
