Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Namespace Global.GreyDogSoftware.UI
    Public Class WindowSkin
        Inherits Form
#Region "WinAPI Imports"
        Private Const WM_SYSCOMMAND As Integer = &H112
        Private Const TPM_LEFTALIGN As UInt32 = &H0
        Private Const TPM_CENTERALIGN As UInt32 = &H4
        Private Const TPM_RIGHTALIGN As UInt32 = &H8
        Private Const TPM_TOPALIGN As UInt32 = &H0
        Private Const TPM_VCENTERALIGN As UInt32 = &H10
        Private Const TPM_BOTTOMALIGN As UInt32 = &H20

        Private Const TPM_HORPOSANIMATION As UInt32 = &H400

        Private Const TPM_RETURNCMD = &H100
        Private Const MF_ENABLED As UInt32 = &H0
        Private Const MF_GRAYED As UInt32 = &H1

        Private Const SC_SIZE As UInt32 = &HF000
        Private Const SC_MOVE As UInt32 = &HF010
        Private Const SC_MINIMIZE As UInt32 = &HF020
        Private Const SC_MAXIMIZE As UInt32 = &HF030
        Private Const SC_CLOSE As Integer = &HF060
        Private Const SC_RESTORE As UInt32 = &HF120

        <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
        Private Shared Function GetSystemMenu(ByVal hWnd As IntPtr, ByVal bRevert As Boolean) As IntPtr
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
        Private Shared Function TrackPopupMenu(ByVal hMenu As IntPtr, ByVal wFlags As Integer, ByVal x As Int32, ByVal y As Int32, ByVal nReserved As Int32, ByVal hWnd As IntPtr, ByVal ignored As IntPtr) As Int32
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
        Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
        Private Shared Function EnableMenuItem(ByVal MenuHandler As IntPtr, ByVal ItemID As UInt32, ByVal Enabled As UInt32) As Boolean
        End Function
#End Region

#Region "Global variables"
        Private Custom_prop_FormBorderStyle As FormBorderStyle = FormBorderStyle.FixedSingle
        Private prop_ButtonAppareance As New ButtonAppareanceConfig(True, Color.FromArgb(64, 255, 255, 255), Color.FromArgb(255, 225, 0, 31), Color.FromArgb(255, 196, 0, 31), 0)
        Private prop_TitleBarConfig As New TitleBarAppareanceConfig(Color.Empty, Color.FromArgb(255, 225, 0, 31), Color.FromArgb(255, 196, 0, 31), 0, Color.FromArgb(31, 31, 31), 48, 0, 25, ButtonLayoutMode.Right)
        Private prop_BorderColor As Color = Color.Black
        Private prop_BorderSize As Integer = 1
        Private prop_WindowMoving As Boolean = True
        Private ResizeEnabled As Boolean = True
        Private WindowsCollapsed As Boolean = False
        Private MousePoint As Point
        Private SystemMenuHandler As IntPtr
        Private OriginalWindowSize As Size = Size
        Private OriginalMinimunSize As Size = MyBase.MinimumSize
        Private prop_TitleBarUserButtons As New Dictionary(Of String, Button)
        Private CurrentScreen As Screen
        Private TaskbarSize As Integer = 0
        Private TaskbarPosition As TaskBarLocation = TaskBarLocation.NoBar
        Private LastWindowLocation As Point
        Private LastWindowSize As Size
        Private LastCollapseWindowSize As Size

        Private WithEvents Frm_Btn_Help As Button
        Private WithEvents Frm_Btn_Collapse As Button
        Private WithEvents Frm_Btn_Minimize As Button
        Private WithEvents Frm_Btn_Maximize As Button
        Private WithEvents Frm_Btn_Close As Button
        Private WithEvents Frm_Txt_Title As Label
        Private WithEvents Frm_TitleBar As Panel
#End Region

#Region "Properties"
#Region "Custom"
        Public Property BorderColor As Color
            Get
                Return prop_BorderColor
            End Get
            Set(value As Color)
                prop_BorderColor = value
                Refresh()
            End Set
        End Property
        Public Property BorderSize As Integer
            Get
                Return prop_BorderSize
            End Get
            Set(value As Integer)
                If value < 0 Then
                    prop_BorderSize = 0
                    Throw New Exception("The border size must be zero or greater")
                Else
                    prop_BorderSize = value
                    Refresh()
                End If

            End Set
        End Property
        Public Property WindowMoving As Boolean
            Get
                Return prop_WindowMoving
            End Get
            Set(value As Boolean)
                prop_WindowMoving = value
            End Set
        End Property
        <Description("Gets or sets the access allowed by user role.")>
        <TypeConverter(GetType(ExpandableObjectConverter))>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
        Public Property ButtonAppareance As ButtonAppareanceConfig
            Get
                Return prop_ButtonAppareance
            End Get
            Set(value As ButtonAppareanceConfig)
                prop_ButtonAppareance = value
            End Set
        End Property
        <Description("Sets the title bar appareance.")>
        <TypeConverter(GetType(ExpandableObjectConverter))>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
        Public Property TitleBarAppareance As TitleBarAppareanceConfig
            Get
                Return prop_TitleBarConfig
            End Get
            Set(value As TitleBarAppareanceConfig)
                prop_TitleBarConfig = value
            End Set
        End Property
#End Region
#Region "Overrides"
        Public Shadows Property MaximizeBox As Boolean
            Get
                Return MyBase.MaximizeBox
            End Get
            Set(ByVal Value As Boolean)
                MyBase.MaximizeBox = Value
                ToggleTitleBarButtons()
            End Set
        End Property
        Public Shadows Property MinimizeBox As Boolean
            Get
                Return MyBase.MinimizeBox
            End Get
            Set(ByVal Value As Boolean)
                MyBase.MinimizeBox = Value
                ToggleTitleBarButtons()
            End Set
        End Property
        Public Shadows Property FormBorderStyle As FormBorderStyle
            Get
                Return Custom_prop_FormBorderStyle
            End Get
            Set(ByVal Value As FormBorderStyle)
                Custom_prop_FormBorderStyle = Value
                ToggleTitleBarButtons()
            End Set
        End Property
        Public Shadows Property Text As String
            Get
                Return MyBase.Text
            End Get
            Set(value As String)
                MyBase.Text = value
                Frm_Txt_Title.Text = value
            End Set
        End Property
        Public Shadows Property StartPosition As FormStartPosition
            Get
                Return MyBase.StartPosition
            End Get
            Set(value As FormStartPosition)
                MyBase.StartPosition = value
            End Set
        End Property
        Public Shadows Property WindowState As FormWindowState
            Get
                Return MyBase.WindowState
            End Get
            Set(value As FormWindowState)
                MyBase.WindowState = value
                If MyBase.WindowState = FormWindowState.Maximized Then
                    Frm_Btn_Maximize.Text = "🗗"
                    Frm_Btn_Maximize.AccessibleName = "Restaurar"
                Else
                    Frm_Btn_Maximize.Text = "🗖"
                    Frm_Btn_Maximize.AccessibleName = "Maximizar"
                End If
            End Set
        End Property
        Public Shadows Property Size As Size
            Get
                Return MyBase.Size
            End Get
            Set(value As Size)
                MyBase.Size = value
                OriginalWindowSize = Size
            End Set
        End Property
        'Public Overrides Property BackgroundImage As Image
        '    Get
        '        Return MyBase.BackgroundImage
        '    End Get
        '    Set(value As Image)
        '        If value IsNot Nothing Then
        '            Dim m_bmp As New Bitmap(Width, Height, Imaging.PixelFormat.Format24bppRgb)
        '            Using g = Graphics.FromImage(m_bmp)
        '                g.DrawImage(value, 0, 0, m_bmp.Width, m_bmp.Height)
        '            End Using
        '            MyBase.BackgroundImage = m_bmp
        '        Else
        '            MyBase.BackgroundImage = Nothing
        '        End If
        '    End Set
        'End Property
