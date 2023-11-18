using Messenger.Data.DAL;
using Messenger.Data.Entity;
using Messenger.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messenger.Main
{
    /// <summary>
    /// Логика взаимодействия для ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        Chats CurrentChat;
        ChatPageViewModel chatPageViewModel;

        public ChatPage(User user)
        {
            InitializeComponent();
            chatPageViewModel = new ChatPageViewModel(user);
        }

        private CancellationTokenSource cancellationToken = new();
        private async void Sync()
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            await Task.Delay(2000);
            lvDataBinding.ItemsSource = ContentDal.GetContentList(CurrentChat.Id).Result;
            Sync();
        }

        public void ReSelectChat(Chats chat)
        {
            cancellationToken.Cancel();
            cancellationToken = new CancellationTokenSource();
            Sync();
            chatPageViewModel.UpdateListMessage(chat);
            CurrentChat = chat;
            lvDataBinding.ItemsSource = chatPageViewModel.ContentsList;
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {   
            if (messageTextBox.Text.ToString().IsNullOrEmpty()) { return; }
            if(CurrentChat == null) { return; }

            chatPageViewModel.AddNewMessage(0, CurrentChat.Id, messageTextBox.Text.ToString(), "Message");
            ReSelectChat(CurrentChat);
            messageTextBox.Text = "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged?.Invoke(this, e);
        }
    }

    
}
