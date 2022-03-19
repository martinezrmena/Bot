using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bot.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bot.ViewModel.Helpers
{
    [Obsolete]
    public class BotServiceHelper
    {
        public Conversation _Conversation
        {
            get;
            set;
        }

        public event EventHandler<BotResponseEventArgs> MessageReceived;

        public BotServiceHelper()
        {
            CreateConversation();
        }

        private async void CreateConversation()
        {
            string endpoint = "/v3/directline/conversations";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://directline.botframework.com");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer BoKJEGd1SlE.aSssnudGFJhgBuZ7wXqyUdd8eNoDXjF2ISsm15HUlsw");

                var response = await client.PostAsync(endpoint, null);
                string json = await response.Content.ReadAsStringAsync();

                _Conversation = JsonConvert.DeserializeObject<Conversation>(json);
            }

            ReadMessage();
        }

        public async void SendActivity(string message)
        {
            string endpoint = $"/v3/directline/conversations/{_Conversation.ConversationId}/activities";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://directline.botframework.com");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer BoKJEGd1SlE.aSssnudGFJhgBuZ7wXqyUdd8eNoDXjF2ISsm15HUlsw");

                Activity activity = new Activity
                {
                    From = new ChannelAccount
                    {
                        Id = "user1"
                    },
                    Text = message,
                    Type = "message"
                };

                string jsonContent = JsonConvert.SerializeObject(activity);
                var buffer = Encoding.UTF8.GetBytes(jsonContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(endpoint, byteContent);
                string json = await response.Content.ReadAsStringAsync();

                var obj = JObject.Parse(json);
                string id = (string)obj.SelectToken("id");
            }
        }

        public async void ReadMessage()
        {
            var client = new ClientWebSocket();
            var cts = new CancellationTokenSource();

            await client.ConnectAsync(new Uri(_Conversation.StreamUrl), cts.Token);

            await Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    WebSocketReceiveResult result;
                    var message = new ArraySegment<byte>(new byte[4096]);
                    do
                    {
                        result = await client.ReceiveAsync(message, cts.Token);
                        try
                        {
                            if (result.MessageType != WebSocketMessageType.Text)
                                break;
                            var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                            string messageJSON = Encoding.UTF8.GetString(messageBytes);

                            BotsResponse botsResponse = JsonConvert.DeserializeObject<BotsResponse>(messageJSON);

                            var args = new BotResponseEventArgs();
                            //args.Activities = botsResponse.Activities;

                            MessageReceived?.Invoke(this, args);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    while (!result.EndOfMessage);
                }
            }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public class BotsResponse
        {
            public string Watermark
            {
                get;
                set;
            }

            public List<Activity> Activities
            {
                get;
                set;
            }
        }
    }
}
