using EmotionApp.MVVM;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EmotionApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel _model;


        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }


        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _model = new MainPageViewModel();
            _model.OnError += model_OnError;
            DataContext = _model;
        }


        private async void model_OnError(object sender, string e)
        {
            var messageDialog = new MessageDialog(e, "Oops?");
            messageDialog.Commands.Add(new UICommand("Ok"));
            await messageDialog.ShowAsync();
        }
    }
}
