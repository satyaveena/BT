Imports System.Net
Imports System.Runtime.Serialization.Json
Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Data.SqlClient
Imports System.Threading

Public Class DistCart

    Public Event CallBackLogIt As EventHandler(Of LogItEventArgs)
    Public Event CallBackAlertMail As EventHandler(Of AlertMailEventArgs)
    Public Event CallBackFileItUTF8 As EventHandler(Of FileItUTF8EventArgs)
    Public Event CallBackDistAttemptComplete As EventHandler(Of CartAttemptCompleteEventArgs)
    Public Event CallBackExLogIt As EventHandler(Of ExLogItEventArgs)

    Dim WithEvents StartTimer As New Timers.Timer
    Dim cart2Dist As DistributionRequestsDistributionRequest
    Dim attemptNo As Short
    Dim delaySecs As Short
    Dim xmlFile As String
    Dim utblESP As DataTable
    Dim utblBRA As DataTable
    Dim utblFUN As DataTable


    Public Sub New(ByVal Cart As DistributionRequestsDistributionRequest, ByVal attempts As Short, ByVal delay As Short, ByVal xmlFileName As String, ByVal utblESPStatus As DataTable, ByVal utblBranches As DataTable, ByVal utblFundCodes As DataTable)
        cart2Dist = Cart
        attemptNo = attempts
        delaySecs = delay
        xmlFile = xmlFileName
        utblESP = utblESPStatus
        utblBRA = utblBranches
        utblFUN = utblFundCodes

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

    End Sub

    Private Sub StartTimer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles StartTimer.Elapsed

        DistACart(cart2Dist, attemptNo, delaySecs, xmlFile)

    End Sub

    Public Sub DistACart(ByVal cart As DistributionRequestsDistributionRequest, ByVal attempts As Short, ByVal delaysecs As Short, ByVal xmlFileName As String)

        Dim lastAttempt As Boolean = False
        If attempts >= Val(My.Settings.CartJobsRetryCount + 1) Then lastAttempt = True

        'This flag is just to tell the caller that this cart should be retried, if they aren't exhausted already.
        '   We start optimistically, set it to true as needed (a retryable failure) and test at the very end before calling back
        Dim enableRetry As Boolean = False

        'we're a go. first create the serializer to work on the Class created to hold DistRequests for serializing
        Dim jsonSerializer As DataContractJsonSerializer
        jsonSerializer = New DataContractJsonSerializer(GetType(ESPWinServices.DistributeRequestJson))

        'Then setup the memory stream for accepting the serialized data
        Dim msJson As MemoryStream = Nothing
        Dim msr As StreamReader = Nothing
        Dim jsonString As String
        Dim req As HttpWebRequest = Nothing
        Dim resp As HttpWebResponse = Nothing
        Dim PostData As Stream = Nothing
        Dim jsonSerializerR As DataContractJsonSerializer
        Dim distResponse As ESPWinServices.DistributeResponseJson = Nothing
        Dim jsonCart As ESPWinServices.DistributeRequestJson
        Dim uri As String
        Dim ctbladapter As New OrdersDataSetTableAdapters.QueriesTableAdapter
        Dim cretvalue As Int32 = 0
        Dim cerrMess As String = ""
        Dim where As Short = 0
        Dim ESPStateCartStatus As String = "Not Supplied"

        'If this is a Retry, we want to pause before doing anything...
        If delaysecs > 0 Then
            For looper = 1 To delaysecs * 10
                Thread.Sleep(100)
            Next
        End If


        'XMLClass to JSONClass=======================================================================================
        Try
            jsonCart = New ESPWinServices.DistributeRequestJson
            jsonCart.branches = New List(Of ESPWinServices.branch)
            jsonCart.items = New List(Of ESPWinServices.Item)
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
            Dim filprefix As String = "Cart2Dist_" & cart.CartID
            FileItUTF8(My.Settings.LogDir, "json", filprefix, jsonString)


            'Prepare for Calling ESP RESTful =========================================================================
            'and here is where you will call the RESTful service and assess the results, acting accordingly.
            uri = My.Settings.ESPBaseURI & "jobs/distribute"

            'but for unit testing we overwrite with my local RESTService
            'uri = "http://localhost:63062/RESTService/esp/jobs/distribute"

            If My.Settings.DebugLogging = True Then Logit("Calling: " & uri)

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
                        Logit("Name:" & header & " Value:" & value)
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

                Try

                    'first, getthe response into a stream
                    httpRespString = resp.GetResponseStream

                    'then read the stream into a string
                    sreader = New StreamReader(httpRespString)
                    jsonString = sreader.ReadToEnd

                    'then archive it for what it's worth.
                    FileItUTF8(My.Settings.LogDir, "json", "DistResp_" & cart.CartID, jsonString)

                    'then create the serializer to read json into the class
                    jsonSerializerR = New DataContractJsonSerializer(GetType(ESPWinServices.DistributeResponseJson))

                    'now, read the string back into a memory stream
                    memstrJson = New MemoryStream(Encoding.Unicode.GetBytes(jsonString))

                    '...and Deserialize the json into the class
                    distResponse = CType(jsonSerializerR.ReadObject(memstrJson), ESPWinServices.DistributeResponseJson)

                    'which leaves you the json string and the class created from it both accessible as needed.

                    For Each status As ESPWinServices.statusCode In distResponse.statuscodes
                        Logit("Distribution Json Status:" & status.code & " json Description:" & status.message)
                    Next

                Catch ex As Exception

                    Logit("Exception Encountered trying to get json from Distribution HTTP response: " & ex.Message)
                    ExLogIt(uri, "Exception Encountered trying to get json from Distribution HTTP response: " & ex.Message)
                    ESPStateCartStatus = resp.StatusDescription
                Finally

                    jsonSerializerR = Nothing

                End Try

            End If


            where = 2
            'If we did not get an Exception - Examine what we got  and act accordingly ================================================================================
            If My.Settings.DebugLogging = True Then Logit("Distribution HTTP Response: " & resp.StatusCode & "-" & resp.StatusDescription)

            If Not IsNothing(distResponse) Then
                Logit("distStatusCode=" & distResponse.statusCodes(0).code.ToString & ", distStatusMessage=" & distResponse.statusCodes(0).message & ", Job ID =" & distResponse.jobId)
                ESPStateCartStatus = distResponse.statusCodes(0).message
            Else
                Logit("Distribution JSON Content not found")
                ESPStateCartStatus = resp.StatusDescription
            End If

            If resp.StatusCode = HttpStatusCode.Accepted Or resp.StatusCode = HttpStatusCode.OK Then
                Logit("All is well for Distributing Cart=" & cart.BasketName & " So I will update it's status " & "for Distribution CartID=" & cart.CartID)
                where = 201
                where = 202
                If Not IsNothing(distResponse.fundCodes) Then
                    where = 2021
                    For Each fund As ESPWinServices.distFund In distResponse.fundCodes
                        If fund.status <> "0" Then utblFUN.Rows.Add(cart.CartID, fund.fundId)
                    Next

                End If
                where = 203

                For Each branch As ESPWinServices.distBranch In distResponse.branches
                    If branch.status <> "0" Then utblBRA.Rows.Add(cart.CartID, branch.branchId)
                Next
                where = 204

                utblESP.Rows.Add(cart.CartID, "DIST", "Submitted", distResponse.jobId, ESPStateCartStatus, "No Exceptions", jsonString)

                where = 205

                Try
                    ctbladapter.procESPSetESPState(utblESP, utblBRA, utblFUN, "OK")
                    cretvalue = ctbladapter.GetReturnValue(5)
                    cerrMess = ctbladapter.GetParam4(5)

                Catch upex As Exception
                    Logit("DIST: Couldn't update ESPState w/Success: " & upex.Message)
                End Try

                where = 207
                If Not cretvalue = 0 Then
                    'Send an Alert that the SP returned an error.
                    Dim AlertMess As String = "Error Calling SP procESPSetESPState for Distribution CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess
                    AlertMail("procESPSetESPSTate Failed.", AlertMess, "dbESPInfo")
                    ExLogIt("procESPSetESPState", AlertMess)
                    Logit("Error. Got Non Zero returnvalue. " & AlertMess)
                Else
                    where = 208
                    If My.Settings.DebugLogging = True Then Logit("Results of procESPSetESPState for Distribution CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess)
                End If

                'Note, we don't re-attempt the Cart Dist at this point, even if the call to update it's status failed, 'cause we think the dist submission succeeded.


            Else
                where = 3
                'Most, if not all, non 20X Http Status codes will cause a .NET WebException, so we don't think we'll get here.
                '   If there is json, we should have it here...
                '   But we will treat this as a failure and log the results for checking later.
                If Not lastAttempt Then

                    Logit("I got an HTTP Status of " & resp.StatusCode.ToString & " for Distribution Cart=" & cart.BasketName & " so I think I failed because it's not 'OK' or 'ACCEPTED'. Will Retry. This was attempt " & attemptNo.ToString & " of " & (Val(My.Settings.CartJobsRetryCount) + 1).ToString)

                Else

                    Dim mess As String = "I got an HTTP Status of " & resp.StatusCode.ToString & " for Distribution Cart=" & cart.BasketName & " so I think I failed because it's not 'OK' or 'ACCEPTED'. Retries Exhausted. User Alert goes to " & cart.UserGuid

                    Dim exMess As String = "I got an HTTP Status of " & resp.StatusCode.ToString & " for Distribution Cart=" & cart.BasketName

                    'TODO: verify this array is base=1, and consider stepping through the array instead of being lazy below
                    If Not IsNothing(distResponse) Then
                        mess = mess & " DistStatusCode=" & distResponse.statusCodes(0).code.ToString & ", DistStatusMessage=" & distResponse.statusCodes(0).message & ", Job ID =" & distResponse.jobId
                        exMess = exMess & " DistStatusCode=" & distResponse.statusCodes(0).code.ToString & ", DistStatusMessage=" & distResponse.statusCodes(0).message & ", Job ID =" & distResponse.jobId
                    End If
                    Logit(mess)
                    ExLogIt("DistCart", exMess)

                    'TODO: confirm no system error distinction, and code for DistWFundFail
                    Dim UACall As Boolean = True
                    Dim UATempl As Integer = UserAlertsService.AlertMessageTemplateIDEnum.ESPDistWOFundFail
                    If resp.StatusCode = HttpStatusCode.BadRequest Then UATempl = UserAlertsService.AlertMessageTemplateIDEnum.ESPDistWOFundFail


                    UACall = SendUserAlert(cart, UATempl)

                    If Not UACall Then
                        Logit("User Alert Call Failed for user=" & cart.UserGuid & " and Distribution Basket=" & cart.CartID)
                        ExLogIt("UserAlertsService", "User Alert Call Failed for user=" & cart.UserGuid & " and Distribution Basket=" & cart.CartID)
                    End If

                    utblESP.Rows.Add(cart.CartID, "DIST", "Failed", DBNull.Value, ESPStateCartStatus, mess, jsonString)

                    Try
                        ctbladapter.procESPSetESPState(utblESP, utblBRA, utblFUN, "OK")
                        cretvalue = ctbladapter.GetReturnValue(5)
                        cerrMess = ctbladapter.GetParam4(5)

                    Catch upex As Exception
                        Logit("DIST: Couldn't update ESPState w/Fail: " & upex.Message)
                    End Try

                    If Not cretvalue = 0 Then
                        'Send an Alert that the SP returned an error.
                        Dim AlertMess As String = "Error Calling SP procESPSetESPState for Distribution CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess
                        AlertMail("procESPSetESPSTate Failed.", AlertMess, "dbESPInfo")
                        Logit("Error. Got Non Zero returnvalue. " & AlertMess)
                        ExLogIt("procESPSetESPState", AlertMess)
                    Else
                        If My.Settings.DebugLogging = True Then Logit("Results of procESPSetESPState for Distribution CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess)
                    End If

                End If

                'Something unexpected happened with the submission, so this is a candidate for a retry for now.
                enableRetry = True

            End If
            where = 4
            'If we did get a .NET Web Exception we act on it here.  I'm not sure if there will be a way to get at the Json in the event of a HTTP 40x which I think will land us here.================================
        Catch wex As WebException
            'Note that while GetResponseStream above produces a .NET WebException that throws you here for 40X or 50X response codes, 
            '   the HttpWebResponse is actually passed in the WebException itself, so reading THAT one will get you whatever is passed back.
            '   In this case, of course, it's how you get to the Json Status Code and Description they send with a 40X.

            If Not lastAttempt Then
                Logit("I got a Web Exception of " & wex.Message & " for Distribution Cart=" & cart.BasketName & " so The Distributing failed. Will retry. This was attempt " & attemptNo.ToString & " of " & Val(My.Settings.CartJobsRetryCount + 1).ToString)


            Else

                Dim mess As String = "I got a Web Exception of " & wex.Message & " for Distribution Cart=" & cart.BasketName & " so The Distributing failed, retrys exhausted, & UserAlert goes to " & cart.UserGuid
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
                        FileItUTF8(My.Settings.LogDir, "json", "DistResp_" & cart.CartID, jsonString)
                        where = 14
                        'then create the serializer to read json into the class
                        jsonSerializerR = New DataContractJsonSerializer(GetType(ESPWinServices.DistributeResponseJson))
                        where = 15
                        'now, read the string back into a memory stream
                        memstrJson = New MemoryStream(Encoding.Unicode.GetBytes(jsonString))
                        where = 16
                        '...and Deserialize the json into the class
                        distResponse = CType(jsonSerializerR.ReadObject(memstrJson), ESPWinServices.DistributeResponseJson)
                        where = 17
                        If Not IsNothing(distResponse) Then
                            jmess = mess
                            where = 18
                            For Each status As ESPWinServices.statusCode In distResponse.statusCodes
                                jmess = jmess & vbCrLf & " Distribution Json Status:" & status.code & " json Description:" & status.message & ", Job ID =" & distResponse.jobId
                            Next
                            where = 19
                            Logit(jmess)
                            ESPStateCartStatus = httpResponse.StatusDescription
                        Else
                            ESPStateCartStatus = distResponse.statusCodes(0).message
                        End If

                    Catch ex As Exception

                        Logit(where.ToString & ")=Exception Encountered trying to get json from Distribution HTTP response: " & ex.Message)
                        ExLogIt(My.Settings.ESPBaseURI & "jobs/distribute", "Exception Encountered trying to get json from HTTP response: " & ex.Message)

                    Finally

                        jsonSerializerR = Nothing

                    End Try
                Else
                    With httpResponse
                        jmess = mess & vbCrLf & "No JSON found in Distribution httpResponse. Content.Type=" & httpResponse.ContentType & " Content.Length=" & httpResponse.ContentLength.ToString
                        Logit(jmess)
                    End With
                End If




                Dim UACall As Boolean = True
                Dim UATempl As Integer

                'TODO: no diff sys/err and logic for W W/O fund
                Select Case exStatusCode
                    Case Is = HttpStatusCode.NotFound
                        UATempl = UserAlertsService.AlertMessageTemplateIDEnum.ESPDistWOFundFail

                    Case Else
                        UATempl = UserAlertsService.AlertMessageTemplateIDEnum.ESPDistWOFundFail

                End Select

                'TODO: Fix up UACall SendUserAlert
                UACall = SendUserAlert(cart, UATempl)

                If Not UACall Then
                    Logit("User Alert Call Failed for user=" & cart.UserGuid & " and Distribution Basket=" & cart.CartID)
                    ExLogIt("UserAlertsService", "User Alert Call Failed for user=" & cart.UserGuid & " and Distribution Basket=" & cart.CartID)
                End If

                utblESP.Rows.Add(cart.CartID, "DIST", "Failed", DBNull.Value, ESPStateCartStatus, mess, jsonString)

                Try
                    ctbladapter.procESPSetESPState(utblESP, utblBRA, utblFUN, "OK")
                    cretvalue = ctbladapter.GetReturnValue(5)
                    cerrMess = ctbladapter.GetParam4(5)

                Catch upex As Exception
                    Logit("DIST: Couldn't update ESPState w/WebEx: " & upex.Message)
                End Try

                If Not cretvalue = 0 Then
                    'Send an Alert that the SP returned an error.
                    Dim AlertMess As String = "Error Calling SP procESPSetESPState for Distribution CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess
                    AlertMail("procESPSetESPSTate Failed.", AlertMess, "dbESPInfo")
                    Logit("Error. Got Non Zero returnvalue. " & AlertMess)
                    ExLogIt("procESPSetESPState", AlertMess)
                Else
                    If My.Settings.DebugLogging = True Then Logit("Results of procESPSetESPState for Distribution CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess)
                End If

            End If

            'We got an official HTTP Error here, so for now that means it is a candidate for Retry
            enableRetry = True

            'And if we got a General Exception anywhere else, we handlie it here =============================================================================================
        Catch ex As Exception

            If lastAttempt Then
                Logit(where.ToString & ")-I got a General Exception of " & ex.Message & " for Distribution Cart=" & cart.BasketName & " so The Distributing failed. Will Retry. This was attempt " & attemptNo.ToString & " of " & Val(My.Settings.CartJobsRetryCount + 1).ToString)

            Else

                Dim mess As String = where.ToString & ")-I got a General Exception of " & ex.Message & " for Distribution Cart=" & cart.BasketName & " so The Distributing failed, Retries Exhausted, & UserAlert goes to: " & cart.UserGuid
                Logit(mess)
                ExLogIt("DistCart", mess)

                'TODO: logic for W W/O fund
                Dim UACall As Boolean = True
                Dim UATempl As UserAlertsService.AlertMessageTemplateIDEnum = UserAlertsService.AlertMessageTemplateIDEnum.ESPDistWOFundFail

                UACall = SendUserAlert(cart, UATempl)

                If Not UACall Then
                    Logit("User Alert Call Failed for user=" & cart.UserGuid & " and Distribution Basket=" & cart.CartID)
                    ExLogIt("UserAlertsService", "User Alert Call Failed for user=" & cart.UserGuid & " and Distribution Basket=" & cart.CartID)
                End If

                utblESP.Rows.Add(cart.CartID, "DIST", "Failed", DBNull.Value, "System Exception", mess, DBNull.Value)

                Try
                    ctbladapter.procESPSetESPState(utblESP, utblBRA, utblFUN, "OK")
                    cretvalue = ctbladapter.GetReturnValue(5)
                    cerrMess = ctbladapter.GetParam4(5)

                Catch upex As Exception
                    Logit("DIST: Couldn't update ESPState w/GenEx: " & upex.Message)
                End Try

                If Not cretvalue = 0 Then
                    'Send an Alert that the SP returned an error.
                    Dim AlertMess As String = "Error Calling SP procESPSetESPState for Distribution CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess
                    AlertMail("procESPSetESPSTate Failed.", AlertMess, "dbESPInfo")
                    Logit("Error. Got Non Zero returnvalue. " & AlertMess)
                    ExLogIt("procESPSetESPState", AlertMess)
                Else
                    If My.Settings.DebugLogging = True Then Logit("Results of procESPSetESPState for Distribution CartID=" & cart.CartID & " Return value=" & cretvalue.ToString & " Message=" & cerrMess)
                End If
            End If

            'We got a .NET Exception here, so for now that means it is a candidate for Retry
            enableRetry = True

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

        RaiseEvent CallBackDistAttemptComplete(Me, rc)

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

    Function SendUserAlert(ByVal cart As DistributionRequestsDistributionRequest, ByVal template As UserAlertsService.AlertMessageTemplateIDEnum) As Boolean

        'be optimistic
        SendUserAlert = True

        Dim uaClient As New UserAlertsService.UserAlertsClient
        Dim uaTmplResp As UserAlertsService.GetUserAlertMessageTemplateResponse = Nothing
        Dim alertToSend As String = ""

        'call the service to get the template
        Try
            uaTmplResp = uaClient.GetUserAlertMessageTemplate(template)
            alertToSend = uaTmplResp.AlertMessageTemplate

        Catch ex As Exception
            'Do something or tell somebody.
            Logit("UserAlert Get Template Failed for BasketID=" & cart.CartID & " :" & ex.Message)
            SendUserAlert = False
            Return SendUserAlert
        End Try


        'replace @cartName with cart.BasketName
        alertToSend = alertToSend.Replace("@cartname", cart.BasketName)

        If My.Settings.DebugLogging = True Then Logit(alertToSend)

        Dim uaCreateResp As UserAlertsService.CreateUserAlertMessageResponse = Nothing

        'call the service and send the edited template along with template name and cart.userGuid
        Try
            uaCreateResp = uaClient.CreateUserAlertMessage(alertToSend, cart.UserGuid, template, "ESPWinServices")
        Catch ex As Exception
            'Do something or tell somebody
            Logit("UserAlert Create Alert Failed for BasketID=" & cart.CartID & " :" & ex.Message)
            SendUserAlert = False
            Return SendUserAlert
        End Try

        'if there are any problems, return false, else return true

        Return SendUserAlert

    End Function


End Class
