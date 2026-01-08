Imports System.Text.RegularExpressions

Public Class MutatorIterator
    Inherits Mutator

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Iterator"
        End Get
    End Property

    Public Overrides ReadOnly Property Description As String
        Get
            Return "Permite el uso de los tags de iteraciones."
        End Get
    End Property

    Public Overrides ReadOnly Property AllowsConfiguration As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property HasHelpDialog As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property ReplacementOrder As ExecutionOrder
        Get
            Return ExecutionOrder.PreFormating
        End Get
    End Property

    Public Overrides Sub OpenHelpDialog()

    End Sub

    Public Overrides Sub OpenConfigDialog()

    End Sub

    Public Overrides Function Mutate(InputText As String) As String
        ' Catching the iterations tags
        Dim IterationPattern As String = "\[[^[]+\]\d+"
        Dim IterationSearch As New Regex(IterationPattern, RegexOptions.IgnoreCase)
        Dim Iteration As MatchCollection = IterationSearch.Matches(InputText)
        For Each IterationTag As Match In Iteration
            ' Replacing each tag
            Dim IterationCountString As String = IterationTag.ToString
            Dim IterationReplacement As String = ""
            IterationCountString = Trim(IterationCountString.Substring(IterationCountString.LastIndexOf("]") + 1))
            Dim IterationContent As String = IterationTag.ToString.Substring(1, IterationTag.ToString.LastIndexOf("]") - 1)
            Dim IterationCount As Integer = Convert.ToInt32(IterationCountString)
            For ItC = 1 To IterationCount
                IterationReplacement = IterationReplacement & IterationContent
            Next
            InputText = InputText.Replace(IterationTag.ToString, Trim(IterationReplacement))
        Next
        Return InputText
    End Function
End Class
