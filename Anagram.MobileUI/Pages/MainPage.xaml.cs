using Anagram.MobileUI.Models;
using Anagram.MobileUI.PageModels;

namespace Anagram.MobileUI.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}