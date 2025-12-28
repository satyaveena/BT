using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Net;
using System.Configuration;
using TS360.Common.FileLogging;
using System.Data;
using System.Data.SqlClient;
using TS360.Common.Email;
using TS360.FTP; 




namespace CRELExtract
{
    class Program
    {
        static void Main(string[] args)
        {


            //Get app config variables. 
            string fileName = "ts360creldata.txt";
            string fileNameDTS = string.Format("{0}_{1}{2}", "ts360creldata", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"),".txt"); 
            string LogFolder = ConfigurationManager.AppSettings["LogFolder"];
            string LogPrefix = ConfigurationManager.AppSettings["LogFilePrefix"];
            string WorkFolder = ConfigurationManager.AppSettings["WorkFolder"];
            string ArchiveFolder = ConfigurationManager.AppSettings["ArchiveFolder"];
            string ServerName = ConfigurationManager.AppSettings["ServerName"];
            string StoredProcedureName = ConfigurationManager.AppSettings["StoredProcedureName"];
            //string connectionString = ConfigurationManager.AppSettings["Biztalk"];
            //SqlConnection connectionString = new SqlConnection ( ConfigurationManager.ConnectionStrings["BiztalkConnection"].ToString()) ; 
            string ArchiveFileNamePlusFolder = string.Format("{0}\\{1}", ArchiveFolder, fileNameDTS);
            string WorkFileNamePlusFolder = string.Format("{0}\\{1}", WorkFolder , fileName);

            //Main process 
            //1. check if file exists if if does, delete as something went wrong before 
            //2. filelogging and streamwriter object creation
            //3. make sql connection to biztalk, call stored proc
            //4. write data out to file 
            //5. ftp file to ftp site 
            

            //1. 
            if (File.Exists(WorkFileNamePlusFolder))
            {
                //delete the old file.  previous process must have aborted.   
                File.Delete(WorkFileNamePlusFolder);
            }


            //2. 
            FileLogRepository fileLog = new FileLogRepository(LogFolder, LogPrefix);
            StreamWriter writer = new StreamWriter(WorkFileNamePlusFolder); 
            
            try
            {
                fileLog.Write(" (Main) started...", FileLogRepository.Level.INFO);

                
                //3. 
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BiztalkConnection"].ToString()))
                {
                    connection.Open();
                    
                    SqlCommand command = new SqlCommand(StoredProcedureName , connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 20; 

                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        //4. 
                        writer.WriteLine(String.Format("{0}", reader["TransNum"]) + "," +
                                          String.Format("{0}", reader["TargetERP"]) + "," +
                                          String.Format("{0}", reader["PORcvd"]) + "," +
                                          String.Format("{0}", reader["POSentERP"]) + "," +
                                          String.Format("{0}", reader["ERPAckRCV"]));
                        Console.WriteLine(String.Format("{0}", reader["TargetERP"]));
                    }

                    writer.Close();  
 
                    if (File.Exists(WorkFileNamePlusFolder ))
                    {
                       //5. 
                        File.Copy(WorkFileNamePlusFolder, ArchiveFileNamePlusFolder);
                        CRELFTPSend crelftpsend = new CRELFTPSend();
                        crelftpsend.FTP(WorkFileNamePlusFolder,fileName ); 
                       
                    }
                    
                }



                fileLog.Write(" (Main) stopped...", FileLogRepository.Level.INFO);
                EmailSomebody.SendEmail("CRELExtract Completed");
                

            }


            catch (Exception ex)
            {
                fileLog.Write(" (Main) exception..." + ex.Message.ToString(), FileLogRepository.Level.INFO);
                EmailSomebody.SendEmailException("CRELExtract Exception: "  + ex.Message.ToString());
            }

        }
    }
}
