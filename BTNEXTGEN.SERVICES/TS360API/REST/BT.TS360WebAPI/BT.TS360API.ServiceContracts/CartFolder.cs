using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts
{
    public class CartFolder
    {
        public string CartFolderId { get; set; }

        public string CartFolderName { get; set; }

        public string ParentFolderId { get; set; }

        public CartFolderType? FolderType { get; set; }

        public string UserId { get; set; }

        public string OrgId { get; set; }

        public float Sequence { get; set; }

        public int TotalCarts { get; set; }
    }
}
