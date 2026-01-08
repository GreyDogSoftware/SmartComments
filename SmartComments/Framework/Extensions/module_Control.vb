Imports System.Runtime.CompilerServices

Namespace Global
    Module module_Control
        <Extension>
        Public Function GetTopParent(currentControl As Control) As Control
            Dim parent As Control = Nothing
            Do While currentControl IsNot Nothing
                parent = If(currentControl.Parent, parent)
                currentControl = currentControl.Parent
            Loop
            Return parent
        End Function
    End Module
End Namespace

