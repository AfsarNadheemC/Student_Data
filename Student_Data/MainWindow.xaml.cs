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
                    "Woolsthorpe-by-Colsterworth, Lincolnshire, England",
                    new DateOnly (2015,1, 4),
                    new ObservableCollection<Exam>
                    {

                        new Exam("Internal 1", "Aug 2025", 95, 90, 95, 90, 93),
                        new Exam("Term 1", "Sep 2025", 33, 42, 44, 36, 40),
                        new Exam("Internal 2", "Nov 2025", 33, 42, 44, 36, 40),

                        new Exam("Term 2", "Dec 2025", 65, 90, 95, 60, 66),
                        new Exam("Internal 3", "Feb 2026", 66, 92, 94, 66, 90),
                        new Exam("Term 3", "Apr 2026", 66, 92, 94, 66, 90),

                    }        ,"A+ve"       , "Newton" , "Hannah"    ),

                new Student(
                    "Albert Einstein",
                    "Ulm, Kingdom of Württemberg, German Empire",
                    new DateOnly (2015,3, 14),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Internal 1", "Aug 2025", 45, 30, 35, 40, 44),
                        new Exam("Term 1", "Sep 2025", 44, 34, 34, 46, 30),
                        new Exam("Internal 2", "Nov 2025", 44, 34, 34, 46, 30),

                        new Exam("Term 2", "Dec 2025", 95, 90, 95, 90, 93),
                        new Exam("Internal 3", "Feb 2026", 33, 42, 44, 36, 40),
                        new Exam("Term 3",  "Apr 2026", 33, 42, 44, 36, 40),

                    }      ,   "B+ve"     , "Hermann"     , "Pauline"    ),

                new Student(
                    "Abdul Kalam",
                    "Rameswaram, Tamil Nadu, India",
                    new DateOnly (2014,10, 15),
                    new ObservableCollection<Exam>
                    {

                        new Exam("Internal 1", "Aug 2025", 55, 40, 65, 50, 61),
                        new Exam("Term 1", "Sep 2025", 61, 52, 74, 46, 50),
                        new Exam("Internal 2","Nov 2025", 51, 42, 64, 46, 70),


                        new Exam("Term 2", "Dec 2025", 65, 90, 95, 60, 66),
                        new Exam("Internal 3", "Feb 2026", 66, 92, 94, 66, 90),
                        new Exam("Term 3",  "Apr 2026", 66, 92, 94, 66, 90),

                    }          ,"O+ve"     , "Jainullabiddin" , "Ashiamma"        ),

                new Student(
                    "Thomas Alva Edison",
                    "Milan, Ohio, U.S.",
                    new DateOnly (2015,2, 11),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Internal 1","Aug 2025", 75, 40, 65, 50, 61),
                        new Exam("Term 1", "Sep 2025", 71, 52, 74, 46, 50),
                        new Exam("Internal 2", "Nov 2025", 51, 42, 64, 46, 70),


                        new Exam("Term 2", "Dec 2025", 15, 30, 35, 10, 11),
                        new Exam("Internal 3", "Feb 2026", 11, 34, 34, 16, 30),
                        new Exam("Term 3",  "Apr 2026", 11, 34, 34, 16, 30),


                    }      ,"AB+ve"      , "Samuel" , "Nancy"           ),

                new Student(
                    "Nikola Tesla",
                    "Smiljan, Austrian Empire",
                    new DateOnly (2015,1,7),
                    new ObservableCollection<Exam>
                    {


                        new Exam("Internal 1", "Aug 2025", 75, 70, 75, 70, 71),
                        new Exam("Term 1", "Sep 2025", 71, 74, 74, 76, 70),
                        new Exam("Internal 2", "Nov 2025", 71, 74, 74, 76, 70),

                        new Exam("Term 2", "Dec 2025", 75, 70, 75, 70, 74),
                        new Exam("Internal 3", "Feb 2026", 74, 74, 74, 76, 70),
                        new Exam("Term 3",  "Apr 2026", 74, 74, 74, 76, 70),


                    }      ,"A-ve"      , "Milutin" , "Duka"          ),

                new Student(
                    "Stephen Hawking",
                    "Oxford, England",
                    new DateOnly (2015,1,2),
                    new ObservableCollection<Exam>
                    {

                        new Exam("Internal 1", "Aug 2025", 45, 30, 35, 40, 44),
                        new Exam("Term 1", "Sep 2025", 44, 34, 34, 46, 30),
                        new Exam("Internal 2", "Nov 2025", 44, 34, 34, 46, 30),

                        new Exam("Term 2", "Dec 2025", 25, 30, 35, 20, 22),
                        new Exam("Internal 3", "Feb 2026", 22, 32, 34, 26, 30),
                        new Exam("Term 3",  "Apr 2026", 22, 32, 34, 26, 30),

                    }         ,"B-ve"    , "Frank" , "Isobel"          ),

                new Student(
                    "Marie Curie",
                    "Warsaw, Congress Poland, Russian Empire",
                    new DateOnly (2014,11, 07), // Year , M , D
                    new ObservableCollection<Exam>
                    {

                        new Exam("Internal 1", "Aug 2025", 95, 90, 95, 90, 93),
                        new Exam("Term 1", "Sep 2025", 33, 42, 44, 36, 40),
                        new Exam("Internal 2", "Nov 2025", 33, 42, 44, 36, 40),

                        new Exam("Term 2", "Dec 2025", 65, 90, 95, 60, 66),
                        new Exam("Internal 3", "Feb 2026", 66, 92, 94, 66, 90),
                        new Exam("Term 3", "Apr 2026", 66, 92, 94, 66, 90),

                    }        ,"O-ve"       , "Skłodowski" , "Bronisława"      ),

                new Student(
                    "Louis Pasteur",
                    "Dole, France",
                    new DateOnly (2014,12, 27),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Internal 1", "Aug 2025", 45, 30, 35, 40, 44),
                        new Exam("Term 1", "Sep 2025", 44, 34, 34, 46, 30),
                        new Exam("Internal 2", "Nov 2025", 44, 34, 34, 46, 30),

                        new Exam("Term 2", "Dec 2025", 95, 90, 95, 90, 93),
                        new Exam("Internal 3", "Feb 2026", 33, 42, 44, 36, 40),
                        new Exam("Term 3",  "Apr 2026", 33, 42, 44, 36, 40),

                    }      ,   "AB-ve"     , "Joseph" , "Étiennette "          ),

                new Student(
                    "Gregor Mendel",
                    "Heinzendorf bei Odrau, Silesia, Austrian Empire",
                    new DateOnly (2014,07, 20),
                    new ObservableCollection<Exam>
                    {

                        new Exam("Internal 1", "Aug 2025", 55, 40, 65, 50, 61),
                        new Exam("Term 1", "Sep 2025", 61, 52, 74, 46, 50),
                        new Exam("Internal 2","Nov 2025", 51, 42, 64, 46, 70),


                        new Exam("Term 2", "Dec 2025", 65, 90, 95, 60, 66),
                        new Exam("Internal 3", "Feb 2026", 66, 92, 94, 66, 90),
                        new Exam("Term 3",  "Apr 2026", 66, 92, 94, 66, 90),

                    }          ,"A+ve"     , "Anton " , "Rosine "         ),

                new Student(
                    "Michael Faraday",
                    "Newington Butts, Surrey, England",
                    new DateOnly (2014,09, 22),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Internal 1","Aug 2025", 55, 40, 65, 50, 61),
                        new Exam("Term 1", "Sep 2025", 61, 52, 74, 46, 50),
                        new Exam("Internal 2", "Nov 2025", 51, 42, 64, 46, 70),


                        new Exam("Term 2", "Dec 2025", 85, 80, 85, 80, 81),
                        new Exam("Internal 3", "Feb 2026", 81, 84, 84, 86, 80),
                        new Exam("Term 3",  "Apr 2026", 81, 84, 84, 86, 80),


                    }      ,"B+ve"      , "James" , "Margaret"            ),

                new Student(
                    "C. V. Raman",
                    "Tiruchirapalli, Madras Presidency, British Raj, British Empire",
                    new DateOnly (2014,11,07),
                    new ObservableCollection<Exam>
                    {


                        new Exam("Internal 1", "Aug 2025", 95, 90, 95, 90, 91),
                        new Exam("Term 1", "Sep 2025", 91, 94, 94, 96, 90),
                        new Exam("Internal 2", "Nov 2025", 91, 94, 94, 96, 90),

                        new Exam("Term 2", "Dec 2025", 95, 90, 90, 90, 94),
                        new Exam("Internal 3", "Feb 2026", 94, 94, 94, 96, 90),
                        new Exam("Term 3",  "Apr 2026", 94, 94, 94, 96, 90),


                    }      ,"O+ve"      , "Ramanathan" , "Parvathi"            ),

                new Student(
                    "James Clerk Maxwell",
                    "Cambridge, England",
                    new DateOnly (2014,06,13),
                    new ObservableCollection<Exam>
                    {

                        new Exam("Internal 1", "Aug 2025", 95, 90, 95, 90, 94),
                        new Exam("Term 1", "Sep 2025", 94, 94, 94, 96, 90),
                        new Exam("Internal 2", "Nov 2025", 94, 94, 94, 96, 90),

                        new Exam("Term 2", "Dec 2025", 95,90, 95, 90, 92),
                        new Exam("Internal 3", "Feb 2026", 92, 92, 94, 96, 90),
                        new Exam("Term 3",  "Apr 2026", 92, 92, 94, 96, 90),

                    }         ,"AB+ve"   , "Jhon"  , "Frances"         ),


                new Student(
                    "Homi Jehangir Bhabha",
                    "Bombay Presidency, British India",
                    new DateOnly (2014,10, 30),
                    new ObservableCollection<Exam>
                    {

                        new Exam("Internal 1", "Aug 2025", 95, 90, 95, 90, 93),
                        new Exam("Term 1", "Sep 2025", 33, 42, 44, 36, 40),
                        new Exam("Internal 2", "Nov 2025", 33, 42, 44, 36, 40),

                        new Exam("Term 2", "Dec 2025", 65, 90, 95, 60, 66),
                        new Exam("Internal 3", "Feb 2026", 66, 92, 94, 66, 90),
                        new Exam("Term 3", "Apr 2026", 66, 92, 94, 66, 90),

                    }        ,"A-ve"       , "Jehangir" , "Meherbai"      ),

                new Student(
                    "Ernest Rutherford",
                    "Brightwater, Nelson Province, Colony of New Zealand",
                    new DateOnly (2014,08, 30),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Internal 1", "Aug 2025", 45, 30, 35, 40, 44),
                        new Exam("Term 1", "Sep 2025", 44, 34, 34, 46, 30),
                        new Exam("Internal 2", "Nov 2025", 44, 34, 34, 46, 30),

                        new Exam("Term 2", "Dec 2025", 95, 90, 95, 90, 93),
                        new Exam("Internal 3", "Feb 2026", 33, 42, 44, 36, 40),
                        new Exam("Term 3",  "Apr 2026", 33, 42, 44, 36, 40),

                    }      ,   "B-ve"     , "James" , "Martha"          ),

                new Student(
                    "Enrico Fermi",
                    "Rome, Italy",
                    new DateOnly (2014,09, 29),
                    new ObservableCollection<Exam>
                    {

                        new Exam("Internal 1", "Aug 2025", 85, 80, 85, 80, 81),
                        new Exam("Term 1", "Sep 2025", 81, 82, 84, 86, 80),
                        new Exam("Internal 2","Nov 2025", 81, 82, 84, 86, 90),


                        new Exam("Term 2", "Dec 2025", 85, 80, 85, 80, 86),
                        new Exam("Internal 3", "Feb 2026", 86, 82, 84, 86, 90),
                        new Exam("Term 3",  "Apr 2026",86, 82, 94, 86, 90),

                    }          ,"AB-ve"     , "Alberto" , "Fermi"         ),

                new Student(
                    "J. Robert Oppenheimer",
                    "New York City, U.S.",
                    new DateOnly (2015,04, 22),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Internal 1","Aug 2025", 55, 40, 65, 50, 61),
                        new Exam("Term 1", "Sep 2025", 61, 52, 74, 46, 50),
                        new Exam("Internal 2", "Nov 2025", 51, 42, 64, 46, 70),


                        new Exam("Term 2", "Dec 2025", 15, 30, 35, 10, 11),
                        new Exam("Internal 3", "Feb 2026", 11, 34, 34, 16, 30),
                        new Exam("Term 3",  "Apr 2026", 11, 34, 34, 16, 30),


                    }      ,"O-ve"      , "Seligmann" , "Ella"            ),

                new Student(
                    "J. J. Thomson",
                    
                    "Manchester, England, UK",
                    new DateOnly (2014,12,18),
                    new ObservableCollection<Exam>
                    {


                        new Exam("Internal 1", "Aug 2025", 75, 70, 75, 70, 71),
                        new Exam("Term 1", "Sep 2025", 71, 74, 74, 76, 70),
                        new Exam("Internal 2", "Nov 2025", 71, 74, 74, 76, 70),

                        new Exam("Term 2", "Dec 2025", 75, 70, 75, 70, 74),
                        new Exam("Internal 3", "Feb 2026", 74, 74, 74, 76, 70),
                        new Exam("Term 3",  "Apr 2026", 74,74, 74, 76, 70),


                    }      ,"A+ve"      , "Joseph" , "Emma"            ),

                new Student(
                    "Aristotle",
                    
                    "Stagira, Chalcidian League",
                    new DateOnly (2015,1,2),
                    new ObservableCollection<Exam>
                    {

                        new Exam("Internal 1", "Aug 2025", 45, 30, 35, 40, 44),
                        new Exam("Term 1", "Sep 2025", 44, 34, 34, 46, 30),
                        new Exam("Internal 2", "Nov 2025", 44, 34, 34, 46, 30),

                        new Exam("Term 2", "Dec 2025", 25, 30, 35, 20, 22),
                        new Exam("Internal 3", "Feb 2026", 22, 32, 34, 26, 30),
                        new Exam("Term 3",  "Apr 2026", 22, 32, 34, 26, 30),

                    }         ,"B+ve"    , "Nicomachus" , "Phaestis"           ),

                             
                new Student(
                    "Plato",
                    "Athens",
                    new DateOnly (2015,04, 22),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Internal 1","Aug 2025", 55, 40, 65, 50, 61),
                        new Exam("Term 1", "Sep 2025", 61, 52, 74, 46, 50),
                        new Exam("Internal 2", "Nov 2025", 51, 42, 64, 46, 70),

                        new Exam("Term 2", "Dec 2025", 15, 30, 35, 10, 11),
                        new Exam("Internal 3", "Feb 2026", 11, 34, 34, 16, 30),
                        new Exam("Term 3",  "Apr 2026", 11, 34, 34, 16, 30),


                    }      ,"AB+ve"      , "Athenian " , "Perictione"            ),

            });

            ViewModel.SetRank();
            ViewModel.SetRollNumber();

            this.DataContext = ViewModel;
        }

        private void StudentData_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (sender is Border SelectedBorder)
                {
                    Point Point = SelectedBorder.TranslatePoint(new Point(0, 0), StudentsScroll);

                    ViewModel.FromMargin = new Thickness(50, Point.Y, 50, StudentsScroll.ActualHeight - (40 + Point.Y));

                    SingleStudentDetailBorder.Visibility = Visibility.Visible;
                    SingleStudentAni = this.Resources["SingleStudentAni"] as Storyboard;
                    SingleStudentAni.Begin();

                    ViewModel.SelectedStudent = (sender as Border).DataContext as Student;

                    ViewModel.IsPersonalInfo = true;
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

        private bool myVar;

        public bool MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private bool _IsExaminations ;

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


        public void SetRank()
        {
            List<Student> TempStudents = Students.OrderByDescending(k => k.Percentage).ToList();

            foreach (Student student in Students)
            {
                student.Rank = TempStudents.IndexOf(student) + 1;
            }
        }

        public void SetRollNumber()
        {
            List<Student> TempStudents = Students.OrderBy(k => k.Name).ToList();

            Students.Clear();

            for(int i = 0; i < TempStudents.Count; i++)
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

    public class Student
    {
        public string Name { get; set; }
        public string RollNumber { get; set; }
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
        public Student(string name, string address, DateOnly dateOfBirth, ObservableCollection<Exam> exams, string bloodGroup, string fatherName , string motherName)
        {
            Name = name;
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
            MotherName = motherName;
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


}

