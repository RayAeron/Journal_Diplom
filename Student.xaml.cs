using Journal_Diplom.JournalTableAdapters;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Journal_Diplom
{
    /// <summary>
    /// Логика взаимодействия для Student.xaml
    /// </summary>
    public partial class Student : MetroWindow
    {
        Journal Journal;
        usersTableAdapter usersTableAdapter;
        Mark_VTableAdapter Mark_VTableAdapter;
        string trysi;

        public Student()
        {
            InitializeComponent();

            WebClient client = new WebClient();
            try
            {
                using (client.OpenRead("http://www.google.com"))
                {
                }
            }
            catch (WebException)
            {
                MessageBox.Show("Проверьте подлючение к интернету");
                this.Close();
                Process.GetCurrentProcess().Kill();
            }

            Journal = new Journal();
            usersTableAdapter = new usersTableAdapter();
            usersTableAdapter.Fill(Journal.users);

            Mark_VTableAdapter = new Mark_VTableAdapter();
            Mark_VTableAdapter.Fill(Journal.Mark_V);
            mark.ItemsSource = Journal.Mark_V.DefaultView;
            mark.CanUserDeleteRows = false;
            mark.CanUserAddRows = false;
            mark.IsReadOnly = true;

            surname_t.MaxLength = 50;
            name_t.MaxLength = 50;
            patronymic_t.MaxLength = 50;
            searh.MaxLength = 50;
        }

        private void mark_Focus(Canvas main_canv, Canvas mark_canve)
        {
            main_canv.Visibility = Visibility.Hidden;
            mark_canv.Visibility = Visibility.Visible;
        }
        private void main_mark_Focus(Canvas main_canv, Canvas mark_canv)
        {
            mark_canv.Visibility = Visibility.Hidden;
            main_canv.Visibility = Visibility.Visible;
        }
        private void person_Focus(Canvas main_canv, Canvas person_canv)
        {
            main_canv.Visibility = Visibility.Hidden;
            person_canv.Visibility = Visibility.Visible;
        }
        private void main_person_Focus(Canvas main_canv, Canvas person_canv)
        {
            person_canv.Visibility = Visibility.Hidden;
            main_canv.Visibility = Visibility.Visible;
        }
        private void back_l(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.Equals("Просмотр оценок"))
            {
                try
                {
                    mark_Focus(main_canv, mark_canv);
                    Title = "Оценки";
                    string trysi = Convert.ToString(login.Content);
                    Mark_VTableAdapter.FillBy(Journal.Mark_V, trysi);
                }
                catch
                {
                    MessageBox.Show("Проверьте соединение с интернетом");
                }
            }
            if (((Button)sender).Content.Equals("Назад"))
            {
                main_mark_Focus(main_canv, mark_canv);
                Title = "Панель студента";
            }
            if (((Button)sender).Content.Equals("Личный кабинет"))
            {
                person_Focus(main_canv, person_canv);
                Title = "Личный кабинет";
                trysi = Convert.ToString(login.Content);
                try
                {
                    usersTableAdapter.FillBy(Journal.users, Convert.ToString(login.Content));
                    if (!Journal.users.Rows.Count.Equals(0))
                    {
                        surname_s.Content = Journal.users.Rows[0].ItemArray[1].ToString();
                        name_s.Content = Journal.users.Rows[0].ItemArray[2].ToString();
                        patronymic_s.Content = Journal.users.Rows[0].ItemArray[3].ToString();
                    }
                }
                catch
                {
                    MessageBox.Show("Проверьте соединение с интернетом");
                }
            }
            if (((Button)sender).Content.Equals("Назад"))
            {
                main_person_Focus(main_canv, person_canv);
                Title = "Панель студента";
            }
        }

        private void search_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mark_VTableAdapter.FillBy1(Journal.Mark_V, searh.Text, Convert.ToString(login.Content));
            }
            catch
            {
                MessageBox.Show("Проверьте соединение с интернетом");
            }
        }

        private void surname_b_Click(object sender, RoutedEventArgs e)
        {
            if (surname_t.Text != "")
            {
                try
                {
                    usersTableAdapter.UpdateQuery1(surname_t.Text, trysi);
                    surname_s.Content = surname_t.Text;
                    surname_t.Text = null;
                }
                catch
                {
                    MessageBox.Show("Проверьте соединение с интернетом");
                }
            }
            else MessageBox.Show("Введите данные");
        }

        private void name_b_Click(object sender, RoutedEventArgs e)
        {
            if (name_t.Text != "")
            {
                try
                {
                    usersTableAdapter.UpdateQuery2(name_t.Text, trysi);
                    name_s.Content = name_t.Text;
                    name_t.Text = null;
                }
                catch
                {
                    MessageBox.Show("Проверьте соединение с интернетом");
                }
            }
            else MessageBox.Show("Введите данные");
        }

        private void patronymic_b_Click(object sender, RoutedEventArgs e)
        {
            if (patronymic_t.Text != "")
            {
                try
                {
                    usersTableAdapter.UpdateQuery3(patronymic_t.Text, trysi);
                    patronymic_s.Content = patronymic_t.Text;
                    patronymic_t.Text = null;
                }
                catch
                {
                    MessageBox.Show("Проверьте соединение с интернетом");
                }
            }
            else MessageBox.Show("Введите данные");
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }
    }
}
