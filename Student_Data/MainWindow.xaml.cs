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
        public MainWindow()
        {
            InitializeComponent();
        }

        public class Exam
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public float Language1 { get; set; }
            public float Language2 { get; set; }
            public float Maths { get; set; }
            public float Science { get; set; }
            public float SocialStudies { get; set; }
            public float SubjectMaxMark { get; set; }
            public float TotalScored { get; set; }
            public float TotalMarks { get; set; }
        }

    }
}