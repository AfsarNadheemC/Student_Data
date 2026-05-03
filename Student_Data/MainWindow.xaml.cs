using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Student_Data
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModel ViewModel;
        Storyboard SingleStudentAni;

        //string ConnectionString = "Data Source=DESKTOP-E3FL44L\\SQLEXPRESS;Initial Catalog=School;Integrated Security=True;";

        SqlConnection Connection = new SqlConnection("Data Source=DESKTOP-E3FL44L\\SQLEXPRESS;Initial Catalog=School;Integrated Security=True;");

        public MainWindow()
        {
            InitializeComponent();

            LoginPage.Visibility = Visibility.Visible;

            Connection.Open();

            var StudentRows = GetDataRowCollection("Students");
            var Internal1Rows = GetDataRowCollection("Internal1");
            var Internal2Rows = GetDataRowCollection("Internal2");
            var Internal3Rows = GetDataRowCollection("Internal3");
            var Term1Rows = GetDataRowCollection("Term1");
            var Term2Rows = GetDataRowCollection("Term2");
            var Term3Rows = GetDataRowCollection("Term3");


            ObservableCollection<Student> Students = new ObservableCollection<Student>();

            for (int i = 0; i < StudentRows.Count; i++)
            {
                Students.Add(new Student(StudentRows[i].ItemArray, Internal1Rows[i].ItemArray, Term1Rows[i].ItemArray, Internal2Rows[i].ItemArray, Term2Rows[i].ItemArray, Internal3Rows[i].ItemArray, Term3Rows[i].ItemArray));
            }

            ViewModel = new ViewModel(Students);

            SetRank();

            Connection.Close();

            this.DataContext = ViewModel;

            //ViewModel.LoginNotification = "1234567890";
        }
        public DataRowCollection GetDataRowCollection(string TableName)
        {

            string StudentSql = $"Select * from {TableName}";

            SqlCommand StudentCmd = new SqlCommand(StudentSql, Connection);

            SqlDataReader StudentReader = StudentCmd.ExecuteReader();

            DataTable StudentDt = new DataTable();

            StudentDt.Load(StudentReader);

            return StudentDt.Rows;

        }


        public void SetRank()
        {
            List<Student> TempStudents = ViewModel.Students.OrderByDescending(k => k.Percentage).ToList();
            ViewModel.Students = new ObservableCollection<Student>(ViewModel.Students.OrderBy(k => k.Name));

            ViewModel.Percentage = ViewModel.Students.Sum(k => k.Percentage) / ViewModel.Students.Count;

            foreach (Student student in ViewModel.Students)
            {
                string sql = "UPDATE STUDENTS SET Grade = @Grade WHERE RollNo = @RollNo";

                student.Rank = TempStudents.IndexOf(student) + 1;

                SqlCommand cmd = new SqlCommand(sql, Connection);

                cmd.Parameters.AddWithValue("@Grade", student.Rank);
                cmd.Parameters.AddWithValue("@RollNo", student.RollNumber);

                if (!(cmd.ExecuteNonQuery() > 0))
                {
                    MessageBox.Show("!");
                }
            }
        }


        private void StudentData_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (sender is Border SelectedBorder)
                {
                    if (!(SelectedBorder.DataContext is Student SD)) return;
                    if (ViewModel.SelectedStudent == null)
                    {
                        ViewModel.SelectedStudent = SD;
                        ViewModel.SelectedStudent.IsSelected = true;
                    }
                    else
                    {
                        if (SD.Equals(ViewModel.SelectedStudent))
                        {

                            Point Point = SelectedBorder.TranslatePoint(new Point(0, 0), StudentsScroll);

                            ViewModel.FromMargin = new Thickness(50, Point.Y, 50, StudentsScroll.ActualHeight - (40 + Point.Y));

                            SingleStudentDetailBorder.Visibility = Visibility.Visible;

                            if (this.Resources["SingleStudentAni"] is Storyboard _Temp)
                            {
                                SingleStudentAni = _Temp;
                                SingleStudentAni.Begin();
                            }

                            ViewModel.IsPersonalInfo = true;
                            SD.IsSelected = true;
                        }
                        ViewModel.SelectedStudent.IsSelected = false;
                        ViewModel.SelectedStudent = SD;
                        ViewModel.SelectedStudent.IsSelected = true;
                        ViewModel.IsPersonalInfo = true;
                    }

                }


            }
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SingleStudentDetailBorder.Visibility = Visibility.Collapsed;

            if (!ViewModel.SelectedStudent.IsReadOnly)
            {
                ViewModel.SelectedStudent.IsReadOnly = true;
            }

        }

        private void SingleStudentAni_Completed(object sender, EventArgs e)
        {
            SingleStudentAni.Stop();
            SingleStudentDetailBorder.Margin = new Thickness(5);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

            ViewModel.IsPersonalInfo = true;

            SingleStudentDetailBorder.Visibility = Visibility.Visible;

            if (ViewModel.SelectedStudent != null)
            {
                ViewModel.SelectedStudent.IsSelected = false;
            }

            if (ViewModel.Students.Count > 0)
            {
                ViewModel.SelectedStudent = new Student(ViewModel.Students.Max(a => a.RollNumber) + 1);
            }
            else
            {
                ViewModel.SelectedStudent = new Student(1);
            }

            NameTxt.Focus();

            ViewModel.SingleText = "Add";

        }



        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedStudent != null)
            {
                Connection.Open();

                using (SqlTransaction transaction = Connection.BeginTransaction())
                {


                    SqlCommand cmd = Connection.CreateCommand();
                    cmd.Transaction = transaction;

                    cmd.CommandText = "DELETE FROM Students WHERE RollNo = @RollNo";
                    cmd.Parameters.AddWithValue("@RollNo", ViewModel.SelectedStudent.RollNumber);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();

                    cmd.CommandText = "DELETE FROM Internal1 WHERE RollNo = @RollNo";
                    cmd.Parameters.AddWithValue("@RollNo", ViewModel.SelectedStudent.RollNumber);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();

                    cmd.CommandText = "DELETE FROM Internal2 WHERE RollNo = @RollNo";
                    cmd.Parameters.AddWithValue("@RollNo", ViewModel.SelectedStudent.RollNumber);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();

                    cmd.CommandText = "DELETE FROM Internal3 WHERE RollNo = @RollNo";
                    cmd.Parameters.AddWithValue("@RollNo", ViewModel.SelectedStudent.RollNumber);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();

                    cmd.CommandText = "DELETE FROM Term1 WHERE RollNo = @RollNo";
                    cmd.Parameters.AddWithValue("@RollNo", ViewModel.SelectedStudent.RollNumber);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();

                    cmd.CommandText = "DELETE FROM Term2 WHERE RollNo = @RollNo";
                    cmd.Parameters.AddWithValue("@RollNo", ViewModel.SelectedStudent.RollNumber);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();

                    cmd.CommandText = "DELETE FROM Term3 WHERE RollNo = @RollNo";
                    cmd.Parameters.AddWithValue("@RollNo", ViewModel.SelectedStudent.RollNumber);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();

                    transaction.Commit();

                }

                ViewModel.Students.Remove(ViewModel.SelectedStudent);

                SetRank();

                Connection.Close();

            }


        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedStudent != null)
            {
                ViewModel.IsPersonalInfo = true;

                SingleStudentDetailBorder.Visibility = Visibility.Visible;

                ViewModel.SelectedStudent.IsReadOnly = false;

                ViewModel.SingleText = "Update";
            }
        }

        private void SingleAdd_Click(object sender, RoutedEventArgs e)
        {

            if (!(SingleStudentDetailBorder.DataContext is Student SD)) return;

            //1 / 1 / 1753 12:00:00 AM and 12 / 31 / 9999 11:59:59 PM.'

            if (SD.DateOfBirth < new DateTime(1753, 01, 01) || SD.DateOfBirth > new DateTime(9999, 12, 31))
            {
                MessageBox.Show("Invalid Date");
                return;
            }

            if ((string.IsNullOrWhiteSpace(SD.Name) || string.IsNullOrWhiteSpace(SD.FatherName) || string.IsNullOrWhiteSpace(SD.MotherName) || string.IsNullOrWhiteSpace(SD.BloodGroup) || string.IsNullOrWhiteSpace(SD.Address)))
            {
                MessageBox.Show("Some Fields are Missing");
                return;
            }

            if (ViewModel.SingleText == "Add")
            {

                if (ViewModel.SelectedStudent.IsAnyExamMissing)
                {
                    if (MessageBox.Show("Looks like you haven't entered one or more than one exam marks\n Do You want to continue", "Learn From Mistakes", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                Connection.Open();

                string sql1 = "INSERT INTO Students (RollNo , FullName , FatherName, MotherName , BloodGroup , DateOfBirth , FullAddress) Values (@RollNo , @FullName , @FatherName, @MotherName , @BloodGroup , @DateOfBirth ,@FullAddress)";
                SqlCommand cmd1 = new SqlCommand(sql1, Connection);

                cmd1.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd1.Parameters.AddWithValue("@FullName", SD.Name);
                cmd1.Parameters.AddWithValue("@FatherName", SD.FatherName);
                cmd1.Parameters.AddWithValue("@MotherName", SD.MotherName);
                cmd1.Parameters.AddWithValue("@BloodGroup", SD.BloodGroup);
                cmd1.Parameters.AddWithValue("@DateOfBirth", SD.DateOfBirth);
                cmd1.Parameters.AddWithValue("@FullAddress", SD.Address);

                string sql2 = "INSERT INTO Internal1 (RollNo , Lang1 , Lang2, Maths , Science , Social ) Values (@RollNo , @Lang1 , @Lang2, @Maths , @Science , @Social )";
                SqlCommand cmd2 = new SqlCommand(sql2, Connection);

                cmd2.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd2.Parameters.AddWithValue("@Lang1", SD.Exams[0].Language1);
                cmd2.Parameters.AddWithValue("@Lang2", SD.Exams[0].Language2);
                cmd2.Parameters.AddWithValue("@Maths", SD.Exams[0].Maths);
                cmd2.Parameters.AddWithValue("@Science", SD.Exams[0].Science);
                cmd2.Parameters.AddWithValue("@Social", SD.Exams[0].SocialStudies);

                string sql3 = "INSERT INTO Term1 (RollNo , Lang1 , Lang2, Maths , Science , Social ) Values (@RollNo , @Lang1 , @Lang2, @Maths , @Science , @Social )";
                SqlCommand cmd3 = new SqlCommand(sql3, Connection);

                cmd3.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd3.Parameters.AddWithValue("@Lang1", SD.Exams[1].Language1);
                cmd3.Parameters.AddWithValue("@Lang2", SD.Exams[1].Language2);
                cmd3.Parameters.AddWithValue("@Maths", SD.Exams[1].Maths);
                cmd3.Parameters.AddWithValue("@Science", SD.Exams[1].Science);
                cmd3.Parameters.AddWithValue("@Social", SD.Exams[1].SocialStudies);

                string sql4 = "INSERT INTO Internal2 (RollNo , Lang1 , Lang2, Maths , Science , Social ) Values (@RollNo , @Lang1 , @Lang2, @Maths , @Science , @Social )";
                SqlCommand cmd4 = new SqlCommand(sql4, Connection);

                cmd4.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd4.Parameters.AddWithValue("@Lang1", SD.Exams[2].Language1);
                cmd4.Parameters.AddWithValue("@Lang2", SD.Exams[2].Language2);
                cmd4.Parameters.AddWithValue("@Maths", SD.Exams[2].Maths);
                cmd4.Parameters.AddWithValue("@Science", SD.Exams[2].Science);
                cmd4.Parameters.AddWithValue("@Social", SD.Exams[2].SocialStudies);

                string sql5 = "INSERT INTO Term2 (RollNo , Lang1 , Lang2, Maths , Science , Social ) Values (@RollNo , @Lang1 , @Lang2, @Maths , @Science , @Social )";
                SqlCommand cmd5 = new SqlCommand(sql5, Connection);

                cmd5.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd5.Parameters.AddWithValue("@Lang1", SD.Exams[3].Language1);
                cmd5.Parameters.AddWithValue("@Lang2", SD.Exams[3].Language2);
                cmd5.Parameters.AddWithValue("@Maths", SD.Exams[3].Maths);
                cmd5.Parameters.AddWithValue("@Science", SD.Exams[3].Science);
                cmd5.Parameters.AddWithValue("@Social", SD.Exams[3].SocialStudies);

                string sql6 = "INSERT INTO Internal3 (RollNo , Lang1 , Lang2, Maths , Science , Social ) Values (@RollNo , @Lang1 , @Lang2, @Maths , @Science , @Social )";
                SqlCommand cmd6 = new SqlCommand(sql6, Connection);

                cmd6.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd6.Parameters.AddWithValue("@Lang1", SD.Exams[4].Language1);
                cmd6.Parameters.AddWithValue("@Lang2", SD.Exams[4].Language2);
                cmd6.Parameters.AddWithValue("@Maths", SD.Exams[4].Maths);
                cmd6.Parameters.AddWithValue("@Science", SD.Exams[4].Science);
                cmd6.Parameters.AddWithValue("@Social", SD.Exams[4].SocialStudies);

                string sql7 = "INSERT INTO Term3 (RollNo , Lang1 , Lang2, Maths , Science , Social ) Values (@RollNo , @Lang1 , @Lang2, @Maths , @Science , @Social )";
                SqlCommand cmd7 = new SqlCommand(sql7, Connection);

                cmd7.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd7.Parameters.AddWithValue("@Lang1", SD.Exams[5].Language1);
                cmd7.Parameters.AddWithValue("@Lang2", SD.Exams[5].Language2);
                cmd7.Parameters.AddWithValue("@Maths", SD.Exams[5].Maths);
                cmd7.Parameters.AddWithValue("@Science", SD.Exams[5].Science);
                cmd7.Parameters.AddWithValue("@Social", SD.Exams[5].SocialStudies);

                if (cmd1.ExecuteNonQuery() > 0 &&
                    cmd2.ExecuteNonQuery() > 0 &&
                    cmd3.ExecuteNonQuery() > 0 &&
                    cmd4.ExecuteNonQuery() > 0 &&
                    cmd5.ExecuteNonQuery() > 0 &&
                    cmd6.ExecuteNonQuery() > 0 &&
                    cmd7.ExecuteNonQuery() > 0)
                {
                    //MessageBox.Show("Added");

                    SD.SetPercentage();

                    ViewModel.Students.Add(SD);
                }

                SetRank();

                //ViewModel.StudentsView.Refresh();

                Connection.Close();

                SingleStudentDetailBorder.Visibility = Visibility.Collapsed;

            }
            else
            {
                SD.IsReadOnly = true;

                ViewModel.SelectedStudent = SD;

                Connection.Open();

                string sql1 = "UPDATE Students SET FullName = @FullName , FatherName = @FatherName, MotherName = @MotherName , BloodGroup = @BloodGroup , DateOfBirth = @DateOfBirth , FullAddress = @FullAddress WHERE RollNo = @RollNo";
                SqlCommand cmd1 = new SqlCommand(sql1, Connection);

                cmd1.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd1.Parameters.AddWithValue("@FullName", SD.Name);
                cmd1.Parameters.AddWithValue("@FatherName", SD.FatherName);
                cmd1.Parameters.AddWithValue("@MotherName", SD.MotherName);
                cmd1.Parameters.AddWithValue("@BloodGroup", SD.BloodGroup);
                cmd1.Parameters.AddWithValue("@DateOfBirth", SD.DateOfBirth);
                cmd1.Parameters.AddWithValue("@FullAddress", SD.Address);

                string sql2 = "UPDATE Internal1 SET Lang1 = @Lang1 , Lang2 = @Lang2, Maths = @Maths , Science = @Science , Social = @Social WHERE RollNo = @RollNo";
                SqlCommand cmd2 = new SqlCommand(sql2, Connection);

                cmd2.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd2.Parameters.AddWithValue("@Lang1", SD.Exams[0].Language1);
                cmd2.Parameters.AddWithValue("@Lang2", SD.Exams[0].Language2);
                cmd2.Parameters.AddWithValue("@Maths", SD.Exams[0].Maths);
                cmd2.Parameters.AddWithValue("@Science", SD.Exams[0].Science);
                cmd2.Parameters.AddWithValue("@Social", SD.Exams[0].SocialStudies);

                string sql3 = "UPDATE Term1 SET Lang1 = @Lang1 , Lang2 = @Lang2, Maths = @Maths , Science = @Science , Social = @Social WHERE RollNo = @RollNo";
                SqlCommand cmd3 = new SqlCommand(sql3, Connection);

                cmd3.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd3.Parameters.AddWithValue("@Lang1", SD.Exams[1].Language1);
                cmd3.Parameters.AddWithValue("@Lang2", SD.Exams[1].Language2);
                cmd3.Parameters.AddWithValue("@Maths", SD.Exams[1].Maths);
                cmd3.Parameters.AddWithValue("@Science", SD.Exams[1].Science);
                cmd3.Parameters.AddWithValue("@Social", SD.Exams[1].SocialStudies);

                string sql4 = "UPDATE Internal2 SET Lang1 = @Lang1 , Lang2 = @Lang2, Maths = @Maths , Science = @Science , Social = @Social WHERE RollNo = @RollNo";
                SqlCommand cmd4 = new SqlCommand(sql4, Connection);

                cmd4.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd4.Parameters.AddWithValue("@Lang1", SD.Exams[2].Language1);
                cmd4.Parameters.AddWithValue("@Lang2", SD.Exams[2].Language2);
                cmd4.Parameters.AddWithValue("@Maths", SD.Exams[2].Maths);
                cmd4.Parameters.AddWithValue("@Science", SD.Exams[2].Science);
                cmd4.Parameters.AddWithValue("@Social", SD.Exams[2].SocialStudies);

                string sql5 = "UPDATE Term2 SET Lang1 = @Lang1 , Lang2 = @Lang2, Maths = @Maths , Science = @Science , Social = @Social WHERE RollNo = @RollNo";
                SqlCommand cmd5 = new SqlCommand(sql5, Connection);

                cmd5.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd5.Parameters.AddWithValue("@Lang1", SD.Exams[3].Language1);
                cmd5.Parameters.AddWithValue("@Lang2", SD.Exams[3].Language2);
                cmd5.Parameters.AddWithValue("@Maths", SD.Exams[3].Maths);
                cmd5.Parameters.AddWithValue("@Science", SD.Exams[3].Science);
                cmd5.Parameters.AddWithValue("@Social", SD.Exams[3].SocialStudies);

                string sql6 = "UPDATE Internal3 SET Lang1 = @Lang1 , Lang2 = @Lang2, Maths = @Maths , Science = @Science , Social = @Social WHERE RollNo = @RollNo";
                SqlCommand cmd6 = new SqlCommand(sql6, Connection);

                cmd6.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd6.Parameters.AddWithValue("@Lang1", SD.Exams[4].Language1);
                cmd6.Parameters.AddWithValue("@Lang2", SD.Exams[4].Language2);
                cmd6.Parameters.AddWithValue("@Maths", SD.Exams[4].Maths);
                cmd6.Parameters.AddWithValue("@Science", SD.Exams[4].Science);
                cmd6.Parameters.AddWithValue("@Social", SD.Exams[4].SocialStudies);

                string sql7 = "UPDATE Term3 SET Lang1 = @Lang1 , Lang2 = @Lang2, Maths = @Maths , Science = @Science , Social = @Social WHERE RollNo = @RollNo";
                SqlCommand cmd7 = new SqlCommand(sql7, Connection);

                cmd7.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd7.Parameters.AddWithValue("@Lang1", SD.Exams[5].Language1);
                cmd7.Parameters.AddWithValue("@Lang2", SD.Exams[5].Language2);
                cmd7.Parameters.AddWithValue("@Maths", SD.Exams[5].Maths);
                cmd7.Parameters.AddWithValue("@Science", SD.Exams[5].Science);
                cmd7.Parameters.AddWithValue("@Social", SD.Exams[5].SocialStudies);

                if (cmd1.ExecuteNonQuery() > 0 &&
                    cmd2.ExecuteNonQuery() > 0 &&
                    cmd3.ExecuteNonQuery() > 0 &&
                    cmd4.ExecuteNonQuery() > 0 &&
                    cmd5.ExecuteNonQuery() > 0 &&
                    cmd6.ExecuteNonQuery() > 0 &&
                    cmd7.ExecuteNonQuery() > 0)
                {
                    SD.SetPercentage();
                    //MessageBox.Show("Updated");
                }

                SetRank();


                Connection.Close();
            }

        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(ViewModel.UserName))
            {
                ViewModel.LoginNotification = "Please Enter User Name";
                LoginNotificationPopup.IsOpen = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                ViewModel.LoginNotification = "Please Enter Password";
                LoginNotificationPopup.IsOpen = true;
                return;
            }

            Connection.Open();

            var v = GetDataRowCollection("UserInfo");

            string sql = "SELECT * FROM UserInfo where UserName = @UserName AND Password = @Password";
            SqlCommand sqlCommand = new SqlCommand(sql, Connection);

            sqlCommand.Parameters.AddWithValue("@UserName", ViewModel.UserName);
            sqlCommand.Parameters.AddWithValue("@Password", PasswordBox.Password);

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);

            if (dataTable.Rows.Count == 0)
            {
                ViewModel.LoginNotification = "Invalid User Name or Password";
                LoginNotificationPopup.IsOpen = true;
                Connection.Close();
                return;
            }
            else
            {
                LoginPage.Visibility = Visibility.Collapsed;
            }



            Connection.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {


        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            if (passwordBox.Password.Length == 0)
            {
                passwordBox.Tag = "Password";
            }
            else
            {
                passwordBox.Tag = "";
            }

        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.Tag = "";
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;

            var path = img.Source;
        }

        private void BloodGroupItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.SelectedStudent.BloodGroup = (sender as Grid).DataContext as string;
            BloodGroupPopup.IsOpen = false;
        }

        private void BloodGroupSelectBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BloodGroupPopup.IsOpen = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ExamsScrollViewer.ScrollToTop();
        }
    }

}

