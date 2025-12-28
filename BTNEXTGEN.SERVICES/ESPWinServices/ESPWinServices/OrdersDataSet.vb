Namespace OrdersDataSetTableAdapters
    Partial Class QueriesTableAdapter
        'IMPORTANT NOTE: commandIndex is a reference to which command in the commandcollection this applies to.  The SP being called
        '   needs to be located in the [Dataset].Designer.vb - InitCommandCollection() to plug the correct index in to the calling code.
        '   Whenever a new SP is added to the QueriesTableAdapter, these must be checked.  They appear to incremeent
        '   in chron order, and are NOT in the order that show in the dataset gui (xsd) designer, which is alpha
        Public Function GetReturnValue(ByVal commandIndex As Integer) As Object
            'It is safe to assume that the integer @RETURN_VALUE is always going to be Parameter(0), but this also
            '   can be confirmed in the Designer.vb
            Return Me.CommandCollection(commandIndex).Parameters(0).Value
        End Function

        Public Function GetParam1(ByVal commandIndex As Integer) As Object
            'in this instance, there is another input parameter followed by an input/output parameter "@ErrorMessage" hence the Parameter(2).
            '   This may vary based on the SP Parameters, but is always found in the Designer.vb
            '   When viewing the DataSet.xsd in the Designer, the Parameters are also visible by clicking on the SP and looking at the Paramaters 
            '   Collection in Properties.
            Return Me.CommandCollection(commandIndex).Parameters(1).Value
        End Function

        Public Function GetParam2(ByVal commandIndex As Integer) As Object
            'in this instance, there is another input parameter followed by an input/output parameter "@ErrorMessage" hence the Parameter(2).
            '   This may vary based on the SP Parameters, but is always found in the Designer.vb
            '   When viewing the DataSet.xsd in the Designer, the Parameters are also visible by clicking on the SP and looking at the Paramaters 
            '   Collection in Properties.
            Return Me.CommandCollection(commandIndex).Parameters(2).Value
        End Function

        Public Function GetParam3(ByVal commandIndex As Integer) As Object
            'in this instance, there is another input parameter followed by an input/output parameter "@ErrorMessage" hence the Parameter(2).
            '   This may vary based on the SP Parameters, but is always found in the Designer.vb
            '   When viewing the DataSet.xsd in the Designer, the Parameters are also visible by clicking on the SP and looking at the Paramaters 
            '   Collection in Properties.
            Return Me.CommandCollection(commandIndex).Parameters(3).Value
        End Function

        Public Function GetParam4(ByVal commandIndex As Integer) As Object
            'in this instance, there is another input parameter followed by an input/output parameter "@ErrorMessage" hence the Parameter(2).
            '   This may vary based on the SP Parameters, but is always found in the Designer.vb
            '   When viewing the DataSet.xsd in the Designer, the Parameters are also visible by clicking on the SP and looking at the Paramaters 
            '   Collection in Properties.
            Return Me.CommandCollection(commandIndex).Parameters(4).Value
        End Function


    End Class

    Partial Class procESPGetRankRequestsTableAdapter

        Public Function GetReturnValue(ByVal commandIndex As Integer) As Object
            Return Me.CommandCollection(commandIndex).Parameters(0).Value
        End Function

        Public Function GetParam1(ByVal commandIndex As Integer) As Object
            Return Me.CommandCollection(commandIndex).Parameters(1).Value
        End Function

    End Class

    Partial Class procESPGetDistributionRequestsTableAdapter

        Public Function GetReturnValue(ByVal commandIndex As Integer) As Object
            Return Me.CommandCollection(commandIndex).Parameters(0).Value
        End Function

        Public Function GetParam1(ByVal commandIndex As Integer) As Object
            Return Me.CommandCollection(commandIndex).Parameters(1).Value
        End Function

        Public Sub SetCommandTimeout(ByVal commandIndex As Integer, ByVal timeoutValue As Integer)
            Me.CommandCollection(commandIndex).CommandTimeout = timeoutValue
        End Sub

    End Class
    
    Partial Class procESPGetFundRequestsTableAdapter

        Public Function GetReturnValue(ByVal commandIndex As Integer) As Object
            Return Me.CommandCollection(commandIndex).Parameters(0).Value
        End Function

        Public Function GetParam1(ByVal commandIndex As Integer) As Object
            Return Me.CommandCollection(commandIndex).Parameters(1).Value
        End Function

    End Class

End Namespace



Public Class OrdersDataSet

End Class
