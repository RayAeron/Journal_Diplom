using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Journal_Diplom.JournalTableAdapters;
using MahApps.Metro.Controls;

namespace Journal_Diplom
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Journal Journal;
        usersTableAdapter usersTableAdapter;
        groupTableAdapter groupTableAdapter;
        string check = "no";

        public MainWindow()
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
                MessageBox.Show("Проверьте подключение к интернету");
                this.Close();
                Process.GetCurrentProcess().Kill();
            }

            Title = "Авторизация";
            Journal = new Journal();
            usersTableAdapter = new usersTableAdapter();
            usersTableAdapter.Fill(Journal.users);

            groupTableAdapter = new groupTableAdapter();
            groupTableAdapter.Fill(Journal.group);

            for (int i = 0; i < Journal.group.Rows.Count; i++)
            {
                string bd_group = Journal.group.Rows[i].ItemArray[1].ToString();
                group.Items.Add(bd_group);
            }

            login_l.MaxLength = 50;
            login_r.MaxLength = 50;
            pass_l.MaxLength = 50;
            pass_r.MaxLength = 50;
            surname.MaxLength = 50;
            name.MaxLength = 50;
            patronymic.MaxLength = 50;


        }


        private void pass_recovery_MouseUp(object sender, MouseButtonEventArgs e)
        {
            reck_pass reck_pass = new reck_pass();
            reck_pass.Show();
        }
        private void Back_Focus(Canvas login_canv, Canvas reg_canv)
        {
            login_canv.Visibility = Visibility.Hidden;
            reg_canv.Visibility = Visibility.Visible;
            login_l.Clear();
            pass_l.Clear();
            login_r.Clear();
            pass_r.Clear();
            surname.Clear();
            name.Clear();
            patronymic.Clear();
            group.SelectedIndex = -1;
            error.Content = null;
            error1.Content = null;
        }

        private void back_l(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.Equals("Назад"))
            {
                Back_Focus(reg_canv, login_canv);
                Title = "Авторизация";
            }
            else
            {
                Back_Focus(login_canv, reg_canv);
                Title = "Регистрация";
            }
        }

        private void reg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string group_iner = "";
                groupTableAdapter.FillBy(Journal.group, group.Text);
                if (!Journal.group.Rows.Count.Equals(0))
                {
                    for (int i = 0; i < Journal.group.Rows.Count; i++)
                    {
                        string grop_id = Convert.ToString(Journal.group.Rows[i]["id_group"]);
                        group_iner = grop_id;
                    }
                }
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
                            if (check == "yes")
                            {
                                usersTableAdapter.InsertQuery1(surname.Text, name.Text, patronymic.Text, login_r.Text, pass_r.Password, check);
                            }
                            else
                            {
                                usersTableAdapter.InsertQuery(surname.Text, name.Text, patronymic.Text, login_r.Text, pass_r.Password, check, Convert.ToInt32(group_iner));
                            }
                            m1.Subject = "Регистрация";
                            m1.Body = "Регистрация прошла успешно. Ваш логин:  " + login_r.Text + " | Ваш пароль: " + pass_r.Password + " |";
                            m1.IsBodyHtml = true;
                            SmtpClient smtp1 = new SmtpClient("smtp.gmail.com", 587);
                            smtp1.Credentials = new NetworkCredential("balance.emulation.card@gmail.com", "testbalance");
                            smtp1.EnableSsl = true;
                            smtp1.Send(m1);
                            Back_Focus(reg_canv, login_canv);
                            error.Content = "Регистрация прошла успешно";
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
            
        }

        private void vhod_Click(object sender, RoutedEventArgs e)
        {
            if (login_l.Text != "" && pass_l.Password != "")
            {
                //try
                //{
                    usersTableAdapter.FillBy1(Journal.users, login_l.Text, pass_l.Password);
                    if (!Journal.users.Rows.Count.Equals(0))
                    {
                        usersTableAdapter.FillBy2(Journal.users, login_l.Text);
                        if (!Journal.users.Rows.Count.Equals(0))
                        {
                            string permission = Convert.ToString(Journal.users.Rows[0]["is_staff"]);
                            switch (permission)
                            {
                                case "yes":
                                    Teacher Teacher = new Teacher();
                                    Teacher.login.Content = login_l.Text;
                                    Teacher.Show();
                                    this.Close();
                                    break;

                                case "no":
                                    Student Student = new Student();
                                    Student.login.Content = login_l.Text;
                                    Student.Show();
                                    this.Close();
                                    break;

                                case "adm":
                                    Admin Admin = new Admin();
                                    Admin.Show();
                                    this.Close();
                                    break;
                            }
                        }
                    }
                    else error.Content = "Логин или пароль не совпадают";
                //}
                //catch
                //{
                //    MessageBox.Show("Проверьте подключение к интернету");
                //}
            }
            else error.Content = "Введите данные";

        }

        private void check_staff(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            check = radioButton.Name;
            if (check == "yes")
            {
                group.IsEnabled = false;
                group.Text = "";
            }
            else group.IsEnabled = true;
        }
        private void inf_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        int theme = 0;
        private void n_d_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (theme == 0)
            {
                ImageSourceConverter imgs = new ImageSourceConverter();
                n_d.SetValue(Image.SourceProperty, imgs.ConvertFromString(@"n.png"));
                theme++;
                var bc = new BrushConverter();
                login_canv.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
                login_l.Background = (Brush)bc.ConvertFrom("#FFC9CACF");
                login_l.Foreground = (Brush)bc.ConvertFrom("#FF000000");
                pass_l.Background = (Brush)bc.ConvertFrom("#FFC9CACF");
                pass_l.Foreground = (Brush)bc.ConvertFrom("#FF000000");
            }
            else
            {
                ImageSourceConverter imgs = new ImageSourceConverter();
                n_d.SetValue(Image.SourceProperty, imgs.ConvertFromString(@"d.png"));
                theme--;
                var bc = new BrushConverter();
                login_canv.Background = (Brush)bc.ConvertFrom("#FF252525");
                login_l.Background = (Brush)bc.ConvertFrom("#FF38393E");
                login_l.Foreground = (Brush)bc.ConvertFrom("#FFE6EAFE");
                pass_l.Background = (Brush)bc.ConvertFrom("#FF38393E");
                pass_l.Foreground = (Brush)bc.ConvertFrom("#FFE6EAFE");
            }
        }
        int theme2 = 0;
        private void n_d2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (theme2 == 0)
            {
                ImageSourceConverter imgs = new ImageSourceConverter();
                n_d2.SetValue(Image.SourceProperty, imgs.ConvertFromString(@"n.png"));
                theme2++;
                var bc = new BrushConverter();
            }
            else
            {
                ImageSourceConverter imgs = new ImageSourceConverter();
                n_d2.SetValue(Image.SourceProperty, imgs.ConvertFromString(@"d.png"));
                theme2--;
                var bc = new BrushConverter();

            }
        }

        private void login_r_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}

