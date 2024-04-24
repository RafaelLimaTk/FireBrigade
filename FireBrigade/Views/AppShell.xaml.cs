namespace FireBrigade.Views;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("login/register", typeof(Views.Login.LoginPage));
    }
}