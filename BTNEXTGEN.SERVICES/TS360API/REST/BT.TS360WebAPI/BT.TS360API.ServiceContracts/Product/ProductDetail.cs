using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Product
{
    public class ProductDetail
    {
        public string BTPrograms;
        public string BTPublications;
        public string Annotaion { get; set; }
        public List<ItemData> ProductInformation { get; set; }
        public List<ItemData> Classification { get; set; }
        public List<ItemData> Series { get; set; }
        public List<ItemData> DetailLocation { get; set; }
        public string Awards { get; set; }
        public string Bibliography { get; set; }
        public List<string> ReviewCitations { get; set; }
        public string OtherCitations { get; set; }
        public List<ItemData> Physical { get; set; }
        public string LibrarySubjects { get; set; }
        public string GeneralSubjects { get; set; }
        public string AcademicSubjects { get; set; }
        public string BISACSubjects { get; set; }
        public List<ItemData> AcademicModifiers { get; set; }
        public List<ItemData> AcceleratedReader { get; set; }
        public List<ItemData> ReadingCount { get; set; }
        public List<ItemData> BTSpecificData { get; set; }
        public string PayPerCircCollections { get; set; }
    }
}
