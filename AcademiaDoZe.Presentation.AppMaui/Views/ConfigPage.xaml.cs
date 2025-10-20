//Henrique Churkin Correia Alberton

using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Presentation.AppMaui.Message;
using CommunityToolkit.Mvvm.Messaging;
namespace AcademiaDoZe.Presentation.AppMaui.Views;
public partial class ConfigPage : ContentPage
{
    private VisualElement[] _focusOrder = [];
    public ConfigPage()
    {
        InitializeComponent();
        CarregarTema();
        CarregarBanco();

        TemaPicker.SelectedIndexChanged += OnSalvarTemaClicked;

        _focusOrder = [
        DatabaseTypePicker, ServidorEntry, BancoEntry, UsuarioEntry, SenhaEntry, ComplementoEntry ];
    }
    private void CarregarTema()
    {
        // uso de express�o switch para carregar o �ndice selecionado
        TemaPicker.SelectedIndex = Preferences.Get("Tema", "system") switch { "light" => 0, "dark" => 1, _ => 2, };
    }
    private async void OnSalvarTemaClicked(object sender, EventArgs e)
    {
        string selectedTheme = TemaPicker.SelectedIndex switch { 0 => "light", 1 => "dark", _ => "system" };
        Preferences.Set("Tema", selectedTheme);
        // Disparar mensagem para uso na recarga din�mica
        WeakReferenceMessenger.Default.Send(new TemaPreferencesUpdatedMessage("TemaAlterado"));
        await DisplayAlert("Sucesso", "Dados salvos com sucesso!", "OK");
        // Navegar para dashboard
        await Shell.Current.GoToAsync("//dashboard");
    }
    // Banco de Dados
    private void CarregarBanco()
    {
        foreach (var tipo in Enum.GetValues<EAppDatabaseType>())
        {
            DatabaseTypePicker.Items.Add(tipo.ToString());
        }
        // Carregar os dados existentes, ou valores padr�o, ao abrir a p�gina
        ServidorEntry.Text = Preferences.Get("Servidor", "172.24.32.1");
        BancoEntry.Text = Preferences.Get("Banco", "db_academia_do_ze");
        UsuarioEntry.Text = Preferences.Get("Usuario", "sa");
        SenhaEntry.Text = Preferences.Get("Senha", "abcBolinhas12345");
        ComplementoEntry.Text = Preferences.Get("Complemento", "TrustServerCertificate=True;Encrypt=True;");
        DatabaseTypePicker.SelectedItem = Preferences.Get("DatabaseType", EAppDatabaseType.SqlServer.ToString());
    }
    private async void OnSalvarBdClicked(object sender, EventArgs e)
    {
        Preferences.Set("Servidor", ServidorEntry.Text);
        Preferences.Set("Banco", BancoEntry.Text);
        Preferences.Set("Usuario", UsuarioEntry.Text);
        Preferences.Set("Senha", SenhaEntry.Text);
        Preferences.Set("Complemento", ComplementoEntry.Text);
        Preferences.Set("DatabaseType", DatabaseTypePicker.SelectedItem.ToString());
        // Disparar a mensagem para recarga din�mica
        WeakReferenceMessenger.Default.Send(new BancoPreferencesUpdatedMessage("BancoAlterado"));
        await DisplayAlert("Sucesso", "Dados salvos com sucesso!", "OK");
        // Navegar para dashboard
        await Shell.Current.GoToAsync("//dashboard");
    }
    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        // retornar para dashboard
        await Shell.Current.GoToAsync("//dashboard");
    }
    // Ao fechar a p�gina, chama WeakReferenceMessenger.Default.UnregisterAll(this); para evitar vazamentos de mem�ria - memory leaks
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Desinscreve o mensageiro para evitar memory leaks
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    private void OnEntryCompleted(object sender, EventArgs e)
    {
        if (sender is not VisualElement current)
            return;
        int idx = Array.IndexOf(_focusOrder, current);
        if (idx >= 0)
        {
            if (idx < _focusOrder.Length - 1)
            {
                // foca o próximo controle
                _focusOrder[idx + 1].Focus();
            }
            else
            {
                // último item -> submete
                OnSalvarBdClicked(sender, e);
            }
        }
        else
        {
            // fallback simples: avançar para o primeiro focável se não estiver na lista
            _focusOrder.FirstOrDefault()?.Focus();
        }
    }
}