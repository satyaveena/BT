using BT.TS360API.ServiceContracts.Exceptions;

namespace BT.TS360API.ServiceContracts
{
    public class BasketInformationOfMiniCart : IEntityValidator
    {
        
        public string NumberOfItems { get; set; }

        
        public string ListPrices { get; set; }

       
        public string DateModified { get; set; }

        public void Validate()
        {
            return;
        }
    }
}
