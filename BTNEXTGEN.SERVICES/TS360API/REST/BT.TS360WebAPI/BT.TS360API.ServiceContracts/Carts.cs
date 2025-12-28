using System.Collections;
using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts
{
    public sealed class Carts : ICollection<Cart>
    {
        #region Constructor

        /// <summary>
        /// Carts
        /// </summary>
        /// <param name="userId"></param>

        public Carts(string userId)
        {
            this.UserId = userId;
            items = new List<Cart>();
        }

        #endregion

        #region Private/Protected Properties

        /// <summary>
        /// items for ICollection
        /// </summary>
        private readonly List<Cart> items;
        #endregion
        public string UserId { get; set; }

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Cart> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        /// Count
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }
        /// <summary>
        /// Contains
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(Cart item)
        {
            return items.Contains(item);
        }

        /// <summary>
        /// Copy To
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(Cart[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(Cart item)
        {
            return items.Remove(item);
        }


        /// <summary>
        /// A collection that is read-only does not allow the addition, removal, or modification of elements after the collection is created.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Clear
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="item"></param>
        public void Add(Cart item)
        {
            items.Add(item);
        }

        public void AddRange(IEnumerable<Cart> collection)
        {
            items.AddRange(collection);
        }
    }
}
