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

namespace FinalProjectResumeMaker
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

        private void ResumeInformationButton_Click(object sender, RoutedEventArgs e)
        {
            string message = "You can load or create your resume. You can also convert it to pdf.";
            string title = "Resume Builder Information";
            MessageBox.Show(message, title);
        }
    }
}