using System;
namespace Bot.Models
{
    public class Conversation
    {
        public string ConversationId
        {
            get;
            set;
        }

        public string Token
        {
            get;
            set;
        }

        public string StreamUrl
        {
            get;
            set;
        }

        public string ReferenceGrammarId
        {
            get;
            set;
        }

        public int Expires_in
        {
            get;
            set;
        }
    }
}