#End Region
#End Region

#Region "Enums"
        Public Enum ButtonLayoutMode
            Left
            Right
            AllLeft
            AllRight
        End Enum
        Private Enum TaskBarLocation
            NoBar
            Top
            Bottom
            Left
            Right
        End Enum
#End Region

#Region "Structs"
        Private Structure ShakeInfo
            Public ShakeTimeInMilliSeconds As Double
            Public MaxPixelDrift As Integer
            Public InitialX As Integer
            Public InitialY As Integer
        End Structure
#End Region

#Region "Classes"
        <TypeConverter(GetType(ExpandableObjectConverter))>
        Public Class ButtonAppareanceConfig
            Private prop_AutoSkin As Boolean
            Private prop_ColorDefault As Color
            Private prop_ColorMouseOver As Color
            Private prop_ColorMouseDown As Color
            Private prop_Border As Integer
            Private prop_TextColorHover As Color
            Private prop_TextColorDefault As Color
            <DefaultValue(True)>
            Public Property AutoSkin As Boolean
                Get
                    Return prop_AutoSkin
                End Get
                Set(value As Boolean)
                    prop_AutoSkin = value
                End Set
            End Property
            <DefaultValue(GetType(Color), "&h40FFFFFF")>
            Public Property ColorDefault As Color
                Get
                    Return prop_ColorDefault
                End Get
                Set(value As Color)
                    prop_ColorDefault = value
                End Set
            End Property
            <DefaultValue(GetType(Color), "&hFFE1001F")>
            Public Property ColorMouseOver As Color
                Get
                    Return prop_ColorMouseOver
                End Get
                Set(value As Color)
                    prop_ColorMouseOver = value
                End Set
            End Property
            <DefaultValue(GetType(Color), "&hFFC4001F")>
            Public Property ColorMouseDown As Color
                Get
                    Return prop_ColorMouseDown
                End Get
                Set(value As Color)
                    prop_ColorMouseDown = value
                End Set
            End Property
            <DefaultValue(GetType(Color), "&hFFFFFFFF")>
            Public Property TextColorHover As Color
                Get
                    Return prop_TextColorHover
                End Get
                Set(value As Color)
                    prop_TextColorHover = value
                End Set
            End Property
            <DefaultValue(GetType(Color), "&hFFFFFFFF")>
            Public Property TextColorDefault As Color
                Get
                    Return prop_TextColorDefault
                End Get
                Set(value As Color)
                    prop_TextColorDefault = value
                End Set
            End Property
            <DefaultValue(GetType(Integer), "0")>
            Public Property Border As Integer
                Get
                    Return prop_Border
                End Get
                Set(value As Integer)
                    prop_Border = value
                End Set
            End Property
            Public Sub New()
                Debug.WriteLine("ctor_default")
            End Sub
            Public Sub New(SKinEnabled As Boolean, ColorDefault As Color, ColorOver As Color, ColorDown As Color, BorderSize As Integer)
                prop_AutoSkin = SKinEnabled
                prop_ColorDefault = ColorDefault
                prop_ColorMouseOver = ColorOver
                prop_ColorMouseDown = ColorDown
                prop_Border = BorderSize
                prop_TextColorHover = Color.White
                prop_TextColorDefault = Color.White
            End Sub
        End Class
        <TypeConverter(GetType(ExpandableObjectConverter))>
        Public Class TitleBarAppareanceConfig
            Private prop_ButtonDefaultColor As Color ' Variable for ButtonDefaultColor property
            Private prop_ButtonMouseOverColor As Color ' Variable for ButtonMouseOverColor property
            Private prop_ButtonMouseDownColor As Color ' Variable for ButtonMouseDownColor property
            Private prop_ButtonBorderSize As Integer ' Variable for ButtonBorderSize property
            Private prop_TitleBarBorderSize As Integer ' Variable for TitleBarBorderSize property
            Private prop_TitleBarHeight As Integer ' Variable for TitleBarBorderSize property
            Private prop_TitleBarBackgroundColor As Color ' Variable for TitleBarBackgroundColor property
            Private prop_TitleBarBackgroundColorOpacity As Byte ' Variable for TitleBarBackgroundColorTransparency property
            Private prop_TitleBarButtonLayout As ButtonLayoutMode
            Private prop_CollapseButtonEnabled As Boolean
            Private prop_HelpButtonEnabled As Boolean


            Public Event ConfigUpdated(ByVal Data As TitleBarAppareanceConfig)
            Public Event ButtonLayoutUpdate(ByVal Layout As ButtonLayoutMode)
            Public Event ButtonColorUpdate(ByVal DefaultColor As Color, ByVal OverColor As Color, ByVal DownColor As Color)
            Public Event ButtonBorderUpdate(ByVal Border As Integer)
            Public Event TitleBarColorUpdate(ByVal TitleBarColor As Color)
            Public Event TitleBarSizeUpdate(ByVal Height As Integer, ByVal Border As Integer)
            Public Event CollapseButtonToggle(ByVal Value As Boolean)
            Public Event HelpButtonToggle(ByVal Value As Boolean)

            <DefaultValue(GetType(ButtonLayoutMode), "Right")>
            Public Property ButtonLayout As ButtonLayoutMode
                Get
                    Return prop_TitleBarButtonLayout
                End Get
                Set(value As ButtonLayoutMode)
                    prop_TitleBarButtonLayout = value
                    RaiseEvent ButtonLayoutUpdate(prop_TitleBarButtonLayout)
                End Set
            End Property

            <DefaultValue(GetType(Color), "")>
            Public Property ButtonDefaultColor As Color
                Get
                    Return prop_ButtonDefaultColor
                End Get
                Set(value As Color)
                    prop_ButtonDefaultColor = value
                    RaiseEvent ButtonColorUpdate(prop_ButtonDefaultColor, prop_ButtonMouseOverColor, prop_ButtonMouseDownColor)
                End Set
            End Property

            <DefaultValue(GetType(Color), "&hFFE1001F")>
            Public Property ButtonMouseOverColor As Color
                Get
                    Return prop_ButtonMouseOverColor
                End Get
                Set(value As Color)
                    prop_ButtonMouseOverColor = value
                    RaiseEvent ButtonColorUpdate(prop_ButtonDefaultColor, prop_ButtonMouseOverColor, prop_ButtonMouseDownColor)
                End Set
            End Property

            <DefaultValue(GetType(Color), "&hFFC4001F")>
            Public Property ButtonMouseDownColor As Color
                Get
                    Return prop_ButtonMouseDownColor
                End Get
                Set(value As Color)
                    prop_ButtonMouseDownColor = value
                    RaiseEvent ButtonColorUpdate(prop_ButtonDefaultColor, prop_ButtonMouseOverColor, prop_ButtonMouseDownColor)
                End Set
            End Property

            <DefaultValue(GetType(Integer), "0")>
            Public Property ButtonBorderSize As Integer
                Get
                    Return prop_ButtonBorderSize
                End Get
                Set(value As Integer)
                    prop_ButtonBorderSize = value
                    RaiseEvent ButtonBorderUpdate(value)
                End Set
            End Property

            <DefaultValue(GetType(Integer), "0")>
            Public Property TitleBarBorderSize As Integer
                Get
                    Return prop_TitleBarBorderSize
                End Get
                Set(value As Integer)
                    prop_TitleBarBorderSize = value
                    RaiseEvent TitleBarSizeUpdate(prop_TitleBarHeight, prop_TitleBarBorderSize)
                End Set
            End Property

            <DefaultValue(GetType(Integer), "25")>
            Public Property TitleBarHeight As Integer
                Get
                    Return prop_TitleBarHeight
                End Get
                Set(value As Integer)
                    prop_TitleBarHeight = value
                    RaiseEvent TitleBarSizeUpdate(prop_TitleBarHeight, prop_TitleBarBorderSize)
                End Set
            End Property

            <DefaultValue(GetType(Color), "&h1F1F1F")>
            Public Property TitleBarBackgroundColor As Color
                Get
                    Return Color.FromArgb(prop_TitleBarBackgroundColor.R, prop_TitleBarBackgroundColor.G, prop_TitleBarBackgroundColor.B)
                End Get
                Set(value As Color)
                    prop_TitleBarBackgroundColor = Color.FromArgb(value.R, value.G, value.B)
                    RaiseEvent TitleBarColorUpdate(Color.FromArgb(prop_TitleBarBackgroundColorOpacity, value.R, value.G, value.B))
                End Set
            End Property

            <DefaultValue(GetType(Byte), "48")>
            Public Property TitleBarBackgroundColorOpacity As Byte
                Get
                    Return prop_TitleBarBackgroundColorOpacity
                End Get
                Set(value As Byte)
                    prop_TitleBarBackgroundColorOpacity = value
                    RaiseEvent TitleBarColorUpdate(Color.FromArgb(value, prop_TitleBarBackgroundColor.R, prop_TitleBarBackgroundColor.G, prop_TitleBarBackgroundColor.B))
                End Set
            End Property

            <DefaultValue(True)>
            Public Property CollapseButtonEnabled As Boolean
                Get
                    Return prop_CollapseButtonEnabled
                End Get
                Set(value As Boolean)
                    prop_CollapseButtonEnabled = value
                    RaiseEvent CollapseButtonToggle(value)
                End Set
            End Property

            <DefaultValue(True)>
            Public Property HelpButtonEnabled As Boolean
                Get
                    Return prop_HelpButtonEnabled
                End Get
                Set(value As Boolean)
                    prop_HelpButtonEnabled = value
                    RaiseEvent HelpButtonToggle(value)
                End Set
            End Property

            Public Sub New()

            End Sub

            Public Sub New(BtnDefColor As Color, BtnOverColor As Color, BtnDownColor As Color, BtnBorderSize As Integer,
                           TitleBackColor As Color, TitleBackOpacity As Byte, TitleBorderSize As Integer, TitleHeight As Integer,
                           TitleBtnLayout As ButtonLayoutMode)
                prop_ButtonDefaultColor = BtnDefColor
                prop_ButtonMouseOverColor = BtnOverColor
                prop_ButtonMouseDownColor = BtnDownColor
                prop_ButtonBorderSize = BtnBorderSize
                prop_TitleBarBorderSize = TitleBorderSize
                prop_TitleBarBackgroundColor = TitleBackColor
                prop_TitleBarBackgroundColorOpacity = TitleBackOpacity
                prop_TitleBarButtonLayout = TitleBtnLayout
                prop_TitleBarHeight = TitleHeight
                prop_CollapseButtonEnabled = True
                prop_HelpButtonEnabled = True
            End Sub
        End Class
