using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace Bot.ViewModel.Helpers
{
    public class BotServiceHelper
    {
        public Conversation _Conversation
        {
            get;
            set;
        }

        public BotServiceHelper()
        {
            CreateConversation();
        }

        private async void CreateConversation()
        {
            string endpoint = "/v3/directline/conversations";

            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://directline.botframework.com");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer BoKJEGd1SlE.aSssnudGFJhgBuZ7wXqyUdd8eNoDXjF2ISsm15HUlsw");

                var response = await client.PostAsync(endpoint, null);
                string json = await response.Content.ReadAsStringAsync();

                _Conversation = JsonConvert.DeserializeObject<Conversation>(json);
            }
        }

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
}
