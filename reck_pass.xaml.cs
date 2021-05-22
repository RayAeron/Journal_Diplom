using Journal_Diplom.JournalTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
using MahApps.Metro.Controls;

namespace Journal_Diplom
{
    /// <summary>
    /// Логика взаимодействия для reck_pass.xaml
    /// </summary>
    public partial class reck_pass : MetroWindow
    {
        Journal Journal;
        usersTableAdapter usersTableAdapter;

        static Random random = new Random();
        int check_kod = random.Next(000000, 999999);
        public reck_pass()
        {
            InitializeComponent();

            Journal = new Journal();
            usersTableAdapter = new usersTableAdapter();
            usersTableAdapter.Fill(Journal.users);

            edit_pass.MaxLength = 20;
            rep_edit_pass.MaxLength = 20;
            kod.MaxLength = 6;
            r_pass.MaxLength = 50;
        }
        private void Back_Focus_Focus(Canvas email, Canvas recovery_kod)
        {
            email.Visibility = Visibility.Hidden;
            recovery_kod.Visibility = Visibility.Visible;
        }
        private void Back_Focus(Canvas recovery_kod, Canvas edit)
        {
            recovery_kod.Visibility = Visibility.Hidden;
            edit.Visibility = Visibility.Visible;
        }

        private void back_l(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.Equals("Восстановить"))
            {
                Back_Focus_Focus(email, recovery_kod);
            }
            if (((Button)sender).Content.Equals("Проверить"))
            {
                Back_Focus(recovery_kod, edit);
            }
        }
        /// <summary>
        /// Восстановление пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rec_pass_Click(object sender, RoutedEventArgs e)
        {
            usersTableAdapter.FillBy2(Journal.users, r_pass.Text);
            if (!Journal.users.Rows.Count.Equals(0))
            {
                string check_kod_str = Convert.ToString(check_kod);
                try
                {
                    MailAddress from1 = new MailAddress("balance.emulation.card@gmail.com", "Recovery code");
                    MailAddress to1 = new MailAddress(r_pass.Text);
                    MailMessage m1 = new MailMessage(from1, to1);
                    m1.Subject = "Код подтверждения";
                    m1.Body = "Для того чтобы восстановить пароль введите проверочный код: " + check_kod_str;
                    m1.IsBodyHtml = true;
                    SmtpClient smtp1 = new SmtpClient("smtp.gmail.com", 587);
                    smtp1.Credentials = new NetworkCredential("balance.emulation.card@gmail.com", "testbalance");
                    smtp1.EnableSsl = true;
                    smtp1.Send(m1);
                    Back_Focus_Focus(email, recovery_kod);
                }
                catch
                {
                    error.Content = "Введите пожалуйста правильный адрес электронной почты";
                }
            }
            else error.Content = "Пользователя с такой почтой не существует";
        }
        /// <summary>
        /// Проверка кода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chek_kod_Click(object sender, RoutedEventArgs e)
        {
            string kod_txt = Convert.ToString(kod.Text);
            string check_kod_str = Convert.ToString(check_kod);
            if (kod.Text != "")
            {
                if (check_kod_str == kod_txt)
                {
                    Back_Focus(recovery_kod, edit);
                }
                else error1.Content = "Введён неправильный код";
            }
            else error1.Content = "Введите данные";

        }
        /// <summary>
        /// Смена пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void finaly_edit_pass_Click(object sender, RoutedEventArgs e)
        {
            if (edit_pass.Password == rep_edit_pass.Password)
            {
                usersTableAdapter.FillBy2(Journal.users, r_pass.Text);
                if (!Journal.users.Rows.Count.Equals(0) && edit_pass.Password != "" && rep_edit_pass.Password != "")
                {
                    usersTableAdapter.UpdateQuery(rep_edit_pass.Password, r_pass.Text);
                    this.Close();
                }
                else error2.Content = "Введите данные";
            }
            else error2.Content = "Пароли не совпадают";
        }

        private void kod_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789 ,".IndexOf(e.Text) < 0;
        }

        private void kod_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}