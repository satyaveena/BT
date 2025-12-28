using BT.ETS.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.Manager.Interface
{
    public interface IOrderManager
    {
        InsertedCartResult InsertEtsCart(string espLibraryId, string etsCartId, string cartName, string cartNote, string userId, List<LineItemInput> lines);
    }
}
