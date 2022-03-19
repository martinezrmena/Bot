using System;
using Xamarin.Forms;
using static Bot.ViewModel.MainVM;

namespace Bot.View.UserControls
{
    public class ChatViewCellTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate incomingDataTemplate;
        private readonly DataTemplate outgoingDataTemplate;

        public ChatViewCellTemplateSelector()
        {
            incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var chatMessage = item as ChatMessage;
            if(chatMessage != null)
            {
                return chatMessage.IsIncoming ? incomingDataTemplate : outgoingDataTemplate;
            }
            return null;
        }
    }
}
