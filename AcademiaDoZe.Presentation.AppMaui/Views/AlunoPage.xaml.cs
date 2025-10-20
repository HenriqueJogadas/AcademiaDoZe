//Henrique Churkin Correia Alberton

using AcademiaDoZe.Presentation.AppMaui.ViewModels;
namespace AcademiaDoZe.Presentation.AppMaui.Views;
public partial class AlunoPage : ContentPage
{
    public AlunoPage(ColaboradorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AlunoViewModel viewModel)

        {
            await viewModel.InitializeAsync();
        }
    }
}