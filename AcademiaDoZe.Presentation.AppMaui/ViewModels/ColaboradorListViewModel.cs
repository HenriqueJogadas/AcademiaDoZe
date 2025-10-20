//Henrique Churkin Correia Alberton

using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
namespace AcademiaDoZe.Presentation.AppMaui.ViewModels
{
    public partial class ColaboradorListViewModel : BaseViewModel
    {
        public ObservableCollection<string> FilterTypes { get; } = new() { "Id", "CPF" };
        private readonly IColaboradorService _colaboradorService;
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }
        private string _selectedFilterType = "CPF";
        public string SelectedFilterType
        {
            get => _selectedFilterType;
            set => SetProperty(ref _selectedFilterType, value);
        }
        private ObservableCollection<ColaboradorDTO> _colaboradores = new();
        public ObservableCollection<ColaboradorDTO> Colaboradores
        {
            get => _colaboradores;
            set => SetProperty(ref _colaboradores, value);
        }
        private ColaboradorDTO? _selectedColaborador;
        public ColaboradorDTO? SelectedColaborador
        {
            get => _selectedColaborador;
            set => SetProperty(ref _selectedColaborador, value);
        }
        public ColaboradorListViewModel(IColaboradorService colaboradorService)
        {
            _colaboradorService = colaboradorService;
            Title = "Colaboradores";
        }
        [RelayCommand]
        private async Task AddColaboradorAsync()
        {
            try
            {
                await Shell.Current.GoToAsync("colaborador");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao navegar para tela de cadastro: {ex.Message}", "OK");
            }
        }
        [RelayCommand]
        private async Task EditColaboradorAsync(ColaboradorDTO colaborador)
        {
            try
            {
                if (colaborador == null)
                    return;
                await Shell.Current.GoToAsync($"colaborador?Id={colaborador.Id}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao navegar para tela de edição: {ex.Message}", "OK");
            }
        }
        [RelayCommand]
        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            await LoadColaboradoresAsync();
        }
        [RelayCommand]
        private async Task SearchColaboradoresAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                // Limpa a lista atual

                await MainThread.InvokeOnMainThreadAsync(() =>

                {
                    Colaboradores.Clear();
                });
                IEnumerable<ColaboradorDTO> resultados = Enumerable.Empty<ColaboradorDTO>();
                // Busca os colaboradores de acordo com o filtro
                if (string.IsNullOrWhiteSpace(SearchText))

                {
                    resultados = await _colaboradorService.ObterTodosAsync() ?? Enumerable.Empty<ColaboradorDTO>();
                }
                else if (SelectedFilterType == "Id" && int.TryParse(SearchText, out int id))
                {
                    var colaborador = await _colaboradorService.ObterPorIdAsync(id);

                    if (colaborador != null)

                        resultados = new[] { colaborador };
                }
                else if (SelectedFilterType == "CPF")
                {
                    var colaborador = await _colaboradorService.ObterPorCpfAsync(SearchText);

                    if (colaborador != null)

                        resultados = new[] { colaborador };
                }
                // Atualiza a coleção na thread principal

                await MainThread.InvokeOnMainThreadAsync(() =>

                {
                    foreach (var item in resultados)
                    {
                        Colaboradores.Add(item);
                    }
                    OnPropertyChanged(nameof(Colaboradores));
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao buscar colaboradores: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private async Task LoadColaboradoresAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                // Limpa a lista atual antes de carregar novos dados
                await MainThread.InvokeOnMainThreadAsync(() =>

                {
                    Colaboradores.Clear();
                    OnPropertyChanged(nameof(Colaboradores));
                });
                var colaboradoresList = await _colaboradorService.ObterTodosAsync();
                if (colaboradoresList != null)
                {
                    // Garantir que a atualização da UI aconteça na thread principal

                    await MainThread.InvokeOnMainThreadAsync(() =>

                    {
                        foreach (var colaborador in colaboradoresList)
                        {
                            Colaboradores.Add(colaborador);
                        }
                        OnPropertyChanged(nameof(Colaboradores));
                    });
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar colaboradores: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }
        [RelayCommand]
        private async Task DeleteColaboradorAsync(ColaboradorDTO colaborador)
        {
            if (colaborador == null)
                return;
            bool confirm = await Shell.Current.DisplayAlert(
            "Confirmar Exclusão",

            $"Deseja realmente excluir o colaborador {colaborador.Nome}?",
            "Sim", "Não");
            if (!confirm)
                return;
            try
            {
                IsBusy = true;
                bool success = await _colaboradorService.RemoverAsync(colaborador.Id);
                if (success)
                {
                    Colaboradores.Remove(colaborador);
                    await Shell.Current.DisplayAlert("Sucesso", "Colaborador excluído com sucesso!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erro", "Não foi possível excluir o colaborador.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao excluir colaborador: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
