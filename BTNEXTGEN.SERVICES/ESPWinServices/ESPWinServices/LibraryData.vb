Public Class LibraryData

    Sub New()

    End Sub

    Public libraries As List(Of Library)

End Class

Public Class Library

    Public espLibraryName As String
    Public espLibraryId As String
    Public enabled As Boolean

End Class
