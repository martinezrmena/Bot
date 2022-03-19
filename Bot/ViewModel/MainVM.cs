using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Bot.ViewModel.Helpers;
using Xamarin.Forms;
using Bot.Models;
using Bot.Services;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace Bot.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        #region Properties
        BotService botService;

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
            botService = new BotService();
            SendCommand = new Command(async() => await SendMessage());
            Messages = new ObservableCollection<ChatMessage>();
            SetConfig();
            botService.MessageReceived += BotService_MessageReceived;
        }

        public async void SetConfig()
        {
            await botService.Setup();
        }


        public async Task SendMessage()
        {
            Messages.Add(new ChatMessage
            {
                Text = Message,
                IsIncoming = false
            });

            await botService.SendMessage(Message);
        }

        void BotService_MessageReceived(object sender, BotResponseEventArgs e)
        {
            foreach (var botMessage in e?.BotMessages?.Where(x => x.From != "user1") ?? new List<BotMessage>())
            {
                Messages.Add(new ChatMessage
                {
                    Text = botMessage.Text,
                    IsIncoming = true
                });
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
