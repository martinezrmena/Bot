using System;
using Bot.ViewModel.Helpers;

namespace Bot.ViewModel
{
    public class MainVM
    {
        BotServiceHelper botHelper;
        public MainVM()
        {
            botHelper = new BotServiceHelper();
        }
    }
}
