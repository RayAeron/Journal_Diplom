using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Journal_Diplom
{
    /// <summary>
    /// Логика взаимодействия для select_kurator.xaml
    /// </summary>
    public partial class select_kurator : MetroWindow
    {
        public select_kurator()
        {
            InitializeComponent();
        }

        private void teacher_window_Click(object sender, RoutedEventArgs e)
        {
            Teacher Teacher = new Teacher();
            Teacher.login.Content = login.Content;
            Teacher.Show();
            this.Close();
        }

        private void kurator_window_Click(object sender, RoutedEventArgs e)
        {
            Kurator Kurator = new Kurator();
            Kurator.login.Content = login.Content;
            Kurator.Show();
            this.Close();
        }
    }
}
