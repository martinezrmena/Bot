using System;
using System.Collections.Generic;
using Bot.Models;

namespace Bot.ViewModel.Helpers
{
    public class BotResponseEventArgs : EventArgs
    {
        public List<BotMessage> BotMessages
        {
            get;
            set;
        }
    }
}
