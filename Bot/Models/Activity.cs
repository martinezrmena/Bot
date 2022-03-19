using System;

namespace Bot.Models
{
    public class Activity
    {
        public ChannelAccount From
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }
    }
}
