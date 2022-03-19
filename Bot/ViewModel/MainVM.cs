using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Bot.ViewModel.Helpers;
using Xamarin.Forms;
using Bot.Models;

namespace Bot.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        #region Properties
        BotServiceHelper botHelper;

        public Command SendCommand
        {
            get;
            set;
        }

        public ObservableCollection<ChatMessage> Messages
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
        #endregion

        public MainVM()
        {
            botHelper = new BotServiceHelper();
            SendCommand = new Command(SendActivity);
            Messages = new ObservableCollection<ChatMessage>();

            botHelper.MessageReceived += BotHelper_MessageReceived;
        }

        void SendActivity()
        {
            Messages.Add(new ChatMessage
            {
                Text = Message,
                IsIncoming = false
            });
            botHelper.SendActivity(Message);
        }

        void BotHelper_MessageReceived(object sender, BotServiceHelper.BotResponseEventArgs e)
        {
            foreach (var activity in e.Activities)
            {
                if (activity.From.Id != "user1")
                {
                    Messages.Add(new ChatMessage
                    {
                        Text = activity.Text,
                        IsIncoming = true
                    });
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
