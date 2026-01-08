Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq
Imports GreyDogSoftware.Helpers

Public Class TextCommentGenerator
    Private JsonFile As String
    Private Deserialized As Object
    Private LastMessage As String
    Private Templates As List(Of String)
    Private Content As Dictionary(Of String, List(Of String))
    Private Version As String
    Private FilePacks() As IO.FileInfo

    Private MutatorsList As List(Of Mutator)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MutatorsList = New List(Of Mutator)
        MutatorsList.Add(New MutatorRandoms)
        MutatorsList.Add(New MutatorNewLiner)
        MutatorsList.Add(New MutatorIterator)
        txt_Result.Text = ""
        LoadAvalableFiles()
    End Sub



    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CM_Files.SelectedIndexChanged
        If CM_Files.SelectedIndex >= 0 Then
            LoadFile(FilePacks(CM_Files.SelectedIndex).FullName)
            CM_Templates.Items.Clear()
            If Templates.Count > 0 Then
                For Each TemplateEntry In Templates
                    CM_Templates.Items.Add(TemplateEntry)
                Next
                CM_Templates.Enabled = True
                CM_Templates.SelectedIndex = 0
            Else
                CM_Templates.Enabled = False
            End If
        End If
    End Sub

    Private Sub LoadAvalableFiles()
        ' make a reference to a directory
        If IO.Directory.Exists(Environment.CurrentDirectory & "\packs") Then
            Dim di As New IO.DirectoryInfo(Environment.CurrentDirectory & "\packs")
            FilePacks = di.GetFiles("*.json")
            CM_Files.Items.Clear()
            'list the names of all files in the specified directory
            If FilePacks.Count > 0 Then
                For Each dra As IO.FileInfo In FilePacks
                    CM_Files.Items.Add(dra.Name)
                Next
                CM_Files.Enabled = True
                CM_Files.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub LoadFile(ByVal FileName As String)
        Dim JsonFile As JObject = Nothing
        Dim FileLoaded As Boolean = False
        Content = New Dictionary(Of String, List(Of String))
        Templates = New List(Of String)
        ' Loading the file
        Try
            JsonFile = LoadJsonFile(FileName)
            FileLoaded = True
        Catch ex As Exception
            MsgBox("Error loading json file")
        End Try

        Dim ValidationSchema As String = ""
        Dim SchemaLoaded As Boolean = False
        If FileLoaded Then
            ' File is loaded. Validating the file.
            Try
                'ValidationSchema = LoadFileFromResource("ValidationSchema3")
                ValidationSchema = GetEmbeddedResource("SmartComments.schema_textcomments_v3.json")
                SchemaLoaded = True
            Catch ex As Exception
                MsgBox("Can't load the validation schema")
            End Try
            If SchemaLoaded Then
                ' Schema is loaded. Validating the file.
                If ValidateJson(JsonFile, ValidationSchema) Then
                    txt_FileVersion.Text = JsonFile("version").ToObject(Of String)()
                    txt_Author.Text = JsonFile("author").ToObject(Of String)()
                    For Each Template As JToken In JsonFile("templates")
                        ' Loading templates
                        Dim TemplateData As String = Template.ToObject(Of String)()
                        If Not Templates.Contains(TemplateData) Then
                            Templates.Add(TemplateData)
                        End If
                    Next
                    For Each ContentData As JProperty In JsonFile("content")
                        Dim ContentName As String = ContentData.Name
                        Dim ContentList As New List(Of String)

                        For Each ContentEntry In JsonFile("content")(ContentName)
                            If Not ContentList.Contains(ContentEntry.ToString) Then
                                ContentList.Add(ContentEntry.ToString)
                            End If
                            Dim ax = 0
                        Next
                        If Not Content.ContainsKey(ContentName) Then
                            Content.Add(ContentName.ToLower, ContentList)
                        End If
                        Dim a = 0
                    Next
                Else
                    Debug.WriteLine("Invalid pack")

                End If
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CM_Templates.SelectedIndex > -1 Then
            MakeTheComment()
        Else
            MsgBox("No template selected")
        End If
    End Sub

    Private Function GetRandomWord(ByVal WordStashName As String) As String
        If Content.ContainsKey(WordStashName.ToLower) Then
            'Dim WordsCount = Content(WordStashName.ToLower).Count
            'Randomize()
            'Dim WordIndex As Integer = Int((Content(WordStashName.ToLower).Count * Rnd()))
            Return Content(WordStashName.ToLower).Item(GreyDogSoftware.Helpers.GetRandomNumber(0, Content(WordStashName.ToLower).Count))
        Else
            Return ""
        End If
    End Function



    Private CommentVariables As New Dictionary(Of String, String)
    Private Function GetCasting(ByVal Stack As String) As String
        Dim VariableSearch As New Regex("\$[a-zA-Z0-9]+", RegexOptions.IgnoreCase)
        If VariableSearch.Matches(Stack).Count > 0 Then
            Return VariableSearch.Matches(Stack)(0).ToString
        Else
            Return ""
        End If
    End Function
    Private Function ResolveCasting(ByVal VarName As String) As String
        VarName = VarName.Substring(1)
        If CommentVariables.ContainsKey(VarName) Then
            Return CommentVariables(VarName)
        End If
        Return ""
    End Function
    Private Sub ResolveCastings(ByRef Stack As String)
        ' Variable casting
        ' New Regex = \$[a-zA-Z0-9]+
        ' Old Regex = \#[^\s]+\#
        Do
            Dim VariableCasting As String = GetCasting(Stack)
            If Not String.IsNullOrEmpty(VariableCasting) Then
                Stack = Stack.Replace(VariableCasting, ResolveCasting(VariableCasting))
            Else
                Exit Do
            End If
        Loop
    End Sub


    Private Function GetTag(ByVal SearchText As String) As String
        Dim TagSearch As New Regex("\%[^\s]+\%", RegexOptions.IgnoreCase)
        If TagSearch.Matches(SearchText).Count > 0 Then
            Return TagSearch.Matches(SearchText)(0).ToString
        Else
            Return ""
        End If
    End Function
    Private Function ResolveTag(ByVal Tag As String) As String
        If Not String.IsNullOrEmpty(Tag) Then
            'Debug.WriteLine(CurrentTag)
            Dim Replacer As String = Tag.Substring(1, Tag.Length - 2)
            Return GetRandomWord(Replacer)
        End If
        Return ""
    End Function


    Private Function GetVariable(ByVal Stack As String) As KeyValuePair(Of String, String)
        Dim VariableSearch As New Regex("\$[a-zA-Z0-9]+\{[a-zA-Z0-9% ~,._;:#$\r\n]+\}", RegexOptions.IgnoreCase)
        If VariableSearch.Matches(Stack).Count > 0 Then
            Dim ResultName As String = VariableSearch.Matches(Stack)(0).ToString.Substring(1)
            ResultName = ResultName.Substring(0, ResultName.IndexOf("{"))
            Dim ResultValue As String = VariableSearch.Matches(Stack)(0).ToString.Substring(VariableSearch.Matches(Stack)(0).ToString.IndexOf("{") + 1)
            ResultValue = ResultValue.Substring(0, ResultValue.IndexOf("}"))
            Return New KeyValuePair(Of String, String)(ResultName, ResultValue)
        Else
            Return New KeyValuePair(Of String, String)("", "")
        End If
    End Function
    Private Function ResolveVariable(ByVal VariableValue As String) As String
        Do
            Dim CurrTag As String = GetTag(VariableValue)
            If String.IsNullOrEmpty(CurrTag) Then
                Exit Do
            End If
            VariableValue = Strings.Replace(VariableValue, CurrTag, ResolveTag(CurrTag),, 1)
        Loop
        Return VariableValue
    End Function
    Private Sub ResolveVariables(ByRef Stack As String)
        ' Variable resolving
        ' New Regex = \$[a-zA-Z0-9]+\{[a-zA-Z0-9% ~,._;:]+\}
        ' Old Regex = [\w]+\{[^{]+\}
        ' Resolving variables
        Dim CurVar As KeyValuePair(Of String, String)
        Do
            CurVar = GetVariable(Stack)
            If String.IsNullOrEmpty(CurVar.Key) Then
                Exit Do
            End If
            Dim ResolvedVariable As String = ResolveVariable(CurVar.Value)
            CommentVariables.Add(CurVar.Key, ResolvedVariable)
            Stack = Stack.Replace("$" & CurVar.Key & "{" & CurVar.Value & "}", "")
        Loop
    End Sub



    Private Sub ResolveTags(ByRef SearchText As String)
        Do
            Dim CurrentTag As String = GetTag(SearchText)
            If Not String.IsNullOrEmpty(CurrentTag) Then
                SearchText = Strings.Replace(SearchText, CurrentTag, ResolveTag(CurrentTag), , 1)
            Else
                Exit Do
            End If
        Loop
    End Sub

    Private Sub MakeTheComment()
        ' Formating reference

        ' []x      = Iteration tag. Ex [asdf]3= asdfasdfasdf
        ' ~        = New line
        ' $xxx{}   = Variable declaration
        ' $xxx     = Variable casting
        ' %xxx%    = Random entry pick
        ' #rnd{x,y} = random number from x to y

        CommentVariables = New Dictionary(Of String, String)
        Dim BaseComment As String = CM_Templates.SelectedItem
        ' Formating code
        ' Catching the new line chars

        Debug.WriteLine("Pre formating")
        For Each Mttor As Mutator In MutatorsList
            If Mttor.ReplacementOrder = Mutator.ExecutionOrder.PreFormating Then
                If Mttor.Enabled = True Then
                    Debug.WriteLine("Appliying: " & Mttor.Name)
                    BaseComment = Mttor.Mutate(BaseComment)
                    'Debug.WriteLine("New lines: " & BaseComment)
                End If
            End If
        Next

        ResolveVariables(BaseComment)
        ResolveCastings(BaseComment)
        ResolveTags(BaseComment)

        Debug.WriteLine("Post formating")
        For Each Mttor As Mutator In MutatorsList
            If Mttor.ReplacementOrder = Mutator.ExecutionOrder.PostFormating Then
                If Mttor.Enabled = True Then
                    Debug.WriteLine("Appliying: " & Mttor.Name)
                    BaseComment = Mttor.Mutate(BaseComment)
                    'Debug.WriteLine("New lines: " & BaseComment)
                End If
            End If
        Next

        BaseComment = Trim(BaseComment)

        ' Counting lines
        If BaseComment.Split(vbCrLf).Count > 8 Then
            txt_Result.ScrollBars = ScrollBars.Vertical
        Else
            txt_Result.ScrollBars = ScrollBars.None
        End If

        txt_Result.Text = BaseComment
    End Sub

    Private Sub Form1_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        Using AboutBox As New HelpBox
            AboutBox.ShowDialog()
        End Using
    End Sub
End Class
