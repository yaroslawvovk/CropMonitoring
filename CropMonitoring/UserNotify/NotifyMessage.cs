using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CropMonitoring.UserNotify
{
    class NotifyMessage : INotifyUser
    {
        public void Message(string message)
        {
            MessageBox.Show(message);
        }
    }
}
