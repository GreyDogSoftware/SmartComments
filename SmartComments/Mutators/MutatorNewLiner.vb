Public Class MutatorNewLiner
    Inherits Mutator

    Public Overrides ReadOnly Property Name As String
        Get
            Return "New liner"
        End Get
    End Property

    Public Overrides ReadOnly Property Description As String
        Get
            Return "Resuelve los casteos de nuevas lineas."
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
        Return InputText.Replace("~", vbCrLf)
    End Function
End Class
