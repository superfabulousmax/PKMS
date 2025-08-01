using System.Configuration;
using System.Data;
using System.Windows;
using Prism.Unity;

namespace WPFNotesApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.Register<Services.INoteStore, Services.DbNoteStore>();
        containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
        // register other needed services here
    }

    protected override Window CreateShell()
    {
        var w = Container.Resolve<MainWindow>();
        return w;
    }
}

