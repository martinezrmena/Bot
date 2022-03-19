using System;
namespace Bot.Models
{
    public class ChatMessage
    {
        public string Id
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public bool IsIncoming
        {
            get;
            set;
        }
    }
}
