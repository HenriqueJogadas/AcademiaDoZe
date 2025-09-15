//Henrique Churkin Correia Alberton

using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using CommunityToolkit.Mvvm.Input;
namespace AcademiaDoZe.Presentation.AppMaui.ViewModels
{
    [QueryProperty(nameof(LogradouroId), "Id")]
    public partial class LogradouroViewModel : BaseViewModel
    {
        private readonly ILogradouroService _logradouroService;
        private LogradouroDTO _logradouro = new()
        {
            Cep = string.Empty,
            Nome = string.Empty,
            Bairro = string.Empty,
            Cidade = string.Empty,
            Estado = string.Empty,
            Pais = string.Empty
        };
        public LogradouroDTO Logradouro
        {
            get => _logradouro;

            set => SetProperty(ref _logradouro, value);

        }
        private int _logradouroId;
        public int LogradouroId
        {
            get => _logradouroId;

            set
            {
                if (SetProperty(ref _logradouroId, value))
                {
                    // Quando o LogradouroId é alterado, inicializa os dados

                    Task.Run(InitializeAsync);

                }
            }
        }
        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;

            set => SetProperty(ref _isEditMode, value);

        }
        public LogradouroViewModel(ILogradouroService logradouroService)
        {
            _logradouroService = logradouroService;
            Title = "Detalhes do Logradouro";
        }
        public async Task InitializeAsync()
        {
            if (LogradouroId > 0)
            {
                IsEditMode = true;
                Title = "Editar Logradouro";
                await LoadLogradouroAsync();
            }
            else
            {
                IsEditMode = false;
                Title = "Novo Logradouro";
            }
        }
        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
        [RelayCommand]
        private async Task LoadLogradouroAsync()
        {
            if (LogradouroId <= 0)
                return;
            try
            {
                IsBusy = true;
                var logradouroData = await _logradouroService.ObterPorIdAsync(LogradouroId);

                if (logradouroData != null)

                {
                    Logradouro = logradouroData;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar logradouro: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private async Task SearchByCepAsync()
        {
            if (string.IsNullOrWhiteSpace(Logradouro.Cep))
                return;
            try
            {
                IsBusy = true;
                var logradouroData = await _logradouroService.ObterPorCepAsync(Logradouro.Cep);

                if (logradouroData != null)

                {
                    Logradouro = logradouroData;
                    IsEditMode = true;
                    await Shell.Current.DisplayAlert("Aviso", "CEP já cadastrado! Dados carregados para edição.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Aviso", "CEP não encontrado.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao buscar CEP: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private async Task SaveLogradouroAsync()
        {
            if (IsBusy)
                return;
            if (!ValidateLogradouro(Logradouro))
                return;
            try
            {
                IsBusy = true;
                if (IsEditMode)
                {
                    await _logradouroService.AtualizarAsync(Logradouro);

                    await Shell.Current.DisplayAlert("Sucesso", "Logradouro atualizado com sucesso!", "OK");

                }
                else
                {
                    await _logradouroService.AdicionarAsync(Logradouro);

                    await Shell.Current.DisplayAlert("Sucesso", "Logradouro criado com sucesso!", "OK");

                }
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao salvar logradouro: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private static bool ValidateLogradouro(LogradouroDTO logradouro)
        {
            const string validationTitle = "Validação";
            if (string.IsNullOrWhiteSpace(logradouro.Cep))
            {
                Shell.Current.DisplayAlert(validationTitle, "CEP é obrigatório.", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(logradouro.Nome))
            {
                Shell.Current.DisplayAlert(validationTitle, "Nome é obrigatório.", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(logradouro.Bairro))
            {
                Shell.Current.DisplayAlert(validationTitle, "Bairro é obrigatório.", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(logradouro.Cidade))
            {
                Shell.Current.DisplayAlert(validationTitle, "Cidade é obrigatória.", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(logradouro.Estado))
            {
                Shell.Current.DisplayAlert(validationTitle, "Estado é obrigatório.", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(logradouro.Pais))
            {
                Shell.Current.DisplayAlert(validationTitle, "País é obrigatório.", "OK");
                return false;
            }
            return true;
        }
    }
}
