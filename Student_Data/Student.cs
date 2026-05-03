using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Student_Data
{

    public class Student : INotifyPropertyChanged
    {

        private string _Name;
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }


        private int _RollNumber;
        public int RollNumber
        {
            get => _RollNumber;
            set
            {
                _RollNumber = value;
                OnPropertyChanged(nameof(RollNumber));
            }
        }


        private string _Address;
        public string Address
        {
            get => _Address;
            set
            {
                _Address = value;
                OnPropertyChanged(nameof(Address));
            }
        }


        private DateTime _DateOfBirth;
        public DateTime DateOfBirth
        {
            get => _DateOfBirth;
            set
            {
                _DateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
            }
        }

        public ObservableCollection<Exam> Exams { get; set; }


        private string _BloodGroup;
        public string BloodGroup
        {
            get => _BloodGroup;
            set
            {
                _BloodGroup = value;
                OnPropertyChanged(nameof(BloodGroup));
            }
        }

        private float _Percentage;

        public float Percentage
        {
            get { return _Percentage; }
            set
            {
                _Percentage = value;
                OnPropertyChanged(nameof(Percentage));
                OnPropertyChanged(nameof(ProgressColor));
            }
        }


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

        private string _FatherName;
        public string FatherName
        {
            get => _FatherName;
            set
            {
                _FatherName = value;
                OnPropertyChanged(nameof(FatherName));
            }
        }


        private string _MotherName;
        public string MotherName
        {
            get => _MotherName;
            set
            {
                _MotherName = value;
                OnPropertyChanged(nameof(MotherName));
            }
        }


        private int _Rank;

        public int Rank
        {
            get { return _Rank; }
            set { _Rank = value; OnPropertyChanged(nameof(Rank)); }
        }


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
            set { _IsReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); OnPropertyChanged(nameof(IsReadOnlyNeg)); }
        }


        public bool IsReadOnlyNeg => !IsReadOnly;

        public bool IsAnyExamMissing
        {
            get
            {
                foreach (Exam exam in Exams)
                {
                    if (exam.GetPercentage() == 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public Student(int RollNo)
        {

            Name = "";
            FatherName = "";
            MotherName = "";
            BloodGroup = "";
            Address = "";

            RollNumber = RollNo;

            OnPropertyChanged(nameof(RollNumber));

            DateOfBirth = new DateTime(2013, 1, 1);
            IsReadOnly = false;

            Exams = new ObservableCollection<Exam>
            {
                new Exam ("Internal1" , "10/07/2026" ,0     ,0,0,0,0 ),
                new Exam ("Term1" , "29/08/2026" ,0     ,0,0,0,0 ),

                new Exam ("Internal2" , "01/11/2026" ,0     ,0,0,0,0  ),
                new Exam ("Term2" , "15/12/2026" ,0     ,0,0,0,0  ),

                new Exam ("Internal3" , "20/02/2027" ,0     ,0,0,0,0 ),
                new Exam ("Term3" , "10/04/2027",0     ,0,0,0,0  ),
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

                new Exam ("Internal1" , "10/07/2015" ,(int) Internal1Objects[1] ,(int)  Internal1Objects[2] , (int) Internal1Objects[3] , (int) Internal1Objects[4] ,  (int)Internal1Objects[5] ),
                new Exam ("Term1" , "29/08/2015" ,(int) Term1Objects[1] ,(int)  Term1Objects[2] , (int) Term1Objects[3] , (int) Term1Objects[4] ,  (int)Term1Objects[5] ),

                new Exam ("Internal2" , "01/11/2015" ,(int) Internal2Objects[1] ,(int)  Internal2Objects[2] , (int) Internal2Objects[3] , (int) Internal2Objects[4] ,  (int)Internal2Objects[5] ),
                new Exam ("Term2" , "15/12/2015" ,(int) Term2Objects[1] ,(int)  Term2Objects[2] , (int) Term2Objects[3] , (int) Term2Objects[4] ,  (int)Term2Objects[5] ),

                new Exam ("Internal3" , "20/02/2015" ,(int) Internal3Objects[1] ,(int)  Internal3Objects[2] , (int) Internal3Objects[3] , (int) Internal3Objects[4] ,  (int)Internal3Objects[5] ),
                new Exam ("Term3" , "10/04/2015" ,(int) Term3Objects[1] ,(int)  Term3Objects[2] , (int) Term3Objects[3] , (int) Term3Objects[4] ,  (int)Term3Objects[5] ),

            ];




            SetPercentage();

            IsReadOnly = true;
            (LabelColorDark, LabelColorDim) = CommonColors.GetRandomColor();
        }

        public void SetPercentage()
        {
            float TotalPercentage = 0f;

            foreach (Exam exam in Exams)
            {
                TotalPercentage += exam.GetPercentage();
            }

            Percentage = TotalPercentage / Exams.Count;
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



}
