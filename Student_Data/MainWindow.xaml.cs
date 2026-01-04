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

            string sql = "Select * from Students";

            SqlCommand cmd = new SqlCommand(sql, Connection);

            Connection.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            DataTable dt = new DataTable();

            dt.Load(reader);

            Connection.Close();

            var rs = dt.Rows;

            ObservableCollection<Student> Students = new ObservableCollection<Student>();

            foreach (DataRow v in rs)
            {
                Students.Add(new Student(v.ItemArray));
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
                    if (ViewModel.SelectedStudent == null)
                    {
                        Student SD = (sender as Border).DataContext as Student;
                        ViewModel.SelectedStudent = SD;
                        ViewModel.SelectedStudent.IsSelected = true;
                    }
                    else
                    {
                        Student SD = (sender as Border).DataContext as Student;
                        if (SD.Equals(ViewModel.SelectedStudent))
                        {

                            Point Point = SelectedBorder.TranslatePoint(new Point(0, 0), StudentsScroll);

                            ViewModel.FromMargin = new Thickness(50, Point.Y, 50, StudentsScroll.ActualHeight - (40 + Point.Y));

                            SingleStudentDetailBorder.Visibility = Visibility.Visible;
                            SingleStudentAni = this.Resources["SingleStudentAni"] as Storyboard;
                            SingleStudentAni.Begin();


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

            ViewModel.SelectedStudent = ViewModel.SelectedStudent = new Student((int.Parse(ViewModel.Students.Last().RollNumber) + 1).ToString());

            ViewModel.SingleText = "Add";

        }

        private void Remove_Click(object sender, RoutedEventArgs e)
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

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            SingleStudentDetailBorder.Visibility = Visibility.Visible;

            ViewModel.SelectedStudent.IsReadOnly = false;

            ViewModel.SingleText = "Update";
        }

        private void SingleAdd_Click(object sender, RoutedEventArgs e)
        {

            if (ViewModel.SingleText == "Add")
            {
                //return;
                Student SD = SingleStudentDetailBorder.DataContext as Student;
                ViewModel.Students.Add(SD);

                string sql = "INSERT INTO Students (RollNo , FullName , FatherName, MotherName , BloodGroup , FullAddress, Grade) Values (@RollNo , @FullName , @FatherName, @MotherName , @BloodGroup , @FullAddress, @Grade)";
                SqlCommand cmd = new SqlCommand (sql, Connection);

                Connection.Open();

                cmd.Parameters.AddWithValue("@RollNo" , SD.RollNumber);
                cmd.Parameters.AddWithValue("@FullName", SD.Name);
                cmd.Parameters.AddWithValue("@FatherName", SD.FatherName);
                cmd.Parameters.AddWithValue("@MotherName", SD.MotherName);
                cmd.Parameters.AddWithValue("@BloodGroup", SD.BloodGroup);
                cmd.Parameters.AddWithValue("@FullAddress", SD.Address);
                cmd.Parameters.AddWithValue("@Grade", ViewModel.Students.Last().Rank + 1);

                if ( cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Added");
                    ViewModel.SelectedStudent = new Student( (int.Parse ( ViewModel.Students.Last().RollNumber) + 1 ).ToString());
                }


            
                Connection.Close();

            }
            else
            {
                Student SD = SingleStudentDetailBorder.DataContext as Student;
                ViewModel.Students.Add(SD);

                string sql = "UPDATE INTO Students (RollNo , FullName , FatherName, MotherName , BloodGroup , FullAddress, Grade) Values (@RollNo , @FullName , @FatherName, @MotherName , @BloodGroup , @FullAddress, @Grade)";
                SqlCommand cmd = new SqlCommand(sql, Connection);

                Connection.Open();

                cmd.Parameters.AddWithValue("@RollNo", SD.RollNumber);
                cmd.Parameters.AddWithValue("@FullName", SD.Name);
                cmd.Parameters.AddWithValue("@FatherName", SD.FatherName);
                cmd.Parameters.AddWithValue("@MotherName", SD.MotherName);
                cmd.Parameters.AddWithValue("@BloodGroup", SD.BloodGroup);
                cmd.Parameters.AddWithValue("@FullAddress", SD.Address);
                cmd.Parameters.AddWithValue("@Grade", ViewModel.Students.Last().Rank + 1);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Added");
                    ViewModel.SelectedStudent = new Student((int.Parse(ViewModel.Students.Last().RollNumber) + 1).ToString());
                }



                Connection.Close();
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
                TempStudents[i].RollNumber = "SH5B" + (i + 1).ToString("D3");

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
        private string _RollNumber;

        public string RollNumber
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
        public DateOnly DateOfBirth { get; set; }
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

        public Student(string RollNo)
        {
            RollNumber = RollNo;
            IsReadOnly = false;

            (LabelColorDark, LabelColorDim) = CommonColors.GetRandomColor();
        }

        public Student(object[] Objects)
        {
            RollNumber = Objects[0] as string;
            Name = Objects[1] as string;
            FatherName = Objects[2] as string;
            MotherName = Objects[3] as string;
            BloodGroup = Objects[4] as string;
            Address = Objects[5] as string;


            IsReadOnly = true;
            (LabelColorDark, LabelColorDim) = CommonColors.GetRandomColor();
        }

        public Student(string name, string address, DateOnly dateOfBirth, ObservableCollection<Exam> exams, string bloodGroup, string fatherName, string motherName)
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
        public int Language1 { get; set; }
        public int Language2 { get; set; }
        public int Maths { get; set; }
        public int Science { get; set; }
        public int SocialStudies { get; set; }
        public int TotalScored { get; set; }
        public int TotalMarks { get; set; }

        public Exam(string name, string date, int language1, int language2, int maths, int science, int socialStudies)
        {
            Name = name;
            Date = date;
            Language1 = language1;
            Language2 = language2;
            Maths = maths;
            Science = science;
            SocialStudies = socialStudies;
            TotalScored = language1 + language2 + maths + science + socialStudies;
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

