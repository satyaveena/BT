Imports System.Web.SessionState
Imports System.Threading
Public Class Global_asax
    Inherits System.Web.HttpApplication
    Public Shared MyAppData As New BTPricingServices.AppData
    Public Shared AppPath As String


    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started

        AppPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath

        Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(AppData))

        ' A FileStream is needed to read the XML document.
        Dim fs As New IO.FileStream(Server.MapPath("./App_Data") & "\BTPricingServicesAppData.xml", IO.FileMode.Open)
        Dim reader As New System.Xml.XmlTextReader(fs)

        ' Use the Deserialize method to restore the object's state.
        MyAppData = CType(serializer.Deserialize(reader), AppData)

        'don't forget to close everything
        reader.Close()
        fs.Close()
        reader = Nothing
        fs = Nothing

        Dim Startmess As String = "Runtime Initialized with AppData.  Status Messages Loaded = " & MyAppData.StatusMessage.Count.ToString & ". " & Global_asax.AppPath

        GlobalLogMess(AppPath, Startmess)
        GlobalDebugIt(AppPath, Startmess)



    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request

        GlobalDebugIt(AppPath, "===========Application_BeginRequest has fired... " & Now.ToString)

    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
        Dim AppEx As Exception = Server.GetLastError.GetBaseException
        Try
            GlobalDebugIt(AppPath, "        Runtime Application_Error has fired = " & AppEx.Message & " - " & Now.ToString)
            GlobalLogMess(AppPath, "Runtime Application_Error : " & AppEx.Message & " Stack Trace: " & AppEx.StackTrace)

        Catch ex As Exception
            GlobalDebugIt(AppPath, "Exception While Logging Application_Error. " & ex.Message)
        End Try


    End Sub

    Sub Application_EndRequest(Sender As [Object], e As EventArgs)

        'The below is suppposed to capture input data for a post and append it to the IIS log... but it doesn't work.
        '   It may be just a matter of setting the BinaryRead pointer back to 0, or using the objects that do seem to work.
        '   However, now that we can log to a flat file, No need to try and jam it into the IIS log.

        'If "POST" = Request.HttpMethod Then
        '    Dim bytes As Byte() = Request.BinaryRead(Request.TotalBytes)
        '    Dim s As String = Encoding.UTF8.GetString(bytes)
        '    If Not [String].IsNullOrEmpty(s) Then
        '        Dim QueryStringLength As Integer = 0
        '        If 0 < Request.QueryString.Count Then
        '            QueryStringLength = Request.ServerVariables("QUERY_STRING").Length
        '            Response.AppendToLog("&")
        '        End If


        '        If 4100 > (QueryStringLength + s.Length) Then
        '            Response.AppendToLog(s)
        '        Else
        '            ' append only the first 4090 the limit is a total of 4100 char.
        '            Response.AppendToLog(s.Substring(0, (4090 - QueryStringLength)))
        '            ' indicate buffer exceeded
        '            ' TODO: if s.Length >; 4000 then log to separate file
        '            Response.AppendToLog("|||...|||")
        '        End If
        '    End If
        'End If
        If Trim(Context.Response.StatusCode.ToString) <> "200" Then
            GlobalLogMess(AppPath, "HTTP Not OK! Status = " & Context.Response.StatusCode.ToString & " - " & Context.Response.StatusDescription)
        End If
        'This, nowever, works beautifully, and coupled with the log writer below, writes the entire SOAP XML to a log file.

        'just to make sure we are at the beginging....
        Context.Request.InputStream.Position = 0
        Dim buffer As Byte() = New Byte(Context.Request.InputStream.Length - 1) {}
        Context.Request.InputStream.Read(buffer, 0, buffer.Length)
        Context.Request.InputStream.Position = 0

        Dim soapMessage As String = Encoding.ASCII.GetString(buffer)
        'Now you can do anything you want with this string.

        GlobalDebugIt(AppPath, "===========Application_EndRequest has fired...[" & soapMessage & "] HTTPStatus=" & Context.Response.StatusCode.ToString & " - Request bytes = " & Request.TotalBytes.ToString & " - " & Now.ToString)


    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

    Sub GlobalDebugIt(Optional ByVal path As String = "", Optional ByVal mess As String = "No Global Debug Message Supplied.")

        If path = "" Then path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath

        If Not IsNothing(My.Settings.EnableLogging) Then
            If Not My.Settings.EnableLogging Then Exit Sub
        End If

        Dim linemess As String = mess & " - " & Now.ToString

        'I can't seem to use Format in an expression, so I break the date and padd it up here.
        Dim MM As String
        Dim DD As String

        MM = Today.Month.ToString
        If Len(MM) < 2 Then MM = "0" & MM
        DD = Today.Day.ToString
        If Len(DD) < 2 Then DD = "0" & DD


        Dim DSTamp As String = Today.Year.ToString & MM & DD

        Dim logger As System.IO.StreamWriter = Nothing
        Dim retries As Short = 0

tryagain:

        Try
            logger = My.Computer.FileSystem.OpenTextFileWriter(path & "Logs\" & DSTamp & "Debug_Log.txt", True, Encoding.ASCII)
            logger.WriteLine(linemess)
            logger.Close()

        Catch ex As Exception
            'for now, we don't care if there are issues writing to the log file, but we'll try to close it
            If Not IsNothing(logger) Then logger.Close()
            retries += 1
            Thread.Sleep(100)
            If retries < 10 Then GoTo tryagain

        End Try

        logger = Nothing
        DSTamp = Nothing
        MM = Nothing
        DD = Nothing


    End Sub

    Sub GlobalLogMess(Optional ByVal path As String = "", Optional ByVal mess As String = "No Global Message Supplied")

        If path = "" Then path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath

        'Note that AppPath is set in Application_Start.  For reference, finding the physical path is done one way during the request, and another during Application_Start, listed in order below
        'AppPath = Context.Request.PhysicalApplicationPath 
        'AppPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath

        Dim linemess As String = mess & " - " & Now.ToString

        'I can't seem to use Format in an expression, so I break the date and padd it up here.
        Dim MM As String
        Dim DD As String

        MM = Today.Month.ToString
        If Len(MM) < 2 Then MM = "0" & MM
        DD = Today.Day.ToString
        If Len(DD) < 2 Then DD = "0" & DD


        Dim DSTamp As String = Today.Year.ToString & MM & DD

        Dim logger As System.IO.StreamWriter = Nothing
        Dim retries As Short = 0

tryagain:

        Try
            logger = My.Computer.FileSystem.OpenTextFileWriter(path & "Logs\" & DSTamp & "_Log.txt", True, Encoding.ASCII)
            logger.WriteLine(linemess)
            logger.Close()

        Catch ex As Exception
            'for now, we don't care if there are issues writing to the log file, but we'll try to close it
            If Not IsNothing(logger) Then logger.Close()
            retries += 1
            Thread.Sleep(100)
            If retries < 10 Then GoTo tryagain


        End Try

        logger = Nothing
        DSTamp = Nothing
        MM = Nothing
        DD = Nothing

        'End Log Writer

    End Sub

End Class