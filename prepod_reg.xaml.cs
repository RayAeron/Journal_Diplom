using Journal_Diplom.JournalTableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Journal_Diplom
{
    /// <summary>
    /// Логика взаимодействия для prepod_reg.xaml
    /// </summary>
    public partial class prepod_reg : Page
    {
        Journal Journal;
        usersTableAdapter usersTableAdapter;
        groupTableAdapter groupTableAdapter;
        users2TableAdapter users2TableAdapter;
        string check = "yes";
        public prepod_reg()
        {
            InitializeComponent();

            Journal = new Journal();

            usersTableAdapter = new usersTableAdapter();
            usersTableAdapter.Fill(Journal.users);

            groupTableAdapter = new groupTableAdapter();
            groupTableAdapter.Fill(Journal.group);

            users2TableAdapter = new users2TableAdapter();
            users2TableAdapter.FillBy(Journal.users2, check);
            kurator_grid.ItemsSource = Journal.users2.DefaultView;
            kurator_grid.CanUserDeleteRows = false;
            kurator_grid.CanUserAddRows = false;
            kurator_grid.IsReadOnly = true;


            login_r.MaxLength = 50;
            pass_r.MaxLength = 50;
            surname.MaxLength = 50;
            name.MaxLength = 50;
            patronymic.MaxLength = 50;

            for (int i = 0; i < Journal.group.Rows.Count; i++)
            {
                string bd_group = Journal.group.Rows[i].ItemArray[1].ToString();
                group.Items.Add(bd_group);
            }
            
        }

        private void reg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (login_r.Text != "" && pass_r.Password != "")
                {
                    usersTableAdapter.FillBy(Journal.users, login_r.Text);
                    if (Journal.users.Rows.Count.Equals(0))
                    {
                        try
                        {
                            MailAddress from1 = new MailAddress("balance.emulation.card@gmail.com", "Register");
                            MailAddress to1 = new MailAddress(login_r.Text);
                            MailMessage m1 = new MailMessage(from1, to1);
                            usersTableAdapter.InsertQuery1(surname.Text, name.Text, patronymic.Text, login_r.Text, pass_r.Password, check);
                            m1.Subject = "Регистрация";
                            m1.Body = "Регистрация прошла успешно. Ваш логин:  " + login_r.Text + " | Ваш пароль: " + pass_r.Password + " |";
                            m1.IsBodyHtml = true;
                            SmtpClient smtp1 = new SmtpClient("smtp.gmail.com", 587);
                            smtp1.Credentials = new NetworkCredential("balance.emulation.card@gmail.com", "testbalance");
                            smtp1.EnableSsl = true;
                            smtp1.Send(m1);
                            error1.Content = "Регистрация прошла успешно";
                        }
                        catch
                        {
                            MessageBox.Show("Почта введена неправильно");
                        }
                    }
                    else error1.Content = "Логин существует";
                }
                else error1.Content = "Введите данные";
            }
            catch
            {
                MessageBox.Show("Проверьте подключение к интернету");
            }
            users2TableAdapter.FillBy(Journal.users2, check);
        }

        private void kurator_btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (group.Text != "")
            {
                users2TableAdapter.UpdateQuery1(group.Text, Convert.ToString(login.Content));
                users2TableAdapter.UpdateQuery("kurator", Convert.ToString(login.Content));
            }
            else MessageBox.Show("Выберите группу");
            kurator_btn_add.IsEnabled = false;
            users2TableAdapter.FillBy(Journal.users2, check);
        }

        private void permiss_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selecteDataRow = (DataRowView)kurator_grid.SelectedItem;
            if (selecteDataRow != null)
            {
                kurator_btn_add.IsEnabled = true;
                kurator_btn_del.IsEnabled = true;
                login.Content = selecteDataRow.Row.ItemArray[1].ToString();

            }
        }

        private void kurator_grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            if (propertyDescriptor.DisplayName == "is_staff")
            {
                e.Cancel = true;
            }
            //if (propertyDescriptor.DisplayName == "Password" || propertyDescriptor.DisplayName == "Роль")
            //{
            //    e.Cancel = true;
            //}
        }

        private void kurator_btn_del_Click(object sender, RoutedEventArgs e)
        {
            users2TableAdapter.UpdateQuery2(Convert.ToString(login.Content));
            users2TableAdapter.UpdateQuery3(Convert.ToString(login.Content));
            kurator_btn_add.IsEnabled = false;
            kurator_btn_del.IsEnabled = false;
            users2TableAdapter.FillBy(Journal.users2, check);
        }
    }
}

    

