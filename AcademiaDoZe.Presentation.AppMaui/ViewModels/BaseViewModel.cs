//Henrique Churkin Correia Alberton

using CommunityToolkit.Mvvm.ComponentModel;
namespace AcademiaDoZe.Presentation.AppMaui.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        private bool isBusy; // Indica se uma operação está em andamento, útil para mostrar indicadores de carregamento na UI.
        public bool IsBusy
        {
            get => isBusy;

            set => SetProperty(ref isBusy, value);

        }
        private string title = string.Empty; // Título da ViewModel, pode ser usado para definir o título da página na UI.
        public string Title
        {
            get => title;

            set => SetProperty(ref title, value);

        }
        private bool isRefreshing; // Indica se a ViewModel está em estado de atualização, útil para pull-to-refresh na UI.
        public bool IsRefreshing
        {
            get => isRefreshing;

            set => SetProperty(ref isRefreshing, value);

        }
    }
    // ObservableObject é uma classe base que implementa a interface INotifyPropertyChanged.
    // Permitindo que as propriedades notifiquem automaticamente quando seus valores mudem.
    // Isso é útil para atualizar a interface do usuário em resposta a mudanças nos dados.
    // O atributo [ObservableProperty] é um recurso do CommunityToolkit.Mvvm que simplifica a criação de propriedades observáveis.
}
