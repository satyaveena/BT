Imports System.Net
Imports System.Runtime.Serialization.Json
Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Data.SqlClient
Imports System.Threading

Public Class DoACart

    Public Event CallBackLogIt As EventHandler(Of LogItEventArgs)
    Public Event CallBackAlertMail As EventHandler(Of AlertMailEventArgs)
    Public Event CallBackFileItUTF8 As EventHandler(Of FileItUTF8EventArgs)
    Public Event CallBackExLogIt As EventHandler(Of ExLogItEventArgs)
    Public Event CallBackCartAttemptComplete As EventHandler(Of CartAttemptCompleteEventArgs)
    Public CartObject As ESPWinServices.CartFunctions
    Public where As Short = 0

    'Public Class resptype

    'End Class

    Dim WithEvents StartTimer As New Timers.Timer
    Dim cart2Do As Object
    Dim attemptNo As Short
    Dim delaySecs As Short
    Dim xmlFile As String
    Dim utblESP As DataTable
    Dim utblBRA As DataTable
    Dim utblFUN As DataTable
    Dim typeToDo As String



    Public Sub New(ByVal Cart As Object, ByVal attempts As Short, ByVal delay As Short, ByVal xmlFileName As String, ByVal utblESPStatus As DataTable, ByVal utblBranches As DataTable, ByVal utblFundCodes As DataTable, ByVal type As ESPWinServices.CartType, ByVal CartFunction As ESPWinServices.CartFunctions)
        where = 1
        Try
            'Logit("DoACart New")
            CartObject = CartFunction
            cart2Do = Cart
            attemptNo = attempts
            delaySecs = delay
            xmlFile = xmlFileName
            utblESP = utblESPStatus
            utblBRA = utblBranches
            utblFUN = utblFundCodes
            typeToDo = type

            utblESP.Clear()
            utblBRA.Clear()
            utblFUN.Clear()

            Me.StartTimer.AutoReset = False
            Me.StartTimer.Interval = 100
            'this allows the Working routine to start automatically upon object creation.
            '   It seems to be key to getting things to flow in multiple threads rather than invoking the function from the calling module.
            '   The 100 milisecond delay is because the app creates the callback AFTER the object is created.  
            '   Dunno if that is needed or it can be shortened, etc.
            Me.StartTimer.Start()

        Catch ex As Exception
            Logit(CartObject.Type.ToString & " Cart couldn't make it though NEW " & ex.Message)
        End Try

    End Sub

    Private Sub StartTimer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles StartTimer.Elapsed
        where = 2
        Try
            'Logit("DoACart StartTimerElapsed")
            DoACart(cart2Do, attemptNo, delaySecs, xmlFile, typeToDo, CartObject)

        Catch ex As Exception
            Logit(CartObject.Type.ToString & " Cart couldn't make it though StartTimer_Elapsed. where =" & where.ToString & " : " & ex.Message)

        End Try

    End Sub

    Public Sub DoACart(ByVal cart As Object, ByVal attempts As Short, ByVal delaysecs As Short, ByVal xmlFileName As String, ByVal type As ESPWinServices.CartType, ByVal CartObject As ESPWinServices.CartFunctions)
        where = 10
        DLogit(CartObject.Type.ToString & " DoACart DoACart Started.")

        'Dim where As Short = 0

        Dim lastAttempt As Boolean = False
        If attempts >= Val(My.Settings.CartJobsRetryCount + 1) Then lastAttempt = True

        'This flag is just to tell the caller that this cart should be retried, if they aren't exhausted already.
        '   We start optimistically, set it to true as needed (a retryable failure) and test at the very end before calling back
        Dim enableRetry As Boolean = False

        'we're a go. first create the serializer to work on the Class created to hold Requests for serializing
        Dim jsonSerializer As DataContractJsonSerializer = CartObject.RequestSerializer
        Dim jsonSerializerR As DataContractJsonSerializer = CartObject.ResponseSerializer


        'Then setup the memory stream for accepting the serialized data
        Dim msJson As MemoryStream = Nothing
        Dim msr As StreamReader = Nothing
        Dim jsonString As String
        Dim req As HttpWebRequest = Nothing
        Dim resp As HttpWebResponse = Nothing
        Dim PostData As Stream = Nothing
        Dim uri As String = ""
        Dim ctbladapter As New OrdersDataSetTableAdapters.QueriesTableAdapter
        Dim cretvalue As Int32 = 0
        Dim cerrMess As String = ""
        Dim ESPStateCartStatus As String = "Not Supplied"

        Dim jsonCart As Object = Nothing
        Dim jcartResponse As Object = Nothing

        Dim distWfund As Boolean = False

        'TODO: Make this dynamic, if possible.

        'TODO - I can't find any way around this Select for these objects,
        where = 20
        uri = My.Settings.ESPBaseURI & CartObject.URIPostFix

        If CartObject.Type = ESPWinServices.CartType.FUND Then
            uri = uri & cart.CartID
        End If
        where = 30
        Select Case CartObject.Type

            Case Is = ESPWinServices.CartType.DIST
                jsonCart = New ESPWinServices.DistributeRequestJson
                jsonCart.branches = New List(Of ESPWinServices.branch)
                jsonCart.items = New List(Of ESPWinServices.Item)

                jcartResponse = New ESPWinServices.DistributeResponseJson
                jcartResponse.branches = New List(Of ESPWinServices.distBranch)
                jcartResponse.fundCodes = New List(Of ESPWinServices.distFund)

                If UCase(cart.FundMonitoring) = "Y" Then distWfund = True

            Case Is = ESPWinServices.CartType.FUND
                jsonCart = New ESPWinServices.acceptFundRequestJson
                jsonCart.items = New List(Of ESPWinServices.Item)

                jcartResponse = New ESPWinServices.acceptFundResponseJson
                jcartResponse.fundCodes = New List(Of ESPWinServices.distFund)

            Case Else
                jsonCart = New ESPWinServices.RankRequestJson
                jsonCart.items = New List(Of ESPWinServices.Item)

                jcartResponse = New ESPWinServices.RankResponseJson

        End Select


        where = 40
        'If this is a Retry, we want to pause before doing anything...
        If delaysecs > 0 Then
            For looper = 1 To delaysecs * 10
                Thread.Sleep(100)
            Next
        End If


        where = 100
        'First Level Try - Cart object to JSon ==============================================
        'First Level Try - Cart object to JSon ==============================================
        'First Level Try - Cart object to JSon ==============================================
        'First Level Try - Cart object to JSon ==============================================
        Try
            where = 101
            cart = Convert.ChangeType(cart, CartObject.RequestType.GetType)

            Logit(CartObject.Type.ToString & " Cart Type = " & cart2Do.GetType.ToString)

            'where = 150
            'jsonCart = Convert.ChangeType(jsonCart, CartObject.jsonRequest.GetType)
            'where = 160
            'jcartResponse = Convert.ChangeType(jcartResponse, CartObject.jsonResponse.GetType)

            Logit(CartObject.Type.ToString & " jsonCart=" & jsonCart.GetType.ToString)
            Logit(CartObject.Type.ToString & " jcartresponse=" & jcartResponse.GetType.ToString)



            where = 200
            'jsonCart = New Object
            where = 2001


            'jsonCart = New ESPWinServices.DistributeRequestJson
            where = 201
            'jsonCart.branches = New List(Of ESPWinServices.branch)
            'jsonCart.items = New List(Of ESPWinServices.Item)

            'jsonCart.branches = cart.Branches
            'where = 202
            'jsonCart.items = cart.Items

            'jsonCart.espLibraryId = cart.ESPLibraryID
            ''for testing to force a 40X error....
            ''jsonCart.espLibraryId = "MyBadLibraryID"
            'jsonCart.cartId = cart.CartID
            'jsonCart.userName = cart.UserName
            'jsonCart.fundMonitoring = cart.FundMonitoring

            where = 300

            Select Case CartObject.Type

                Case Is = ESPWinServices.CartType.DIST
                    jsonCart.espLibraryId = cart.ESPLibraryID
                    'for testing to force a 40X error....
                    'jsonCart.espLibraryId = "MyBadLibraryID"
                    jsonCart.cartId = cart.CartID
                    jsonCart.userName = cart.UserName
                    jsonCart.fundMonitoring = cart.FundMonitoring

                    For Each branchy As DistributionRequestsDistributionRequestBranches In cart.branches
                        Dim jsonBranch As New ESPWinServices.branch
                        jsonBranch.branchId = branchy.branchid
                        jsonBranch.code = branchy.code

                        jsonCart.branches.Add(jsonBranch)
                    Next

                    'TODO: The fund and price may need to be dependent on existance and/or if fundMonitoring is "Y"
                    For Each detail As DistributionRequestsDistributionRequestItems In cart.Items
                        Dim jsonItem As New ESPWinServices.Item
                        jsonItem.lineItemId = detail.LineItemID
                        jsonItem.vendorId = detail.BTKey
                        jsonItem.price = detail.price
                        jsonItem.fundId = detail.fundid
                        jsonItem.fundCode = detail.fundcode
                        jsonItem.bisac = detail.bisac

                        jsonCart.items.Add(jsonItem)
                    Next

                Case Is = ESPWinServices.CartType.FUND
                    jsonCart.espLibraryId = cart.ESPLibraryID
                    'for testing to force a 40X error....
                    'jsonCart.espLibraryId = "MyBadLibraryID"
                    jsonCart.cartId = cart.CartID
                    jsonCart.userName = cart.UserName

                    For Each detail As FundRequestsFundRequestItems In cart.Detail
                        Dim jsonItem As New ESPWinServices.Item
                        jsonItem.lineItemId = detail.LineItemID
                        jsonItem.vendorId = detail.BTKey
                        jsonItem.fundCode = detail.fundcode
                        jsonItem.fundId = detail.fundid
                        jsonItem.price = detail.price
                        jsonItem.quantity = detail.Quantity

                        jsonCart.items.Add(jsonItem)
                    Next

                Case Else
                    jsonCart.espLibraryId = cart.ESPLibraryID
                    'for testing to force a 40X error....
                    'jsonCart.espLibraryId = "MyBadLibraryID"
                    jsonCart.cartId = cart.CartID
                    jsonCart.userName = cart.UserName

                    For Each detail As RankRequestsRankRequestItems In cart.Detail
                        Dim jsonItem As New ESPWinServices.Item
                        jsonItem.lineItemId = detail.LineItemID
                        jsonItem.vendorId = detail.BTKey
                        jsonItem.bisac = detail.bisac
                        jsonCart.items.Add(jsonItem)
                    Next


            End Select


            'For Each branchy As ESPWinServices.branch In cart.Branches
            '    Dim jsonBranch As New ESPWinServices.branch
            '    jsonBranch.branchId = branchy.branchId
            '    jsonBranch.code = branchy.code

            '    jsonCart.branches.Add(jsonBranch)
            'Next

            'Dim jsonItem As ESPWinServices.Item

            'where = 500
            'For Each detail As ESPWinServices.Item In cart.Items
            '    jsonItem = New ESPWinServices.Item
            '    jsonItem.lineItemId = detail.lineItemId
            '    jsonItem.vendorId = detail.vendorId
            '    jsonItem.price = detail.price
            '    jsonItem.fundId = detail.fundId
            '    jsonItem.fundCode = detail.fundCode

            '    jsonCart.items.Add(jsonItem)
            'Next

            where = 302
            'JSONClass to JSONString ============================================================================
            'make and instance of the stream and write the Class Object to it
            msJson = New MemoryStream
            jsonSerializer.WriteObject(msJson, jsonCart)

            'set the pointer back to the beginning for reading it back
            msJson.Position = 0

            'create the reader to read the stream into the string
            msr = New StreamReader(msJson)

            'and finally read the stream into the jsonString
            jsonString = msr.ReadToEnd

            'Now archive the string into a .json file
            Dim filprefix As String = "Cart2" & CartObject.Type.ToString & "_" & cart.CartID
            FileItUTF8(My.Settings.LogDir, "json", filprefix, jsonString)


            'Prepare for Calling ESP RESTful =========================================================================
            'and here is where you will call the RESTful service and assess the results, acting accordingly.

            'but for unit testing we overwrite with my local RESTService
            'uri = "http://localhost:63062/RESTService/esp/jobs/distribute"

            DLogit(CartObject.Type.ToString & " Calling: " & uri)

            req = TryCast(WebRequest.Create(uri), HttpWebRequest)
            req.KeepAlive = False
            req.Method = "POST"
            req.Headers.Add("espKey", My.Settings.VendorKey)
            req.Headers.Add("api-version", My.Settings.APIVersion)

            Dim buffer As Byte()
            buffer = Encoding.UTF8.GetBytes(jsonString)
            req.ContentLength = buffer.Length
            req.ContentType = "application/json"

            'OK here is an attempt to dump the HTTP Request Header name/value pairs
            If My.Settings.DebugLogging = True Then
                For i As Short = 0 To req.Headers.Count - 1
                    Dim header As String = req.Headers.GetKey(i)
                    For Each value As String In req.Headers.GetValues(i)
                        Logit(CartObject.Type.ToString & " Header Name:" & header & " Value:" & value)
                    Next

                Next
            End If

            'POST the JSON and get the Response ==================================================================
            PostData = req.GetRequestStream()
            PostData.Write(buffer, 0, buffer.Length)
            PostData.Close()

            resp = TryCast(req.GetResponse(), HttpWebResponse)

            'FORCE AN EXCEPTION if needed for testing
            'Throw New WebException("I Make Poopy!", WebExceptionStatus.UnknownError)
            jsonString = ""
            Dim httpRespString As System.IO.Stream = Nothing
            Dim sreader As StreamReader = Nothing
            Dim memstrJson As MemoryStream = Nothing

            where = 1
            If InStr(resp.ContentType.ToUpper, "JSON") > 0 Then

                '2nd Level Try ========================================
                Try 'Getting Json

                    'first, getthe response into a stream
                    httpRespString = resp.GetResponseStream

                    'then read the stream into a string
                    sreader = New StreamReader(httpRespString)
                    jsonString = sreader.ReadToEnd

                    'then archive it for what it's worth.
                    FileItUTF8(My.Settings.LogDir, "json", CartObject.Type.ToString & "Resp_" & cart.CartID, jsonString)


                    'now, read the string back into a memory stream
                    memstrJson = New MemoryStream(Encoding.Unicode.GetBytes(jsonString))

                    'Dim resptype As restype = Nothing
                    'resptype = cartResponse.GetType


                    '...and Deserialize the json into the class
                    'TODO - I can't find any way around this Select for the CType function,
                    '   which seems to ONLY be able to take the actual class type.
                    'Select Case CartObject.Type

                    '    Case Is = ESPWinServices.CartType.DIST
                    '        cartResponse = CType(jsonSerializerR.ReadObject(memstrJson), ESPWinServices.DistributeResponseJson)
                    '    Case Is = ESPWinServices.CartType.FUND
                    '        cartResponse = CType(jsonSerializerR.ReadObject(memstrJson), ESPWinServices.acceptFundResponseJson)
                    '    Case Else
                    '        cartResponse = CType(jsonSerializerR.ReadObject(memstrJson), ESPWinServices.RankResponseJson)

                    'End Select
                    'cartResponse = CType(jsonSerializerR.ReadObject(memstrJson), BLAHBLAH)
                    jcartResponse = Convert.ChangeType(jsonSerializerR.ReadObject(memstrJson), CartObject.jsonResponse.GetType)


                    'which leaves you the json string and the class created from it both accessible as needed.
                    DLogit(CartObject.Type.ToString & " " & jcartResponse.GetType().ToString & " jcartResponse Converted!")

                    If CartObject.Type = ESPWinServices.CartType.DIST Then
                        For Each status As ESPWinServices.statusCode In jcartResponse.statusCodes
                            Logit(CartObject.Type.ToString & " Json Status:" & status.code & " json Description:" & status.message)
                        Next

                    Else
                        Logit(CartObject.Type.ToString & " Json Status:" & jcartResponse.statusCode & " Description:" & jcartResponse.statusMessage)

                    End If
                    '2nd Level Catch Getting Json ===============================
                Catch ex As Exception

                    Logit(CartObject.Type.ToString & " Exception Encountered trying to get json from HTTP response: " & ex.Message)
                    ExLogIt(uri, CartObject.Type.ToString & " Exception Encountered trying to get json from HTTP response: " & ex.Message)
                    ESPStateCartStatus = resp.StatusDescription
                Finally

                    jsonSerializerR = Nothing

                End Try

            End If


            where = 2

            'If we did not get an Exception - Examine what we got  and act accordingly ================================================================================
            'If we did not get an Exception - Examine what we got  and act accordingly ================================================================================
            'If we did not get an Exception - Examine what we got  and act accordingly ================================================================================
            'If we did not get an Exception - Examine what we got  and act accordingly ================================================================================

            DLogit(CartObject.Type.ToString & " HTTP Response: " & resp.StatusCode & "-" & resp.StatusDescription)

            If Not IsNothing(jcartResponse) Then
                If CartObject.Type = ESPWinServices.CartType.DIST Then
                    For Each status As ESPWinServices.statusCode In jcartResponse.statusCodes
                        Logit(CartObject.Type.ToString & " Json Status:" & status.code & " json Description:" & status.message)
                    Next
                    ESPStateCartStatus = jcartResponse.statusCodes(0).message

                Else
                    Logit(CartObject.Type.ToString & " Json Status:" & jcartResponse.statusCode & " Description:" & jcartResponse.statusMessage)
                    ESPStateCartStatus = jcartResponse.statusMessage

                End If
            Else
                Logit(CartObject.Type.ToString & " JSON Content not found")
                ESPStateCartStatus = resp.StatusDescription
            End If

            If resp.StatusCode = HttpStatusCode.Accepted Or resp.StatusCode = HttpStatusCode.OK Or resp.StatusCode = HttpStatusCode.Created Then
                Logit(CartObject.Type.ToString & " All is well for Cart=" & cart.BasketName & " So I will update it's status " & "for CartID=" & cart.CartID)
                where = 201
                where = 202

                'Send A USER Alert for a Successful Cart Submission (Fund Only Currently) =================================================================
                'Send A USER Alert for a Successful Cart Submission (Fund Only Currently) =================================================================
                'Send A USER Alert for a Successful Cart Submission (Fund Only Currently) =================================================================
                'Send A USER Alert for a Successful Cart Submission (Fund Only Currently) =================================================================

                'We send the Success Alert for FUND, since the cart does not "come back" through the API.
                If CartObject.Type = ESPWinServices.CartType.FUND Then
                    Dim UACall As Boolean = True
                    Dim UATempl As Integer = CartObject.UserAlertTemplateEnum2  'the Alternate template for FUND, which is Success.

                    '2nd Level Try - Send USer Alert - Success
                    Try
                        UACall = SendUserAlert(cart, UATempl)

                    Catch ex As Exception

                        Logit(CartObject.Type.ToString & " Couldn't do UserAlert " & ex.Message)
                    End Try

                    If Not UACall Then
                        Logit(CartObject.Type.ToString & " User Alert Call Failed for user=" & cart.UserGuid & " and Basket=" & cart.CartID)
                        ExLogIt(CartObject.Type.ToString & " UserAlertsService", "Success User Alert Call Failed for user=" & cart.UserGuid & " and Basket=" & cart.CartID)
                    End If

                End If

                'Update ESP State Table with Successful Cart Submission ==============================================================
                'Update ESP State Table with Successful Cart Submission ==============================================================
                'Update ESP State Table with Successful Cart Submission ==============================================================
                'Update ESP State Table with Successful Cart Submission ==============================================================

                'Depending on the transaction type, I may or may not get a list of funds and branches in the success response
                '   If a success, they should all be "0"status which don't get passed to the DB, but we read them all anyway.
                If Not IsNothing(jcartResponse.fundCodes) Then

                    For Each fund As ESPWinServices.distFund In jcartResponse.fundCodes
                        If fund.status <> "0" Then utblFUN.Rows.Add(cart.CartID, fund.fundId)
                    Next

                End If


                If Not IsNothing(jcartResponse.branches) Then

                    For Each branch As ESPWinServices.distBranch In jcartResponse.branches
                        If branch.status <> "0" Then utblBRA.Rows.Add(cart.CartID, branch.branchId)
                    Next

                End If



                If CartObject.Type = ESPWinServices.CartType.FUND Then
                    utblESP.Rows.Add(cart.CartID, CartObject.Type.ToString, "Submitted", DBNull.Value, ESPStateCartStatus, "No Exceptions", jsonString)
                Else
                    utblESP.Rows.Add(cart.CartID, CartObject.Type.ToString, "Submitted", jcartResponse.jobId, ESPStateCartStatus, "No Exceptions", jsonString)

                End If

                where = 205

                '2nd Level Try call SP Success
                Try
                    ctbladapter.procESPSetESPState(utblESP, utblBRA, utblFUN, "OK")
                    cretvalue = ctbladapter.GetReturnValue(5)
                    cerrMess = ctbladapter.GetParam4(5)

                Catch upex As Exception
                    Logit(CartObject.Type.ToString & " Couldn't update ESPState w/Success: " & upex.Message)
                End Try

                where = 207
                If Not cretvalue = 0 Then
                    'Send an Alert that the SP returned an error.
                    Dim AlertMess As String = CartObject.Type.ToString & " Error Calling SP procESPSetESPState for CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess
                    AlertMail(CartObject.Type.ToString & " procESPSetESPSTate Failed.", AlertMess, "dbESPInfo")
                    ExLogIt(CartObject.Type.ToString & " procESPSetESPState", AlertMess)
                    Logit(CartObject.Type.ToString & " Error. Got Non Zero returnvalue. " & AlertMess)
                Else
                    where = 208
                    DLogit(CartObject.Type.ToString & " Results of procESPSetESPState forCartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess)
                End If

                'Note, we don't re-attempt the Cart at this point, even if the call to update it's status failed, 'cause we think the cart submission succeeded.

            Else
                'I got something other than HTTP 20X Status Code, but it didn't trigger a Web Exception =====================================================================
                'I got something other than HTTP 20X Status Code, but it didn't trigger a Web Exception =====================================================================
                'I got something other than HTTP 20X Status Code, but it didn't trigger a Web Exception =====================================================================
                'I got something other than HTTP 20X Status Code, but it didn't trigger a Web Exception =====================================================================

                where = 3
                'Most, if not all, non 20X Http Status codes will cause a .NET WebException, so we don't think we'll get here.
                '   If there is json, we should have it here...
                '   But we will treat this as a failure and log the results for checking later.
                If Not lastAttempt Then

                    Logit(CartObject.Type.ToString & " I got an HTTP Status of " & resp.StatusCode.ToString & " for Cart=" & cart.BasketName & " so I think I failed because it's not 'OK' or 'ACCEPTED'. Will Retry. This was attempt " & attempts.ToString & " of " & (Val(My.Settings.CartJobsRetryCount) + 1).ToString)

                Else

                    Dim mess As String = CartObject.Type.ToString & " I got an HTTP Status of " & resp.StatusCode.ToString & " for Cart=" & cart.BasketName & " so I think I failed because it's not 'OK' or 'ACCEPTED'. " & CartObject.Type.ToString & ": Retries Exhausted. User Alert goes to " & cart.UserGuid

                    Dim exMess As String = CartObject.Type.ToString & " I got an HTTP Status of " & resp.StatusCode.ToString & " for Cart=" & cart.BasketName

                    'TODO: verify this array is base=0, and consider stepping through the array instead of being lazy below
                    If Not IsNothing(jcartResponse) Then
                        mess = mess & CartObject.Type.ToString & " StatusCode=" & jcartResponse.statusCodes(0).code.ToString & ", StatusMessage=" & jcartResponse.statusCodes(0).message & ", Job ID =" & jcartResponse.jobId
                        exMess = exMess & CartObject.Type.ToString & " StatusCode=" & jcartResponse.statusCodes(0).code.ToString & ", StatusMessage=" & jcartResponse.statusCodes(0).message & ", Job ID =" & jcartResponse.jobId
                    End If
                    Logit(mess)
                    ExLogIt(CartObject.Type.ToString & " Cart", exMess)


                    'Send A USER Alert for HTTP Failure Codes without Web Exception =================================================================
                    'Send A USER Alert for HTTP Failure Codes without Web Exception =================================================================
                    'Send A USER Alert for HTTP Failure Codes without Web Exception =================================================================
                    'Send A USER Alert for HTTP Failure Codes without Web Exception =================================================================

                    'TODO: confirm no system error distinction, and code for DistWFundFail
                    Dim UACall As Boolean = True
                    Dim UATempl As Integer = CartObject.UserAlertTemplateEnum 'Default, and what is always used for FUND Error Alerts

                    Select Case CartObject.Type
                        Case Is = ESPWinServices.CartType.DIST
                            If distWfund = True Then UATempl = CartObject.UserAlertTemplateEnum2

                        Case Is = ESPWinServices.CartType.RANK
                            If resp.StatusCode = HttpStatusCode.BadRequest Then UATempl = CartObject.UserAlertTemplateEnum2

                    End Select

                    '2nd Level Try Send Alert Fail No Web Ex
                    Try
                        UACall = SendUserAlert(cart, UATempl)

                    Catch ex As Exception

                        Logit(CartObject.Type.ToString & " Couldn't do UserAlert " & ex.Message)
                    End Try

                    If Not UACall Then
                        Logit(CartObject.Type.ToString & " User Alert Call Failed for user=" & cart.UserGuid & " and Basket=" & cart.CartID)
                        ExLogIt(CartObject.Type.ToString & " UserAlertsService", "Failure User Alert Call Failed for user=" & cart.UserGuid & " and Basket=" & cart.CartID)
                    End If

                    'Update ESP State Tabloe for HTTP Failure Code without Web Exception ===================================
                    'Update ESP State Tabloe for HTTP Failure Code without Web Exception ===================================
                    'Update ESP State Tabloe for HTTP Failure Code without Web Exception ===================================
                    'Update ESP State Tabloe for HTTP Failure Code without Web Exception ===================================

                    'Depending on the transaction type, I may or may not get a list of funds and branches in the success response
                    '   If a success, they should all be "0"status which don't get passed to the DB, but we read them all anyway.
                    If Not IsNothing(jcartResponse.fundCodes) Then

                        For Each fund As ESPWinServices.distFund In jcartResponse.fundCodes
                            If fund.status <> "0" Then utblFUN.Rows.Add(cart.CartID, fund.fundId)
                        Next

                    End If


                    If Not IsNothing(jcartResponse.branches) Then

                        For Each branch As ESPWinServices.distBranch In jcartResponse.branches
                            If branch.status <> "0" Then utblBRA.Rows.Add(cart.CartID, branch.branchId)
                        Next

                    End If



                    utblESP.Rows.Add(cart.CartID, CartObject.Type.ToString, "Failed", DBNull.Value, ESPStateCartStatus, mess, jsonString)

                    '2nd Level Try Call SP Fail no WebEx
                    Try
                        ctbladapter.procESPSetESPState(utblESP, utblBRA, utblFUN, "OK")
                        cretvalue = ctbladapter.GetReturnValue(5)
                        cerrMess = ctbladapter.GetParam4(5)

                    Catch upex As Exception
                        Logit(CartObject.Type.ToString & " Couldn't update ESPState w/Fail: " & upex.Message)
                    End Try

                    If Not cretvalue = 0 Then
                        'Send an Alert that the SP returned an error.
                        Dim AlertMess As String = CartObject.Type.ToString & " Error Calling SP procESPSetESPState forn CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess
                        AlertMail(CartObject.Type.ToString & " procESPSetESPSTate Failed.", AlertMess, "dbESPInfo")
                        Logit(CartObject.Type.ToString & " Error. Got Non Zero returnvalue. " & AlertMess)
                        ExLogIt("procESPSetESPState", AlertMess)
                    Else
                        DLogit(CartObject.Type.ToString & " Results of procESPSetESPState forCartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess)
                    End If

                End If

                'Something unexpected happened with the submission, so this is a candidate for a retry for now.
                enableRetry = True

            End If
            where = 4

            'I did not get an HTTP 20X Status Code, which triggered a Web Exception =====================================================================
            'I did not get an HTTP 20X Status Code, which triggered a Web Exception =====================================================================
            'I did not get an HTTP 20X Status Code, which triggered a Web Exception =====================================================================
            'I did not get an HTTP 20X Status Code, which triggered a Web Exception =====================================================================

            'First Level Catch - Web Exception Only
        Catch wex As WebException
            'Note that while GetResponseStream above produces a .NET WebException that throws you here for 40X or 50X response codes, 
            '   the HttpWebResponse is actually passed in the WebException itself, so reading THAT one will get you whatever is passed back.
            '   In this case, of course, it's how you get to the Json Status Code and Description they send with a 40X.

            If Not lastAttempt Then
                Logit(CartObject.Type.ToString & " I got a Web Exception of " & wex.Message & " for Cart=" & cart.BasketName & " so The Cart failed. Will retry. This was attempt " & attempts.ToString & " of " & Val(My.Settings.CartJobsRetryCount + 1).ToString)
                'We got an official HTTP Error here, so for now that means it is a candidate for Retry
                enableRetry = True

            Else

                Dim mess As String = CartObject.Type.ToString & " I got a Web Exception of " & wex.Message & " forCart=" & cart.BasketName & " so The Cart failed, retrys exhausted, & UserAlert goes to " & cart.UserGuid
                Dim jmess As String = ""

                'Only In the last attempt do I try to get the json and responsestream, but I just decide which UserAlert Template to use based on the HTTP code.
                '   Currently, only a 404 is a FailLibrary.  everything else is FailSystem

                Dim exResponse As WebResponse = wex.Response
                Dim exStatusCode As HttpStatusCode

                Dim httpResponse As HttpWebResponse = CType(exResponse, HttpWebResponse)
                exStatusCode = httpResponse.StatusCode

                jsonString = ""
                Dim httpRespString As System.IO.Stream = Nothing
                Dim sreader As StreamReader = Nothing
                Dim memstrJson As MemoryStream = Nothing

                If InStr(httpResponse.ContentType.ToUpper, "JSON") > 0 Then

                    'Try within Web Ex First Level Catch
                    Try
                        where = 11
                        'first, getthe response into a stream
                        httpRespString = httpResponse.GetResponseStream
                        where = 12
                        'then read the stream into a string
                        sreader = New StreamReader(httpRespString)
                        jsonString = sreader.ReadToEnd
                        where = 13
                        'then archive it for what it's worth.
                        FileItUTF8(My.Settings.LogDir, "json", CartObject.Type.ToString & "RespWex_" & cart.CartID, jsonString)
                        where = 14
                        'then create the serializer to read json into the class
                        'jsonSerializerR = New DataContractJsonSerializer(GetType(ESPWinServices.DistributeResponseJson))
                        where = 15
                        'now, read the string back into a memory stream
                        memstrJson = New MemoryStream(Encoding.Unicode.GetBytes(jsonString))
                        where = 16
                        '...and Deserialize the json into the class
                        'TODO - I can't find any way around this Select for the CType function,
                        '   which seems to ONLY be able to take the actual class type.
                        'Select Case CartObject.Type

                        '    Case Is = ESPWinServices.CartType.DIST
                        '        jcartResponse = CType(jsonSerializerR.ReadObject(memstrJson), ESPWinServices.DistributeResponseJson)
                        '    Case Is = ESPWinServices.CartType.FUND
                        '        jcartResponse = CType(jsonSerializerR.ReadObject(memstrJson), ESPWinServices.acceptFundResponseJson)
                        '    Case Else
                        '        jcartResponse = CType(jsonSerializerR.ReadObject(memstrJson), ESPWinServices.RankResponseJson)

                        'End Select
                        jcartResponse = Convert.ChangeType(jsonSerializerR.ReadObject(memstrJson), CartObject.jsonResponse.GetType)
                        Logit(CartObject.Type.ToString & " " & jcartResponse.GetType().ToString & " jcartResponse during wex Converted.")
                        where = 17
                        'We got a web exceptioin here, so it is unlikely we will get json pertaining to the cart structure...
                        If Not IsNothing(jcartResponse) Then
                            jmess = mess
                            where = 18
                            'Logit(CartObject.Type.ToString & " Json Status:" & jcartResponse.statusCode & " Description:" & jcartResponse.statusMessage)
                            'ESPStateCartStatus = jcartResponse.status.message & " http:" & httpResponse.StatusDescription
                            If CartObject.Type = ESPWinServices.CartType.DIST Then
                                If Not IsNothing(jcartResponse.statuscodes) Then
                                    For Each status As ESPWinServices.statusCode In jcartResponse.statusCodes
                                        Logit(CartObject.Type.ToString & " Json Status:" & status.code & " json Description:" & status.message)
                                    Next
                                    ESPStateCartStatus = jcartResponse.statusCodes(0).message

                                Else
                                    Logit(CartObject.Type.ToString & " Json Status:" & jcartResponse.statusCode & " Description:" & jcartResponse.statusMessage)
                                    ESPStateCartStatus = jcartResponse.status.message
                                End If
                                where = 19
                                Logit(jmess)
                            Else
                                ESPStateCartStatus = httpResponse.StatusDescription
                            End If
                        End If

                    Catch ex As Exception

                        Logit(CartObject.Type.ToString & where.ToString & ")=Exception Encountered trying to get json from HTTP response: " & ex.Message)
                        ExLogIt(My.Settings.ESPBaseURI & CartObject.Type.ToString, "Exception Encountered trying to get json from HTTP response: " & ex.Message)

                    Finally

                        jsonSerializerR = Nothing

                    End Try
                Else
                    With httpResponse
                        jmess = mess & vbCrLf & CartObject.Type.ToString & " No JSON found in httpResponse. Content.Type=" & httpResponse.ContentType & " Content.Length=" & httpResponse.ContentLength.ToString
                        Logit(jmess)
                    End With
                End If


                'Send User Alert for Non 20X HttpStatus WITH Web Exception Triggered ===================================
                'Send User Alert for Non 20X HttpStatus WITH Web Exception Triggered ===================================
                'Send User Alert for Non 20X HttpStatus WITH Web Exception Triggered ===================================
                'Send User Alert for Non 20X HttpStatus WITH Web Exception Triggered ===================================

                Dim UACall As Boolean = True
                Dim UATempl As Integer = CartObject.UserAlertTemplateEnum 'Default, and what is always used for FUND Error Alerts

                Select Case CartObject.Type
                    Case Is = ESPWinServices.CartType.DIST
                        If distWfund = True Then UATempl = CartObject.UserAlertTemplateEnum2

                    Case Is = ESPWinServices.CartType.RANK
                        If resp.StatusCode = HttpStatusCode.NotFound Then UATempl = CartObject.UserAlertTemplateEnum2

                End Select

                'TODO: Fix up UACall SendUserAlert
                'Try within Web Ex Catch - 1st Level UAlert Fail Web Ex
                Try
                    UACall = SendUserAlert(cart, UATempl)

                Catch ex As Exception
                    Logit(CartObject.Type.ToString & " Couldn't do UserAlert " & ex.Message)
                End Try

                If Not UACall Then
                    Logit(CartObject.Type.ToString & " User Alert Call Failed for user=" & cart.UserGuid & " and Basket=" & cart.CartID)
                    ExLogIt(CartObject.Type.ToString & " UserAlertsService", "Failure User Alert Call Failed for user=" & cart.UserGuid & " and Basket=" & cart.CartID)
                End If

                'Update ESP State Table for Non 20X HttpStatus WITH Web Exception Triggered ===================================
                'Update ESP State Table for Non 20X HttpStatus WITH Web Exception Triggered ===================================
                'Update ESP State Table for Non 20X HttpStatus WITH Web Exception Triggered ===================================
                'Update ESP State Table for Non 20X HttpStatus WITH Web Exception Triggered ===================================

                'Depending on the transaction type, I may or may not get a list of funds and branches in the success response
                '   If a success, they should all be "0"status which don't get passed to the DB, but we read them all anyway.
                If Not IsNothing(jcartResponse.fundCodes) Then

                    For Each fund As ESPWinServices.distFund In jcartResponse.fundCodes
                        If fund.status <> "0" Then utblFUN.Rows.Add(cart.CartID, fund.fundId)
                    Next

                End If


                If Not IsNothing(jcartResponse.branches) Then

                    For Each branch As ESPWinServices.distBranch In jcartResponse.branches
                        If branch.status <> "0" Then utblBRA.Rows.Add(cart.CartID, branch.branchId)
                    Next

                End If



                utblESP.Rows.Add(cart.CartID, CartObject.Type.ToString, "Failed", DBNull.Value, ESPStateCartStatus, mess, jsonString)

                'Try Within Web Ex Catch 1st Level SP for Failure Web Ex
                Try
                    ctbladapter.procESPSetESPState(utblESP, utblBRA, utblFUN, "OK")
                    cretvalue = ctbladapter.GetReturnValue(5)
                    cerrMess = ctbladapter.GetParam4(5)

                Catch upex As Exception
                    Logit(CartObject.Type.ToString & " Couldn't update ESPState w/WebEx: " & upex.Message)
                End Try

                If Not cretvalue = 0 Then
                    'Send an Alert that the SP returned an error.
                    Dim AlertMess As String = CartObject.Type.ToString & " Error Calling SP procESPSetESPState for CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess
                    AlertMail("procESPSetESPSTate Failed.", AlertMess, "dbESPInfo")
                    Logit(CartObject.Type.ToString & " Error. Got Non Zero returnvalue. " & AlertMess)
                    ExLogIt("procESPSetESPState", AlertMess)
                Else
                    DLogit(CartObject.Type.ToString & " Results of procESPSetESPState for CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess)
                End If

                'Last Attempt
                enableRetry = False
            End If

            'A General and Unexpected Exception has occurred somewhere in the submission/response block other than a Web Exception
            'A General and Unexpected Exception has occurred somewhere in the submission/response block other than a Web Exception
            'A General and Unexpected Exception has occurred somewhere in the submission/response block other than a Web Exception
            'A General and Unexpected Exception has occurred somewhere in the submission/response block other than a Web Exception
            '1st Level Catch for anything but Web Exception
        Catch ex As Exception

            If Not lastAttempt Then
                Logit(CartObject.Type.ToString & where.ToString & ")-I got a General Exception of " & ex.Message & " for Cart=" & cart.BasketName & " so The Cart failed. Will Retry. This was attempt " & attempts.ToString & " of " & Val(My.Settings.CartJobsRetryCount + 1).ToString)
                'We got a .NET General Exception here, so for now that means it is a candidate for Retry
                enableRetry = True

            Else

                Dim mess As String = CartObject.Type.ToString & where.ToString & ")-I got a General Exception of " & ex.Message & " for Cart=" & cart.BasketName & " so The Cart failed, Retries Exhausted, & UserAlert goes to: " & cart.UserGuid
                Logit(mess)
                If Not IsNothing(ex.StackTrace) Then Logit(ex.StackTrace)
                ExLogIt(CartObject.Type.ToString & "Cart", mess)


                'Send A User Alert For any General Exception during the process (Non-Web)  ===========================================
                'Send A User Alert For any General Exception during the process (Non-Web)  ===========================================
                'Send A User Alert For any General Exception during the process (Non-Web)  ===========================================
                'Send A User Alert For any General Exception during the process (Non-Web)  ===========================================

                'TODO: logic for W W/O fund
                Dim UACall As Boolean = True
                Dim UATempl As Integer = CartObject.UserAlertTemplateEnum 'Default, and what is always used for FUND Error Alerts
                Select Case CartObject.Type
                    Case Is = ESPWinServices.CartType.DIST
                        If distWfund = True Then UATempl = CartObject.UserAlertTemplateEnum2

                    Case Is = ESPWinServices.CartType.RANK
                        If resp.StatusCode = HttpStatusCode.BadRequest Then UATempl = CartObject.UserAlertTemplateEnum2

                End Select


                'Try within General Exception Catch - User Alert Gen Ex
                Try
                    UACall = SendUserAlert(cart, UATempl)

                Catch ex1 As Exception
                    Logit(CartObject.Type.ToString & " Couldn't do UserAlert " & ex1.Message)

                End Try

                If Not UACall Then
                    Logit(CartObject.Type.ToString & " User Alert Call Failed for user=" & cart.UserGuid & " and Basket=" & cart.CartID)
                    ExLogIt("UserAlertsService", CartObject.Type.ToString & " User Alert Call Failed for user=" & cart.UserGuid & " and Basket=" & cart.CartID)
                End If

                'Update ESP State Table for a General System Exception (Non-Web) ======================================================
                'Update ESP State Table for a General System Exception (Non-Web) ======================================================
                'Update ESP State Table for a General System Exception (Non-Web) ======================================================
                'Update ESP State Table for a General System Exception (Non-Web) ======================================================

                'Depending on the transaction type, I may or may not get a list of funds and branches in the success response
                '   If a success, they should all be "0"status which don't get passed to the DB, but we read them all anyway.
                If Not IsNothing(jcartResponse.fundCodes) Then

                    For Each fund As ESPWinServices.distFund In jcartResponse.fundCodes
                        If fund.status <> "0" Then utblFUN.Rows.Add(cart.CartID, fund.fundId)
                    Next

                End If


                If Not IsNothing(jcartResponse.branches) Then

                    For Each branch As ESPWinServices.distBranch In jcartResponse.branches
                        If branch.status <> "0" Then utblBRA.Rows.Add(cart.CartID, branch.branchId)
                    Next

                End If


                utblESP.Rows.Add(cart.CartID, CartObject.Type.ToString, "Failed", DBNull.Value, "System Exception", mess, DBNull.Value)

                'Try within General Ex Catch - SP update Failed General Exception
                Try
                    ctbladapter.procESPSetESPState(utblESP, utblBRA, utblFUN, "OK")
                    cretvalue = ctbladapter.GetReturnValue(5)
                    cerrMess = ctbladapter.GetParam4(5)

                Catch upex As Exception
                    Logit(CartObject.Type.ToString & " Couldn't update ESPState w/GenEx: " & upex.Message)
                End Try

                If Not cretvalue = 0 Then
                    'Send an Alert that the SP returned an error.
                    Dim AlertMess As String = CartObject.Type.ToString & " Error Calling SP procESPSetESPState for CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess
                    AlertMail("procESPSetESPSTate Failed.", AlertMess, "dbESPInfo")
                    Logit(CartObject.Type.ToString & " Error. Got Non Zero returnvalue. " & AlertMess)
                    ExLogIt("procESPSetESPState", AlertMess)
                Else
                    DLogit(CartObject.Type.ToString & " Results of procESPSetESPState for CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess)
                End If

                'this is the last attempt
                enableRetry = False
            End If


        Finally

            'clean up your mess
            If Not IsNothing(msr) Then
                msr.Close()
                msr.Dispose()
            End If
            If Not IsNothing(msJson) Then
                msJson.Close()
                msJson.Dispose()
            End If
            If Not IsNothing(PostData) Then
                PostData.Close()
                PostData.Dispose()
            End If
        End Try

        'We should always get here, all errors or exceptions should have been logged/reported/Alerted so All we do on the return
        '   is pass back the cart, what attempt number this was, and whether or not a retry is requested.
        '   Note that the EventHandler in the calling module handles the logic for whether or not a retry is actually performed... based on the number of attempts.
        Dim rc As New CartAttemptCompleteEventArgs

        rc.attemptNo = attempts
        rc.cart = cart
        rc.enableRetry = enableRetry
        rc.xmlFileName = xmlFileName

        RaiseEvent CallBackCartAttemptComplete(Me, rc)

    End Sub
    Sub DLogit(ByVal mess As String)
        If My.Settings.DebugLogging = False Then Exit Sub

        Logit(mess)

    End Sub
    Sub Logit(ByVal mess As String)

        Dim e As New LogItEventArgs
        e.mess = mess

        RaiseEvent CallBackLogIt(Me, e)

    End Sub
    Sub AlertMail(ByVal subj As String, ByVal mess As String, ByVal group As String)

        Dim e As New AlertMailEventArgs
        e.subject = subj
        e.body = mess
        e.group = group

        RaiseEvent CallBackAlertMail(Me, e)

    End Sub
    Sub FileItUTF8(ByVal path As String, ByVal extension As String, ByVal fileprefix As String, ByVal content As String)

        Dim e As New FileItUTF8EventArgs
        e.content = content
        e.dirPath = path
        e.extension = extension
        e.filePrefix = fileprefix

        RaiseEvent CallBackFileItUTF8(Me, e)

    End Sub

    Sub ExLogIt(ByVal method As String, ByVal exMessage As String, Optional ByVal RequestMsg As String = "", Optional ByVal ResponseMsg As String = "")

        Dim e As New ExLogItEventArgs
        e.method = method
        e.exMessage = exMessage
        e.RequestMsg = RequestMsg
        e.ResponseMsg = ResponseMsg

        RaiseEvent CallBackExLogIt(Me, e)


    End Sub

    Function SendUserAlert(ByVal cart As Object, ByVal template As UserAlertsService.AlertMessageTemplateIDEnum) As Boolean
        'TODO: Is it OK we just take this cart as an Object?

        'be optimistic
        SendUserAlert = True

        Dim uaClient As New UserAlertsService.UserAlertsClient
        Dim uaTmplResp As UserAlertsService.GetUserAlertMessageTemplateResponse = Nothing
        Dim alertToSend As String = ""
        Dim URL As String = ""

        Dim where As String = ""

        cart = Convert.ChangeType(cart, CartObject.RequestType.GetType)


        'call the service to get the template
        Try
            uaTmplResp = uaClient.GetUserAlertMessageTemplate(template)
            alertToSend = uaTmplResp.AlertMessageTemplate
            If template = UserAlertsService.AlertMessageTemplateIDEnum.ESPFundComplete Then
                'get the URL to pass back prefixing the CartID
                URL = uaClient.GetUserAlertMessageTemplate(template).ConfigReferenceValue
            End If

        Catch ex As Exception
            'Do something or tell somebody.
            If Not IsNothing(ex.StackTrace) Then
                Logit(CartObject.Type.ToString & " UserAlert Get Template Failed for BasketID=" & cart.CartID & " :" & ex.Message & ex.StackTrace)

            Else
                Logit(CartObject.Type.ToString & " UserAlert Get Template Failed for BasketID=" & cart.CartID & " :" & ex.Message)

            End If
            SendUserAlert = False
            Return SendUserAlert
        End Try

        Try
            DLogit(CartObject.Type.ToString & " Sending Alert For CartType: " & cart.ToString)
            DLogit(CartObject.Type.ToString & " Sending Alert Using TemplateEnum: " & template.ToString)

            'replace @cartName with cart.BasketName
            alertToSend = alertToSend.Replace("@cartname", cart.BasketName)
            If template = UserAlertsService.AlertMessageTemplateIDEnum.ESPFundComplete Then
                alertToSend = alertToSend.Replace("@URL", URL & cart.CartID)
            End If


        Catch ex As Exception
            'Do something or tell somebody.
            If Not IsNothing(ex.StackTrace) Then
                Logit(CartObject.Type.ToString & " UserAlert String.Replace Failed for BasketID=" & cart.CartID & " :" & ex.Message & ex.StackTrace)
            Else
                Logit(CartObject.Type.ToString & " UserAlert String.Replace Failed for BasketID=" & cart.CartID & " :" & ex.Message)

            End If
            SendUserAlert = False
            Return SendUserAlert

        End Try

        DLogit(CartObject.Type.ToString & " Alert to Send: " & alertToSend)

        Dim uaCreateResp As UserAlertsService.CreateUserAlertMessageResponse = Nothing

        'call the service and send the edited template along with template name and cart.userGuid
        Try
            uaCreateResp = uaClient.CreateUserAlertMessage(alertToSend, cart.UserGuid, template, "ESPWinServices")
        Catch ex As Exception
            'Do something or tell somebody
            Logit(CartObject.Type.ToString & " UserAlert Create Alert Failed for BasketID=" & cart.CartID & " :" & ex.Message)
            SendUserAlert = False
            Return SendUserAlert
        End Try

        'if there are any problems, return false, else return true

        Return SendUserAlert

    End Function


End Class

