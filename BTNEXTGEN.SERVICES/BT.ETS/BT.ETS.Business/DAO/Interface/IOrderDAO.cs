using BT.ETS.Business.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.DAO.Interface
{
    /// <summary>
    /// Interface 
    /// </summary>
    public interface IOrderDAO
    {
        DataSet InsertEtsCart(string espLibraryId, string etsCartId, string cartName, string cartNote, string userId, List<LineItemInput> lines);
    }
}
