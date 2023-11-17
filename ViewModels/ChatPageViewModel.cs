using Messenger.Data.DAL;
using Messenger.Data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger.ViewModels
{
    
    public class ChatPageViewModel : INotifyPropertyChanged
    {
        private User user { get; set; }
        public List<Content> contentsList;
        public List<Content> ContentsList
        {
            get => contentsList;
            set
            {
                contentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ContentsList)));
            }
        }
        private int chat_id;
        public int ChatId
        {
            get => chat_id;
            set
            {
                chat_id = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ChatId)));
            }
        }

        public ChatPageViewModel(User Myuser) 
        {
            user = Myuser;
            ContentsList = new();

        }

        public async void AddNewMessage(int id, int currentIdChat, String context, String type)
        {
            Content content = new Content()
            {
                Id = id,
                Id_chats = currentIdChat,
                Contents = context,
                TypeContent = type,
                CreateDt = DateTime.Now,
                User = user
            };
            await ContentDal.Add(content);
        }

        public void UpdateListMessage(Chats chat)
        {
            ContentsList?.Clear();
            ContentsList = ContentDal.GetContentList(chat.Id).Result;
            UpdateMessage();

        }

        public async void UpdateMessage()
        {
            foreach (Content content in ContentsList)
            {
                await ContentDal.LoadUserInfo(content);
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged?.Invoke(this, e);
        }
    }
}
