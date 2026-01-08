Public MustInherit Class Mutator
    Public MustOverride ReadOnly Property Name As String
    Public MustOverride ReadOnly Property Description As String
    Public MustOverride ReadOnly Property AllowsConfiguration As Boolean
    Public MustOverride ReadOnly Property HasHelpDialog As Boolean
    Public Property Enabled As Boolean = True
    Public Property ShowDebugMessages As Boolean = False
    Public MustOverride ReadOnly Property ReplacementOrder As ExecutionOrder

    Public Enum ExecutionOrder
        PreFormating
        PostFormating
    End Enum

    Public Sub New()

    End Sub

    Public MustOverride Function Mutate(ByVal InputText As String) As String
    Public MustOverride Sub OpenConfigDialog()
    Public MustOverride Sub OpenHelpDialog()
End Class