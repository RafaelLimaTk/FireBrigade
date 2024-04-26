namespace FireBrigade.Views;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("login/register", typeof(Views.Login.RigisterPage));
		Routing.RegisterRoute("home", typeof(Views.Home.HomePage));
        Routing.RegisterRoute("home/brigadeemergency", typeof(Views.BrigadeEmergency.BrigadeEmergencyPage));
		Routing.RegisterRoute("home/listbrigadeemergency", typeof(Views.BrigadeEmergency.ListBrigadeEmergencyPage));
    }
}