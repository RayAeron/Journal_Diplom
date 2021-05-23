using Journal_Diplom.JournalTableAdapters;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : MetroWindow
    {
        Journal Journal;
        usersTableAdapter usersTableAdapter;
        users1TableAdapter users1TableAdapter;
        markTableAdapter markTableAdapter;
        Mark_VTableAdapter Mark_VTableAdapter;
        disciplineTableAdapter disciplineTableAdapter;
        groupTableAdapter groupTableAdapter;
        group1TableAdapter group1TableAdapter;
        string check = "add";
        public Admin()
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

            //frame.Navigate(new Discipline_Frame());

            Journal = new Journal();
            usersTableAdapter = new usersTableAdapter();
            usersTableAdapter.Fill(Journal.users);

            users1TableAdapter = new users1TableAdapter();
            users1TableAdapter.Fill(Journal.users1);
            permiss_grid.ItemsSource = Journal.users1.DefaultView;
            permiss_grid.CanUserDeleteRows = false;
            permiss_grid.CanUserAddRows = false;
            permiss_grid.IsReadOnly = true;

            groupTableAdapter = new groupTableAdapter();
            groupTableAdapter.Fill(Journal.group);
            group_grid.ItemsSource = Journal.group.DefaultView;
            group_grid.CanUserDeleteRows = false;
            group_grid.CanUserAddRows = false;
            group_grid.IsReadOnly = true;

            group1TableAdapter = new group1TableAdapter();
            group1TableAdapter.Fill(Journal.group1);

            Mark_VTableAdapter = new Mark_VTableAdapter();
            Mark_VTableAdapter.Fill(Journal.Mark_V);
            mark.ItemsSource = Journal.Mark_V.DefaultView;
            mark.CanUserDeleteRows = false;
            mark.CanUserAddRows = false;
            mark.IsReadOnly = true;

            markTableAdapter = new markTableAdapter();
            markTableAdapter.Fill(Journal.mark);

            disciplineTableAdapter = new disciplineTableAdapter();
            disciplineTableAdapter.Fill(Journal.discipline);
            for (int i = 0; i < Journal.users.Rows.Count; i++)
            {
                string student_tyty = Journal.users.Rows[i].ItemArray[4].ToString();
                if (Journal.users.Rows[i].ItemArray[6].ToString() == "no")
                {
                    mark_student.Items.Add(student_tyty);
                }
            }

            for (int i = 0; i < Journal.group.Rows.Count; i++)
            {
                string bd_group = Journal.group.Rows[i].ItemArray[1].ToString();
                group_search.Items.Add(bd_group);

            }
            for (int i = 0; i < Journal.group.Rows.Count; i++)
            {
                string bd_group = Journal.group.Rows[i].ItemArray[1].ToString();
                group_s.Items.Add(bd_group);
                group_cmb_permiss.Items.Add(bd_group);
            }

            for (int i = 0; i < Journal.discipline.Rows.Count; i++)
            {
                string bd_discipline = Journal.discipline.Rows[i].ItemArray[1].ToString();
                mark_d.Items.Add(bd_discipline);
            }

            group_name.MaxLength = 11;
            name.MaxLength = 100;
            searh.MaxLength = 50;
            searh_student.MaxLength = 50;
        }

        private void group_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string group_iner = "";
                group1TableAdapter.FillBy(Journal.group1, group_search.Text);
                if (!Journal.group.Rows.Count.Equals(0))
                {
                    for (int i = 0; i < Journal.group1.Rows.Count; i++)
                    {
                        string grop_id = Convert.ToString(Journal.group1.Rows[i]["id_group"]);
                        group_iner = grop_id;
                    }
                }
                //Mark_VTableAdapter.FillBy2(Journal.Mark_V, group_search.Text);

                mark_student.Items.Clear();
                mark_d.Items.Clear();
                try
                {
                    usersTableAdapter.FillBy3(Journal.users, int.Parse(group_iner));
                    for (int i = 0; i < Journal.users.Rows.Count; i++)
                    {
                        mark_student.Items.Add(Journal.users.Rows[i]["Email"]);
                    }
                }
                catch
                {
                    MessageBox.Show("Выберите студента");
                }
                try
                {
                    disciplineTableAdapter.FillBy1(Journal.discipline, int.Parse(group_iner));
                    for (int i = 0; i < Journal.discipline.Rows.Count; i++)
                    {
                        mark_d.Items.Add(Journal.discipline.Rows[i]["Name_discipline"]);
                    }
                }
                catch
                {
                    MessageBox.Show("Выберите группу");
                }
            }
            catch
            {
                MessageBox.Show("Проверьте соединение с интернетом");
            }
        }


        private void search_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (searh.Text == "")
                {
                    Mark_VTableAdapter.Fill(Journal.Mark_V);
                }
                else
                {
                    Mark_VTableAdapter.FillBy(Journal.Mark_V, searh.Text);
                }
            }
            catch
            {
                MessageBox.Show("Проверьте соединение с интернетом");
            }
        }


        private void mark_ad_Click(object sender, RoutedEventArgs e)
        {

            if (((Button)sender).Content.Equals("Добавить оценку"))
            {

                mark_n.Text = mark_select.Text;
                try
                {
                    string users_iner = "";
                    usersTableAdapter.FillBy(Journal.users, mark_student.Text);
                    if (!Journal.users.Rows.Count.Equals(0))
                    {
                        for (int i = 0; i < Journal.users.Rows.Count; i++)
                        {
                            string users_id = Convert.ToString(Journal.users.Rows[i]["id_user"]);
                            users_iner = users_id;
                        }
                    }

                    string discipline_iner = "";
                    disciplineTableAdapter.FillBy(Journal.discipline, mark_d.Text);
                    if (!Journal.discipline.Rows.Count.Equals(0))
                    {
                        for (int i = 0; i < Journal.discipline.Rows.Count; i++)
                        {
                            string discipline_id = Convert.ToString(Journal.discipline.Rows[i]["id_discipline"]);
                            discipline_iner = discipline_id;
                        }
                    }
                    markTableAdapter.FillBy1(Journal.mark, mark_date.Text, Convert.ToInt32(discipline_iner), Convert.ToInt32(users_iner));
                    if (Journal.mark.Rows.Count.Equals(0))
                    {
                        if (mark_d.Text != "" && mark_student.Text != "" && mark_date.Text != "" && mark_n.Text != "")
                        {
                            markTableAdapter.InsertQuery(mark_n.Text, mark_date.Text, Convert.ToInt32(discipline_iner), Convert.ToInt32(users_iner));
                            Mark_VTableAdapter.Fill(Journal.Mark_V);
                            mark_d.Text = "";
                            mark_student.Text = "";
                            mark_date.Text = "";
                            mark_n.Text = "";
                            date_picker.Text = "";
                            mark_select.Text = "";
                        }
                        else MessageBox.Show("Введите данные");
                    }
                    else MessageBox.Show("Нельзя добавить больше одной оценки в день по одной дисциплине!!!");
                }
                catch
                {
                    MessageBox.Show("Проверьте соединение с интернетом");
                }
            }

            if (((Button)sender).Content.Equals("Изменить оценку"))
            {
                try
                {
                    string users_iner = "";
                    usersTableAdapter.FillBy(Journal.users, mark_student.Text);
                    if (!Journal.users.Rows.Count.Equals(0))
                    {
                        for (int i = 0; i < Journal.users.Rows.Count; i++)
                        {
                            string users_id = Convert.ToString(Journal.users.Rows[i]["id_user"]);
                            users_iner = users_id;
                        }
                    }
                    string discipline_iner = "";
                    disciplineTableAdapter.FillBy(Journal.discipline, mark_d.Text);
                    if (!Journal.discipline.Rows.Count.Equals(0))
                    {
                        for (int i = 0; i < Journal.discipline.Rows.Count; i++)
                        {
                            string discipline_id = Convert.ToString(Journal.discipline.Rows[i]["id_discipline"]);
                            discipline_iner = discipline_id;
                        }
                    }
                    //
                    string id_mrk = Convert.ToString(id_mrk_lbl.Content);
                    //if (mark_select.Text != "")
                    {
                        markTableAdapter.UpdateQuery(mark_select.Text, Convert.ToInt32(id_mrk));
                        mark_select.Text = null;
                        Mark_VTableAdapter.Fill(Journal.Mark_V);
                    }
                    //else MessageBox.Show("Введите данные");
                    //
                    //if (date_picker.Text != "" && mark_date.Text != "")
                    {
                        markTableAdapter.UpdateQuery1(date_picker.Text, Convert.ToInt32(id_mrk));
                        date_picker.Text = null;
                        mark_date.Text = null;
                        Mark_VTableAdapter.Fill(Journal.Mark_V);
                    }
                    //else MessageBox.Show("Введите данные");
                    //
                    //if (mark_d.Text != "")
                    {
                        markTableAdapter.UpdateQuery2(Convert.ToInt32(discipline_iner), Convert.ToInt32(id_mrk));
                        mark_d.Text = null;
                        Mark_VTableAdapter.Fill(Journal.Mark_V);
                    }
                    //else MessageBox.Show("Введите данные");
                    //
                    //if (mark_student.Text != "")
                    {
                        markTableAdapter.UpdateQuery3(Convert.ToInt32(users_iner), Convert.ToInt32(id_mrk));
                        mark_student.Text = null;
                        Mark_VTableAdapter.Fill(Journal.Mark_V);
                    }
                    //else MessageBox.Show("Введите данные");

                }
                catch
                {
                    MessageBox.Show("Проверьте соединение с интернетом");
                }
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }

        private void add_group_btn_Click(object sender, RoutedEventArgs e)
        {

            if (((Button)sender).Content.Equals("Добавить группу"))
            {
                try
                {
                    groupTableAdapter.FillBy2(Journal.group, group_name.Text);
                    if (!Journal.mark.Rows.Count.Equals(0))
                    {
                        if (group_name.Text != "" && name.Text != "" && code.Text != "")
                        {
                            groupTableAdapter.InsertQuery(group_name.Text, code.Text, name.Text);
                            error.Content = null;
                        }
                        else MessageBox.Show("Введите данные");
                    }
                    else MessageBox.Show("Нельзя добавлять одинаковые группы!!!");
                }
                catch
                {
                    MessageBox.Show("Проверьте соединение с интернетом");
                }
            }
            if (((Button)sender).Content.Equals("Изменить группу"))
            {
                try
                {
                    string group = Convert.ToString(group_id.Content);
                    if (group_name.Text != "")
                    {
                        groupTableAdapter.UpdateQuery(group_name.Text, Convert.ToInt32(group));
                    }
                    if (code.Text != "")
                    {
                        groupTableAdapter.UpdateQuery1(code.Text, Convert.ToInt32(group));
                    }
                    if (name.Text != "")
                    {
                        groupTableAdapter.UpdateQuery2(name.Text, Convert.ToInt32(group));
                    }
                }
                catch
                {
                    MessageBox.Show("Проверьте соединение с интернетом");
                }

            }
            try
            {
                groupTableAdapter.Fill(Journal.group);
                group_s.Items.Clear();
                for (int i = 0; i < Journal.group.Rows.Count; i++)
                {
                    string bd_group = Journal.group.Rows[i].ItemArray[1].ToString();
                    group_s.Items.Add(bd_group);
                }
            }
            catch
            {
                MessageBox.Show("Проверьте соединение с интернетом");
            }
            group_name.Text = "";
            name.Text = "";
            code.Text = "";

        }

        private void check_mark(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            check = radioButton.Name;
            if (check == "edit")
            {
                mark_ad.Content = "Изменить оценку";
            }
            else
            {
                mark_ad.Content = "Добавить оценку";
            }
        }

        private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            mark_date.Text = Convert.ToString(date_picker.Text);
        }

        private void group_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selecteDataRow = (DataRowView)group_grid.SelectedItem;
            if (selecteDataRow != null)
            {
                group_id.Content = selecteDataRow.Row.ItemArray[0].ToString();
                group_name.Text = selecteDataRow.Row.ItemArray[1].ToString();
                code.Text = selecteDataRow.Row.ItemArray[2].ToString();
                name.Text = selecteDataRow.Row.ItemArray[3].ToString();
            }
        }

        private void group_b_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (group_s.Text == "")
                {
                    groupTableAdapter.Fill(Journal.group);
                }
                else
                {
                    groupTableAdapter.FillBy(Journal.group, group_s.Text);
                }
                
            }
            catch
            {
                MessageBox.Show("Проверьте соединение с интернетом");
            }
        }

        private void canv_next(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.Equals("Оценки"))
            {
                canv_mark_add_edit.Visibility = Visibility.Visible;
                canv_group_add_edit.Visibility = Visibility.Hidden;
                permiss_edit.Visibility = Visibility.Hidden;
                frame.Visibility = Visibility.Hidden;
                frame_prepod.Visibility = Visibility.Hidden;
                disciplineTableAdapter.Fill(Journal.discipline);
                Mark_VTableAdapter.Fill(Journal.Mark_V);
                mark_d.Items.Clear();
                for (int i = 0; i < Journal.discipline.Rows.Count; i++)
                {
                    string bd_discipline = Journal.discipline.Rows[i].ItemArray[1].ToString();
                    mark_d.Items.Add(bd_discipline);
                }
                ///
                group_name.Text = "";
                name.Text = "";
                code.Text = "";
                group_s.Text = "";
                permiss_combo.Text = "";
                searh_student.Text = "";
                group_cmb_permiss.Text = null;
                group_btn_permiss.IsEnabled = false;
                del_user_btn.IsEnabled = false;
                string pageName = null;
                while (pageName == "Discipline_Frame.xaml")
                {
                    frame.NavigationService.RemoveBackEntry();
                    JournalEntry entry = frame.RemoveBackEntry();
                    pageName = System.IO.Path.GetFileName(entry.Source.ToString());
                }
                string kuratorPage = null;
                while (kuratorPage == "prepod_reg.xaml")
                {
                    frame_prepod.NavigationService.RemoveBackEntry();
                    JournalEntry entry = frame_prepod.RemoveBackEntry();
                    kuratorPage = System.IO.Path.GetFileName(entry.Source.ToString());
                }
            }
            if (((Button)sender).Content.Equals("Группы"))
            {
                canv_group_add_edit.Visibility = Visibility.Visible;
                canv_mark_add_edit.Visibility = Visibility.Hidden;
                permiss_edit.Visibility = Visibility.Hidden;
                frame.Visibility = Visibility.Hidden;
                frame_prepod.Visibility = Visibility.Hidden;
                ///
                searh.Text = "";
                mark_d.Text = "";
                mark_student.Text = "";
                mark_date.Text = "";
                mark_n.Text = "";
                date_picker.Text = "";
                mark_select.Text = "";
                permiss_combo.Text = "";
                searh_student.Text = "";
                group_cmb_permiss.Text = null;
                group_btn_permiss.IsEnabled = false;
                del_user_btn.IsEnabled = false;
                string pageName = null;
                while (pageName == "Discipline_Frame.xaml")
                {
                    frame.NavigationService.RemoveBackEntry();
                    JournalEntry entry = frame.RemoveBackEntry();
                    pageName = System.IO.Path.GetFileName(entry.Source.ToString());
                }
                string kuratorPage = null;
                while (kuratorPage == "prepod_reg.xaml")
                {
                    frame_prepod.NavigationService.RemoveBackEntry();
                    JournalEntry entry = frame_prepod.RemoveBackEntry();
                    kuratorPage = System.IO.Path.GetFileName(entry.Source.ToString());
                }
            }
            if (((Button)sender).Content.Equals("Права пользователей"))
            {
                permiss_edit.Visibility = Visibility.Visible;
                canv_mark_add_edit.Visibility = Visibility.Hidden;
                canv_group_add_edit.Visibility = Visibility.Hidden;
                frame.Visibility = Visibility.Hidden;
                frame_prepod.Visibility = Visibility.Hidden;
                ///
                searh.Text = "";
                group_name.Text = "";
                name.Text = "";
                code.Text = "";
                group_s.Text = "";
                mark_d.Text = "";
                mark_student.Text = "";
                mark_date.Text = "";
                mark_n.Text = "";
                date_picker.Text = "";
                mark_select.Text = "";
                group_cmb_permiss.Text = null;
                group_btn_permiss.IsEnabled = false;
                del_user_btn.IsEnabled = false;
                group_cmb_permiss.Items.Clear();
                groupTableAdapter.Fill(Journal.group);
                for (int i = 0; i < Journal.group.Rows.Count; i++)
                {
                    string bd_group = Journal.group.Rows[i].ItemArray[1].ToString();
                    group_cmb_permiss.Items.Add(bd_group);
                }
                string pageName = null;
                while (pageName == "Discipline_Frame.xaml")
                {
                    frame.NavigationService.RemoveBackEntry();
                    JournalEntry entry = frame.RemoveBackEntry();
                    pageName = System.IO.Path.GetFileName(entry.Source.ToString());
                }
                string kuratorPage = null;
                while (kuratorPage == "prepod_reg.xaml")
                {
                    frame_prepod.NavigationService.RemoveBackEntry();
                    JournalEntry entry = frame_prepod.RemoveBackEntry();
                    kuratorPage = System.IO.Path.GetFileName(entry.Source.ToString());
                }
            }
            if (((Button)sender).Content.Equals("Дисциплины"))
            {
                frame.Navigate(new Discipline_Frame());
                frame.Visibility = Visibility.Visible;
                frame_prepod.Visibility = Visibility.Hidden;
                permiss_edit.Visibility = Visibility.Hidden;
                canv_mark_add_edit.Visibility = Visibility.Hidden;
                canv_group_add_edit.Visibility = Visibility.Hidden;
                ///
                searh.Text = "";
                group_name.Text = "";
                name.Text = "";
                code.Text = "";
                group_s.Text = "";
                mark_d.Text = "";
                mark_student.Text = "";
                mark_date.Text = "";
                mark_n.Text = "";
                date_picker.Text = "";
                mark_select.Text = "";
                permiss_combo.Text = "";
                searh_student.Text = "";
                group_cmb_permiss.Text = null;
                group_btn_permiss.IsEnabled = false;
                del_user_btn.IsEnabled = false;
            }
            if (((Button)sender).Content.Equals("Кураторы"))
            {
                frame_prepod.Navigate(new prepod_reg());
                frame_prepod.Visibility = Visibility.Visible;
                frame.Visibility = Visibility.Hidden;
                permiss_edit.Visibility = Visibility.Hidden;
                canv_mark_add_edit.Visibility = Visibility.Hidden;
                canv_group_add_edit.Visibility = Visibility.Hidden;
                ///
                searh.Text = "";
                group_name.Text = "";
                name.Text = "";
                code.Text = "";
                group_s.Text = "";
                mark_d.Text = "";
                mark_student.Text = "";
                mark_date.Text = "";
                mark_n.Text = "";
                date_picker.Text = "";
                mark_select.Text = "";
                permiss_combo.Text = "";
                searh_student.Text = "";
                group_cmb_permiss.Text = null;
                group_btn_permiss.IsEnabled = false;
                del_user_btn.IsEnabled = false;
            }
        }

        private void check_group(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            check = radioButton.Name;
            if (check == "edit_group")
            {
                add_group_btn.Content = "Изменить группу";
            }
            else
            {
                add_group_btn.Content = "Добавить группу";
            }
        }

        private void del_group_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            DataRowView selectedatarow = (DataRowView)group_grid.SelectedItem;
            //for (int i = 0; i < Journal.users.Rows.Count; i++)
            //{
            //    string users_del = Journal.users.Rows[i].ItemArray[7].ToString();
            //    usersTableAdapter.DeleteQuery1(Convert.ToInt32(selectedatarow.Row.ItemArray[0]));
            //    Mark_VTableAdapter.Fill(Journal.Mark_V);
            //    users1TableAdapter.Fill(Journal.users1);
            //}
            groupTableAdapter.DeleteQuery(Convert.ToInt32(selectedatarow.Row.ItemArray[0]));
            disciplineTableAdapter.DeleteQuery1();
            groupTableAdapter.Fill(Journal.group);
            users1TableAdapter.Fill(Journal.users1);
            Mark_VTableAdapter.Fill(Journal.Mark_V);
            group_name.Text = "";
            name.Text = "";
            code.Text = "";
            //}
            //catch
            //{
            //    MessageBox.Show("Проверьте соединение с интернетом");
            //}
        }

        private void mark_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selecteDataRow = (DataRowView)mark.SelectedItem;
            if (selecteDataRow != null)
            {
                id_mrk_lbl.Content = selecteDataRow.Row.ItemArray[0].ToString();
                mark_select.Text = selecteDataRow.Row.ItemArray[1].ToString();
                date_picker.Text = selecteDataRow.Row.ItemArray[2].ToString();
                mark_student.Text = selecteDataRow.Row.ItemArray[3].ToString();
                mark_d.Text = selecteDataRow.Row.ItemArray[7].ToString();
            }
        }

        private void edit_permiss_Click(object sender, RoutedEventArgs e)
        {
            string group_null = null;
            int group_null_int = Convert.ToInt32(group_null);
            if (permiss_combo.Text != "")
            {
                //try
                //{
                users1TableAdapter.FillBy4(Journal.users1, Convert.ToString(login.Content), Convert.ToString(permiss.Content));
                if (!Journal.users.Rows.Count.Equals(0))
                {
                    if (permiss_combo.Text == "yes")
                    {
                        users1TableAdapter.UpdateQuery(permiss_combo.Text, Convert.ToString(login.Content));
                        users1TableAdapter.UpdateQuery1(Convert.ToString(login.Content));
                        users1TableAdapter.Fill(Journal.users1);
                        permiss_combo.Text = null;
                        edit_permiss.IsEnabled = false;
                    }
                    if (permiss_combo.Text == "no")
                    {
                        users1TableAdapter.UpdateQuery(permiss_combo.Text, Convert.ToString(login.Content));
                        users1TableAdapter.Fill(Journal.users1);
                        permiss_combo.Text = null;
                        edit_permiss.IsEnabled = false;
                        group_cmb_permiss.Text = null;
                        group_btn_permiss.IsEnabled = false;
                        del_user_btn.IsEnabled = false;
                    }
                }
                //}
                //catch
                //{
                //    MessageBox.Show("Проверьте соединение с интернетом");
                //}
            }
            else MessageBox.Show("Выберите один из вариантов из списка!");
        }

        private void permiss_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selecteDataRow = (DataRowView)permiss_grid.SelectedItem;
            if (selecteDataRow != null)
            {
                id_user.Content = selecteDataRow.Row.ItemArray[0].ToString();
                id_group_lbl.Content = selecteDataRow.Row.ItemArray[7].ToString();
                permiss.Content = selecteDataRow.Row.ItemArray[6].ToString();
                login.Content = selecteDataRow.Row.ItemArray[4].ToString();
                edit_permiss.IsEnabled = true;
                group_btn_permiss.IsEnabled = true;
                string log = Convert.ToString(permiss.Content);
                if (log == "adm")
                {
                    edit_permiss.IsEnabled = false;
                    del_user_btn.IsEnabled = false;
                }
                else
                {
                    edit_permiss.IsEnabled = true;
                    del_user_btn.IsEnabled = true;
                }
            }
        }

        private void permiss_grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            if (propertyDescriptor.DisplayName == "Pass")
            {
                e.Cancel = true;
            }
        }

        private void searh_student_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(searh_student.Text == "")
                {
                    users1TableAdapter.Fill(Journal.users1);
                }
                else
                {
                    users1TableAdapter.FillBy(Journal.users1, searh_student.Text);
                }
            }
            catch
            {
                MessageBox.Show("Проверьте соединение с интернетом");
            }
        }

        private void del_mark_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedatarow = (DataRowView)mark.SelectedItem;
                markTableAdapter.DeleteQuery(Convert.ToInt32(selectedatarow.Row.ItemArray[0]));
                Mark_VTableAdapter.Fill(Journal.Mark_V);
                mark_d.Text = "";
                mark_student.Text = "";
                mark_date.Text = "";
                mark_n.Text = "";
                date_picker.Text = "";
                mark_select.Text = "";
            }
            catch
            {
                MessageBox.Show("Проверьте соединение с интернетом");
            }
        }

        private void del_user_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedatarow = (DataRowView)permiss_grid.SelectedItem;
                for (int i = 0; i < Journal.discipline.Rows.Count; i++)
                {
                    int discipline_del_int = Convert.ToInt32(id_user.Content);
                    disciplineTableAdapter.DeleteQuery2(discipline_del_int);
                }
                usersTableAdapter.DeleteQuery(Convert.ToInt32(selectedatarow.Row.ItemArray[0]));
                users1TableAdapter.Fill(Journal.users1);
                Mark_VTableAdapter.Fill(Journal.Mark_V);
                permiss_combo.Text = null;
                edit_permiss.IsEnabled = false;
                del_user_btn.IsEnabled = false;
                group_cmb_permiss.Text = null;
                group_btn_permiss.IsEnabled = false;
                del_user_btn.IsEnabled = false;
            }
            catch
            {
                MessageBox.Show("Сначала удалите дисциплины связанные с данным преподавателем");
            }
        }

        private void code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Char.IsDigit(e.Text, 0) || (e.Text == "."))
            {
                e.Handled = false;
            }
            else e.Handled = true;
        }

        private void code_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void code_LostFocus(object sender, RoutedEventArgs e)
        {
            if (code.Text.Length < 8)
            {
                error.Content = "Длина 'Кода специальности' 8 символов";
            }
        }

        private void mark_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            if (propertyDescriptor.DisplayName == "id_mark")
            {
                e.Cancel = true;
            }
        }

        private void group_btn_permiss_Click(object sender, RoutedEventArgs e)
        {

            int group_iner = 0;
            group1TableAdapter.FillBy(Journal.group1, group_cmb_permiss.Text);
            if (!Journal.group1.Rows.Count.Equals(0))
            {
                for (int i = 0; i < Journal.group1.Rows.Count; i++)
                {
                    int grop_id = Convert.ToInt32(Journal.group1.Rows[i]["id_group"]);
                    group_iner = grop_id;
                }
            }

            users1TableAdapter.FillBy1(Journal.users1, Convert.ToString(login.Content), Convert.ToString(permiss.Content));
            if (!Journal.users1.Rows.Count.Equals(0))
            {
                if (Convert.ToString(permiss.Content) == "yes")
                {
                    MessageBox.Show("Нельзя добавить группу преподавателю!");
                    permiss_combo.Text = null;
                    group_cmb_permiss.Text = null;
                    edit_permiss.IsEnabled = false;
                    group_btn_permiss.IsEnabled = false;
                }
                if (Convert.ToString(permiss.Content) == "no")
                {
                    users1TableAdapter.UpdateQuery2(group_iner, Convert.ToString(login.Content));
                    permiss_combo.Text = null;
                    group_cmb_permiss.Text = null;
                    edit_permiss.IsEnabled = false;
                    group_btn_permiss.IsEnabled = false;
                }
                users1TableAdapter.Fill(Journal.users1);
                permiss_combo.Text = null;
                group_cmb_permiss.Text = null;
                edit_permiss.IsEnabled = false;
                group_btn_permiss.IsEnabled = false;
            }
        }
    }
}

