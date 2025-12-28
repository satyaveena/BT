Namespace ExceptionLoggingDataSetTableAdapters
    Partial Class QueriesTableAdapter
        'commandIndex is a reference to which command in the commandcollection this applies to.  The SP being called
        '   needs to be located in the Dataset .Designer.vb to plug the correct index in to the calling code.
        '   They are zero based top down, so top SP is 0, next is 1, etc. Other the ReturnValue, they are
        '   generically named because they could vary from SP to SP.

        Public Function GetReturnValue(ByVal commandIndex As Integer) As Object
            'It is safe to assume that the integer @RETURN_VALUE is always going to be Parameter(0), but this also
            '   can be confirmed in the Designer.vb (if @RETURN_VALUE has been specified?  In any event, it seems
            '   as if it is always there in position 0
            Return Me.CommandCollection(commandIndex).Parameters(0).Value
        End Function

        Public Function GetParam1(ByVal commandIndex As Integer) As Object
            'This is for access to a second return parameter, typically a response/error message of some sort.
            '   This may vary based on the SP Parameters, but is always found in the Designer.vb
            '   When viewing the DataSet.xsd in the Designer, the Parameters are also visible by clicking on the SP and looking at the Paramaters 
            '   Collection in Properties.
            Return Me.CommandCollection(commandIndex).Parameters(1).Value
        End Function

        Public Function GetParam2(ByVal commandIndex As Integer) As Object
            'This is for access to a third return parameter.  Not as common, but it doesn't hurt to have it, unless to try to
            'access it when there isn't one for the particular SP in the Queries Table Adapter.
            '   This may vary based on the SP Parameters, but is always found in the Designer.vb
            '   When viewing the DataSet.xsd in the Designer, the Parameters are also visible by clicking on the SP and looking at the Paramaters 
            '   Collection in Properties.
            Return Me.CommandCollection(commandIndex).Parameters(2).Value
        End Function

        Public Sub SetCommandTimeout(ByVal commandIndex As Integer, ByVal timeoutValue As Integer)
            'OK, it looks like you can also get to some other read/write properties using this Partial Class
            '   In this case, we can dynamically set the timeout for whichever of the SPs is referenced by the commandIndex
            Me.CommandCollection(commandIndex).CommandTimeout = timeoutValue

        End Sub
    End Class

End Namespace