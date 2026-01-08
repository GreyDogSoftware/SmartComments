Imports System.Runtime.CompilerServices

Namespace Global
    Module module_Color
        Public Structure HSBColor
            Private prop_hue As Single
            Private prop_saturation As Single
            Private prop_luminosity As Single
            Private prop_Alpha As Byte

            Public Property Alpha As Byte
                Get
                    Return prop_Alpha
                End Get
                Set(ByVal Value As Byte)
                    prop_Alpha = Value
                End Set
            End Property
            Public Property Hue As Single
                Get
                    Return prop_hue
                End Get
                Set(ByVal Value As Single)
                    prop_hue = Value
                End Set
            End Property
            Public Property Saturation As Single
                Get
                    Return prop_saturation
                End Get
                Set(ByVal Value As Single)
                    prop_saturation = Value
                End Set
            End Property
            Public Property Brightness As Single
                Get
                    Return prop_luminosity
                End Get
                Set(ByVal Value As Single)
                    prop_luminosity = Value
                End Set
            End Property

            Public Overloads Shared Widening Operator CType(ByVal color As Drawing.Color) As HSBColor
                Dim hslColor As HSBColor = New HSBColor
                hslColor.Alpha = color.A
                hslColor.Hue = color.GetHue
                hslColor.Brightness = color.GetBrightness
                hslColor.Saturation = color.GetSaturation
                Return hslColor
            End Operator
            Public Overloads Shared Widening Operator CType(ByVal hslColor As HSBColor) As Drawing.Color
                If hslColor.Saturation = 0 Then
                    Return Drawing.Color.FromArgb(hslColor.Brightness, hslColor.Brightness, hslColor.Brightness)
                Else
                    Dim Red As Single
                    Dim Green As Single
                    Dim Blue As Single
                    Dim TempHue As Single
                    Dim Temp1 As Single = If(hslColor.Brightness < 0.5, hslColor.Brightness * (1.0 + hslColor.Saturation), hslColor.Brightness + hslColor.Saturation - hslColor.Brightness * hslColor.Saturation)
                    Dim Temp2 As Single = 2 * hslColor.Brightness - Temp1
                    TempHue = hslColor.Hue / 360
                    Red = HueToRGB(TempHue + (1 / 3), Temp1, Temp2)
                    Green = HueToRGB(TempHue, Temp1, Temp2)
                    Blue = HueToRGB(TempHue - (1 / 3), Temp1, Temp2)
                    Return Drawing.Color.FromArgb(255, Red * 255, Green * 255, Blue * 255)
                End If
            End Operator

            Private Shared Function HueToRGB(ByVal HueValue As Single, ByVal Temp1 As Single, ByVal Temp2 As Single) As Single
                If HueValue < 0 Then
                    HueValue = HueValue + 1
                ElseIf HueValue > 1 Then
                    HueValue = HueValue - 1
                End If

                If HueValue < (1 / 6) Then
                    Return Temp2 + (Temp1 - Temp2) * 6 * HueValue
                End If
                If HueValue < (1 / 2) Then
                    Return Temp1
                End If
                If HueValue < (2 / 3) Then
                    Return Temp2 + (Temp1 - Temp2) * (2 / 3 - HueValue) * 6
                End If

                Return Temp2
            End Function
            Private Shared Function IsHexadecimal(ByVal ColorToCheck As String) As Boolean
                Return System.Text.RegularExpressions.Regex.IsMatch(ColorToCheck, "\A\b[0-9a-fA-F]+\b\Z")
            End Function

            Public Shared Function FromARGBHex(ByVal ColorCode As String) As HSBColor
                If ColorCode.Length > 8 Then
                    ColorCode = ColorCode.Substring(0, 8)
                Else
                    ColorCode = ColorCode.PadLeft(8, "F")
                End If
                Dim AlphaString As String = ColorCode.Substring(0, 2)
                Dim RedString As String = ColorCode.Substring(2, 2)
                Dim GreenString As String = ColorCode.Substring(4, 2)
                Dim BlueString As String = ColorCode.Substring(6, 2)

                Dim NewColor As Drawing.Color = Drawing.Color.FromArgb(Convert.ToByte(AlphaString, 16), Convert.ToByte(RedString, 16), Convert.ToByte(GreenString, 16), Convert.ToByte(BlueString, 16))
                Dim HSBNewColor As New HSBColor
                HSBNewColor.Hue = NewColor.GetHue
                HSBNewColor.Saturation = NewColor.GetSaturation
                HSBNewColor.Brightness = NewColor.GetBrightness
                Return HSBNewColor
            End Function
            Public Shared Function FromAHSB(ByVal Alpha As Byte, ByVal Hue As Single, ByVal Saturation As Single, ByVal Brightness As Single) As HSBColor
                Dim HSBNewColor As New HSBColor
                HSBNewColor.Alpha = Alpha
                HSBNewColor.Hue = Hue
                HSBNewColor.Saturation = Saturation
                HSBNewColor.Brightness = Brightness
                Return HSBNewColor
            End Function
            Public Shared Function FromAHSB(ByVal Hue As Single, ByVal Saturation As Single, ByVal Brightness As Single) As HSBColor
                Return FromAHSB(255, Hue, Saturation, Brightness)
            End Function
        End Structure
        <Extension()>
        Public Function AdjustBrightness(ByRef Color As Drawing.Color, ByVal Brightness As Single) As Drawing.Color
            Brightness = Brightness / 100
            Dim NewBrightness As Single = Color.GetBrightness + Brightness
            Dim NewColor As HSBColor = HSBColor.FromAHSB(Color.GetHue, Color.GetSaturation, NewBrightness)
            Return NewColor
        End Function
        <Extension()>
        Public Function GetColor(ByVal ColorValue As UInteger) As Drawing.Color
            Dim HexValue As String = Hex(ColorValue)
            HexValue = HexValue.PadLeft(8, "0")
            'Dim Alpha As Integer = Convert.ToInt32(HexValue.Substring(0, 2), 16)
            Dim Red As Integer = Convert.ToInt32(HexValue.Substring(2, 2), 16)
            Dim Green As Integer = Convert.ToInt32(HexValue.Substring(4, 2), 16)
            Dim Blue As Integer = Convert.ToInt32(HexValue.Substring(6, 2), 16)
            Return Drawing.Color.FromArgb(255, Red, Green, Blue)
        End Function
        <Extension()>
        Public Function GetTextColor(ByVal ColorToCheck As Drawing.Color) As Drawing.Color
            Dim Bright As Integer = (Convert.ToInt32(ColorToCheck.R) + Convert.ToInt32(ColorToCheck.G) + Convert.ToInt32(ColorToCheck.B)) / 3
            Return If(Bright > 128, Drawing.Color.Black, Drawing.Color.White)
        End Function
        <Extension()>
        Public Function IsHexadecimal(ByVal ColorToCheck As String) As Boolean
            Return System.Text.RegularExpressions.Regex.IsMatch(ColorToCheck, "\A\b[0-9a-fA-F]+\b\Z")
        End Function
    End Module
End Namespace

