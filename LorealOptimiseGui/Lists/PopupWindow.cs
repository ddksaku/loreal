using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LorealOptimiseGui.Lists
{
    public class PopupWindow : Window
    {
        private Grid grid = new Grid();

        public EventHandler CloseWindowEvent;

        public PopupWindow(string title)
            : base()
        {
            this.Title = "Loreal - " + title;
            this.Content = grid;
            this.Width = 900;
            this.Height = 800;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.CloseWindowEvent += new EventHandler(PopupWindow_CloseWindowEvent);
        }

        void PopupWindow_CloseWindowEvent(object sender, EventArgs e)
        {
            this.Close();
        }

        public void AddControl(Control ctrl)
        {
            grid.Children.Add(ctrl);
        }

    }

}
