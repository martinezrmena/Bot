using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Bot.Models;
using Bot.ViewModel.Helpers;
using Newtonsoft.Json;

namespace Bot.Services
{
    public class BotService
    {
        private string conversationId;
        private string token;
        private HttpClient _httpClient;
        public event EventHandler<BotResponseEventArgs> MessageReceived;

        public BotService()
        {
        }

        public async Task<bool> Setup()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://directline.botframework.com/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.BarerToken);
            var response = await _httpClient.PostAsync("/api/tokens/conversation", null);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync();

                token = JsonConvert.DeserializeObject<string>(result.Result);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await _httpClient.PostAsync("/api/conversations", null);
                if (response.IsSuccessStatusCode)
                {
                    var conversationInfo = await response.Content.ReadAsStringAsync();
                    conversationId = JsonConvert.DeserializeObject<Conversation>(conversationInfo).ConversationId;
                    return true;
                }
                return true;

            }
            return false;
        }

        public async Task<BotMessage> SendMessage(string message, string name = "user1")
        {
            var messageToSend = new BotMessage() { From = name, Text = message };
            var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
            var conversationUrl = "https://directline.botframework.com/api/conversations/" + conversationId + "/messages/";

            var response = await _httpClient.PostAsync(conversationUrl, contentPost);

            var messagesReceived = await _httpClient.GetAsync(conversationUrl);
            var messagesReceivedData = await messagesReceived.Content.ReadAsStringAsync();
            var messagesRoot = JsonConvert.DeserializeObject<BotMessageRoot>(messagesReceivedData);
            var messages = messagesRoot.Messages;

            var renewUrl = "https://directline.botframework.com/api/tokens/" + conversationId + "/renew/";
            response = await _httpClient.GetAsync(renewUrl);

            var args = new BotResponseEventArgs();
            args.BotMessages = messages;

            MessageReceived?.Invoke(this, args);

            return messages.LastOrDefault();
        }

    }
}
