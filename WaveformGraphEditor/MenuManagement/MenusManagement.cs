using CommonTools;
using Newtonsoft.Json;
using SericeTool;
using SericeTool.Interfaces;
using System.Collections.ObjectModel;
using WaveEditorModel.VModel.Menus;
using Microsoft.Extensions.DependencyInjection;
namespace MenuManagement
{
    public class MenusManagement : IDisposable, ITransientOperation,
    IScopedOperation,
    ISingletonOperation
    {
        private bool disposedValue;

        public MenusManagement()
        {
            Initialization();
        }


        private JsonMenus MeumItems { get; set; }
        #region Properties
        public ObservableCollection<Meum_Item> meum_Items { get; private set; }
        #endregion
        public string OperationId { get; } = Guid.NewGuid().ToString()[^4..];

        #region Private Methods
        private void Initialization()
        {
            using IServiceScope serviceScope = ServiceHelper.GetInstance().GetServiceProvider().CreateScope();
            ToolsService ts = serviceScope.ServiceProvider.GetRequiredService<ToolsService>();


            string path = ts.GetLoaclIniPath();
            path = $"{path}\\MenuItemIni.json";
            JsonFilesTool? jsonFilesTool = ts.JsonFilesTool;
            if (jsonFilesTool != null)
            {
                var josndata = jsonFilesTool.Read(path);

#pragma warning disable CS8601 // 引用类型赋值可能为 null。
                MeumItems = JsonConvert.DeserializeObject<JsonMenus>(josndata.ToString());
#pragma warning restore CS8601 // 引用类型赋值可能为 null。

                Meum_Item meum_Item = new();
                MeumItems?.Convert(meum_Item);
                meum_Items = meum_Item.MeumItems;
            }
        }

        

        #endregion

        #region Public Methods
        
        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~MenusManagement()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}