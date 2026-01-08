Imports System.Text.RegularExpressions
Imports GreyDogSoftware.Helpers

Public Class MutatorRandoms
    Inherits Mutator

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Randomizer"
        End Get
    End Property

    Public Overrides ReadOnly Property Description As String
        Get
            Return "Permite el uso de los tags numericos de numeros aleatorios."
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
        Do
            Dim CurrentRnd As String = GetRandom(InputText)
            If Not String.IsNullOrEmpty(CurrentRnd) Then
                Dim rnd As Integer = ResolveRandom(CurrentRnd)
                InputText = Strings.Replace(InputText, CurrentRnd, rnd.ToString, , 1)
            Else
                Exit Do
            End If
        Loop
        Return InputText
    End Function
    Private Function GetRandom(ByVal Stack As String) As String
        Dim VariableSearch As New Regex("\#rnd\{\d+,\d+\}", RegexOptions.IgnoreCase)
        If VariableSearch.Matches(Stack).Count > 0 Then
            Return VariableSearch.Matches(Stack)(0).ToString
        Else
            Return ""
        End If
    End Function
    Private Function ResolveRandom(ByVal Random As String) As Integer
        Dim Min As Integer = 0
        Dim Max As Integer = 0
        Dim MinStr As String = Random.Substring(Random.IndexOf("{") + 1, Random.IndexOf(",") - Random.IndexOf("{") - 1)
        Dim MaxStr As String = Random.Substring(Random.IndexOf(",") + 1, Random.IndexOf("}") - Random.IndexOf(",") - 1)
        Min = CInt(MinStr)
        Max = CInt(MaxStr)
        Return GetRandomNumber(Min, Max)
    End Function
End Class
