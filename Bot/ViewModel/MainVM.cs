using System;
using System.ComponentModel;
using Bot.ViewModel.Helpers;
using Xamarin.Forms;

namespace Bot.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        BotServiceHelper botHelper;

        public Command SendCommand
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        public MainVM()
        {
            botHelper = new BotServiceHelper();
            SendCommand = new Command(SendActivity);
        }

        void SendActivity()
        {
            botHelper.SendActivity(Message);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}