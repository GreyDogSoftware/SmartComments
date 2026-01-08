<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TextCommentGenerator
    Inherits GreyDogSoftware.UI.WindowSkin

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TextCommentGenerator))
        Me.Button1 = New System.Windows.Forms.Button()
        Me.CM_Templates = New System.Windows.Forms.ComboBox()
        Me.txt_Result = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CM_Files = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txt_FileVersion = New System.Windows.Forms.Label()
        Me.txt_Author = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(15, Byte), Integer))
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Segoe UI", 13.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.White
        Me.Button1.Location = New System.Drawing.Point(15, 216)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(445, 42)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Comentar!"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'CM_Templates
        '
        Me.CM_Templates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CM_Templates.Enabled = False
        Me.CM_Templates.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.CM_Templates.FormattingEnabled = True
        Me.CM_Templates.Location = New System.Drawing.Point(76, 57)
        Me.CM_Templates.Name = "CM_Templates"
        Me.CM_Templates.Size = New System.Drawing.Size(384, 21)
        Me.CM_Templates.TabIndex = 1
        '
        'txt_Result
        '
        Me.txt_Result.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_Result.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Result.Location = New System.Drawing.Point(15, 84)
        Me.txt_Result.Multiline = True
        Me.txt_Result.Name = "txt_Result"
        Me.txt_Result.ReadOnly = True
        Me.txt_Result.Size = New System.Drawing.Size(445, 126)
        Me.txt_Result.TabIndex = 2
        Me.txt_Result.Text = "1" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "5" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "6" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "7" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "8" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "9" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "10" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "11" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "12" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "13" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "14" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "15" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "16" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "17" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "18" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "19" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "20"
        Me.txt_Result.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(15, 60)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Plantilla"
        '
        'CM_Files
        '
        Me.CM_Files.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CM_Files.Enabled = False
        Me.CM_Files.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.CM_Files.FormattingEnabled = True
        Me.CM_Files.Items.AddRange(New Object() {"Politica nacional (Chile)"})
        Me.CM_Files.Location = New System.Drawing.Point(76, 32)
        Me.CM_Files.Name = "CM_Files"
        Me.CM_Files.Size = New System.Drawing.Size(384, 21)
        Me.CM_Files.TabIndex = 21
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Location = New System.Drawing.Point(15, 35)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(43, 13)
        Me.Label7.TabIndex = 22
        Me.Label7.Text = "Archivo"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(15, 261)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(33, 13)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = "Titulo"
        '
        'txt_FileVersion
        '
        Me.txt_FileVersion.BackColor = System.Drawing.Color.Transparent
        Me.txt_FileVersion.Location = New System.Drawing.Point(54, 261)
        Me.txt_FileVersion.Name = "txt_FileVersion"
        Me.txt_FileVersion.Size = New System.Drawing.Size(406, 13)
        Me.txt_FileVersion.TabIndex = 24
        Me.txt_FileVersion.Text = "Archivo no cargado"
        '
        'txt_Author
        '
        Me.txt_Author.BackColor = System.Drawing.Color.Transparent
        Me.txt_Author.Location = New System.Drawing.Point(54, 281)
        Me.txt_Author.Name = "txt_Author"
        Me.txt_Author.Size = New System.Drawing.Size(406, 13)
        Me.txt_Author.TabIndex = 26
        Me.txt_Author.Text = "Archivo no cargado"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(15, 281)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 13)
        Me.Label3.TabIndex = 25
        Me.Label3.Text = "Autor"
        '
        'TextCommentGenerator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderSize = 0
        Me.ClientSize = New System.Drawing.Size(472, 303)
        Me.Controls.Add(Me.txt_Author)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txt_FileVersion)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.CM_Files)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txt_Result)
        Me.Controls.Add(Me.CM_Templates)
        Me.Controls.Add(Me.Button1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "TextCommentGenerator"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Smart Comment Creator"
        Me.Controls.SetChildIndex(Me.Button1, 0)
        Me.Controls.SetChildIndex(Me.CM_Templates, 0)
        Me.Controls.SetChildIndex(Me.txt_Result, 0)
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.CM_Files, 0)
        Me.Controls.SetChildIndex(Me.Label7, 0)
        Me.Controls.SetChildIndex(Me.Label8, 0)
        Me.Controls.SetChildIndex(Me.txt_FileVersion, 0)
        Me.Controls.SetChildIndex(Me.Label3, 0)
        Me.Controls.SetChildIndex(Me.txt_Author, 0)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents CM_Templates As ComboBox
    Friend WithEvents txt_Result As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents CM_Files As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents txt_FileVersion As Label
    Friend WithEvents txt_Author As Label
    Friend WithEvents Label3 As Label
End Class
