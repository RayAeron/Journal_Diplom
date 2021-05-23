using Journal_Diplom.JournalTableAdapters;
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
    /// Логика взаимодействия для Kurator.xaml
    /// </summary>
    public partial class Kurator : MetroWindow
    {
        Journal Journal;
        Mark_kuratorTableAdapter Mark_kuratorTableAdapter;
        users2TableAdapter users2TableAdapter;
        usersTableAdapter usersTableAdapter;
        public Kurator()
        {
            InitializeComponent();

            

            Journal = new Journal();
            Mark_kuratorTableAdapter = new Mark_kuratorTableAdapter();
            Mark_kuratorTableAdapter.FillBy(Journal.Mark_kurator, MainWindow.group_kurator);
            group_grid.ItemsSource = Journal.Mark_kurator.DefaultView;
            group_grid.CanUserDeleteRows = false;
            group_grid.CanUserAddRows = false;
            group_grid.IsReadOnly = true;

            usersTableAdapter = new usersTableAdapter();
            usersTableAdapter.Fill(Journal.users);

            users2TableAdapter = new users2TableAdapter();
            users2TableAdapter.Fill(Journal.users2);
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }
    }
}
