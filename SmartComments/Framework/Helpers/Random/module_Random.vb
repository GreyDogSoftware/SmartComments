Namespace Global.GreyDogSoftware.Helpers
    Module Random
        Public Enum RandomMode
            OnlyNumbers
            OnlyLetters
            OnlyUpperCaseLetters
            OnlyLowerCaseLetters
            OnlySimbols
            NumbersAndLetters
            NumbersAndLettersUpperCase
            NumbersAndLettersLowerCase
            Anything
        End Enum
        Public Function GetRandomString(ByVal WordLenght As Integer, ByVal Mode As RandomMode) As String
            Dim OnlyNumbers As String = "0123456789"
            Dim OnlyUpperCaseLetters As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            Dim OnlyLowerCaseLetters As String = "abcdefghijklmnopqrstuvwxyz"
            Dim OnlySimbols As String = "!""·$%&/()=?¿¡'|@#~€¬[]}{,.-;:_`+´¨\/* "
            Dim CurrentCharset As String = ""
            Select Case Mode
                Case RandomMode.OnlyNumbers
                    CurrentCharset = OnlyNumbers
                Case RandomMode.OnlyLetters
                    CurrentCharset = OnlyUpperCaseLetters & OnlyLowerCaseLetters
                Case RandomMode.OnlyUpperCaseLetters
                    CurrentCharset = OnlyUpperCaseLetters
                Case RandomMode.OnlyLowerCaseLetters
                    CurrentCharset = OnlyLowerCaseLetters
                Case RandomMode.OnlySimbols
                    CurrentCharset = OnlySimbols
                Case RandomMode.NumbersAndLetters
                    CurrentCharset = OnlyUpperCaseLetters & OnlyLowerCaseLetters & OnlyNumbers
                Case RandomMode.NumbersAndLettersUpperCase
                    CurrentCharset = OnlyUpperCaseLetters & OnlyNumbers
                Case RandomMode.NumbersAndLettersLowerCase
                    CurrentCharset = OnlyLowerCaseLetters & OnlyNumbers
                Case RandomMode.Anything
                    CurrentCharset = OnlyUpperCaseLetters & OnlyLowerCaseLetters & OnlyNumbers & OnlySimbols
            End Select
            Dim Result As String = ""
            Dim LetterIndex As Integer
            For i As Integer = 1 To WordLenght
                LetterIndex = GetRandomNumber(0, CurrentCharset.Length - 1)
                Result = Result & CurrentCharset(LetterIndex)
            Next
            Return Result
        End Function
        Public Function GetRandomNumber(ByVal Min As Integer, ByVal Max As Integer) As Integer
            ' by making Generator static, we preserve the same instance '
            ' (i.e., do not create new instances with the same seed over and over) '
            ' between calls '
            Static Generator As System.Random = New System.Random()
            Return Generator.Next(Min, Max)
        End Function
    End Module

End Namespace
