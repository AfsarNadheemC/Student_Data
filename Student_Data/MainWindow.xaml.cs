using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
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
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;


namespace Student_Data
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModel ViewModel;
        Storyboard SingleStudentAni;

        string ConnectionString = "Data Source=DESKTOP-E3FL44L\\SQLEXPRESS;Initial Catalog=School;Integrated Security=True;";

        SqlConnection Connection = new SqlConnection("Data Source=DESKTOP-E3FL44L\\SQLEXPRESS;Initial Catalog=School;Integrated Security=True;");

        public MainWindow()
        {
            InitializeComponent();

            Connection.Open();

            var StudentRows = GetDataRowCollection("Students");
            var Internal1Rows = GetDataRowCollection("Internal1");
            var Internal2Rows = GetDataRowCollection("Internal2");
            var Internal3Rows = GetDataRowCollection("Internal3");
            var Term1Rows = GetDataRowCollection("Term1");
            var Term2Rows = GetDataRowCollection("Term2");
            var Term3Rows = GetDataRowCollection("Term3");

            Connection.Close();

            ObservableCollection<Student> Students = new ObservableCollection<Student>();

            for (int i = 0; i < StudentRows.Count; i++)
            {
                var Student = StudentRows[i];

                Students.Add(new Student(StudentRows[i].ItemArray, Internal1Rows[i].ItemArray, Term1Rows[i].ItemArray, Internal2Rows[i].ItemArray, Term2Rows[i].ItemArray, Internal3Rows[i].ItemArray, Term3Rows[i].ItemArray));
            }

            ViewModel = new ViewModel(Students);

            SetRank();

            this.DataContext = ViewModel;

            ViewModel.LoginNotification = "1234567890";
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
            Connection.Open();


            List<Student> TempStudents = ViewModel.Students.OrderByDescending(k => k.Percentage).ToList();
            ViewModel.Students = new ObservableCollection<Student>(ViewModel.Students.OrderBy(k => k.Name));

            ViewModel.Percentage = ViewModel.Students.Sum(k => k.Percentage) / ViewModel.Students.Count;

            foreach (Student student in ViewModel.Students)
            {
                string sql = "UPDATE STUDENTS SET Grade = @Grade";

                student.Rank = TempStudents.IndexOf(student) + 1;

                SqlCommand cmd = new SqlCommand(sql, Connection);

                cmd.Parameters.AddWithValue("@Grade", student.Rank);

                if (!(cmd.ExecuteNonQuery() > 0))
                {
                    MessageBox.Show("!");
                }

            }

            Connection.Close();
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
            SingleStudentDetailBorder.Visibility = Visibility.Visible;

            if (ViewModel.SelectedStudent != null)
            {
                ViewModel.SelectedStudent.IsSelected = false;
            }

            if (ViewModel.Students.Count > 0)
            {
                ViewModel.SelectedStudent = new Student(ViewModel.Students.Count + 1);
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

                string sql = "DELETE FROM Students WHERE RollNo = @RollNo";

                SqlCommand cmd = new SqlCommand(sql, Connection);

                Connection.Open();

                cmd.Parameters.AddWithValue("RollNo", ViewModel.SelectedStudent.RollNumber);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    ViewModel.Students.Remove(ViewModel.SelectedStudent);
                }



                Connection.Close();

            }


        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedStudent != null)
            {

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

            if ((SD.Name == null || SD.FatherName == null || SD.MotherName == null || SD.BloodGroup == null || SD.Address == null))
            {
                MessageBox.Show("Some Fields are Missing");
                return;
            }

            if (ViewModel.SingleText == "Add")
            {
                //return;

                string sql = "INSERT INTO Students (RollNo , FullName , FatherName, MotherName , BloodGroup , DateOfBirth , FullAddress) Values (@RollNo , @FullName , @FatherName, @MotherName , @BloodGroup , @DateOfBirth ,@FullAddress)";
                SqlCommand cmd = new SqlCommand(sql, Connection);

                Connection.Open();

                cmd.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd.Parameters.AddWithValue("@FullName", SD.Name);
                cmd.Parameters.AddWithValue("@FatherName", SD.FatherName);
                cmd.Parameters.AddWithValue("@MotherName", SD.MotherName);
                cmd.Parameters.AddWithValue("@BloodGroup", SD.BloodGroup);
                cmd.Parameters.AddWithValue("@DateOfBirth", SD.DateOfBirth);
                cmd.Parameters.AddWithValue("@FullAddress", SD.Address);

                if (ViewModel.Students.Count > 0)
                {
                    cmd.Parameters.AddWithValue("@Grade", ViewModel.Students.Last().Rank + 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Grade", 1);
                }



                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Added");
                    ViewModel.Students.Add(SD);
                }



                Connection.Close();

                SingleStudentDetailBorder.Visibility = Visibility.Collapsed;

            }
            else
            {
                SD.IsReadOnly = true;

                ViewModel.SelectedStudent = SD;

                string sql = "UPDATE Students SET FullName = @FullName , FatherName = @FatherName, MotherName = @MotherName , BloodGroup = @BloodGroup , DateOfBirth = @DateOfBirth , FullAddress = @FullAddress, Grade = @Grade WHERE RollNo = @RollNo";
                SqlCommand cmd = new SqlCommand(sql, Connection);

                Connection.Open();

                cmd.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd.Parameters.AddWithValue("@FullName", SD.Name);
                cmd.Parameters.AddWithValue("@FatherName", SD.FatherName);
                cmd.Parameters.AddWithValue("@MotherName", SD.MotherName);
                cmd.Parameters.AddWithValue("@BloodGroup", SD.BloodGroup);
                cmd.Parameters.AddWithValue("@DateOfBirth", SD.DateOfBirth);
                cmd.Parameters.AddWithValue("@FullAddress", SD.Address);
                cmd.Parameters.AddWithValue("@Grade", ViewModel.Students.Last().Rank + 1);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Updated");
                    //ViewModel.SelectedStudent = new Student((int.Parse(ViewModel.Students.Last().RollNumber) + 1).ToString());
                }



                Connection.Close();
            }

        }

        private void AddExams()
        {

        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string Search = SearchTextBox.Text;

            if (string.IsNullOrWhiteSpace(Search))
            {
                StudentsItemsControl.ItemsSource = ViewModel.Students;
            }
            else
            {
                StudentsItemsControl.ItemsSource = ViewModel.Students.Where(a => a.Name.IndexOf(Search, StringComparison.OrdinalIgnoreCase) > -1);
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(ViewModel.UserName) )
            {
                ViewModel.LoginNotification = "Please Enter User Name";
                LoginNotificationPopup.IsOpen = true;
                return;
            }

            if ( string.IsNullOrWhiteSpace(PasswordBox.Password))
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

            if ( passwordBox.Password.Length == 0)
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
    }

    public class ViewModel(ObservableCollection<Student> students) : INotifyPropertyChanged
    {

        public string SchoolName { get; set; } = "Springfield High School";
        public string TeacherName { get; set; } = "Galileo Galilei";
        public string Class { get; set; } = "5B";
        public float Percentage { get; set; }
        public ObservableCollection<Student> Students { get; set; } = students;

        private Student? _SelectedStudent;

        public Student SelectedStudent
        {
            get { return _SelectedStudent; }
            set { _SelectedStudent = value; OnPropertyChanged(nameof(SelectedStudent)); }
        }

        private Thickness _FromMargin;

        public Thickness FromMargin
        {
            get { return _FromMargin; }
            set { _FromMargin = value; OnPropertyChanged(nameof(FromMargin)); }
        }

        private bool myVar;

        public bool MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private bool _IsExaminations;

        public bool IsExaminations
        {
            get
            {
                return _IsExaminations;
            }
            set
            {
                _IsExaminations = value;
                OnPropertyChanged(nameof(IsExaminations));
            }
        }

        private bool _IsPersonalInfo = true;

        public bool IsPersonalInfo
        {
            get
            {
                return _IsPersonalInfo;
            }
            set
            {
                _IsPersonalInfo = value;
                OnPropertyChanged(nameof(IsPersonalInfo));
            }
        }

        private string _SingleText;

        public string SingleText
        {
            get { return _SingleText; }
            set { _SingleText = value; OnPropertyChanged(nameof(SingleText)); }
        }


        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; OnPropertyChanged(nameof(UserName)); }
        }

        private string _LoginNotification;

        public string LoginNotification
        {
            get
            {
                return _LoginNotification;
            }
            set
            {
                _LoginNotification = value; OnPropertyChanged(nameof(LoginNotification));
            }
        }
        //private string _Password;

        //public string Password
        //{
        //    get { return _Password; }
        //    set { _Password = value; OnPropertyChanged(nameof(Password)); }
        //}

        public void SetRollNumber()
        {
            List<Student> TempStudents = Students.OrderBy(k => k.Name).ToList();

            Students.Clear();

            for (int i = 0; i < TempStudents.Count; i++)
            {

                Students.Add(TempStudents[i]);
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class Student : INotifyPropertyChanged
    {
        public string Name { get; set; }
        private int _RollNumber;

        public int RollNumber
        {
            get
            {
                return _RollNumber;
            }
            set
            {
                _RollNumber = value;
                OnPropertyChanged(nameof(RollNumber));
            }
        }


        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ObservableCollection<Exam> Exams { get; set; }
        public string BloodGroup { get; set; }
        public float Percentage { get; set; }

        public SolidColorBrush ProgressColor
        {
            get
            {

                if (Percentage > 75)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else if (Percentage > 50)
                {
                    return new SolidColorBrush(Colors.Orange);
                }
                return new SolidColorBrush(Colors.Red);
            }
        }


        public Color LabelColorDark { get; set; }
        public Color LabelColorDim { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public int Rank { get; set; }
        public char Initial
        {
            get
            {
                return Name[0];
            }
        }

        private bool _IsSelected;

        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value; OnPropertyChanged(nameof(IsSelected));
            }
        }



        private bool _IsReadOnly;

        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            set { _IsReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); }
        }

        public Student(int RollNo)
        {
            RollNumber = RollNo;
            DateOfBirth = new DateTime(2002, 1, 1);
            IsReadOnly = false;

            Exams = new ObservableCollection<Exam>
            {
                new Exam ("Internal1" , "10/07/2015" ,0     ,0,0,0,0 ),
                new Exam ("Term1" , "29/08/2015" ,0     ,0,0,0,0 ),

                new Exam ("Internal2" , "01/11/2015" ,0     ,0,0,0,0  ),
                new Exam ("Term2" , "15/12/2015" ,0     ,0,0,0,0  ),

                new Exam ("Internal3" , "20/02/2015" ,0     ,0,0,0,0 ),
                new Exam ("Term3" , "10/04/2015",0     ,0,0,0,0  ),
            };



            (LabelColorDark, LabelColorDim) = CommonColors.GetRandomColor();
        }

        public Student(object[] Objects)
        {
            if (Objects[0] is int RollNo)
            {
                RollNumber = RollNo;
            }
            Name = Objects[1] as string;
            FatherName = Objects[2] as string;
            MotherName = Objects[3] as string;
            BloodGroup = Objects[4] as string;

            if (Objects[5] is DateTime DOB)
            {
                DateOfBirth = DOB;
            }

            Address = Objects[6] as string;


            IsReadOnly = true;
            (LabelColorDark, LabelColorDim) = CommonColors.GetRandomColor();
        }

        public Student(object[] StudentObjects, object[] Internal1Objects, object[] Term1Objects, object[] Internal2Objects, object[] Term2Objects, object[] Internal3Objects, object[] Term3Objects)
        {
            if (StudentObjects[0] is int RollNo)
            {
                RollNumber = RollNo;
            }
            Name = StudentObjects[1] as string;
            FatherName = StudentObjects[2] as string;
            MotherName = StudentObjects[3] as string;
            BloodGroup = StudentObjects[4] as string;

            if (StudentObjects[5] is DateTime DOB)
            {
                DateOfBirth = DOB;
            }

            Address = StudentObjects[6] as string;

            Exams =

            [

                new Exam ("Internal1" , "10/07/2015" ,(int) Internal1Objects[0] ,(int)  Internal1Objects[1] , (int) Internal1Objects[2] , (int) Internal1Objects[3] ,  (int)Internal1Objects[4] ),
                new Exam ("Term1" , "29/08/2015" ,(int) Term1Objects[0] ,(int)  Term1Objects[1] , (int) Term1Objects[2] , (int) Term1Objects[3] ,  (int)Term1Objects[4] ),

                new Exam ("Internal2" , "01/11/2015" ,(int) Internal2Objects[0] ,(int)  Internal2Objects[1] , (int) Internal2Objects[2] , (int) Internal2Objects[3] ,  (int)Internal2Objects[4] ),
                new Exam ("Term2" , "15/12/2015" ,(int) Term2Objects[0] ,(int)  Term2Objects[1] , (int) Term2Objects[2] , (int) Term2Objects[3] ,  (int)Term2Objects[4] ),

                new Exam ("Internal3" , "20/02/2015" ,(int) Internal3Objects[0] ,(int)  Internal3Objects[1] , (int) Internal3Objects[2] , (int) Internal3Objects[3] ,  (int)Internal3Objects[4] ),
                new Exam ("Term3" , "10/04/2015" ,(int) Term3Objects[0] ,(int)  Term3Objects[1] , (int) Term3Objects[2] , (int) Term3Objects[3] ,  (int)Term3Objects[4] ),

            ];



            float TotalPercentage = 0f;

            foreach (Exam exam in Exams)
            {
                TotalPercentage += exam.GetPercentage();
            }

            Percentage = TotalPercentage / Exams.Count;


            IsReadOnly = true;
            (LabelColorDark, LabelColorDim) = CommonColors.GetRandomColor();
        }

        public Student(string name, string address, DateTime dateOfBirth, ObservableCollection<Exam> exams, string bloodGroup, string fatherName, string motherName)
        {
            Name = name;    //
            Address = address;  //
            DateOfBirth = dateOfBirth;
            Exams = exams;
            BloodGroup = bloodGroup;    //

            float TotalPercentage = 0f;

            foreach (Exam exam in Exams)
            {
                TotalPercentage += exam.GetPercentage();
            }

            Percentage = TotalPercentage / Exams.Count;


            (LabelColorDark, LabelColorDim) = CommonColors.GetRandomColor();
            FatherName = fatherName;
            MotherName = motherName;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }



    public class Exam : INotifyPropertyChanged
    {

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string _Date;

        public string Date
        {
            get { return _Date; }
            set { _Date = value; OnPropertyChanged(nameof(Date)); }
        }

        private int _Language1;

        public int Language1
        {
            get { return _Language1; }
            set
            {
                if (value >= 0 && value <= 100) { _Language1 = value; }
                OnPropertyChanged(nameof(Language1)); OnPropertyChanged(nameof(TotalScored)); OnPropertyChanged(nameof(Percentage));
            }
        }

        private int _Language2;

        public int Language2
        {
            get { return _Language2; }
            set
            {
                if (value >= 0 && value <= 100) { _Language2 = value; }
                OnPropertyChanged(nameof(Language2)); OnPropertyChanged(nameof(TotalScored)); OnPropertyChanged(nameof(Percentage));
            }
        }

        private int _Maths;

        public int Maths
        {
            get { return _Maths; }
            set
            {
                if (value >= 0 && value <= 100) { _Maths = value; }
                OnPropertyChanged(nameof(Maths)); OnPropertyChanged(nameof(TotalScored)); OnPropertyChanged(nameof(Percentage));
            }
        }

        private int _Science;

        public int Science
        {
            get { return _Science; }
            set
            {
                if (value >= 0 && value <= 100) { _Science = value; }
                OnPropertyChanged(nameof(Science)); OnPropertyChanged(nameof(TotalScored)); OnPropertyChanged(nameof(Percentage));
            }
        }

        private int _SocialStudies;

        public int SocialStudies
        {
            get { return _SocialStudies; }
            set
            {
                if (value >= 0 && value <= 100) { _SocialStudies = value; }
                OnPropertyChanged(nameof(SocialStudies)); OnPropertyChanged(nameof(TotalScored)); OnPropertyChanged(nameof(Percentage));
            }
        }


        public int TotalScored
        {
            get
            {
                return (int)(Language1 + Language2 + Maths + Science + SocialStudies);

            }
        }

        public int Percentage
        {
            get
            {
                return (TotalScored * 100 / TotalMarks);
            }
        }

        public int TotalMarks { get; } = 500;


        public Exam(string name, string date, int language1, int language2, int maths, int science, int socialStudies)
        {
            Name = name;
            Date = date;
            Language1 = language1;
            Language2 = language2;
            Maths = maths;
            Science = science;
            SocialStudies = socialStudies;
        }

        public float GetPercentage()
        {
            return (TotalScored * 100 / TotalMarks);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }


    }

    public class CommonColors()
    {

        public static Color[] BackColors = new Color[]
{
            (Color)ColorConverter.ConvertFromString("#e9e0fd"  ), // Violet
            (Color)ColorConverter.ConvertFromString("#ffd6e7") , // Red 
            (Color)ColorConverter.ConvertFromString("#ccffd0") , // Green
            (Color)ColorConverter.ConvertFromString("#c0f5ef") , // Blue Green
            (Color)ColorConverter.ConvertFromString("#c2ecfc") , // Blue
            (Color)ColorConverter.ConvertFromString("#ffe1d6") , // Orange
            (Color)ColorConverter.ConvertFromString("#ffd6da") , // Maroon

};


        public static Color[] ForeColors = new Color[]
        {
            (Color)ColorConverter.ConvertFromString("#6950d4"  ), // Violet
            (Color)ColorConverter.ConvertFromString("#e2026b") , // Red
      (Color)ColorConverter.ConvertFromString("#058c50") , // Green
         (Color)ColorConverter.ConvertFromString("#046d68") , // Blue Green
    (Color)ColorConverter.ConvertFromString("#0282b8") , // Blue
        (Color)ColorConverter.ConvertFromString("#cd4b26") , // Orange
          (Color)ColorConverter.ConvertFromString("#96062f") , // Maroon

        };

        public static (Color, Color) GetRandomColor()
        {
            Random RD = new Random();

            int Index = RD.Next(6);

            return (ForeColors[Index], BackColors[Index]);

        }
    }

    public enum Page
    {
        Personal_Details,
        Exams
    }

    public class InverseBoolToVis : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

