Module C2VB
    Public Shared Function CreateCart(utblListTransferBasketsType As DataTable, utblListTransferBasketDetailsType As DataTable, ByRef basketGuid__1 As String, ByRef dbReturn As String) As DataTable
        Dim TS360Basket As New DataTable()

        Dim dataConnect = ConfigurationManager.ConnectionStrings("Orders").ConnectionString

        Dim OrdersDBConnection As New SqlConnection(dataConnect)

        Dim basketGUID__2 = String.Empty
        dbReturn = String.Empty

        Try
            ' Stored Proc. name
            Dim storedProc As New SqlCommand("procListTransferInsertBasketsAPI", OrdersDBConnection)

            storedProc.CommandType = CommandType.StoredProcedure

            ' Note the UDT parameter
            storedProc.Parameters.Add(New SqlParameter("@parBasket", SqlDbType.Structured))
            storedProc.Parameters("@parBasket").Value = utblListTransferBasketsType

            storedProc.Parameters.Add(New SqlParameter("@parBasketDetails", SqlDbType.Structured))
            storedProc.Parameters("@parBasketDetails").Value = utblListTransferBasketDetailsType


            Dim returnOutput As New SqlParameter("@Return", SqlDbType.Int)
            returnOutput.Direction = ParameterDirection.Output
            Dim messageOutput As New SqlParameter("@Message", SqlDbType.NVarChar, 8000)
            messageOutput.Direction = ParameterDirection.Output
            Dim basketLineCountOutput As New SqlParameter("@BasketLineCount", SqlDbType.Int)
            basketLineCountOutput.Direction = ParameterDirection.Output
            Dim basketlinecountINSERTEDOutput As New SqlParameter("@basketlinecountINSERTED", SqlDbType.Int)
            basketlinecountINSERTEDOutput.Direction = ParameterDirection.Output
            Dim basketGUIDOutput As New SqlParameter("@basketGUID", SqlDbType.NVarChar, 50)
            basketGUIDOutput.Direction = ParameterDirection.Output

            ' read the recordset into a datareader to determine invalid vs restricted // loaderror & btkey

            storedProc.Parameters.Add(returnOutput)
            storedProc.Parameters.Add(messageOutput)
            storedProc.Parameters.Add(basketLineCountOutput)
            storedProc.Parameters.Add(basketlinecountINSERTEDOutput)
            storedProc.Parameters.Add(basketGUIDOutput)

            '@Return int output, 
            '@Message nvarchar(max) OUTPUT,
            '@BasketLineCount int output,
            '@basketlinecountINSERTED int output,
            '@basketGUID nvarchar(50) OUTPUT 

            OrdersDBConnection.Open()
            'storedProc.ExecuteNonQuery();



            Dim sda As New SqlDataAdapter(storedProc)
            Dim ds As New DataSet()
            sda.Fill(ds)

            basketGUID__2 = basketGUIDOutput.Value.ToString()
            dbReturn = returnOutput.Value.ToString()
            Dim dbMessage = messageOutput.Value.ToString()

            ' Retrieving total stored tables from common DataSet      
            If ds.Tables.Count > 0 Then
                TS360Basket = ds.Tables(0)

                ' To display all rows of a table, we use foreach loop for each DataTable.
                'foreach (DataRow dr in dt1.Rows)
                '{
                '    Console.WriteLine("Student Name: " + dr[sName]);
                '}

            End If
        Catch ex As Exception
            ' log exception and exit gracefully 
            LogAPIMessage("DAL.CreateCart", "", "", "", ex.Message)
        Finally
            OrdersDBConnection.Close()
        End Try

        basketGuid__1 = basketGUID__2

        Return TS360Basket

    End Function

End Module
