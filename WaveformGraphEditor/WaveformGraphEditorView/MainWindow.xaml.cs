using AvalonDock.Layout;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WaveformGraphEditor.VModel;
using WaveformGraphEditor.Views;
using WaveEditorModel.VModel.VModel;
using SericeTool;
using Microsoft.Extensions.DependencyInjection;
using MenuManagement;
using WaveformGraphEditorView;
using WaveEditorModel.VModel;

namespace WaveformGraphEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public HeavyLoad HeavyLoad { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            MainWindowMode mainWindowMode = new();
            mainWindowMode.editorModes.CollectionChanged += EditorModes_CollectionChanged;

            IServiceProvider serviceProvider = ServiceHelper.GetInstance().GetServiceProvider();
            using IServiceScope serviceScope = serviceProvider.CreateScope();
           
            DataContext = mainWindowMode;
        }

        private void EditorModes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                if(sender is ObservableCollection<WaveEditorMode> modes)
                {
                    var mode = modes[e.NewStartingIndex];
                    WaveEditor content = new();
                    HeavyLoad = new HeavyLoad();
                    content.SetDataContext(mode);

                    LayoutDocument docDocument = new LayoutDocument
                    {
                        Title = mode.Title,
                        Content = content
                    };
                    
                    docDocument.Closed +=mode.Close;
                    docGrup.Children.Add(docDocument);
                    docDocument.Closed += DocClosed;
                }
            }
           
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            IniEditorParameterModel iniEditorParameterModel = new();
            IniEditorParameter iniEditorParameter = new()
            {
                DataContext = iniEditorParameterModel
            };
            bool? v = iniEditorParameter.ShowDialog();
            if (v == null||v==false)
                return;
            

            WaveEditorMode mode = new();
            mode.Inidata(iniEditorParameterModel);
            WaveEditor content = new();
            (this.DataContext as MainWindowMode)?.Execute(mode);
            HeavyLoad = new HeavyLoad();
            content.DataContext = mode;
        }


        private void DocClosed(object sender, EventArgs e)
        {
           // HeavyLoad.Load = null;
            GC.Collect();
        }

        

        private void 工具箱MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class HeavyLoad : HeavyLoadBase, INotifyPropertyChanged
    {
        public byte[] Load { get; set; } = new byte[100000000];//100MB

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class HeavyLoadBase
    {
    }
}
