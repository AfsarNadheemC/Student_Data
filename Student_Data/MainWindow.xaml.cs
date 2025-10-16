using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Student_Data
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModel ViewModel;
        Storyboard SingleStudentAni;
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new ViewModel(new ObservableCollection<Student>
            {

                new Student(
                    "Isaac Newton",
                    "AD07C001",
                    "Woolsthorpe-by-Colsterworth, Lincolnshire, England",
                    new DateTime (2015,1, 4),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Term 1", new DateTime(2023, 10, 15), 65, 90, 95, 60, 66, 100),
                        new Exam("Term 2", new DateTime(2023, 12, 20), 66, 92, 94, 66, 90, 100),
                        new Exam("Term 3", new DateTime(2023, 12, 20), 66, 92, 94, 66, 90, 100),
                    }        ,"B+ve"       , "Qwerty"     ),

                new Student(
                    "Albert Einstein",
                    "AD07C001",
                    "Ulm, Kingdom of Württemberg, German Empire",
                    new DateTime (2015,3, 14),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Term 1", new DateTime(2023, 10, 15), 95, 90, 95, 90, 93, 100),
                        new Exam("Term 2", new DateTime(2023, 12, 20), 33, 42, 44, 36, 40, 100),
                        new Exam("Term 3", new DateTime(2023, 12, 20), 33, 42, 44, 36, 40, 100),
                    }      ,   "B+ve"     , "Qwerty"         ),

                new Student(
                    "Abdul Kalam",
                    "AD07C001",
                    "Rameswaram, Tamil Nadu, India",
                    new DateTime (2014,10, 15),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Term 1", new DateTime(2023, 2, 15), 55, 40, 65, 50, 61, 100),
                        new Exam("Term 2", new DateTime(2023, 12, 20), 61, 52, 74, 46, 50, 100),
                        new Exam("Term 3", new DateTime(2023, 12, 20), 51, 42, 64, 46, 70, 100),
                    }          ,"B+ve"     , "Qwerty"        ),

                new Student(
                    "Thomas Alva Edison",
                    "AD01C001",
                    "Milan, Ohio, U.S.",
                    new DateTime (2015,2, 11),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Term 1", new DateTime(2023, 1, 15), 15, 30, 35, 10, 11, 100),
                        new Exam("Term 4", new DateTime(2023, 12, 20), 11, 34, 34, 16, 30, 100),
                        new Exam("Term 3", new DateTime(2023, 12, 20), 11, 34, 34, 16, 30, 100),
                    }      ,"B+ve"      , "Qwerty"           ),

                new Student(
                    "Nikola Tesla",
                    "AD07C001",
                    "Smiljan, Austrian Empire",
                    new DateTime (4015,1,7),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Term 1", new DateTime(2023, 4, 15), 45, 30, 35, 40, 44, 100),
                        new Exam("Term 4", new DateTime(2023, 12, 20), 44, 34, 34, 46, 30, 100),
                        new Exam("Term 3", new DateTime(2023, 12, 20), 44, 34, 34, 46, 30, 100),
                    }      ,"B+ve"      , "Qwerty"           ),

                new Student(
                    "Stephen Hawking",
                    "AD07C001",
                    "Oxford, England",
                    new DateTime (2015,1,2),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Term 1", new DateTime(2023, 2, 15), 25, 30, 35, 20, 22, 100),
                        new Exam("Term 2", new DateTime(2023, 12, 20), 22, 32, 34, 26, 30, 100),
                        new Exam("Term 3", new DateTime(2023, 12, 20), 22, 32, 34, 26, 30, 100),
                    }         ,"B+ve"    , "Qwerty"          ),

            });

            ViewModel.SetRank();

            this.DataContext = ViewModel;
        }

        private void StudentData_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (sender is Border SelectedBorder)
                {
                    Point Point = SelectedBorder.TranslatePoint(new Point(0, 0), StudentsItemsControl);

                    ViewModel.FromMargin = new Thickness(40, Point.Y, 40, StudentsItemsControl.ActualHeight - (40 + Point.Y));

                    SingleStudentDetailBorder.Visibility = Visibility.Visible;
                    SingleStudentAni = this.Resources["SingleStudentAni"] as Storyboard;
                    SingleStudentAni.Begin();

                    ViewModel.SelectedStudent = (sender as Border).DataContext as Student;

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
    }

    public class ViewModel(ObservableCollection<Student> students) : INotifyPropertyChanged
    {
        public string SchoolName { get; set; } = "Springfield High School";
        public string TeacherName { get; set; } = "Galileo Galilei";
        public string Class { get; set; } = "5B";
        public int Percentage { get; set; }
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

        public void SetRank()
        {
            List<Student> TempStudents = Students.OrderBy(k => k.Percentage).ToList();

            foreach (Student student in Students)
            {
                student.Rank = TempStudents.IndexOf(student);
            }


        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class Student
    {
        public string Name { get; set; }
        public string RollNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ObservableCollection<Exam> Exams { get; set; }
        public string BloodGroup { get; set; }
        public float Percentage { get; set; }
        public SolidColorBrush ProgressColor { get; set; }
        public SolidColorBrush LabelColorDark { get; set; }
        public SolidColorBrush LabelColorDim { get; set; }
        public string FatherName { get; set; }
        public int Rank { get; set; }
        public char Initial
        {
            get
            {
                return Name[0];
            }
        }
        public Student(string name, string rollNumber, string address, DateTime dateOfBirth, ObservableCollection<Exam> exams, string bloodGroup, string fatherName)
        {
            Name = name;
            RollNumber = rollNumber;
            Address = address;
            DateOfBirth = dateOfBirth;
            Exams = exams;
            BloodGroup = bloodGroup;

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
        }
    }


    public class Exam
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Language1 { get; set; }
        public int Language2 { get; set; }
        public int Maths { get; set; }
        public int Science { get; set; }
        public int SocialStudies { get; set; }
        public int SubjectMaxMark { get; set; }
        public int TotalScored { get; set; }
        public int TotalMarks { get; set; }

        public Exam(string name, DateTime date, int language1, int language2, int maths, int science, int socialStudies, int subjectMaxMark)
        {
            Name = name;
            Date = date;
            Language1 = language1;
            Language2 = language2;
            Maths = maths;
            Science = science;
            SocialStudies = socialStudies;
            SubjectMaxMark = subjectMaxMark;
            TotalScored = language1 + language2 + maths + science + socialStudies;
            TotalMarks = subjectMaxMark * 5;
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

        public static SolidColorBrush[] BackBrushes = new SolidColorBrush[]
{
            new SolidColorBrush( (Color)ColorConverter.ConvertFromString("#e9e0fd")  ), // Violet
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffd6e7")) , // Red
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ccffd0")) , // Green
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c0f5ef")) , // Blue Green
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c2ecfc")) , // Blue
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffe1d6")) , // Orange
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffd6da")) , // Maroon

};

        public static SolidColorBrush[] ForeBrushes = new SolidColorBrush[]
        {
            new SolidColorBrush( (Color)ColorConverter.ConvertFromString("#6950d4")  ), // Violet
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e2026b")) , // Red
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#058c50")) , // Green
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#046d68")) , // Blue Green
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0282b8")) , // Blue
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#cd4b26")) , // Orange
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#96062f")) , // Maroon

        };

        public static (SolidColorBrush, SolidColorBrush) GetRandomColor()
        {
            Random RD = new Random();

            int Index = RD.Next(6);

            return (ForeBrushes[Index], BackBrushes[Index]);

        }
    }
}

