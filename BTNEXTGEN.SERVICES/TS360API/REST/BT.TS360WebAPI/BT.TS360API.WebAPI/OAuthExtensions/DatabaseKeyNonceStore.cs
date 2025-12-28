using System.Data;

namespace BT.TS360API.WebAPI.OAuth
{
	using System;
	using System.Collections.Generic;
	using System.Data.SqlClient;
	using System.Linq;
	using DotNetOpenAuth.Messaging.Bindings;
    using BT.TS360API.WebAPI.Common.DataAccess;
    using BT.TS360API.WebAPI.Common.Helper;

	/// <summary>
	/// A database-persisted nonce store.
	/// </summary>
	public class DatabaseKeyNonceStore : INonceStore, ICryptoKeyStore {
		/// <summary>
		/// Initializes a new instance of the <see cref="DatabaseKeyNonceStore"/> class.
		/// </summary>
		public DatabaseKeyNonceStore() {
		}

        private OAuthDAO _oAuthDao = new OAuthDAO();
		#region INonceStore Members

		/// <summary>
		/// Stores a given nonce and timestamp.
		/// </summary>
		/// <param name="context">The context, or namespace, within which the
		/// <paramref name="nonce"/> must be unique.
		/// The context SHOULD be treated as case-sensitive.
		/// The value will never be <c>null</c> but may be the empty string.</param>
		/// <param name="nonce">A series of random characters.</param>
		/// <param name="timestampUtc">The UTC timestamp that together with the nonce string make it unique
		/// within the given <paramref name="context"/>.
		/// The timestamp may also be used by the data store to clear out old nonces.</param>
		/// <returns>
		/// True if the context+nonce+timestamp (combination) was not previously in the database.
		/// False if the nonce was stored previously with the same timestamp and context.
		/// </returns>
		/// <remarks>
		/// The nonce must be stored for no less than the maximum time window a message may
		/// be processed within before being discarded as an expired message.
		/// This maximum message age can be looked up via the				
		/// property.
		/// </remarks>
		public bool StoreNonce(string context, string nonce, DateTime timestampUtc) {
            Logger.Info(string.Format("StoreNonce:{0}{1}{2}", context, nonce, timestampUtc));
			//Utilities.DataContext.Nonces.InsertOnSubmit(new Nonce { Context = context, Code = nonce, Timestamp = timestampUtc });
			try {
				//Utilities.DataContext.SubmitChanges();
                _oAuthDao.AddNonce(context, nonce, timestampUtc);
				return true;
			} catch (SqlException sle) {
                Logger.Error(sle.ToString());
				return false;
			}
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return false;
            }
		}

		#endregion

		#region ICryptoKeyStore Members

		public CryptoKey GetKey(string bucket, string handle) {
			// It is critical that this lookup be case-sensitive, which can only be configured at the database.
            //var matches = from key in Utilities.DataContext.SymmetricCryptoKeys
            //              where key.Bucket == bucket && key.Handle == handle
            //              select new CryptoKey(key.Secret, key.ExpiresUtc.AsUtc());

			//return matches.FirstOrDefault();		
            try
            {
                var ds = _oAuthDao.GetCryptoKey(bucket, handle);
                if (ds != null && ds.Tables.Count > 0)
                {
                    var table = ds.Tables[0];
                    if (table.Rows.Count > 0)
                    {
                        var secret = (byte[])table.Rows[0]["Secret"];//DataAccessHelper.ConvertObjectToByteArray(table.Rows[0]["Secret"]);
                        var expiresUtc = DatabaseHelper.ConvertToDateTime(table.Rows[0]["ExpiresUtc"]);
                        if (expiresUtc != null) return new CryptoKey(secret, expiresUtc.Value.AsUtc());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
		    return null;
		}

		public IEnumerable<KeyValuePair<string, CryptoKey>> GetKeys(string bucket) {
            //return from key in Utilities.DataContext.SymmetricCryptoKeys
            //       where key.Bucket == bucket
            //       orderby key.ExpiresUtc descending
            //       select new KeyValuePair<string, CryptoKey>(key.Handle, new CryptoKey(key.Secret, key.ExpiresUtc.AsUtc()));
		    
            var result = new List<KeyValuePair<string, CryptoKey>>();
            try
            {
                var ds = _oAuthDao.GetCryptoKeysByBucket(bucket);
                if (ds != null && ds.Tables.Count > 0)
                {
                    var table = ds.Tables[0];
                    foreach (DataRow row in table.Rows)
                    {
                        var handle = row["Handle"].ToString();
                        var secret = (byte[])row["Secret"];
                        var expiresUtc = DatabaseHelper.ConvertToDateTime(row["ExpiresUtc"]);
                        if (expiresUtc != null)
                        {
                            var kvp = new KeyValuePair<string, CryptoKey>(handle, new CryptoKey(secret, expiresUtc.Value.AsUtc()));
                            result.Add(kvp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
		    return result;
		}

		public void StoreKey(string bucket, string handle, CryptoKey key) {
            //var keyRow = new SymmetricCryptoKey() {
            //    Bucket = bucket,
            //    Handle = handle,
            //    Secret = key.Key,
            //    ExpiresUtc = key.ExpiresUtc,
            //};

            //Utilities.DataContext.SymmetricCryptoKeys.InsertOnSubmit(keyRow);
            //Utilities.DataContext.SubmitChanges();
            try
            {
                _oAuthDao.AddCryptoKey(bucket, handle, key.Key, key.ExpiresUtc);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
		}

		public void RemoveKey(string bucket, string handle) {
            //var match = Utilities.DataContext.SymmetricCryptoKeys.FirstOrDefault(k => k.Bucket == bucket && k.Handle == handle);
            //if (match != null) {
            //    Utilities.DataContext.SymmetricCryptoKeys.DeleteOnSubmit(match);
            //}
            try
            {
                _oAuthDao.DeleteCryptokey(bucket, handle);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
		}

		#endregion
	}
}