#End Region

#Region "Methods"
#Region "Designerless code import"
        'Form reemplaza a Dispose para limpiar la lista de componentes.
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Requerido por el Diseñador de Windows Forms
        Private components As IContainer
        'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
        'Se puede modificar usando el Diseñador de Windows Forms.  
        'No lo modifique con el editor de código.
        Private Sub InitializeComponent()
            Me.Frm_TitleBar = New System.Windows.Forms.Panel()
            Me.Frm_Btn_Help = New System.Windows.Forms.Button()
            Me.Frm_Btn_Collapse = New System.Windows.Forms.Button()
            Me.Frm_Btn_Minimize = New System.Windows.Forms.Button()
            Me.Frm_Btn_Maximize = New System.Windows.Forms.Button()
            Me.Frm_Btn_Close = New System.Windows.Forms.Button()
            Me.Frm_Txt_Title = New System.Windows.Forms.Label()
            Me.Frm_TitleBar.SuspendLayout()
            Me.SuspendLayout()
            '
            'Frm_TitleBar
            '
            Me.Frm_TitleBar.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar
            Me.Frm_TitleBar.BackColor = System.Drawing.Color.FromArgb(CType(CType(96, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_TitleBar.Controls.Add(Me.Frm_Btn_Help)
            Me.Frm_TitleBar.Controls.Add(Me.Frm_Btn_Collapse)
            Me.Frm_TitleBar.Controls.Add(Me.Frm_Btn_Minimize)
            Me.Frm_TitleBar.Controls.Add(Me.Frm_Btn_Maximize)
            Me.Frm_TitleBar.Controls.Add(Me.Frm_Btn_Close)
            Me.Frm_TitleBar.Controls.Add(Me.Frm_Txt_Title)
            Me.Frm_TitleBar.Dock = System.Windows.Forms.DockStyle.Top
            Me.Frm_TitleBar.Location = New System.Drawing.Point(5, 5)
            Me.Frm_TitleBar.Name = "Frm_TitleBar"
            Me.Frm_TitleBar.Size = New System.Drawing.Size(490, 25)
            Me.Frm_TitleBar.TabIndex = 1
            '
            'Frm_Btn_Help
            '
            Me.Frm_Btn_Help.Dock = System.Windows.Forms.DockStyle.Left
            Me.Frm_Btn_Help.FlatAppearance.BorderSize = 0
            Me.Frm_Btn_Help.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Help.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Help.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Help.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.Frm_Btn_Help.Location = New System.Drawing.Point(39, 0)
            Me.Frm_Btn_Help.Name = "Frm_Btn_Help"
            Me.Frm_Btn_Help.Size = New System.Drawing.Size(39, 25)
            Me.Frm_Btn_Help.TabIndex = 10
            Me.Frm_Btn_Help.TabStop = False
            Me.Frm_Btn_Help.Text = "?"
            Me.Frm_Btn_Help.UseVisualStyleBackColor = True
            '
            'Frm_Btn_Collapse
            '
            Me.Frm_Btn_Collapse.Dock = System.Windows.Forms.DockStyle.Left
            Me.Frm_Btn_Collapse.FlatAppearance.BorderSize = 0
            Me.Frm_Btn_Collapse.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Collapse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Collapse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Collapse.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.Frm_Btn_Collapse.Location = New System.Drawing.Point(0, 0)
            Me.Frm_Btn_Collapse.Name = "Frm_Btn_Collapse"
            Me.Frm_Btn_Collapse.Size = New System.Drawing.Size(39, 25)
            Me.Frm_Btn_Collapse.TabIndex = 11
            Me.Frm_Btn_Collapse.TabStop = False
            Me.Frm_Btn_Collapse.Text = "⚊"
            Me.Frm_Btn_Collapse.UseVisualStyleBackColor = True
            '
            'Frm_Btn_Minimize
            '
            Me.Frm_Btn_Minimize.Dock = System.Windows.Forms.DockStyle.Right
            Me.Frm_Btn_Minimize.FlatAppearance.BorderSize = 0
            Me.Frm_Btn_Minimize.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Minimize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Minimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.Frm_Btn_Minimize.Location = New System.Drawing.Point(373, 0)
            Me.Frm_Btn_Minimize.Name = "Frm_Btn_Minimize"
            Me.Frm_Btn_Minimize.Size = New System.Drawing.Size(39, 25)
            Me.Frm_Btn_Minimize.TabIndex = 8
            Me.Frm_Btn_Minimize.TabStop = False
            Me.Frm_Btn_Minimize.Text = "_"
            Me.Frm_Btn_Minimize.UseVisualStyleBackColor = True
            '
            'Frm_Btn_Maximize
            '
            Me.Frm_Btn_Maximize.Dock = System.Windows.Forms.DockStyle.Right
            Me.Frm_Btn_Maximize.FlatAppearance.BorderSize = 0
            Me.Frm_Btn_Maximize.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Maximize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Maximize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Maximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.Frm_Btn_Maximize.Location = New System.Drawing.Point(412, 0)
            Me.Frm_Btn_Maximize.Name = "Frm_Btn_Maximize"
            Me.Frm_Btn_Maximize.Size = New System.Drawing.Size(39, 25)
            Me.Frm_Btn_Maximize.TabIndex = 7
            Me.Frm_Btn_Maximize.TabStop = False
            Me.Frm_Btn_Maximize.Text = "🗖"
            Me.Frm_Btn_Maximize.UseVisualStyleBackColor = True
            '
            'Frm_Btn_Close
            '
            Me.Frm_Btn_Close.Dock = System.Windows.Forms.DockStyle.Right
            Me.Frm_Btn_Close.FlatAppearance.BorderSize = 0
            Me.Frm_Btn_Close.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.Frm_Btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.Frm_Btn_Close.Location = New System.Drawing.Point(451, 0)
            Me.Frm_Btn_Close.Name = "Frm_Btn_Close"
            Me.Frm_Btn_Close.Size = New System.Drawing.Size(39, 25)
            Me.Frm_Btn_Close.TabIndex = 6
            Me.Frm_Btn_Close.TabStop = False
            Me.Frm_Btn_Close.Text = "🗙"
            Me.Frm_Btn_Close.UseVisualStyleBackColor = True
            '
            'Frm_Txt_Title
            '
            Me.Frm_Txt_Title.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Frm_Txt_Title.Location = New System.Drawing.Point(0, 0)
            Me.Frm_Txt_Title.Name = "Frm_Txt_Title"
            Me.Frm_Txt_Title.Size = New System.Drawing.Size(490, 25)
            Me.Frm_Txt_Title.TabIndex = 9
            Me.Frm_Txt_Title.Text = "Label1"
            Me.Frm_Txt_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'WindowSkin
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer), CType(CType(31, Byte), Integer))
            Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            Me.ClientSize = New System.Drawing.Size(500, 400)
            Me.Controls.Add(Me.Frm_TitleBar)
            Me.ForeColor = System.Drawing.Color.White
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.MinimumSize = New System.Drawing.Size(320, 120)
            Me.Name = "WindowSkin"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Form"
            Me.Frm_TitleBar.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
#End Region

#Region "Window shake"
        Protected Delegate Sub MoveMeDelegate(ByVal X As Integer, ByVal Y As Integer)
        Public Sub ShakeWindow(ByVal ShakeStrength As Integer)
            If WindowState = FormWindowState.Normal Then
                Dim info As New ShakeInfo
                info.ShakeTimeInMilliSeconds = 500
                info.MaxPixelDrift = ShakeStrength
                info.InitialX = Location.X
                info.InitialY = Location.Y
                System.Threading.ThreadPool.QueueUserWorkItem(AddressOf ShakeMe, info)
            End If
        End Sub
        Private Sub ShakeMe(ByVal info As ShakeInfo)
            Dim startTime As Date = Now
            Dim OffsetX As Integer = -1
            Dim OffsetY As Integer = -1
            Dim WorkingX As Integer = info.InitialX
            Dim WorkingY As Integer = info.InitialY
            Dim XPosDirection As Boolean = True
            Dim YPosDirection As Boolean = True
            While Now.Subtract(startTime).TotalMilliseconds <= info.ShakeTimeInMilliSeconds
                WorkingX += OffsetX
                WorkingY += OffsetY
                MoveMe(WorkingX, WorkingY)

                If XPosDirection Then
                    If Location.X <= info.InitialX - info.MaxPixelDrift Then
                        OffsetX = 1
                    End If
                ElseIf Location.X >= info.InitialX + info.MaxPixelDrift Then
                    OffsetX = -1
                End If
                If OffsetX <= 0 Then
                    XPosDirection = True
                Else
                    XPosDirection = False
                End If

                If YPosDirection Then
                    If Location.Y <= (info.InitialY - (info.MaxPixelDrift / 2)) Then
                        OffsetY = 1
                    End If
                ElseIf Location.Y >= (info.InitialY + (info.MaxPixelDrift / 2)) Then
                    OffsetY = -1
                End If

                If OffsetY <= 0 Then
                    YPosDirection = True
                Else
                    YPosDirection = False
                End If
            End While
            MoveMe(info.InitialX, info.InitialY)
        End Sub
        Private Sub MoveMe(ByVal X As Integer, ByVal Y As Integer)
            If Me.InvokeRequired Then
                Dim move As New MoveMeDelegate(AddressOf DoMoveMe)
                Me.Invoke(move, X, Y)
            Else
                DoMoveMe(X, Y)
            End If
        End Sub
        Private Sub DoMoveMe(ByVal X As Integer, ByVal Y As Integer)
            Me.Location = New Point(X, Y)
        End Sub
#End Region

#Region "Window flash"
        Private Declare Function FlashWindowEx Lib "User32" (ByRef fwInfo As FLASHWINFO) As Boolean
        ' As defined by: http://msdn.microsoft.com/en-us/library/ms679347(v=vs.85).aspx
        Public Enum FlashWindowFlags As UInt32
            ' Stop flashing. The system restores the window to its original state.
            FLASHW_STOP = 0
            ' Flash the window caption.
            FLASHW_CAPTION = 1
            ' Flash the taskbar button.
            FLASHW_TRAY = 2
            ' Flash both the window caption and taskbar button.
            ' This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
            FLASHW_ALL = 3
            ' Flash continuously, until the FLASHW_STOP flag is set.
            FLASHW_TIMER = 4
            ' Flash continuously until the window comes to the foreground.
            FLASHW_TIMERNOFG = 12
        End Enum
        Public Structure FLASHWINFO
            Public cbSize As UInt32
            Public hwnd As IntPtr
            Public dwFlags As FlashWindowFlags
            Public uCount As UInt32
            Public dwTimeout As UInt32
        End Structure
        Public Shared Function FlashWindow(ByRef handle As IntPtr, ByVal FlashTitleBar As Boolean, ByVal FlashTray As Boolean, ByVal FlashCount As Integer) As Boolean
            If handle = Nothing Then
                Return False
            End If
            Try
                Dim fwi As New FLASHWINFO
                With fwi
                    .hwnd = handle
                    If FlashTitleBar Then .dwFlags = .dwFlags Or FlashWindowFlags.FLASHW_CAPTION
                    If FlashTray Then .dwFlags = .dwFlags Or FlashWindowFlags.FLASHW_TRAY
                    .uCount = CUInt(FlashCount)
                    If FlashCount = 0 Then .dwFlags = .dwFlags Or FlashWindowFlags.FLASHW_TIMERNOFG
                    .dwTimeout = 0 ' Use the default cursor blink rate.
                    .cbSize = CUInt(Marshal.SizeOf(fwi))
                End With
                Return FlashWindowEx(fwi)
            Catch
                Return False
            End Try
        End Function
#End Region

#Region "Event handlers"
        Private Sub ButtonHover(sender As Button, e As EventArgs)
            sender.ForeColor = prop_ButtonAppareance.TextColorHover
        End Sub
        Private Sub ButtonLeave(sender As Button, e As EventArgs)
            sender.ForeColor = prop_ButtonAppareance.TextColorDefault
        End Sub
        Private Sub Update_CollapseButtonVisibility(ByVal State As Boolean)
            Frm_Btn_Collapse.Visible = State
        End Sub
        Private Sub Update_HelpButtonVisibility(ByVal State As Boolean)
            Frm_Btn_Help.Visible = State
        End Sub
        Private Sub Update_TitlebarButtonColor(ByVal Def As Color, ByVal Over As Color, ByVal Down As Color)
            Frm_Btn_Close.BackColor = Def
            Frm_Btn_Close.FlatAppearance.MouseOverBackColor = Over
            Frm_Btn_Close.FlatAppearance.MouseDownBackColor = Down

            Frm_Btn_Maximize.BackColor = Def
            Frm_Btn_Maximize.FlatAppearance.MouseOverBackColor = Over
            Frm_Btn_Maximize.FlatAppearance.MouseDownBackColor = Down

            Frm_Btn_Minimize.BackColor = Def
            Frm_Btn_Minimize.FlatAppearance.MouseOverBackColor = Over
            Frm_Btn_Minimize.FlatAppearance.MouseDownBackColor = Down

            Frm_Btn_Collapse.BackColor = Def
            Frm_Btn_Collapse.FlatAppearance.MouseOverBackColor = Over
            Frm_Btn_Collapse.FlatAppearance.MouseDownBackColor = Down

            Frm_Btn_Help.BackColor = Def
            Frm_Btn_Help.FlatAppearance.MouseOverBackColor = Over
            Frm_Btn_Help.FlatAppearance.MouseDownBackColor = Down
        End Sub
        Private Sub Update_TitleBarColor(ByVal Data As Color)
            Frm_TitleBar.BackColor = Data
        End Sub
        Private Sub Update_TitleBarButtonLayout(ByVal NewLayout As ButtonLayoutMode)
            Select Case NewLayout
                Case ButtonLayoutMode.Left
                    Frm_Btn_Collapse.Dock = DockStyle.Right
                    Frm_Btn_Help.Dock = DockStyle.Right
                    Frm_Btn_Minimize.Dock = DockStyle.Left
                    Frm_Btn_Maximize.Dock = DockStyle.Left
                    Frm_Btn_Close.Dock = DockStyle.Left
                    Frm_Btn_Close.BringToFront()
                    Frm_Btn_Minimize.BringToFront()
                    Frm_Btn_Maximize.BringToFront()
                Case ButtonLayoutMode.Right
                    Frm_Btn_Collapse.Dock = DockStyle.Left
                    Frm_Btn_Help.Dock = DockStyle.Left
                    Frm_Btn_Minimize.Dock = DockStyle.Right
                    Frm_Btn_Maximize.Dock = DockStyle.Right
                    Frm_Btn_Close.Dock = DockStyle.Right
                    Frm_Btn_Close.BringToFront()
                    Frm_Btn_Maximize.BringToFront()
                    Frm_Btn_Minimize.BringToFront()
                Case ButtonLayoutMode.AllLeft
                    Frm_Btn_Collapse.Dock = DockStyle.Left
                    Frm_Btn_Help.Dock = DockStyle.Left
                    Frm_Btn_Minimize.Dock = DockStyle.Left
                    Frm_Btn_Maximize.Dock = DockStyle.Left
                    Frm_Btn_Close.Dock = DockStyle.Left
                Case ButtonLayoutMode.AllRight
                    Frm_Btn_Collapse.Dock = DockStyle.Right
                    Frm_Btn_Help.Dock = DockStyle.Right
                    Frm_Btn_Minimize.Dock = DockStyle.Right
                    Frm_Btn_Maximize.Dock = DockStyle.Right
                    Frm_Btn_Close.Dock = DockStyle.Right
            End Select
        End Sub
        Private Sub TemplateForm_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
            'Frm_Content.Panel1.BackColor = Drawing.Color.FromArgb(31, 255, 255, 255)
        End Sub
        Private Sub TemplateForm_Activated(sender As Object, e As EventArgs) Handles Me.Activated
            'Frm_Content.Panel1.BackColor = Drawing.Color.FromArgb(12, 255, 255, 255)
        End Sub
        Private Sub Frm_Txt_Title_DoubleClick(sender As Object, e As EventArgs) Handles Frm_Txt_Title.DoubleClick
            If MaximizeBox Then
                If WindowState = FormWindowState.Maximized Then
                    WindowState = FormWindowState.Normal
                Else
                    WindowState = FormWindowState.Maximized
                End If
            End If
        End Sub
        Private Sub Frm_Txt_Title_MouseMove(sender As Object, e As MouseEventArgs) Handles Frm_Txt_Title.MouseMove
            If prop_WindowMoving Then
                If e.Button = MouseButtons.Left Then
                    Dim mousePos = Control.MousePosition
                    mousePos.Offset(MousePoint.X, MousePoint.Y)
                    Location = mousePos
                End If
            End If
        End Sub
        Private Sub Frm_Txt_Title_MouseDown(sender As Object, e As MouseEventArgs) Handles Frm_Txt_Title.MouseDown
            If e.Button = MouseButtons.Right Then
                Dim WindowHandle As IntPtr = Handle
                Dim MenuHandle As IntPtr = SystemMenuHandler
                If Not MaximizeBox Then
                    EnableMenuItem(MenuHandle, SC_MAXIMIZE, MF_GRAYED)
                    EnableMenuItem(MenuHandle, SC_RESTORE, MF_GRAYED)
                End If

                If Not MinimizeBox Then
                    EnableMenuItem(MenuHandle, SC_MINIMIZE, MF_GRAYED)
                End If

                If Not ControlBox Then
                    EnableMenuItem(MenuHandle, SC_CLOSE, MF_GRAYED)
                End If
                If Not MenuHandle = IntPtr.Zero Then
                    Dim MenuCommand As Integer = TrackPopupMenu(MenuHandle, TPM_RETURNCMD Or TPM_LEFTALIGN Or TPM_TOPALIGN, Location.X + e.Location.X, Location.Y + e.Location.Y, 0, WindowHandle, IntPtr.Zero)
                    If MenuCommand > 0 Then
                        SendMessage(WindowHandle, &H112, CType(MenuCommand, IntPtr), IntPtr.Zero)
                        Select Case MenuCommand
                            Case SC_SIZE
                            Case SC_MOVE
                            Case SC_MINIMIZE
                            Case SC_MAXIMIZE
                                Frm_Btn_Maximize.Text = "🗗"
                            Case SC_CLOSE
                            Case SC_RESTORE
                                Frm_Btn_Maximize.Text = "🗖"
                        End Select
                    End If
                End If
            Else
                MousePoint = New Point(-e.X, -e.Y)
            End If
        End Sub
        Private Sub Frm_Btn_Close_Click(sender As Object, e As EventArgs) Handles Frm_Btn_Close.Click
            Close()
        End Sub
        Private Sub Frm_Btn_Minimize_Click(sender As Object, e As EventArgs) Handles Frm_Btn_Minimize.Click
            WindowState = FormWindowState.Minimized
        End Sub
        Private Sub Frm_Btn_Collapse_Click(sender As Object, e As EventArgs) Handles Frm_Btn_Collapse.Click
            If WindowState = FormWindowState.Normal Then
                If WindowsCollapsed Then
                    Size = LastCollapseWindowSize
                    Frm_Btn_Collapse.Text = "⚊"
                    Frm_Btn_Collapse.AccessibleName = "Contraer"
                    WindowsCollapsed = False
                    MinimumSize = New Size(OriginalMinimunSize.Width, OriginalMinimunSize.Height)
                Else
                    LastCollapseWindowSize = Size
                    Frm_Btn_Collapse.Text = "🞣"
                    Frm_Btn_Collapse.AccessibleName = "Expandir"
                    WindowsCollapsed = True
                    MinimumSize = New Size(OriginalMinimunSize.Width, Frm_TitleBar.Height)
                    Height = Frm_TitleBar.Height
                End If
            End If
        End Sub
        Private Sub Frm_Btn_Help_Click(sender As Object, e As EventArgs) Handles Frm_Btn_Help.Click
            SendKeys.Send("{F1}")
        End Sub
        Private Sub Frm_Btn_Maximize_Click(sender As Object, e As EventArgs) Handles Frm_Btn_Maximize.Click
            If MaximizeBox Then
                If WindowState = FormWindowState.Maximized Then
                    SuspendLayout()
                    If prop_TitleBarConfig.CollapseButtonEnabled Then
                        Frm_Btn_Collapse.Visible = True
                    End If
                    WindowState = FormWindowState.Normal
                    Location = LastWindowLocation
                    Size = LastWindowSize
                    ResumeLayout()
                Else
                    SuspendLayout()
                    CurrentScreen = Screen.FromControl(Me)
                    GetTaskbarLocation()

                    If prop_TitleBarConfig.CollapseButtonEnabled Then
                        Frm_Btn_Collapse.Visible = False
                    End If
                    LastWindowLocation = Location
                    LastWindowSize = Size

                    Dim BoundX As Integer = 0
                    Dim BoundY As Integer = 0
                    Dim SizeX As Integer = CurrentScreen.WorkingArea.Size.Width
                    Dim SizeY As Integer = CurrentScreen.WorkingArea.Size.Height
                    Select Case TaskbarPosition
                        Case TaskBarLocation.Bottom
                            BoundX = 0
                            BoundY = 0
                        Case TaskBarLocation.Left
                            BoundX = CurrentScreen.Bounds.Size.Width - CurrentScreen.WorkingArea.Size.Width
                            BoundY = 0
                        Case TaskBarLocation.Right
                            BoundX = 0
                            BoundY = 0
                        Case TaskBarLocation.Top
                            BoundX = 0
                            BoundY = CurrentScreen.Bounds.Size.Height - CurrentScreen.WorkingArea.Size.Height
                        Case TaskBarLocation.NoBar
                            BoundX = 0
                            BoundY = 0
                    End Select
                    MaximumSize = New Size(SizeX, SizeY)
                    MaximizedBounds = New Rectangle(BoundX, BoundY, SizeX, SizeY)
                    WindowState = FormWindowState.Maximized
                    ResumeLayout()
                End If
            End If
        End Sub
        Private Sub WIndowSkin_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
            If Me.WindowState = FormWindowState.Maximized Then
                'Location = Screen.FromControl(Me).WorkingArea.Location
            End If
        End Sub
        Private Sub WIndowSkin_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin

        End Sub
        Private Sub WIndowSkin_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        End Sub
        Private Sub WindowSkin_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
            UpdateResizingBorders()
        End Sub

        Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
            MyBase.OnPaint(pevent)
        End Sub
        Private Sub WindowSkin_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
            If prop_BorderSize > 0 Then
                With e.Graphics
                    .DrawLine(New Pen(prop_BorderColor, prop_BorderSize), New PointF(0, 0 + (prop_BorderSize / 2)), New PointF(Width, 0 + (prop_BorderSize / 2)))
                    .DrawLine(New Pen(prop_BorderColor, prop_BorderSize), New PointF(Width - (prop_BorderSize / 2), 0), New PointF(Width - (prop_BorderSize / 2), Height))
                    .DrawLine(New Pen(prop_BorderColor, prop_BorderSize), New PointF(0 + (prop_BorderSize / 2), 0), New PointF(0 + (prop_BorderSize / 2), Height))
                    .DrawLine(New Pen(prop_BorderColor, prop_BorderSize), New PointF(0, Height - (prop_BorderSize / 2)), New PointF(Width, Height - (prop_BorderSize / 2)))
                End With
            End If
            'e.Graphics.FillRectangle(Brushes.Green, BorderTop)
            'e.Graphics.FillRectangle(Brushes.Green, BorderBottom)
            'e.Graphics.FillRectangle(Brushes.Green, BorderLeft)
            'e.Graphics.FillRectangle(Brushes.Green, BorderRight)

            'e.Graphics.FillRectangle(Brushes.Blue, BorderTopLeft)
            'e.Graphics.FillRectangle(Brushes.Blue, BorderTopRight)
            'e.Graphics.FillRectangle(Brushes.Blue, BorderBottomLeft)
            'e.Graphics.FillRectangle(Brushes.Blue, BorderBottomRight)
        End Sub
#End Region

        Public Sub New()
            InitializeComponent()
        End Sub
        Private Sub TemplateForm_Load(sender As Object, e As EventArgs) Handles MyBase.HandleCreated
            SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.DoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
            UpdateStyles()

            AddHandler prop_TitleBarConfig.TitleBarColorUpdate, AddressOf Update_TitleBarColor
            AddHandler prop_TitleBarConfig.ButtonColorUpdate, AddressOf Update_TitlebarButtonColor
            AddHandler prop_TitleBarConfig.ButtonLayoutUpdate, AddressOf Update_TitleBarButtonLayout
            AddHandler prop_TitleBarConfig.CollapseButtonToggle, AddressOf Update_CollapseButtonVisibility
            AddHandler prop_TitleBarConfig.HelpButtonToggle, AddressOf Update_HelpButtonVisibility

            Update_TitlebarButtonColor(prop_TitleBarConfig.ButtonDefaultColor, prop_TitleBarConfig.ButtonMouseOverColor, prop_TitleBarConfig.ButtonMouseDownColor)

            OriginalWindowSize = Size
            Frm_Txt_Title.Text = Text
            ToggleTitleBarButtons()
            Update_TitleBarColor(Color.FromArgb(prop_TitleBarConfig.TitleBarBackgroundColorOpacity, prop_TitleBarConfig.TitleBarBackgroundColor.R, prop_TitleBarConfig.TitleBarBackgroundColor.G, prop_TitleBarConfig.TitleBarBackgroundColor.B))

            SkinChildControls(Controls)
            SystemMenuHandler = GetSystemMenu(Handle, False)
            MyBase.FormBorderStyle = FormBorderStyle.None
            GetTaskbarLocation()

            UpdateResizingBorders()
            'Using Pth As New GraphicsPath
            '    Dim CornerRadius As Integer = 10
            '    Pth.AddArc(0, 0, CornerRadius, CornerRadius, 180, 90) ' top left
            '    Pth.AddArc(Width - CornerRadius, 0, CornerRadius, CornerRadius, 270, 90) ' top right
            '    Pth.AddArc(Width - CornerRadius, Height - CornerRadius, CornerRadius, CornerRadius, 0, 90) ' bottom right
            '    Pth.AddArc(0, Height - CornerRadius, CornerRadius, CornerRadius, 90, 90) ' bottom left
            '    Using Reg As New Region(Pth)
            '        Region = Reg
            '    End Using
            'End Using
        End Sub
        Friend Sub RegisterTitlebarButton(ByVal ButtonName As String, ByVal NewButton As Button)
            With NewButton
                .Dock = DockStyle.Left
                .TextAlign = ContentAlignment.MiddleCenter
                .AccessibleName = .Text
                .AccessibleRole = AccessibleRole.TitleBar
                .FlatStyle = FlatStyle.Flat
                .ForeColor = Drawing.Color.White
                .TabStop = False
                .BackColor = prop_TitleBarConfig.ButtonDefaultColor
                .FlatAppearance.MouseDownBackColor = prop_TitleBarConfig.ButtonMouseDownColor
                .FlatAppearance.MouseOverBackColor = prop_TitleBarConfig.ButtonMouseOverColor
                .FlatAppearance.CheckedBackColor = Color.Empty
                .FlatAppearance.BorderSize = 0
                '.FlatAppearance.BorderColor = MainWindowPanel.Panel1.BackColor
            End With
            prop_TitleBarUserButtons.Add(ButtonName, NewButton)
            Frm_TitleBar.Controls.Add(prop_TitleBarUserButtons(ButtonName))
            prop_TitleBarUserButtons(ButtonName).BringToFront()
        End Sub
        Private Sub GetTaskbarLocation()
            ' 1 - We start updating the display object based on the current form position.
            CurrentScreen = Screen.FromControl(Me)

            Dim PosX As Integer = If(CurrentScreen.WorkingArea.Location.X < 0, CurrentScreen.WorkingArea.Location.X * -1, CurrentScreen.WorkingArea.Location.X)
            Dim PosY As Integer = If(CurrentScreen.WorkingArea.Location.Y < 0, CurrentScreen.WorkingArea.Location.Y * -1, CurrentScreen.WorkingArea.Location.Y)

            ' 2 - Get the screen index in a cartesian axis
            If CurrentScreen.WorkingArea.Location.X < 0 Or CurrentScreen.WorkingArea.Location.Y < 0 Then
                ' Negative screen
                ' Screen is at the left or over the center screen. Lets compensate for those negative numbers.
                PosX = If(PosX > 0, If(PosX = CurrentScreen.Bounds.Size.Width, 0, CurrentScreen.Bounds.Size.Width - PosX), 0)
                PosY = If(PosY > 0, If(PosY = CurrentScreen.Bounds.Size.Height, 0, CurrentScreen.Bounds.Size.Height - PosY), 0)
            ElseIf CurrentScreen.WorkingArea.Location.X >= CurrentScreen.Bounds.Size.Width Then
                ' Positive screen
                ' Screen is at the right or below the center screen. We need to substract the screen resolution to get the actual coordinates
                PosX = If(PosX >= CurrentScreen.Bounds.Size.Width, PosX - CurrentScreen.Bounds.Size.Width, PosX)
                PosY = If(PosY >= CurrentScreen.Bounds.Size.Height, PosY - CurrentScreen.Bounds.Size.Height, PosY)
            Else
                ' Central screen
                ' The current screen is the one at the center. We do nothing.
            End If

            ' 3 - Now that we know the coordinates of the work area... lets deduce where is the current screen taskbar located.
            If CurrentScreen.WorkingArea.Size.Height <> CurrentScreen.Bounds.Size.Height Then
                TaskbarPosition = If(PosY > 0, TaskBarLocation.Top, TaskBarLocation.Bottom)
            ElseIf CurrentScreen.WorkingArea.Size.Width <> CurrentScreen.Bounds.Size.Width Then
                TaskbarPosition = If(PosX > 0, TaskBarLocation.Left, TaskBarLocation.Right)
            Else
                TaskbarPosition = TaskBarLocation.NoBar
            End If
        End Sub
        Private Sub ToggleTitleBarButtons()
            If ControlBox Then
                Select Case Custom_prop_FormBorderStyle
                    Case FormBorderStyle.Fixed3D, FormBorderStyle.FixedSingle, FormBorderStyle.Sizable, FormBorderStyle.FixedDialog
                        If MaximizeBox Then
                            Frm_Btn_Maximize.Visible = True
                        Else
                            Frm_Btn_Maximize.Visible = False
                        End If
                        If MinimizeBox Then
                            Frm_Btn_Minimize.Visible = True
                        Else
                            Frm_Btn_Minimize.Visible = False
                        End If
                        Frm_Btn_Close.Visible = True
                    Case FormBorderStyle.FixedToolWindow, FormBorderStyle.SizableToolWindow
                        Frm_Btn_Maximize.Visible = False
                        Frm_Btn_Minimize.Visible = False
                        Frm_Btn_Close.Visible = True
                    Case FormBorderStyle.None
                        Frm_Btn_Maximize.Visible = False
                        Frm_Btn_Minimize.Visible = False
                        Frm_Btn_Close.Visible = False
                End Select
                Frm_Btn_Collapse.Visible = prop_TitleBarConfig.CollapseButtonEnabled
                Frm_Btn_Help.Visible = prop_TitleBarConfig.HelpButtonEnabled
            Else
                Frm_Btn_Maximize.Visible = False
                Frm_Btn_Minimize.Visible = False
                Frm_Btn_Close.Visible = False
                Frm_Btn_Collapse.Visible = False
                Frm_Btn_Help.Visible = False
            End If
            Select Case FormBorderStyle
                Case FormBorderStyle.Fixed3D, FormBorderStyle.FixedDialog, FormBorderStyle.FixedSingle, FormBorderStyle.FixedToolWindow, FormBorderStyle.None
                    ResizeEnabled = False
                Case FormBorderStyle.Sizable, FormBorderStyle.SizableToolWindow
                    ResizeEnabled = True
            End Select
            'ToggleBorder()
        End Sub
        Private Sub ToggleBorder()
            If Custom_prop_FormBorderStyle = FormBorderStyle.None Then
                'Frm_Content.Panel1Collapsed = True
            Else
                'Frm_Content.Panel1Collapsed = False
            End If
        End Sub
        Private Sub SkinChildControls(MasterControl As Control.ControlCollection)
            For Index As Integer = 0 To MasterControl.Count - 1
                Select Case MasterControl(Index).GetType.ToString.ToLower
                    Case "system.windows.forms.label"
                        Dim TheControl As Label = CType(MasterControl(Index), Label)
                    Case "system.windows.forms.linklabel"
                        Dim TheControl As LinkLabel = CType(MasterControl(Index), LinkLabel)
                        TheControl.BackColor = Drawing.Color.Transparent
                    Case "system.windows.forms.combobox"
                        Dim TheControl As ComboBox = CType(MasterControl(Index), ComboBox)
                        TheControl.FlatStyle = FlatStyle.Flat
                    Case "system.windows.forms.button"
                        If prop_ButtonAppareance.AutoSkin Then
                            Dim TheControl As Button = CType(MasterControl(Index), Button)
                            TheControl.FlatStyle = FlatStyle.Flat
                            TheControl.FlatAppearance.MouseDownBackColor = prop_ButtonAppareance.ColorMouseDown
                            TheControl.FlatAppearance.MouseOverBackColor = prop_ButtonAppareance.ColorMouseOver
                            TheControl.FlatAppearance.BorderSize = prop_ButtonAppareance.Border
                            If prop_ButtonAppareance.ColorDefault <> Color.Transparent Then
                                TheControl.FlatAppearance.BorderColor = prop_ButtonAppareance.ColorDefault
                            End If
                            TheControl.BackColor = prop_ButtonAppareance.ColorDefault
                            AddHandler TheControl.MouseEnter, AddressOf ButtonHover
                            AddHandler TheControl.MouseLeave, AddressOf ButtonLeave
                        End If
                    Case "system.windows.forms.checkbox"
                        Dim TheControl As CheckBox = CType(MasterControl(Index), CheckBox)
                        TheControl.FlatStyle = FlatStyle.Flat
                        TheControl.BackColor = Color.Transparent
                    Case "system.windows.forms.datagridview"
                        Dim TheControl As DataGridView = CType(MasterControl(Index), DataGridView)
                        TheControl.BackColor = Color.Black
                    Case "system.windows.forms.groupbox"
                        SkinChildControls(CType(MasterControl(Index), GroupBox).Controls)
                    Case "system.windows.forms.splitcontainer"
                        SkinChildControls(CType(MasterControl(Index), SplitContainer).Panel1.Controls)
                        SkinChildControls(CType(MasterControl(Index), SplitContainer).Panel2.Controls)
                    Case "system.windows.forms.SplitterPanel"
                        SkinChildControls(CType(MasterControl(Index), SplitterPanel).Controls)
                    Case "system.windows.forms.panel"
                        If CType(MasterControl(Index), Panel).Name = "Frm_TitleBar" Then
                            ' Skip the title bar
                        Else
                            SkinChildControls(CType(MasterControl(Index), Panel).Controls)
                        End If
                End Select
            Next
        End Sub

        Private Const HTLEFT As Integer = 10
        Private Const HTRIGHT As Integer = 11
        Private Const HTTOP As Integer = 12
        Private Const HTTOPLEFT As Integer = 13
        Private Const HTTOPRIGHT As Integer = 14
        Private Const HTBOTTOM As Integer = 15
        Private Const HTBOTTOMLEFT As Integer = 16
        Private Const HTBOTTOMRIGHT As Integer = 17

        Private BorderThiccness As Integer = 5
        Private BorderTop As New Rectangle(BorderThiccness, 0, Width, BorderThiccness)
        Private BorderBottom As New Rectangle(BorderThiccness, Height - BorderThiccness, Width - (BorderThiccness * 2), BorderThiccness)
        Private BorderLeft As New Rectangle(0, BorderThiccness, BorderThiccness, Width - (BorderThiccness * 2))
        Private BorderRight As New Rectangle(Width - BorderThiccness, BorderThiccness, BorderThiccness, Height)

        Private BorderTopLeft As New Rectangle(0, 0, BorderThiccness, BorderThiccness)
        Private BorderTopRight As New Rectangle(Width - BorderThiccness, 0, BorderThiccness, BorderThiccness)
        Private BorderBottomLeft As New Rectangle(0, Height - BorderThiccness, BorderThiccness, BorderThiccness)
        Private BorderBottomRight As New Rectangle(Width - BorderThiccness, Height - BorderThiccness, BorderThiccness, BorderThiccness)

        Protected Overrides Sub WndProc(ByRef m As Message)
            MyBase.WndProc(m)
            If ResizeEnabled Then
                If m.Msg = &H84 Then
                    Dim cursor = Me.PointToClient(MousePosition)
                    If BorderTop.Contains(cursor) Then
                        m.Result = HTTOP
                    ElseIf BorderBottom.Contains(cursor) Then
                        m.Result = HTBOTTOM
                    ElseIf BorderLeft.Contains(cursor) Then
                        m.Result = HTLEFT
                    ElseIf BorderRight.Contains(cursor) Then
                        m.Result = HTRIGHT
                    ElseIf BorderTopLeft.Contains(cursor) Then
                        m.Result = HTTOPLEFT
                    ElseIf BorderTopRight.Contains(cursor) Then
                        m.Result = HTTOPRIGHT
                    ElseIf BorderBottomLeft.Contains(cursor) Then
                        m.Result = HTBOTTOMLEFT
                    ElseIf BorderBottomRight.Contains(cursor) Then
                        m.Result = HTBOTTOMRIGHT
                    End If
                End If
            End If

        End Sub

        Private Sub UpdateResizingBorders()
            BorderTop.Width = Width - (BorderThiccness * 2)
            BorderBottom.Width = Width - (BorderThiccness * 2)
            BorderBottom.Y = Height - BorderThiccness

            BorderLeft.Height = Height - (BorderThiccness * 2)
            BorderRight.Height = Height - (BorderThiccness * 2)
            BorderRight.X = Width - BorderThiccness

            BorderTopLeft.X = 0
            BorderTopLeft.Y = 0

            BorderTopRight.X = Width - BorderThiccness
            BorderTopRight.Y = 0

            BorderBottomLeft.X = 0
            BorderBottomLeft.Y = Height - BorderThiccness

            BorderBottomRight.X = Width - BorderThiccness
            BorderBottomRight.Y = Height - BorderThiccness
            'Refresh()
        End Sub

#End Region
    End Class
End Namespace