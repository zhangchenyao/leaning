using CommonTools;
using MenuManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SericeTool;
using SericeTool.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WaveEditorModel.VModel;
using WaveformGraphEditorView;

namespace WaveformGraphEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private async void MainStar(object sender,StartupEventArgs e)
        {
            IniService();
            var win = new MainWindow();
            win.Show();
            //var win = new Window2();
            //win.Show();
            //var win = new Window3();
            //win.ShowDialog();
            //var win = new IniEditorParameter();
            //IniEditorParameterModel iniEditorParameterModel = new IniEditorParameterModel();
            //win.DataContext = iniEditorParameterModel;




        }

        public static async Task IniService()
        {
            try
            {
                using IHost host = CreateHostBuilder().Build();
               
                var services = ServiceHelper.GetInstance(host.Services).GetServiceProvider();
                await host.RunAsync();
            }
            catch (Exception e)
            {
            }
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
              .ConfigureServices((_, services) =>  services.AddSingleton<MenusManagement>().AddScoped<ToolsService>());
                  
        }

    }
}
