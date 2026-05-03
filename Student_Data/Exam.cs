using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Data
{

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

}
