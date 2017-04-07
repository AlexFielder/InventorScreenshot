Imports Inventor
Imports System.Drawing.Imaging
Imports System.Drawing.Printing
Imports System.Drawing.Drawing2D
Imports System.Drawing
Imports System.Windows.Forms

Public Class ScreenshotForm

#Region "variables"  'variables for  this class

  'enum for select options
  Enum SelectOptionsEnum
    eApplication = 1
    eDocument = 2
    eWindow = 3
    eObject = 4 'reserved for future use
  End Enum

  'class for select parameters: size and corner point
  Class clsSelectParam

    Public oCornerPt As System.Drawing.Point = _
      New System.Drawing.Point(0, 0)
    Public oSize As Size = New Size(0, 0)

    Public Sub New()
    End Sub

    Public Sub New(ByVal oP As System.Drawing.Point, ByVal oS As Size)
      oCornerPt = oP : oSize = oS
    End Sub

    Public Sub New( _
      ByVal X As Integer, _
      ByVal Y As Integer, _
      ByVal width As Integer, _
      ByVal height As Integer)

      oCornerPt.X = X : oCornerPt.Y = Y
      oSize.Width = width : oSize.Height = height
    End Sub

  End Class

  'class for select setting: background, forground, gray
  Class clsSelectSetting

    Public oBG As Integer = 0
    Public oFG As Integer = 0
    Public oGray As Integer = 0

    'compare two settings are same or not
    Public Shared Function compare( _
      ByVal oP1 As clsSelectSetting, _
      ByVal oP2 As clsSelectSetting) As Boolean

      compare = True
      If oP1.oBG <> oP2.oBG Or oP1.oFG <> oP2.oFG Or oP1.oGray <> oP2.oGray Then
        compare = False
      End If

    End Function

    'set one setting to another one
    Public Sub setEqualTo(ByVal oPNew As clsSelectSetting)

      oBG = oPNew.oBG
      oFG = oPNew.oFG
      oGray = oPNew.oGray

    End Sub

  End Class

  ' Inventor application object
  Dim m_inventorApplication As Inventor.Application

  'global variable for  corner point
  Dim oCornerPt As System.Drawing.Point = _
    New System.Drawing.Point(0, 0)
  'global variable for size 
  Dim oSize As Size = New Size(0, 0)

  'select parameter for each select mode
  Dim oSelectParam_App As clsSelectParam
  Dim oSelectParam_Doc As clsSelectParam
  Dim oSelectParam_Win As clsSelectParam

  'select settings for each select mode
  Dim oSelectSetting_App As clsSelectSetting = New clsSelectSetting
  Dim oSelectSetting_Doc As clsSelectSetting = New clsSelectSetting
  Dim oSelectSetting_Win As clsSelectSetting = New clsSelectSetting
  Dim oSelectSetting_Current As clsSelectSetting = New clsSelectSetting

  'final bitmap
  Dim oResultBitmap As Bitmap = Nothing

  'bitmap for each select mode
  Dim oBitmap_App As Bitmap = _
    New Bitmap(1, 1, PixelFormat.Format32bppArgb)
  Dim oBitmap_Doc As Bitmap = _
    New Bitmap(1, 1, PixelFormat.Format32bppArgb)
  Dim oBitmap_Win As Bitmap = _
    New Bitmap(1, 1, PixelFormat.Format32bppArgb)

  'select options
  ' 1: Application; 2: Document; 3: Window; 4: Object
  Public oSelectOption As SelectOptionsEnum

  ' flag if user has ever selected window
  Dim oHasSelectWindow As Boolean = False

  'interaction is running
  Public bTakeSnapShot As Boolean = False

  'user is going to select  window
  Private selectingWin As Boolean = False

#End Region  'variables

