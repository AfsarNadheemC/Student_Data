using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
                        new Exam("Term 1", new DateTime(2023, 10, 15), 85, 90, 95, 80, 88, 100,  500),
                        new Exam("Term 2", new DateTime(2023, 12, 20), 88, 92, 94, 86, 90, 100,  500),
                        new Exam("Term 3", new DateTime(2023, 12, 20), 88, 92, 94, 86, 90, 100,  500),
                    }                    ),

                new Student(
                    "Albert Einstein",
                    "AD07C001",
                    "Ulm, Kingdom of Württemberg, German Empire",
                    new DateTime (2015,3, 14),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Term 1", new DateTime(2023, 10, 15), 85, 90, 95, 80, 88, 100,  500),
                        new Exam("Term 2", new DateTime(2023, 12, 20), 88, 92, 94, 86, 90, 100,  500),
                        new Exam("Term 3", new DateTime(2023, 12, 20), 88, 92, 94, 86, 90, 100,  500),
                    }                    ),

                new Student(
                    "Abdul Kalam",
                    "AD07C001",
                    "Rameswaram, Tamil Nadu, India",
                    new DateTime (2014,10, 15),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Term 1", new DateTime(2023, 10, 15), 85, 90, 95, 80, 88, 100,  500),
                        new Exam("Term 2", new DateTime(2023, 12, 20), 88, 92, 94, 86, 90, 100,  500),
                        new Exam("Term 3", new DateTime(2023, 12, 20), 88, 92, 94, 86, 90, 100,  500),
                    }                    ),

                new Student(
                    "Thomas Alva Edison",
                    "AD07C001",
                    "Milan, Ohio, U.S.",
                    new DateTime (2015,2, 11),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Term 1", new DateTime(2023, 10, 15), 85, 90, 95, 80, 88, 100,  500),
                        new Exam("Term 2", new DateTime(2023, 12, 20), 88, 92, 94, 86, 90, 100,  500),
                        new Exam("Term 3", new DateTime(2023, 12, 20), 88, 92, 94, 86, 90, 100,  500),
                    }                    ),

                new Student(
                    "Nikola Tesla",
                    "AD07C001",
                    "Smiljan, Austrian Empire",
                    new DateTime (2015,1,7),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Term 1", new DateTime(2023, 10, 15), 85, 90, 95, 80, 88, 100,  500),
                        new Exam("Term 2", new DateTime(2023, 12, 20), 88, 92, 94, 86, 90, 100,  500),
                        new Exam("Term 3", new DateTime(2023, 12, 20), 88, 92, 94, 86, 90, 100,  500),
                    }                    ),

                new Student(
                    "Stephen Hawking",
                    "AD07C001",
                    "Oxford, England",
                    new DateTime (2015,1,8),
                    new ObservableCollection<Exam>
                    {
                        new Exam("Term 1", new DateTime(2023, 10, 15), 85, 90, 95, 80, 88, 100,  500),
                        new Exam("Term 2", new DateTime(2023, 12, 20), 88, 92, 94, 86, 90, 100,  500),
                        new Exam("Term 3", new DateTime(2023, 12, 20), 88, 92, 94, 86, 90, 100,  500),
                    }                    ),

            });

        }
    }

    public class ViewModel
    {
        public ObservableCollection<Student> Students { get; set; }

        public ViewModel(ObservableCollection<Student> students)
        {
            Students = students;
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public string RollNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ObservableCollection<Exam> Exams { get; set; }

        public Student(string name, string rollNumber, string address, DateTime dateOfBirth, ObservableCollection<Exam> exams)
        {
            Name = name;
            RollNumber = rollNumber;
            Address = address;
            DateOfBirth = dateOfBirth;
            Exams = exams;
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

        public Exam(string name, DateTime date, int language1, int language2, int maths, int science, int socialStudies, int subjectMaxMark, int totalMarks)
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
            TotalMarks = totalMarks;
        }
    }
}

