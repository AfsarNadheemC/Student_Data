using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Student_Data
{

    public class ViewModel : INotifyPropertyChanged
    {

        private string _Search;

        public string Search
        {
            get { return _Search; }
            set { _Search = value; StudentsView.Refresh(); }
        }


        public string SchoolName { get; set; }
        public string TeacherName { get; set; }
        public string Class { get; set; }
        private double _Percentage;

        public double Percentage
        {
            get { return _Percentage; }
            set { _Percentage = value; }
        }
        public ICollectionView StudentsView { get; set; }


        private ObservableCollection<Student> _Students;
        public ObservableCollection<Student> Students
        {
            get => _Students;
            set
            {
                _Students = value;
                OnPropertyChanged(nameof(Students));
                StudentsView = CollectionViewSource.GetDefaultView(Students);
                StudentsView.Filter = StudentsFilter;
                OnPropertyChanged(nameof(StudentsView));
            }
        }


        private Student? _SelectedStudent;

        public Student SelectedStudent
        {
            get { return _SelectedStudent; }
            set { _SelectedStudent = value; OnPropertyChanged(nameof(SelectedStudent)); }
        }

        public ObservableCollection<string> BloodGroupList { get; set; }

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

        public ViewModel(ObservableCollection<Student> students)
        {
            SchoolName = "Springfield High School";
            TeacherName = "Galileo Galilei";
            Class = "5B";
            Students = students;
            BloodGroupList = new ObservableCollection<string> { "A+ve", "A-ve", "B+ve", "B-ve", "AB+ve", "AB-ve", "O+ve", "O-ve" };
        }



        public bool StudentsFilter(object obj)
        {
            if (obj is Student student)
            {
                if (string.IsNullOrWhiteSpace(Search))
                {
                    return true;
                }
                else
                {
                    return student.Name.IndexOf(Search, StringComparison.OrdinalIgnoreCase) > -1;
                }
            }
            return false;
        }

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

}
