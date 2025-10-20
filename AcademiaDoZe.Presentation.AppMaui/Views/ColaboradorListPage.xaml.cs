//Henrique Churkin Correia Alberton

using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Presentation.AppMaui.ViewModels;
namespace AcademiaDoZe.Presentation.AppMaui.Views;
public partial class ColaboradorListPage : ContentPage
{
    public ColaboradorListPage(ColaboradorListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ColaboradorListViewModel viewModel)
        {
            await viewModel.LoadColaboradoresCommand.ExecuteAsync(null);
        }
    }
    private async void OnEditButtonClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button && button.BindingContext is ColaboradorDTO colaborador && BindingContext is ColaboradorListViewModel viewModel)
            {
                await viewModel.EditColaboradorCommand.ExecuteAsync(colaborador);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao editar colaborador: {ex.Message}", "OK");
        }
    }
    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button && button.BindingContext is ColaboradorDTO colaborador && BindingContext is ColaboradorListViewModel viewModel)
            {
                await viewModel.DeleteColaboradorCommand.ExecuteAsync(colaborador);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao excluir colaborador: {ex.Message}", "OK");
        }
    }
}