#Region "Form"

  Public Sub New(ByVal oApp As Inventor.Application)

    InitializeComponent()

    If oApp Is Nothing Then
      MsgBox("Error: Inventor Application is null")
      Exit Sub
    End If

    m_inventorApplication = oApp

    'initialize the option as eApplication
    oSelectOption = SelectOptionsEnum.eApplication

    'disable window button
    ButtonWindow.Enabled = False

    'Param for selecting application
    oSelectParam_App = _
      New clsSelectParam( _
        m_inventorApplication.Left, _
        m_inventorApplication.Top, _
        m_inventorApplication.width, _
        m_inventorApplication.height)

    'params for selecting document
    If Not m_inventorApplication.ActiveView Is Nothing Then
      oSelectParam_Doc = _
        New clsSelectParam( _
          m_inventorApplication.ActiveView.Left, _
          m_inventorApplication.ActiveView.Top, _
          m_inventorApplication.ActiveView.width, _
          m_inventorApplication.ActiveView.height)
    End If

    'params for selecting window, wait the user to input
    oSelectParam_Win = New clsSelectParam()

    ComboBoxBG.SelectedIndex = 0
    ComboBoxFG.SelectedIndex = 0
    ComboBoxGray.SelectedIndex = 0

    'picurebox's mode is StretchImage
    PictureBox1.SizeMode = Windows.Forms.PictureBoxSizeMode.StretchImage

    'original size values of the dialog
    'the third and forth are for newer values
    Me.Tag = _
      Me.Width.ToString() + "," + Me.Height.ToString() + "," + "0" + "," + "0"

    Try
      Dim oCtrl As Control
      Dim oSubCtrl As Control
      For Each oCtrl In Me.Controls

        'size values of the sub-control
        oCtrl.Tag = _
          oCtrl.Left.ToString + "," + oCtrl.Top.ToString + "," + _
          oCtrl.Width.ToString + "," + oCtrl.Height.ToString

        If oCtrl.Name = "GroupBoxSelectOp" Or _
           oCtrl.Name = "GroupBoxSettings" Or _
           oCtrl.Name = "GroupBoxOutput" Then

          For Each oSubCtrl In oCtrl.Controls
            oSubCtrl.Tag = _
              oSubCtrl.Left.ToString + "," + oSubCtrl.Top.ToString + "," + _
              oSubCtrl.Width.ToString + "," + oSubCtrl.Height.ToString
          Next
        End If
      Next

    Catch ex As Exception
      MsgBox(ex.ToString())
    End Try

  End Sub

  Private Sub ScreenshotForm_Load( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) _
    Handles MyBase.Load

    If bTakeSnapShot Then
      Exit Sub
    End If

    bTakeSnapShot = False

    Try
      Dim oCtrl As Control
      Dim oSubCtrl As Control
      For Each oCtrl In Me.Controls

        'setting font by the inventor font
        Dim oFontStyle As FontStyle = FontStyle.Regular
        If oCtrl.Font.Bold Then
          oFontStyle = FontStyle.Bold
        End If
        oCtrl.Font = _
          New Font( _
            m_inventorApplication.GeneralOptions.TextAppearance, _
            m_inventorApplication.GeneralOptions.TextSize, _
            oFontStyle, GraphicsUnit.Point)

        'size values of the sub-control
        If oCtrl.Name = "GroupBoxSelectOp" Or _
           oCtrl.Name = "GroupBoxSettings" Or _
           oCtrl.Name = "GroupBoxOutput" Then

          For Each oSubCtrl In oCtrl.Controls

            'change font of sub controls by the inventor font
            oFontStyle = FontStyle.Regular
            oSubCtrl.Font = _
              New Font( _
                m_inventorApplication.GeneralOptions.TextAppearance, _
                m_inventorApplication.GeneralOptions.TextSize, _
                oFontStyle, GraphicsUnit.Point)
          Next
        End If
      Next

    Catch ex As Exception
      MsgBox(ex.ToString())
    End Try
  End Sub

  'ready to snapshot after dialog close
  Private Sub closeDialogForSnapshot()
    Me.Close()
    bTakeSnapShot = True
  End Sub

  'when  the mode is Application
  Private Sub RadioApplication_Click( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) _
    Handles RadioApplication.Click

    If oSelectOption <> SelectOptionsEnum.eApplication Then


      ' record for next use
      If oSelectOption = SelectOptionsEnum.eDocument Then
        oSelectSetting_Doc.setEqualTo(oSelectSetting_Current)
      ElseIf oSelectOption = SelectOptionsEnum.eWindow Then
        oSelectSetting_Win.setEqualTo(oSelectSetting_Current)
      End If

      oSelectOption = SelectOptionsEnum.eApplication
      ButtonWindow.Enabled = False

      'final check if it  needs to be updated
      If Not (clsSelectSetting.compare(oSelectSetting_App, oSelectSetting_Current)) Or _
         oBitmap_App Is Nothing Then  ' when the dialog is re-opened

        closeDialogForSnapshot()
      Else
        'set existing image to picturebox
        If Not oBitmap_App Is Nothing Then
          PictureBox1.Image = oBitmap_App
          oResultBitmap = oBitmap_App
        End If
      End If

    End If
  End Sub

  'when the selection is document
  Private Sub RadioDocument_Click( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) _
    Handles RadioDocument.Click

    If oSelectOption <> SelectOptionsEnum.eDocument Then

      ' record it for next use
      If oSelectOption = SelectOptionsEnum.eApplication Then
        oSelectSetting_App.setEqualTo(oSelectSetting_Current)
      ElseIf oSelectOption = SelectOptionsEnum.eWindow Then
        oSelectSetting_Win.setEqualTo(oSelectSetting_Current)
      End If

      oSelectOption = SelectOptionsEnum.eDocument
      ButtonWindow.Enabled = False

      'final check if it  needs to be updated
      If Not (clsSelectSetting.compare(oSelectSetting_Doc, oSelectSetting_Current)) Or _
         oBitmap_Doc Is Nothing Then ' ' when the dialog is re-opened

        closeDialogForSnapshot()
      Else
        'set existing image to picturebox
        If Not oBitmap_Doc Is Nothing Then
          PictureBox1.Image = oBitmap_Doc
          oResultBitmap = oBitmap_Doc
        End If
      End If
    End If
  End Sub

  'when the mode is window
  Private Sub RadioWindow_Click( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) _
    Handles RadioWindow.Click

    If oSelectOption <> SelectOptionsEnum.eWindow Then

      ' record it for next use
      If oSelectOption = SelectOptionsEnum.eApplication Then
        oSelectSetting_App.setEqualTo(oSelectSetting_Current)
      ElseIf oSelectOption = SelectOptionsEnum.eDocument Then
        oSelectSetting_Doc.setEqualTo(oSelectSetting_Current)
      End If

      oSelectOption = SelectOptionsEnum.eWindow
      ButtonWindow.Enabled = True

      'final check if it needs to be updated
      If Not (clsSelectSetting.compare(oSelectSetting_Win, oSelectSetting_Current)) Or _
         oBitmap_Win Is Nothing Then

        closeDialogForSnapshot()
      Else
        If Not (oHasSelectWindow) Then
          'need to select by user. so set preview to warning
          PictureBox1.Image = GetWarningBitmap()
          Exit Sub
        Else
          'set existing image to picturebox
          If Not oBitmap_Win Is Nothing Then
            PictureBox1.Image = oBitmap_Win
            oResultBitmap = oBitmap_Win
          End If
        End If
      End If
    End If
  End Sub

  Private Function GetWarningBitmap() As Bitmap

    Dim bmpWidth As Integer = PictureBox1.Width
    Dim bmpHeight As Integer = PictureBox1.Height

    Dim bmp As New Bitmap(bmpWidth, bmpHeight)
    Dim gfx As Graphics = Graphics.FromImage(bmp)

    Dim font As New Font("Tahoma", 14, FontStyle.Bold)

    Dim format As New StringFormat()
    format.Alignment = StringAlignment.Center
    format.LineAlignment = StringAlignment.Center

    Dim rect As New Rectangle(0, 0, bmpWidth, bmpHeight)

    Dim foreBrush As New SolidBrush(System.Drawing.Color.White)
    Dim backBrush As New SolidBrush(System.Drawing.Color.DarkGray)

    gfx.FillRectangle(backBrush, 0, 0, bmpWidth, bmpHeight)
    gfx.DrawString("Please Select Window", font, foreBrush, rect, format)

    Return bmp

  End Function

  'change background color
  Private Sub ComboBoxBG_SelectionChangeCommitted( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) _
    Handles ComboBoxBG.SelectionChangeCommitted

    If oSelectSetting_Current.oBG <> ComboBoxBG.SelectedIndex Then
      oSelectSetting_Current.oBG = ComboBoxBG.SelectedIndex
      closeDialogForSnapshot()
    End If

  End Sub

  'change foreground color
  Private Sub ComboBoxFG_SelectionChangeCommitted( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) _
    Handles ComboBoxFG.SelectionChangeCommitted

    If oSelectSetting_Current.oFG <> ComboBoxFG.SelectedIndex Then
      oSelectSetting_Current.oFG = ComboBoxFG.SelectedIndex
      closeDialogForSnapshot()
    End If

  End Sub

  'change gray setting
  Private Sub ComboBoxGray_SelectionChangeCommitted( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) _
    Handles ComboBoxGray.SelectionChangeCommitted

    If oSelectSetting_Current.oGray <> ComboBoxGray.SelectedIndex Then
      oSelectSetting_Current.oGray = ComboBoxGray.SelectedIndex
      If oSelectSetting_Current.oFG = 0 Then
        closeDialogForSnapshot()
      Else
        ' already force foreground, no need to do gray
      End If
    End If

  End Sub

  'to save the screenshot
  Private Sub OK_Click( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) _
    Handles OK.Click

    If CheckBoxClipboard.Checked Then
      System.Windows.Forms.Clipboard.SetImage(oResultBitmap)
    End If

    If CheckBoxFile.Checked Then
      Try
        'pop out the dialog to specify the file name.
        Dim ofileDlg As System.Windows.Forms.SaveFileDialog = _
          New System.Windows.Forms.SaveFileDialog
        ofileDlg.Filter = _
          "JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|" + _
          "GIF (*.gif)|*.gif|TIFF (*.tif)|*.tif|All files (*.*)|*.*"

        'save
        If ofileDlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
          oResultBitmap.Save( _
            ofileDlg.FileName, _
            GetFormatForFile(ofileDlg.FileName))
        End If
      Catch ex As Exception
        MsgBox("Image creation failed:" & vbCr & ex.ToString())
      End Try
    End If

    If CheckBoxPrinter.Checked Then
      Try
        Dim pdoc As PrintDocument = New PrintDocument()
        AddHandler pdoc.PrintPage, AddressOf pdoc_PrintPage

        Dim pdlg As System.Windows.Forms.PrintDialog = _
          New System.Windows.Forms.PrintDialog()
                pdlg.Document = pdoc

                'known issue of MS: Print dialog does not show on 64bits OS.
                'http://social.msdn.microsoft.com/Forums/en-US/netfx64bit/thread/a707d202-1a8b-43b1-9fff-08aa7ceb200a
                '*******
                pdlg.UseEXDialog = True
                '******

        If pdlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
          pdoc.Print()
        End If
      Catch ex As Exception
        MsgBox(ex.ToString)
      End Try
    End If


  End Sub

  'print event
  Public Sub pdoc_PrintPage( _
    ByVal sender As Object, _
    ByVal e As System.Drawing.Printing.PrintPageEventArgs)

    'copied from AutoCAD Screenshot plugin created by Kean

    Dim toPrint As Bitmap = oResultBitmap
    Dim wid As Integer = toPrint.Width
    Dim hgt As Integer = toPrint.Height
    Dim ratio As Double = CType(wid / hgt, Double)

    If wid <> e.MarginBounds.Width Then
      wid = e.MarginBounds.Width
      hgt = CType(wid / ratio, Integer)
    End If

    If hgt > e.MarginBounds.Height Then
      hgt = e.MarginBounds.Height
      wid = CType(ratio * hgt, Integer)
    End If

    e.Graphics.InterpolationMode = _
      InterpolationMode.HighQualityBicubic
    e.Graphics.DrawImage( _
      toPrint, e.MarginBounds.X, _
      e.MarginBounds.Y, wid, hgt)

  End Sub

  Private Sub ScreenshotForm_KeyDown( _
    ByVal sender As System.Object, _
    ByVal e As System.Windows.Forms.KeyEventArgs) _
    Handles MyBase.KeyDown

    ' shortcut ESC key for exit
    If e.KeyCode = Keys.Escape Then
      Cancel_Click(Me, System.EventArgs.Empty)
    End If

  End Sub

  'exit the dialog
  Private Sub Cancel_Click( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) _
    Handles Cancel.Click

    Me.Close()
    bTakeSnapShot = False
  End Sub

  'user is going to select window
  Private Sub ButtonWindow_Click( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) _
    Handles ButtonWindow.Click

    selectingWin = True
    closeDialogForSnapshot()
  End Sub

  'resize the controls in the dialog
  Private Sub ScreenshotForm_SizeChanged( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) _
    Handles MyBase.SizeChanged

    If Me.Tag Is Nothing Then Exit Sub

    Dim tmp As String() = New String(3) {}
    tmp = Me.Tag.ToString().Split(",")

    ' forbid the dialog to be smaller than the original value
    If Me.Width < Convert.ToInt16(tmp(0)) Or _
       Me.Height < Convert.ToInt16(tmp(1)) Then

      Me.Size = _
        New Size(Convert.ToInt16(tmp(0)), Convert.ToInt16(tmp(1)))
      Exit Sub

    End If

    'use the newer values
    Dim changeX As Integer = _
      Me.Width - Convert.ToInt16(tmp(0))
    Dim changeY As Integer = _
      Me.Height - Convert.ToInt16(tmp(1))

    Dim oCtrl As Control
    For Each oCtrl In Me.Controls
      tmp = oCtrl.Tag.ToString().Split(",")
      oCtrl.Width = Convert.ToInt16(tmp(2))

      If oCtrl.Name = "GroupBoxSelectOp" Then
        oCtrl.Height = Convert.ToInt16(tmp(3)) + CInt(changeY / 2)
      End If

      If oCtrl.Name = "GroupBoxSettings" Then
        oCtrl.Top = Convert.ToInt16(tmp(1)) + CInt(changeY / 2)
        oCtrl.Height = Convert.ToInt16(tmp(3)) + CInt(changeY / 2)
      End If

      If oCtrl.Name = "GroupBoxOutput" Then
        oCtrl.Top = Convert.ToInt16(tmp(1)) + CInt(changeY)
        oCtrl.Height = Convert.ToInt16(tmp(3))
      End If

      If oCtrl.Name = "PictureBox1" Then
        oCtrl.Left = Convert.ToInt16(tmp(0))
        oCtrl.Top = Convert.ToInt16(tmp(1))
        oCtrl.Width = Convert.ToInt16(tmp(2)) + changeX
        oCtrl.Height = Convert.ToInt16(tmp(3)) + changeY
      End If

      If oCtrl.Name = "OK" Then
        oCtrl.Left = Convert.ToInt16(tmp(0))
        oCtrl.Top = Convert.ToInt16(tmp(1)) + changeY
      End If

      If oCtrl.Name = "Cancel" Then
        oCtrl.Left = Convert.ToInt16(tmp(0)) + changeX
        oCtrl.Top = Convert.ToInt16(tmp(1)) + changeY
      End If
    Next
  End Sub

  'resize the sub-control in the group box
  Private Sub GroupBoxSettings_SizeChanged( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) _
    Handles GroupBoxSettings.SizeChanged

    If GroupBoxSettings.Tag Is Nothing Then Exit Sub

    Dim tmp As String() = New String(3) {}
    tmp = GroupBoxSettings.Tag.ToString().Split(",")

    If GroupBoxSettings.Width < Convert.ToInt16(tmp(2)) Or _
       GroupBoxSettings.Height < Convert.ToInt16(tmp(3)) Then

      GroupBoxSettings.Size = _
        New Size(Convert.ToInt16(tmp(2)), Convert.ToInt16(tmp(3)))
      Exit Sub
    End If

    Dim changeX As Integer = _
      GroupBoxSettings.Width - Convert.ToInt16(tmp(2))
    Dim changeY As Integer = _
      GroupBoxSettings.Height - Convert.ToInt16(tmp(3))

    Dim oCtrl As Control
    For Each oCtrl In GroupBoxSettings.Controls

      tmp = oCtrl.Tag.ToString().Split(",")
      oCtrl.Left = Convert.ToInt16(tmp(0))

      If oCtrl.Name = "Label1" Or oCtrl.Name = "ComboBoxBG" Then
        oCtrl.Top = Convert.ToInt16(tmp(1)) + CInt(changeY / 4)
      End If

      If oCtrl.Name = "Label2" Or oCtrl.Name = "ComboBoxFG" Then
        oCtrl.Top = Convert.ToInt16(tmp(1)) + CInt(changeY * 2 / 4)
      End If

      If oCtrl.Name = "Label3" Or oCtrl.Name = "ComboBoxGray" Then
        oCtrl.Top = Convert.ToInt16(tmp(1)) + CInt(changeY * 3 / 4)
      End If
    Next
  End Sub

  'resize the sub-control in the group box
  Private Sub GroupBoxSelectOp_SizeChanged( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) _
    Handles GroupBoxSelectOp.SizeChanged

    If GroupBoxSelectOp.Tag Is Nothing Then Exit Sub

    Dim tmp As String() = New String(3) {}
    tmp = GroupBoxSelectOp.Tag.ToString().Split(",")

    If GroupBoxSelectOp.Width < Convert.ToInt16(tmp(2)) Or _
      GroupBoxSelectOp.Height < Convert.ToInt16(tmp(3)) Then

      GroupBoxSelectOp.Size = _
        New Size(Convert.ToInt16(tmp(2)), Convert.ToInt16(tmp(3)))
      Exit Sub
    End If

    Dim changeX = GroupBoxSelectOp.Width - Convert.ToInt16(tmp(2))
    Dim changeY = GroupBoxSelectOp.Height - Convert.ToInt16(tmp(3))

    Dim oCtrl As Control
    For Each oCtrl In GroupBoxSelectOp.Controls

      tmp = oCtrl.Tag.ToString().Split(",")
      oCtrl.Left = Convert.ToInt16(tmp(0))

      If oCtrl.Name = "RadioApplication" Then
        oCtrl.Top = Convert.ToInt16(tmp(1)) + CInt(changeY / 4)
      End If

      If oCtrl.Name = "RadioDocument" Then
        oCtrl.Top = Convert.ToInt16(tmp(1)) + CInt(changeY * 2 / 4)
      End If

      If oCtrl.Name = "RadioWindow" Or oCtrl.Name = "ButtonWindow" Then
        oCtrl.Top = Convert.ToInt16(tmp(1)) + CInt(changeY * 3 / 4)
      End If

    Next
  End Sub

