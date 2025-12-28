Public Class LogItEventArgs
    Inherits EventArgs

    Public Property mess As String

End Class

Public Class AlertMailEventArgs
    Inherits EventArgs

    Public subject As String
    Public body As String
    Public group As String

End Class

Public Class FileItUTF8EventArgs
    Inherits EventArgs

    Public dirPath As String
    Public extension As String
    Public filePrefix As String
    Public content As String

End Class

Public Class RankAttemptCompleteEventArgs
    Inherits EventArgs

    Public cart As RankRequestsRankRequest
    Public enableRetry As Boolean
    Public attemptNo As Short
    Public xmlFileName As String

End Class

Public Class CartAttemptCompleteEventArgs
    Inherits EventArgs

    Public cart As Object
    Public enableRetry As Boolean
    Public attemptNo As Short
    Public xmlFileName As String

End Class

Public Class FundAttemptCompleteEventArgs
    Inherits EventArgs

    Public cart As FundRequestsFundRequest
    Public enableRetry As Boolean
    Public attemptNo As Short
    Public xmlFileName As String

End Class

Public Class ExLogItEventArgs
    Inherits EventArgs

    Public method As String
    Public exMessage As String
    Public RequestMsg As String
    Public ResponseMsg As String

End Class