using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts.Search
{
    
    public class ProductContent
    {
        
        public string Annotation { get; set; }

        
        public string Excerpt { get; set; }

        
        public string Reviews { get; set; }

        
        public string TOC { get; set; }

        
        public bool HasAnnotation { get; set; }

        
        public bool HasExcerpts { get; set; }

        
        public bool HasReviews { get; set; }

        
        public bool HasTOC { get; set; }

        
        public bool HasReturnKey { get; set; }
        
        public bool HasMuze { get; set; }
    }

    
    public class AdditionContent
    {
        
        public string Title { get; set; }

        
        public string Content { get; set; }

        
        public string ReviewTypeId { get; set; }

        
        public string Sequence { get; set; }

        
        public string DisplayName { get; set; }
    }

    
    public class ReviewCitationObject
    {
        
        public string ReviewCitation { get; set; }

        
        public string ReviewId { get; set; }
    }

    
    public class ProductDetailsObject
    {
        
        public List<string> BTPublications { get; set; }

        
        public List<string> BTPrograms { get; set; }

        
        public Dictionary<string, string> GeneralInfo { get; set; }

        
        public List<string> Attributes { get; set; }

        
        public List<string> Platforms { get; set; }

        
        public List<string> Versions { get; set; }

        
        public List<string> Regions { get; set; }

        
        public List<string> SoundTypes { get; set; }

        
        public List<string> BISACSubjects { get; set; }

        
        public List<string> LibrarySubjects { get; set; }

        
        public List<string> AcademicSubjects { get; set; }

        
        public Dictionary<string, string> ContinuationsSeries { get; set; }

        
        public List<string> Awards { get; set; }

        
        public List<string> Bibliographies { get; set; }

        
        public List<ReviewCitationObject> ReviewCitations { get; set; }

        public List<string> OtherCitations { get; set; }

        
        public List<string> AcceleratedTopics { get; set; }

        public List<PPCSubscription> PayPerCirCollections { get; set; }
    }
    
    public class ProductInterestGroupContent
    {
        
        public string PIGId { get; set; }

        
        public string PIGName { get; set; }

        
        public string Description { get; set; }
    }
}