#End Region ' Form

#Region "Bitmap"

  'update the bitmap according to select mode and settings
  Private Sub updateBitmap()

    If m_inventorApplication Is Nothing Then
      Exit Sub
    End If

    'prepare the size and corner point
    Select Case oSelectOption
      Case SelectOptionsEnum.eApplication
        'point and size are always same for application
        oCornerPt = oSelectParam_App.oCornerPt
        oSize = oSelectParam_App.oSize
      Case SelectOptionsEnum.eDocument
        'point and size are always same for document
        oCornerPt = oSelectParam_Doc.oCornerPt
        oSize = oSelectParam_Doc.oSize
      Case SelectOptionsEnum.eWindow
        If Not (oHasSelectWindow) Then
          'need to select by user. so set preview to warning
          PictureBox1.Image = GetWarningBitmap()
          Exit Sub
        Else
          oCornerPt = oSelectParam_Win.oCornerPt
          oSize = oSelectParam_Win.oSize
        End If
    End Select

    Dim oBgColor As System.Drawing.Color = _
      System.Drawing.Color.Empty

    'first priority is background color
    Select Case ComboBoxBG.SelectedIndex
      Case -1
        oResultBitmap = createRawBitmap(oSize, oCornerPt)
        If m_inventorApplication.ActiveDocument Is Nothing Then
          Exit Sub
        End If

        'normal background color 
        Dim oInvBGColor As Inventor.Color
        If TypeOf (m_inventorApplication.ActiveDocument) Is DrawingDocument Then
          Dim oDrawDoc As DrawingDocument = _
            m_inventorApplication.ActiveDocument
          oInvBGColor = oDrawDoc.SheetSettings.SheetColor
        Else
          oInvBGColor = m_inventorApplication.ActiveColorScheme.ScreenColor
        End If
        oBgColor = _
          System.Drawing.Color.FromArgb( _
            oInvBGColor.Red, _
            oInvBGColor.Green, _
            oInvBGColor.Blue)

      Case 0    ' normal          

        oResultBitmap = createRawBitmap(oSize, oCornerPt)
        If m_inventorApplication.ActiveDocument Is Nothing Then
          Exit Sub
        End If

        'normal background color 
        Dim oInvBGColor As Inventor.Color
        If TypeOf (m_inventorApplication.ActiveDocument) Is DrawingDocument Then
          Dim oDrawDoc As DrawingDocument = _
            m_inventorApplication.ActiveDocument
          oInvBGColor = oDrawDoc.SheetSettings.SheetColor
        Else
          oInvBGColor = m_inventorApplication.ActiveColorScheme.ScreenColor
        End If
        oBgColor = _
          System.Drawing.Color.FromArgb( _
            oInvBGColor.Red, _
            oInvBGColor.Green, _
            oInvBGColor.Blue)

      Case 1 'white Background
        oResultBitmap = _
          getBmpByForceBGColor(Drawing.Color.White)
        oBgColor = Drawing.Color.White
    End Select

    'second priority is forecolor or grayscale

    Select Case ComboBoxFG.SelectedIndex
      Case 0
        Select Case ComboBoxGray.SelectedIndex
          Case 0
            'do nothing
          Case 1 'gray
            oResultBitmap = _
              ConvertToGrayscale( _
                oResultBitmap, _
                oBgColor, _
                False, _
                Drawing.Color.Empty)
        End Select
      Case 1 'black
        oResultBitmap = _
          ConvertToGrayscale( _
            oResultBitmap, _
            oBgColor, _
            True, _
            Drawing.Color.Black)
    End Select


    'set image to picturebox
    If Not oResultBitmap Is Nothing Then
      PictureBox1.Image = oResultBitmap

      'store the bitmaps for the current mode
      Select Case oSelectOption
        Case SelectOptionsEnum.eApplication
          oBitmap_App = oResultBitmap
        Case SelectOptionsEnum.eDocument
          oBitmap_Doc = oResultBitmap
        Case SelectOptionsEnum.eWindow
          oBitmap_Win = oResultBitmap
      End Select
    End If

  End Sub

  'create raw bitmap (before conversion)
  Private Function createRawBitmap( _
    ByVal bmSize As Size, _
    ByVal bmPt As System.Drawing.Point) As Bitmap

    Dim bmp As Bitmap = _
      New Bitmap(bmSize.Width, bmSize.Height, PixelFormat.Format32bppArgb)
    Dim gfx As Graphics = _
      Graphics.FromImage(bmp)
    gfx.CopyFromScreen( _
      bmPt.X, _
      bmPt.Y, _
      0, _
      0, _
      bmSize, _
      CopyPixelOperation.SourceCopy)

    createRawBitmap = bmp

  End Function

  'get bitmap with force background
  Private Function getBmpByForceBGColor( _
    ByVal forceColor As System.Drawing.Color) As Bitmap

    If m_inventorApplication.ActiveDocument Is Nothing Then
      getBmpByForceBGColor = Nothing
      Exit Function
    End If

    Dim oDoc As Document = m_inventorApplication.ActiveDocument

    'in drawing document, change the sheet color and snapshot
    If TypeOf (oDoc) Is DrawingDocument Then
      'change back color and snapshot, and set the color to the original value

      Dim oDrawDoc As DrawingDocument = oDoc
      Dim oSheetOldColor As Inventor.Color = _
        oDrawDoc.SheetSettings.SheetColor

      Dim oInvForceColor As Inventor.Color = _
        m_inventorApplication.TransientObjects.CreateColor( _
          forceColor.R, _
          forceColor.G, _
          forceColor.B)

      'change color and snapshot the bitmap
      oDrawDoc.SheetSettings.SheetColor = oInvForceColor
      m_inventorApplication.ActiveView.Update()
      getBmpByForceBGColor = createRawBitmap(oSize, oCornerPt)
      'set color back
      oDrawDoc.SheetSettings.SheetColor = oSheetOldColor

    Else 'other type of document, set color info of color skema

      Dim oOldBGType As Inventor.BackgroundTypeEnum = _
        m_inventorApplication.ColorSchemes.BackgroundType
      Dim oOldBGImage As String = ""
      If oOldBGType = BackgroundTypeEnum.kImageBackgroundType Then
        oOldBGImage = _
          m_inventorApplication.ActiveColorScheme.ImageFullFileName
      End If

      'change background color
      m_inventorApplication.ColorSchemes.BackgroundType = _
        BackgroundTypeEnum.kImageBackgroundType

      If forceColor = Drawing.Color.White Then
        m_inventorApplication.ActiveColorScheme.ImageFullFileName = _
          System.IO.Path.GetDirectoryName( _
            Me.GetType().Assembly.Location) + "\Resources\white.bmp"
      End If

      m_inventorApplication.ActiveView.Update()
      getBmpByForceBGColor = _
        createRawBitmap(oSize, oCornerPt)

      'set the color back
      m_inventorApplication.ColorSchemes.BackgroundType = oOldBGType
      If oOldBGType = BackgroundTypeEnum.kImageBackgroundType Then
        m_inventorApplication.ActiveColorScheme.ImageFullFileName = _
          oOldBGImage
      End If

    End If
  End Function

  'gray conversion
  Public Function ConvertToGrayscale( _
    ByVal source As Bitmap, _
    ByVal bgColor As System.Drawing.Color, _
    ByVal forceFgColor As Boolean, _
    ByVal fgcolor As System.Drawing.Color) As Bitmap

    ' From http://www.bobpowell.net/grayscale.htm 

    Me.Enabled = False

    Dim bm As New Bitmap(source.Width, source.Height)
    Dim x As Integer
    Dim y As Integer
    Dim c As System.Drawing.Color


    Dim oProgress As Long
    oProgress = 0

    Dim oProgressForm As ProgressForm = New ProgressForm
    oProgressForm.Show()

    Try
      For y = 0 To bm.Height - 1
        For x = 0 To bm.Width - 1

          c = source.GetPixel(x, y)

          Dim lum As Integer
          If forceFgColor Then
            If Not (SameColors(c, bgColor)) Then
              bm.SetPixel(x, y, fgcolor)
            Else
              'set the original color  
              bm.SetPixel(x, y, c)
            End If
          Else
            lum = CInt(c.R * 0.3 + c.G * 0.59 + c.B * 0.11)
            bm.SetPixel(x, y, System.Drawing.Color.FromArgb(lum, lum, lum))
          End If

          'update progress bar
          oProgress += 1
          oProgressForm.setProgress(oProgress, bm.Height * bm.Width)
        Next
      Next

    Catch ex As Exception
    End Try

    oProgressForm.Dispose()
    oProgressForm = Nothing

    Me.Enabled = True

    Return bm

  End Function

  'compare if the same color
  Private Function SameColors( _
    ByVal a As System.Drawing.Color, _
    ByVal b As System.Drawing.Color) As Boolean

    If a.R = b.R And a.B = b.B And a.G = b.G Then
      SameColors = True
    Else
      SameColors = False
    End If

  End Function

