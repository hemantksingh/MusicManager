using System.Windows;

namespace MusicManager
{
    public interface IMainView
    {
        void Show();
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        public MainWindow(MainViewModel dataContext)
        {
            InitializeComponent();
            this.DataContext = dataContext;
        }
    }
}
