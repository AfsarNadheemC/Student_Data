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

            string StudentSql = "Select * from Students";

            SqlCommand StudentCmd = new SqlCommand(StudentSql, Connection);            

            SqlDataReader StudentReader = StudentCmd.ExecuteReader();

            DataTable StudentDt = new DataTable();

            StudentDt.Load(StudentReader);

            var StudentRows = StudentDt.Rows;


            string Internal1Sql = "Select * from Internal1";

            SqlCommand Internal1Cmd = new SqlCommand(Internal1Sql, Connection);

            SqlDataReader Internal1Reader = Internal1Cmd.ExecuteReader();

            DataTable Internal1Dt = new DataTable();

            Internal1Dt.Load(Internal1Reader);

            var Internal1Rows = Internal1Dt.Rows;

            Connection.Close();

            ObservableCollection<Student> Students = new ObservableCollection<Student>();

            for (int i =0; i <  StudentRows.Count; i++)
            {
               var Student =  StudentRows[i];
               var Internal1 = Internal1Rows[i];


                Students.Add(new Student(Student.ItemArray , Internal1.ItemArray));
            }

            ViewModel = new ViewModel(Students);

            this.DataContext = ViewModel;


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
                ViewModel.SelectedStudent = new Student(ViewModel.Students.Last().RollNumber + 1);
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

                string sql = "INSERT INTO Students (RollNo , FullName , FatherName, MotherName , BloodGroup , DateOfBirth , FullAddress, Grade) Values (@RollNo , @FullName , @FatherName, @MotherName , @BloodGroup , @DateOfBirth ,@FullAddress, @Grade)";
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
                    //ViewModel.SelectedStudent = new Student(ViewModel.Students.Last().RollNumber + 1);
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
    }

    public class ViewModel(ObservableCollection<Student> students) : INotifyPropertyChanged
    {

        public string SchoolName { get; set; } = "Springfield High School";
        public string TeacherName { get; set; } = "Galileo Galilei";
        public string Class { get; set; } = "5B";
        public float Percentage { get; set; }
        public ObservableCollection<Student> Students { get; set; } = students;

        private Student _SelectedStudent;

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


        public void SetRank()
        {
            List<Student> TempStudents = Students.OrderByDescending(k => k.Percentage).ToList();

            Percentage = Students.Sum(k => k.Percentage) / Students.Count;

            foreach (Student student in Students)
            {
                student.Rank = TempStudents.IndexOf(student) + 1;
            }
        }

        public void SetRollNumber()
        {
            List<Student> TempStudents = Students.OrderBy(k => k.Name).ToList();

            Students.Clear();

            for (int i = 0; i < TempStudents.Count; i++)
            {
                //TempStudents[i].RollNumber = "SH5B" + (i + 1).ToString("D3");

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
        public SolidColorBrush ProgressColor { get; set; }
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

        public Student(object[] StudentObjects , object[] Internal1Objects)
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
                new Exam ("Internal" , "01/01/2002" ,(int?) Internal1Objects[0] ,(int?)  Internal1Objects[1] , (int?) Internal1Objects[2] , (int?) Internal1Objects[3] ,  (int?)Internal1Objects[4] )
            ];

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

            switch (Percentage)
            {

                case > 75:
                    ProgressColor = new SolidColorBrush(Colors.Green);
                    break;

                case > 40:
                    ProgressColor = new SolidColorBrush(Colors.Orange);

                    break;

                default:
                    ProgressColor = new SolidColorBrush(Colors.Red);

                    break;

            }


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



    public class Exam
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public int? Language1 { get; set; }
        public int? Language2 { get; set; }
        public int? Maths { get; set; }
        public int? Science { get; set; }
        public int? SocialStudies { get; set; }
        public int TotalScored { get; set; }
        public int TotalMarks { get; set; }

        public Exam(string name, string date, int? language1, int? language2, int? maths, int? science, int? socialStudies)
        {
            Name = name;
            Date = date;
            Language1 = language1;
            Language2 = language2;
            Maths = maths;
            Science = science;
            SocialStudies = socialStudies;
            TotalScored = (int) (language1 + language2 + maths + science + socialStudies);
            TotalMarks = 500;
        }

        public float GetPercentage()
        {
            return (TotalScored * 100 / TotalMarks);
        }

        public int Percentage
        {
            get
            {
                return (TotalScored * 100 / TotalMarks);
            }
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

