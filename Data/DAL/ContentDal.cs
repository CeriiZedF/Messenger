using Messenger.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger.Data.DAL
{
    public class ContentDal
    {
        private static DataContext dataContext = ((App)Application.Current).dataContext;

        public static async Task Add(Content content)
        {
            dataContext.Add(content);
            dataContext.SaveChanges();
        }

        public static async Task<List<Content>> GetContentList(int id_chats)
        {
            var temp = dataContext.contents.Where(c => c.Id_chats == id_chats).ToList();
            return temp;
        }

        public static async Task LoadUserInfo(Content content)
        {
            dataContext.Entry(content).Reference("User").Load();
        }

        
    }
}
