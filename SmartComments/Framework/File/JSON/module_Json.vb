Imports System.IO
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json.Schema

#Disable Warning BC40000

Module module_Json
    Friend Function LoadFileFromDisk(ByVal FilePath As String) As String
        Dim Result As String = ""
        If File.Exists(FilePath) Then
            Result = File.ReadAllText(FilePath)
        Else
            Throw New Exception("FileNotFound")
        End If
        Return Result
    End Function
    Friend Function LoadFileFromResource(ByVal FileName As String) As String
        If Not String.IsNullOrEmpty(FileName) Then
            Try
                If My.Resources.ResourceManager.GetObject(FileName).GetType Is GetType(String) Then
                    Return My.Resources.ResourceManager.GetObject(FileName)
                End If
            Catch ex As Exception
                Throw New Exception("Error loading the required resource")
            End Try
        End If
        Throw New Exception("Empty resource name")
    End Function

    Friend Function LoadJsonString(ByVal JsonString As String) As JObject
        Dim Json As JObject
        Try
            Json = JObject.Parse(JsonString)
        Catch ex As Exception
            Throw New Exception("Invalid json string")
        End Try
        Return Json
    End Function

    Friend Function LoadJsonFile(ByVal JsonFile As String) As JObject
        Dim Json As JObject
        Dim FileData As String

        Try
            FileData = LoadFileFromDisk(JsonFile)
        Catch ex As Exception
            Throw New Exception("File Not found.")
        End Try

        Try
            Json = LoadJsonString(FileData)
        Catch ex As Exception
            Throw New Exception("Bad file format.")
        End Try
        Return Json
    End Function

    Friend Function ValidateJson(ByVal Json As JObject, ByVal Schema As String) As Boolean
        Dim SchemaValidation As JsonSchema
        Try
            SchemaValidation = JsonSchema.Parse(Schema)
        Catch ex As Exception
            Return False
        End Try
        If Json.IsValid(SchemaValidation) Then
            Return True
        Else
            Return False
        End If
    End Function
    Friend Function ValidateJson(ByVal Json As String, ByVal Schema As String) As Boolean
        Dim JsonValidation As JObject
        Try
            JsonValidation = JObject.Parse(Json)
        Catch ex As Exception
            Return False
        End Try

        Dim SchemaValidation As JsonSchema
        Try
            SchemaValidation = JsonSchema.Parse(Schema)
        Catch ex As Exception
            Return False
        End Try

        If JsonValidation.IsValid(SchemaValidation) Then
            Return True
        Else
            Return False
        End If
    End Function
    Friend Function ValidateJson(ByVal Json As JObject, ByVal Schema As JsonSchema) As Boolean
        If Json.IsValid(Schema) Then
            Return True
        Else
            Return False
        End If
    End Function
    Friend Function ValidateJson(ByVal Json As String, ByVal Schema As JsonSchema) As Boolean
        Dim JsonValidation As JObject
        Try
            JsonValidation = JObject.Parse(Json)
        Catch ex As Exception
            Return False
        End Try
        If JsonValidation.IsValid(Schema) Then
            Return True
        Else
            Return False
        End If
    End Function
End Module