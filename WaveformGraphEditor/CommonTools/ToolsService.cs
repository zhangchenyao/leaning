using SericeTool.Interfaces;

namespace CommonTools
{
    public class ToolsService : IDisposable, ITransientOperation,
    IScopedOperation,
    ISingletonOperation
    {
        private bool disposedValue;

        public ToolsService()
        {
            JsonFilesTool=new JsonFilesTool();
        }

        public JsonFilesTool? JsonFilesTool { get;  private set; }

        public string OperationId { get; } = Guid.NewGuid().ToString()[^4..];

        public string GetLoaclIniPath()
        {
            string path = Environment.CurrentDirectory;
#pragma warning disable CS8602 // 解引用可能出现空引用。
            string fullName = Directory.GetParent(path).FullName;
#pragma warning restore CS8602 // 解引用可能出现空引用。
            Directory.SetCurrentDirectory(fullName);
            path = Directory.GetCurrentDirectory();
            path = $"{path}\\ini";
            return path;
        }

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
        // ~ToolsManagement()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}