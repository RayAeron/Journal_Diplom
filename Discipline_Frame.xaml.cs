using Journal_Diplom.JournalTableAdapters;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Discipline_Frame.xaml
    /// </summary>
    public partial class Discipline_Frame : Page
    {
        Journal Journal;
        usersTableAdapter usersTableAdapter;
        disciplineTableAdapter disciplineTableAdapter;
        groupTableAdapter groupTableAdapter;
        Discipline_VTableAdapter Discipline_VTableAdapter;
        markTableAdapter markTableAdapter;
        Mark_VTableAdapter Mark_VTableAdapter;
        string check = "add_disc";
        public Discipline_Frame()
        {
            InitializeComponent();

            Journal = new Journal();
            usersTableAdapter = new usersTableAdapter();
            usersTableAdapter.Fill(Journal.users);

            groupTableAdapter = new groupTableAdapter();
            groupTableAdapter.Fill(Journal.group);

            disciplineTableAdapter = new disciplineTableAdapter();
            disciplineTableAdapter.Fill(Journal.discipline);

            markTableAdapter = new markTableAdapter();
            markTableAdapter.Fill(Journal.mark);

            Mark_VTableAdapter = new Mark_VTableAdapter();
            Mark_VTableAdapter.Fill(Journal.Mark_V);

            Discipline_VTableAdapter = new Discipline_VTableAdapter();
            Dwiscipline_VTableAdapter.Fill(Journal.Discipline_V);
            discipline_grid.ItemsSource = Journal.Discipline_V.DefaultView;
            discipline_grid.CanUserDeleteRows = false;
            discipline_grid.CanUserAddRows = false;
            discipline_grid.IsReadOnly = true;

            for (int i = 0; i < Journal.users.Rows.Count; i++)
            {
                string student_tyty = Journal.users.Rows[i].ItemArray[4].ToString();
                if (Journal.users.Rows[i].ItemArray[6].ToString() == "yes")
                {
                    discipline_teacher.Items.Add(student_tyty);
                }
            }

            for (int i = 0; i < Journal.group.Rows.Count; i++)
            {
                string bd_group = Journal.group.Rows[i].ItemArray[1].ToString();
                discipline_group.Items.Add(bd_group);
            }
            name_disc.MaxLength = 100;
        }

        private void discipline_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selecteDataRow = (DataRowView)discipline_grid.SelectedItem;
            if (selecteDataRow != null)
            {
                id_disc.Content = selecteDataRow.Row.ItemArray[0].ToString();
                name_disc.Text = selecteDataRow.Row.ItemArray[1].ToString();
                discipline_group.Text = selecteDataRow.Row.ItemArray[2].ToString();
                discipline_teacher.Text = selecteDataRow.Row.ItemArray[6].ToString();
            }
        }

        private void check_disc(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            check = radioButton.Name;
            if (check == "edit_disc")
            {
                add_discipline_btn.Content = "Изменить дисциплину";
            }
            else
            {
                add_discipline_btn.Content = "Добавить дисциплину";
            }
        }

        private void add_discipline_btn_Click(object sender, RoutedEventArgs e)
        {
            if (name_disc.Text != "" && discipline_group.Text != "" && discipline_teacher.Text != "")
            {
                if (((Button)sender).Content.Equals("Добавить дисциплину"))
                {
                    try
                    {
                        string users_iner = "";
                        usersTableAdapter.FillBy(Journal.users, discipline_teacher.Text);
                        if (!Journal.users.Rows.Count.Equals(0))
                        {
                            for (int i = 0; i < Journal.users.Rows.Count; i++)
                            {
                                string users_id = Convert.ToString(Journal.users.Rows[i]["id_user"]);
                                users_iner = users_id;
                            }
                        }

                        string group_iner = "";
                        groupTableAdapter.FillBy(Journal.group, discipline_group.Text);
                        if (!Journal.group.Rows.Count.Equals(0))
                        {
                            for (int i = 0; i < Journal.group.Rows.Count; i++)
                            {
                                string grop_id = Convert.ToString(Journal.group.Rows[i]["id_group"]);
                                group_iner = grop_id;
                            }
                        }
                        disciplineTableAdapter.FillBy2(Journal.discipline, name_disc.Text, Convert.ToInt32(group_iner), Convert.ToInt32(users_iner));
                        if (Journal.discipline.Rows.Count.Equals(0))
                        {
                            if (name_disc.Text != "" && discipline_group.Text != "" && discipline_teacher.Text != "")
                            {
                                disciplineTableAdapter.InsertQuery(name_disc.Text, Convert.ToInt32(group_iner), Convert.ToInt32(users_iner));
                            }
                            else MessageBox.Show("Введите данные");
                        }
                        else MessageBox.Show("Такая дисциплина уже есть!");
                        disciplineTableAdapter.Fill(Journal.discipline);
                    }
                    catch
                    {

                    }
                }
                if (((Button)sender).Content.Equals("Изменить дисциплину"))
                {

                    string users_iner = "";
                    usersTableAdapter.FillBy(Journal.users, discipline_teacher.Text);
                    if (!Journal.users.Rows.Count.Equals(0))
                    {
                        for (int i = 0; i < Journal.users.Rows.Count; i++)
                        {
                            string users_id = Convert.ToString(Journal.users.Rows[i]["id_user"]);
                            users_iner = users_id;
                        }
                    }
                    string group_iner = "";
                    groupTableAdapter.FillBy(Journal.group, discipline_group.Text);
                    if (!Journal.group.Rows.Count.Equals(0))
                    {
                        for (int i = 0; i < Journal.group.Rows.Count; i++)
                        {
                            string grop_id = Convert.ToString(Journal.group.Rows[i]["id_group"]);
                            group_iner = grop_id;
                        }
                    }
                    //
                    string id_dis = Convert.ToString(id_disc.Content);
                    if (name_disc.Text != "")
                    {
                        disciplineTableAdapter.UpdateQuery1(name_disc.Text, Convert.ToInt32(id_dis));
                        name_disc.Text = null;
                        Discipline_VTableAdapter.Fill(Journal.Discipline_V);
                    }
                    else MessageBox.Show("Введите данные");
                    //
                    if (discipline_group.Text != "")
                    {
                        disciplineTableAdapter.UpdateQuery2(Convert.ToInt32(group_iner), Convert.ToInt32(id_dis));
                        discipline_group.Text = null;
                        Discipline_VTableAdapter.Fill(Journal.Discipline_V);
                    }
                    else MessageBox.Show("Введите данные");
                    //
                    if (discipline_teacher.Text != "")
                    {
                        disciplineTableAdapter.UpdateQuery3(Convert.ToInt32(users_iner), Convert.ToInt32(id_dis));
                        discipline_teacher.Text = null;
                        Discipline_VTableAdapter.Fill(Journal.Discipline_V);
                    }
                    else MessageBox.Show("Введите данные");
                }
                Discipline_VTableAdapter.Fill(Journal.Discipline_V);
                name_disc.Clear();
                discipline_group.Text = "";
                discipline_teacher.Text = "";
            }
            else MessageBox.Show("Введите данные");
        }

        private void del_dis_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            DataRowView selectedatarow = (DataRowView)discipline_grid.SelectedItem;
            disciplineTableAdapter.DeleteQuery(Convert.ToInt32(selectedatarow.Row.ItemArray[0]));
            Discipline_VTableAdapter.Fill(Journal.Discipline_V);
            Mark_VTableAdapter.Fill(Journal.Mark_V);
            name_disc.Clear();
            discipline_group.Text = "";
            discipline_teacher.Text = "";
            //}
            //catch
            //{
            //    MessageBox.Show("Ошибка");
            //}
        }

    }
}
