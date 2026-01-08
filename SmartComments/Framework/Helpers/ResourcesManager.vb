Imports System.Text

Namespace Global.GreyDogSoftware.Helpers
    Public Module Resources
        Public Function GetEmbeddedResource(name As String) As String
            Return GetEmbeddedResource(System.Reflection.Assembly.GetExecutingAssembly, name)
        End Function

        Public Function GetEmbeddedResource(assembly As System.Reflection.Assembly, name As String) As String
            Dim buf As String = ""
            Using s As System.IO.Stream = assembly.GetManifestResourceStream(name)
                Using sr As New System.IO.StreamReader(s, Encoding.Default)
                    buf = sr.ReadToEnd
                    sr.Close()
                End Using
                s.Close()
            End Using
            Return buf
        End Function

        'Friend Shared Function GetStringFromResource(ByVal FileName As String) As String
        '    If Not String.IsNullOrEmpty(FileName) Then
        '        Try
        '            If My.Resources.ResourceManager.GetObject(FileName).GetType Is GetType(String) Then
        '                Return My.Resources.ResourceManager.GetObject(FileName)
        '            End If
        '        Catch ex As Exception
        '            Throw New Exception("Error loading the required resource")
        '        End Try
        '    End If
        '    Throw New Exception("Empty resource name")
        'End Function
    End Module
End Namespace

