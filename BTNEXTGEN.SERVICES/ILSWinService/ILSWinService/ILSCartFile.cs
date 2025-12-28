using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using BT.TS360API.Common.DataAccess;
using BT.TS360Constants;
using BT.TS360API.ServiceContracts.Request;

namespace ILSWinService
{
    public class ILSCartFile
    {
        private static ILSCartFile instance;
        StreamWriter sw;
        StreamReader sr;
        FileStream fs;
        static string path = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\ILSCartInfo.txt";
        private static object lockProcess = new object();
        private ILSCartFile()
        {
            fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fs.Close();
        }
        public static ILSCartFile Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ILSCartFile();
                }
                return instance;
            }
        }


        public void WriteILSFile(ILSOrderRequest ilsOrderRequest)
        {
            using (sw = new StreamWriter(path, true))
            {
                sw.WriteLine(ilsOrderRequest.CartId + "," + ilsOrderRequest.UserId);
            }
        }

        public string DeleteCartFromILSFile(string cartid)
        {
            lock (this)
            {
                List<string> list = File.ReadAllLines(path).ToList();
                int index = list.FindIndex(s => s.Contains(cartid));
                if (index >= 0)
                    list.RemoveAt(index);
                File.WriteAllLines(path, list);
            }
            return cartid;
        }

        public void ResetILSFile()
        {
            lock (this)
            {
                List<string> list = File.ReadAllLines(path).ToList();
                while (list.Count != 0)
                {
                    for (int i = 0; i <= list.Count; i++)
                    {
                        string cartId = list[0].Split(',')[0];
                        string userId = list[0].Split(',')[1];
                        CartDAOManager.Instance.SetILSBasketState(cartId, userId, CartStatus.Submitted, ILSState.ILSNew, null, null, null );
                        list.Remove(list[0]);
                    }
                }

                File.WriteAllLines(path, list);
            }
        }
    }
}

