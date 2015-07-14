using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace LorealOptimiseGui
{
    /// <summary>
    /// Interaction logic for LongTaskProgressWindow.xaml
    /// </summary>
    public partial class LongTaskProgressWindow : Window
    {
        public LongTaskProgressWindow()
        {
            InitializeComponent();

            this.Cursor = Cursors.Wait;
            this.txtProg.Cursor = Cursors.Wait;

            Reference = 1;
        }

        private static Action EmptyDelegate = delegate() { };

        void Refresh(UIElement uiElement)
        {
            // Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }

        public string ProgressMessage
        {
            set
            {
                txtProg.Text = value + Environment.NewLine + txtProg.Text;
                Refresh(this.txtProg);
            }
        }

        public void ClearMessage()
        {
            txtProg.Text = string.Empty;
        }

        public int Reference;
      
    }
}
