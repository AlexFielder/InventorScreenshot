<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ScreenshotForm
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ScreenshotForm))
    Me.PictureBox1 = New System.Windows.Forms.PictureBox
    Me.Cancel = New System.Windows.Forms.Button
    Me.OK = New System.Windows.Forms.Button
    Me.GroupBoxSettings = New System.Windows.Forms.GroupBox
    Me.Label3 = New System.Windows.Forms.Label
    Me.Label2 = New System.Windows.Forms.Label
    Me.Label1 = New System.Windows.Forms.Label
    Me.ComboBoxGray = New System.Windows.Forms.ComboBox
    Me.ComboBoxFG = New System.Windows.Forms.ComboBox
    Me.ComboBoxBG = New System.Windows.Forms.ComboBox
    Me.ButtonWindow = New System.Windows.Forms.Button
    Me.RadioWindow = New System.Windows.Forms.RadioButton
    Me.RadioApplication = New System.Windows.Forms.RadioButton
    Me.RadioDocument = New System.Windows.Forms.RadioButton
    Me.GroupBoxSelectOp = New System.Windows.Forms.GroupBox
    Me.GroupBoxOutput = New System.Windows.Forms.GroupBox
    Me.CheckBoxPrinter = New System.Windows.Forms.CheckBox
    Me.CheckBoxFile = New System.Windows.Forms.CheckBox
    Me.CheckBoxClipboard = New System.Windows.Forms.CheckBox
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupBoxSettings.SuspendLayout()
    Me.GroupBoxSelectOp.SuspendLayout()
    Me.GroupBoxOutput.SuspendLayout()
    Me.SuspendLayout()
    '
    'PictureBox1
    '
    Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.PictureBox1.Location = New System.Drawing.Point(293, 20)
    Me.PictureBox1.Name = "PictureBox1"
    Me.PictureBox1.Size = New System.Drawing.Size(300, 250)
    Me.PictureBox1.TabIndex = 10
    Me.PictureBox1.TabStop = False
    '
    'Cancel
    '
    Me.Cancel.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.Cancel.AutoSize = True
    Me.Cancel.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Cancel.Location = New System.Drawing.Point(503, 308)
    Me.Cancel.Name = "Cancel"
    Me.Cancel.Size = New System.Drawing.Size(90, 46)
    Me.Cancel.TabIndex = 9
    Me.Cancel.Text = "Exit"
    Me.Cancel.UseVisualStyleBackColor = True
    '
    'OK
    '
    Me.OK.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.OK.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.OK.Location = New System.Drawing.Point(293, 308)
    Me.OK.Name = "OK"
    Me.OK.Size = New System.Drawing.Size(98, 46)
    Me.OK.TabIndex = 8
    Me.OK.Text = "Save Screenshot"
    Me.OK.UseVisualStyleBackColor = True
    '
    'GroupBoxSettings
    '
    Me.GroupBoxSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.GroupBoxSettings.Controls.Add(Me.Label3)
    Me.GroupBoxSettings.Controls.Add(Me.Label2)
    Me.GroupBoxSettings.Controls.Add(Me.Label1)
    Me.GroupBoxSettings.Controls.Add(Me.ComboBoxGray)
    Me.GroupBoxSettings.Controls.Add(Me.ComboBoxFG)
    Me.GroupBoxSettings.Controls.Add(Me.ComboBoxBG)
    Me.GroupBoxSettings.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.GroupBoxSettings.Location = New System.Drawing.Point(16, 147)
    Me.GroupBoxSettings.Name = "GroupBoxSettings"
    Me.GroupBoxSettings.Size = New System.Drawing.Size(253, 138)
    Me.GroupBoxSettings.TabIndex = 7
    Me.GroupBoxSettings.TabStop = False
    Me.GroupBoxSettings.Text = "Settings"
    '
    'Label3
    '
    Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.Label3.AutoSize = True
    Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label3.Location = New System.Drawing.Point(24, 107)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(65, 16)
    Me.Label3.TabIndex = 12
    Me.Label3.Text = "GrayScale"
    '
    'Label2
    '
    Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.Label2.AutoSize = True
    Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label2.Location = New System.Drawing.Point(24, 72)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(74, 16)
    Me.Label2.TabIndex = 12
    Me.Label2.Text = "Foreground"
    '
    'Label1
    '
    Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.Label1.AutoSize = True
    Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label1.Location = New System.Drawing.Point(24, 33)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(74, 16)
    Me.Label1.TabIndex = 12
    Me.Label1.Text = "Background"
    '
    'ComboBoxGray
    '
    Me.ComboBoxGray.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    Me.ComboBoxGray.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.ComboBoxGray.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ComboBoxGray.ForeColor = System.Drawing.Color.Blue
    Me.ComboBoxGray.FormatString = "WithGray;WithoutGray"
    Me.ComboBoxGray.FormattingEnabled = True
    Me.ComboBoxGray.Items.AddRange(New Object() {"Off", "On"})
    Me.ComboBoxGray.Location = New System.Drawing.Point(122, 104)
    Me.ComboBoxGray.Name = "ComboBoxGray"
    Me.ComboBoxGray.Size = New System.Drawing.Size(121, 24)
    Me.ComboBoxGray.TabIndex = 5
    '
    'ComboBoxFG
    '
    Me.ComboBoxFG.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    Me.ComboBoxFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.ComboBoxFG.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ComboBoxFG.ForeColor = System.Drawing.Color.Blue
    Me.ComboBoxFG.FormatString = "White;Black"
    Me.ComboBoxFG.FormattingEnabled = True
    Me.ComboBoxFG.Items.AddRange(New Object() {"Normal", "Black"})
    Me.ComboBoxFG.Location = New System.Drawing.Point(122, 70)
    Me.ComboBoxFG.Name = "ComboBoxFG"
    Me.ComboBoxFG.Size = New System.Drawing.Size(121, 24)
    Me.ComboBoxFG.TabIndex = 4
    '
    'ComboBoxBG
    '
    Me.ComboBoxBG.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    Me.ComboBoxBG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.ComboBoxBG.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ComboBoxBG.ForeColor = System.Drawing.Color.Blue
    Me.ComboBoxBG.FormattingEnabled = True
    Me.ComboBoxBG.ImeMode = System.Windows.Forms.ImeMode.Off
    Me.ComboBoxBG.Items.AddRange(New Object() {"Normal", "White"})
    Me.ComboBoxBG.Location = New System.Drawing.Point(122, 31)
    Me.ComboBoxBG.Name = "ComboBoxBG"
    Me.ComboBoxBG.Size = New System.Drawing.Size(121, 24)
    Me.ComboBoxBG.TabIndex = 3
    '
    'ButtonWindow
    '
    Me.ButtonWindow.Anchor = System.Windows.Forms.AnchorStyles.Bottom
    Me.ButtonWindow.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ButtonWindow.Location = New System.Drawing.Point(138, 93)
    Me.ButtonWindow.Name = "ButtonWindow"
    Me.ButtonWindow.Size = New System.Drawing.Size(94, 26)
    Me.ButtonWindow.TabIndex = 5
    Me.ButtonWindow.Text = "Window<"
    Me.ButtonWindow.UseVisualStyleBackColor = True
    '
    'RadioWindow
    '
    Me.RadioWindow.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.RadioWindow.AutoSize = True
    Me.RadioWindow.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.RadioWindow.Location = New System.Drawing.Point(29, 95)
    Me.RadioWindow.Name = "RadioWindow"
    Me.RadioWindow.Size = New System.Drawing.Size(72, 20)
    Me.RadioWindow.TabIndex = 3
    Me.RadioWindow.Text = "Window"
    Me.RadioWindow.UseVisualStyleBackColor = True
    '
    'RadioApplication
    '
    Me.RadioApplication.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.RadioApplication.AutoSize = True
    Me.RadioApplication.Checked = True
    Me.RadioApplication.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.RadioApplication.Location = New System.Drawing.Point(29, 25)
    Me.RadioApplication.Name = "RadioApplication"
    Me.RadioApplication.Size = New System.Drawing.Size(88, 20)
    Me.RadioApplication.TabIndex = 1
    Me.RadioApplication.TabStop = True
    Me.RadioApplication.Text = "Application"
    Me.RadioApplication.UseVisualStyleBackColor = True
    '
    'RadioDocument
    '
    Me.RadioDocument.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.RadioDocument.AutoSize = True
    Me.RadioDocument.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.RadioDocument.Location = New System.Drawing.Point(29, 60)
    Me.RadioDocument.Name = "RadioDocument"
    Me.RadioDocument.Size = New System.Drawing.Size(83, 20)
    Me.RadioDocument.TabIndex = 0
    Me.RadioDocument.Text = "Document"
    Me.RadioDocument.UseVisualStyleBackColor = True
    '
    'GroupBoxSelectOp
    '
    Me.GroupBoxSelectOp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.GroupBoxSelectOp.Controls.Add(Me.RadioWindow)
    Me.GroupBoxSelectOp.Controls.Add(Me.RadioApplication)
    Me.GroupBoxSelectOp.Controls.Add(Me.RadioDocument)
    Me.GroupBoxSelectOp.Controls.Add(Me.ButtonWindow)
    Me.GroupBoxSelectOp.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.GroupBoxSelectOp.Location = New System.Drawing.Point(16, 4)
    Me.GroupBoxSelectOp.Name = "GroupBoxSelectOp"
    Me.GroupBoxSelectOp.Size = New System.Drawing.Size(253, 137)
    Me.GroupBoxSelectOp.TabIndex = 12
    Me.GroupBoxSelectOp.TabStop = False
    Me.GroupBoxSelectOp.Text = "Select Options"
    '
    'GroupBoxOutput
    '
    Me.GroupBoxOutput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.GroupBoxOutput.Controls.Add(Me.CheckBoxPrinter)
    Me.GroupBoxOutput.Controls.Add(Me.CheckBoxFile)
    Me.GroupBoxOutput.Controls.Add(Me.CheckBoxClipboard)
    Me.GroupBoxOutput.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.GroupBoxOutput.Location = New System.Drawing.Point(16, 300)
    Me.GroupBoxOutput.Name = "GroupBoxOutput"
    Me.GroupBoxOutput.Size = New System.Drawing.Size(253, 54)
    Me.GroupBoxOutput.TabIndex = 14
    Me.GroupBoxOutput.TabStop = False
    Me.GroupBoxOutput.Text = "Output Location"
    '
    'CheckBoxPrinter
    '
    Me.CheckBoxPrinter.AutoSize = True
    Me.CheckBoxPrinter.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.CheckBoxPrinter.Location = New System.Drawing.Point(174, 24)
    Me.CheckBoxPrinter.Name = "CheckBoxPrinter"
    Me.CheckBoxPrinter.Size = New System.Drawing.Size(65, 20)
    Me.CheckBoxPrinter.TabIndex = 2
    Me.CheckBoxPrinter.Text = "Printer"
    Me.CheckBoxPrinter.UseVisualStyleBackColor = True
    '
    'CheckBoxFile
    '
    Me.CheckBoxFile.AutoSize = True
    Me.CheckBoxFile.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.CheckBoxFile.Location = New System.Drawing.Point(103, 24)
    Me.CheckBoxFile.Name = "CheckBoxFile"
    Me.CheckBoxFile.Size = New System.Drawing.Size(47, 20)
    Me.CheckBoxFile.TabIndex = 1
    Me.CheckBoxFile.Text = "File"
    Me.CheckBoxFile.UseVisualStyleBackColor = True
    '
    'CheckBoxClipboard
    '
    Me.CheckBoxClipboard.AutoSize = True
    Me.CheckBoxClipboard.Checked = True
    Me.CheckBoxClipboard.CheckState = System.Windows.Forms.CheckState.Checked
    Me.CheckBoxClipboard.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.CheckBoxClipboard.Location = New System.Drawing.Point(14, 24)
    Me.CheckBoxClipboard.Name = "CheckBoxClipboard"
    Me.CheckBoxClipboard.Size = New System.Drawing.Size(81, 20)
    Me.CheckBoxClipboard.TabIndex = 0
    Me.CheckBoxClipboard.Text = "Clipboard"
    Me.CheckBoxClipboard.UseVisualStyleBackColor = True
    '
    'ScreenshotForm
    '
    Me.AcceptButton = Me.Cancel
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(612, 366)
    Me.Controls.Add(Me.GroupBoxOutput)
    Me.Controls.Add(Me.PictureBox1)
    Me.Controls.Add(Me.GroupBoxSelectOp)
    Me.Controls.Add(Me.GroupBoxSettings)
    Me.Controls.Add(Me.Cancel)
    Me.Controls.Add(Me.OK)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.KeyPreview = True
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "ScreenshotForm"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
    Me.Text = "Screenshot"
    CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupBoxSettings.ResumeLayout(False)
    Me.GroupBoxSettings.PerformLayout()
    Me.GroupBoxSelectOp.ResumeLayout(False)
    Me.GroupBoxSelectOp.PerformLayout()
    Me.GroupBoxOutput.ResumeLayout(False)
    Me.GroupBoxOutput.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
  Friend WithEvents Cancel As System.Windows.Forms.Button
  Friend WithEvents OK As System.Windows.Forms.Button
  Friend WithEvents GroupBoxSettings As System.Windows.Forms.GroupBox
  Friend WithEvents ComboBoxGray As System.Windows.Forms.ComboBox
  Friend WithEvents ComboBoxFG As System.Windows.Forms.ComboBox
  Friend WithEvents ComboBoxBG As System.Windows.Forms.ComboBox
  Friend WithEvents ButtonWindow As System.Windows.Forms.Button
  Friend WithEvents RadioWindow As System.Windows.Forms.RadioButton
  Friend WithEvents RadioApplication As System.Windows.Forms.RadioButton
  Friend WithEvents RadioDocument As System.Windows.Forms.RadioButton
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents GroupBoxSelectOp As System.Windows.Forms.GroupBox
  Friend WithEvents GroupBoxOutput As System.Windows.Forms.GroupBox
  Friend WithEvents CheckBoxClipboard As System.Windows.Forms.CheckBox
  Friend WithEvents CheckBoxPrinter As System.Windows.Forms.CheckBox
  Friend WithEvents CheckBoxFile As System.Windows.Forms.CheckBox
End Class
