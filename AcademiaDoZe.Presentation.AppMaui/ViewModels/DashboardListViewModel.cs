//Henrique Churkin Correia Alberton

using AcademiaDoZe.Application.Interfaces;
using CommunityToolkit.Mvvm.Input;
namespace AcademiaDoZe.Presentation.AppMaui.ViewModels
{
    public partial class DashboardListViewModel : BaseViewModel
    {
        private readonly ILogradouroService _logradouroService;
        private readonly IAlunoService _alunoService;
        private readonly IColaboradorService _colaboradorService;
        private readonly IMatriculaService _matriculaService;
        private int _totalLogradouros;
        public int TotalLogradouros { get => _totalLogradouros; set => SetProperty(ref _totalLogradouros, value); }
        private int _totalAlunos;
        public int TotalAlunos { get => _totalAlunos; set => SetProperty(ref _totalAlunos, value); }
        private int _totalColaboradores;
        public int TotalColaboradores { get => _totalColaboradores; set => SetProperty(ref _totalColaboradores, value); }
        private int _totalMatriculas;
        public int TotalMatriculas { get => _totalMatriculas; set => SetProperty(ref _totalMatriculas, value); }
        public DashboardListViewModel(ILogradouroService logradouroService, IAlunoService alunoService, IColaboradorService colaboradorService, IMatriculaService matriculaService)
        {
            _logradouroService = logradouroService;
            _alunoService = alunoService;
            _colaboradorService = colaboradorService;
            _matriculaService = matriculaService;
            Title = "Dashboard";
        }
        [RelayCommand]
        private async Task LoadDashboardDataAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                var logradourosTask = _logradouroService.ObterTodosAsync();
                var logradouros = new List<object>();
                try { logradouros = (await logradourosTask).ToList<object>(); }
                catch (Exception ex) { await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar logradouros: {ex.Message}", "OK"); }
                TotalLogradouros = logradouros.Count;
                var alunosTask = _alunoService.ObterTodosAsync();
                var alunos = new List<object>();
                try { alunos = (await alunosTask).ToList<object>(); }
                catch (Exception ex) { await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar alunos: {ex.Message}", "OK"); }
                TotalAlunos = alunos.Count;
                var colaboradoresTask = _colaboradorService.ObterTodosAsync();
                var colaboradores = new List<object>();
                try { colaboradores = (await colaboradoresTask).ToList<object>(); }
                catch (Exception ex) { await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar colaboradores: {ex.Message}", "OK"); }
                TotalColaboradores = colaboradores.Count;
                var matriculasTask = _matriculaService.ObterTodasAsync();
                var matriculas = new List<object>();
                try { matriculas = (await matriculasTask).ToList<object>(); }
                catch (Exception ex) { await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar matrículas: {ex.Message}", "OK"); }
                TotalMatriculas = matriculas.Count;
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private async Task NavigateToLogradourosAsync() => await Shell.Current.GoToAsync("//logradouros");
        [RelayCommand]
        private async Task NavigateToAlunosAsync() => await Shell.Current.GoToAsync("//alunos");
        [RelayCommand]
        private async Task NavigateToColaboradoresAsync() => await Shell.Current.GoToAsync("//colaboradores");
        [RelayCommand]
        private async Task NavigateToMatriculasAsync() => await Shell.Current.GoToAsync("//matriculas");
    }
}