#End Region  'bitmap

#Region "other"

  'do select in screen
  Public Sub DoSelect()

    Select Case oSelectOption
      Case SelectOptionsEnum.eApplication

        If m_inventorApplication.SoftwareVersion.Major > 13 Then
          m_inventorApplication.UserInterfaceManager.DoEvents()
        Else
          System.Windows.Forms.Application.DoEvents()
        End If

      Case SelectOptionsEnum.eDocument

        If m_inventorApplication.SoftwareVersion.Major > 13 Then
          m_inventorApplication.UserInterfaceManager.DoEvents()
        Else
          System.Windows.Forms.Application.DoEvents()
        End If

      Case SelectOptionsEnum.eWindow

        If selectingWin Then

          selectingWin = False ' ready for next status

          'get new corner and size of window
          Dim oInterEventsM As InteractionEventsManager = _
            New InteractionEventsManager(m_inventorApplication)

          Dim otempSize As Size = New Size(0, 0)
          Dim otempInvPoint2d As Inventor.Point2d = _
            m_inventorApplication.TransientGeometry.CreatePoint2d(0, 0)

          oInterEventsM.DoSelectRegion(otempSize, otempInvPoint2d)

          'record the size and corner point.
          If otempSize.Height = 0 Or otempSize.Width = 0 Then
            ' the user may escape the selecting without selecting anything
            ' so do nothing 
            Exit Sub
          Else
            oHasSelectWindow = True
            oSelectParam_Win.oCornerPt.X = otempInvPoint2d.X
            oSelectParam_Win.oCornerPt.Y = otempInvPoint2d.Y
            oSelectParam_Win.oSize = otempSize
          End If
        Else
          'just change selection mode. so update the bitmap directly
          If m_inventorApplication.SoftwareVersion.Major > 13 Then
            m_inventorApplication.UserInterfaceManager.DoEvents()
          Else
            System.Windows.Forms.Application.DoEvents()
          End If
        End If

    End Select

    updateBitmap()

  End Sub

  'get file format
  Private Function GetFormatForFile( _
    ByVal filename As String) As ImageFormat

    ' If all else fails, let's create a PNG 
    ' (might also choose to throw an exception) 

    Dim imf As ImageFormat = ImageFormat.Png
    If filename.Contains(".") Then
      ' Get the filename's extension (what follows the last ".") 

      Dim ext As String = _
        filename.Substring(filename.LastIndexOf(".") + 1)

      ' Get the first three characters of the extension 

      If ext.Length > 3 Then
        ext = ext.Substring(0, 3)
      End If

      ' Choose the format based on the extension (in lowercase) 

      Select Case ext.ToLower()
        Case "bmp"
          imf = ImageFormat.Bmp
          Exit Select
        Case "gif"
          imf = ImageFormat.Gif
          Exit Select
        Case "jpg"
          imf = ImageFormat.Jpeg
          Exit Select
        Case "tif"
          imf = ImageFormat.Tiff
          Exit Select
        Case "wmf"
          imf = ImageFormat.Wmf
          Exit Select
        Case Else
          imf = ImageFormat.Png
          Exit Select
      End Select
    End If
    Return imf
  End Function

  'prepare the bitmap before loading the dialog.
  'Since the document may have been modified,
  'the bitmap should reflect this change
  Public Sub prepareBitmapsBeforeLoadDialog()

    ' the dialog is re-opened, ready to re-generate when changing mode next time
    If oSelectOption <> SelectOptionsEnum.eApplication Then
      oBitmap_App = Nothing
    End If
    If oSelectOption <> SelectOptionsEnum.eDocument Then
      oBitmap_Doc = Nothing
    End If
    If oSelectOption <> SelectOptionsEnum.eWindow And oHasSelectWindow Then
      oBitmap_Win = Nothing
    End If

    'just re-generate the bitmap for current mode whendialog is re-opened
    updateBitmap()

  End Sub

#End Region

End Class