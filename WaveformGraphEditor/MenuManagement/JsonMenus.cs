using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEditorModel.VModel.Menus;

namespace MenuManagement
{
    internal class JsonMenus
    {
        public string Heard { get; set; }
        public string Name { get; set; }

        public int Id { get; set; }
        
        public List<JsonMenus> Jsons { get; set; }
        public int MenuType { get; set; }

        internal void Convert(Meum_Item meum_Item)
        {
            meum_Item.Content = Heard;

            foreach(var item in Jsons)
            {
                Meum_Item meum_Item1 = new();
                item.Convert(meum_Item1);
                meum_Item.MeumItems.Add(meum_Item1);
            }
        }
    }
}
