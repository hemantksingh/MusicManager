using System.Windows;
using Autofac;

namespace MusicManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IContainer  container = BuildAutofacContainer();
            this.DataContext = container.Resolve<MusicManagerViewModel>();
        }

        private IContainer BuildAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<MusicManagerModule>();

            return builder.Build();
        }
    }
}
