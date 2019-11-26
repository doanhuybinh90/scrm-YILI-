using Pb.Wechat.MpGroups;
using System.Collections.Generic;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpGroups
{
    public class GroupTreeItemModel
    {
        public IEnumerable<MpGroup> ModelList { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<int> SelectedItems { get; set; }
        public GroupTreeItemModel()
        {

        }

        public GroupTreeItemModel(IEnumerable<MpGroup> modelList,int id, string name, IEnumerable<int> selectedItems)
        {
            ModelList = modelList;
            Id = id;
            Name = name;
            SelectedItems = selectedItems;
        }
    }
}
