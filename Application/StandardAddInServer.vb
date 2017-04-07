Imports Inventor
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Namespace InventorScreenshot
  <ProgIdAttribute("InventorScreenshot.StandardAddInServer"), _
  GuidAttribute("b3aa6727-f2d0-4c6d-8150-16fa9c493dff")> _
  Public Class StandardAddInServer
    Implements Inventor.ApplicationAddInServer

    ' Inventor application object.
    Private m_inventorApplication As Inventor.Application

    ' GUID of the AddIn
    Private m_ClientId As String

    ' Variables for Screenshot
    'button defintion of Screenshot
    Private WithEvents oSSButtonDef As ButtonDefinition
    Dim oSSForm As ScreenshotForm

    'hard-coded delaytime becasue in some case, app.config may corrupt
    Shared m_delaytime As Integer = 500


#Region "ApplicationAddInServer Members"

    Public Sub Activate( _
      ByVal addInSiteObject As Inventor.ApplicationAddInSite, _
      ByVal firstTime As Boolean) _
      Implements Inventor.ApplicationAddInServer.Activate

      ' This method is called by Inventor when it loads the AddIn.
      ' The AddInSiteObject provides access to the Inventor Application object.
      ' The FirstTime flag indicates if the AddIn is loaded for the first time.

      ' Initialize AddIn members.
      m_inventorApplication = addInSiteObject.Application

      m_ClientId = "{b3aa6727-f2d0-4c6d-8150-16fa9c493dff}"

      'icons for buttons
      Dim largeIconSize As Integer

      If IsRibbonUI() Then  'Ribbon UI
        largeIconSize = 32
      Else   'classic UI
        largeIconSize = 24
      End If

      Dim controlDefs As ControlDefinitions = _
        m_inventorApplication.CommandManager.ControlDefinitions
      Dim smallPicture1 As stdole.IPictureDisp = _
        Microsoft.VisualBasic.Compatibility.VB6.IconToIPicture( _
          New System.Drawing.Icon(My.Resources.CMButton, 16, 16))
      Dim largePicture1 As stdole.IPictureDisp = _
        Microsoft.VisualBasic.Compatibility.VB6.IconToIPicture( _
          New System.Drawing.Icon( _
            My.Resources.CMButton, _
            largeIconSize, _
            largeIconSize))

      'Screenshot button
      oSSButtonDef = _
        controlDefs.AddButtonDefinition( _
          "Screenshot", "InventorScreenshot:SSDef", _
          CommandTypesEnum.kNonShapeEditCmdType, m_ClientId, , "Screenshot", _
          smallPicture1, largePicture1)

      'add the button to drawing ribbon

      If IsRibbonUI() Then  'add Ribbon UI
        AddRibbonUI()
      Else  'Add Classic UI

        AddClassicUI()
      End If

    End Sub

    Public Sub Deactivate() _
      Implements Inventor.ApplicationAddInServer.Deactivate

      ' This method is called by Inventor when the AddIn is unloaded.
      ' The AddIn will be unloaded either manually by the user or
      ' when the Inventor session is terminated.

      If Not oSSForm Is Nothing Then
        oSSForm.Dispose()
        oSSForm = Nothing
      End If

      ' Release objects.
      Marshal.ReleaseComObject(m_inventorApplication)
      m_inventorApplication = Nothing

      System.GC.WaitForPendingFinalizers()
      System.GC.Collect()

    End Sub

    Public ReadOnly Property Automation() As Object _
      Implements Inventor.ApplicationAddInServer.Automation

      ' This property is provided to allow the AddIn to expose an API 
      ' of its own to other programs. Typically, this  would be done by
      ' implementing the AddIn's API interface in a class and returning 
      ' that class object through this property.

      Get
        Return Nothing
      End Get

    End Property

    Public Sub ExecuteCommand( _
      ByVal commandID As Integer) _
      Implements Inventor.ApplicationAddInServer.ExecuteCommand

      ' Note:this method is now obsolete, you should use the 
      ' ControlDefinition functionality for implementing commands.

    End Sub

#End Region

#Region "COM Registration"

    ' Registers this class as an AddIn for Inventor.
    ' This function is called when the assembly is registered for COM.
    <ComRegisterFunctionAttribute()> _
    Public Shared Sub Register(ByVal t As Type)

      Dim clssRoot As RegistryKey = Registry.ClassesRoot
      Dim clsid As RegistryKey = Nothing
      Dim subKey As RegistryKey = Nothing

      Try
        clsid = clssRoot.CreateSubKey("CLSID\" + AddInGuid(t))
        clsid.SetValue(Nothing, "InventorScreenshot")
        subKey = _
          clsid.CreateSubKey( _
            "Implemented Categories\{39AD2B5C-7A29-11D6-8E0A-0010B541CAA8}")
        subKey.Close()

        subKey = clsid.CreateSubKey("Settings")
        subKey.SetValue("AddInType", "Standard")
        subKey.SetValue("LoadOnStartUp", "1")

        subKey.SetValue("SupportedSoftwareVersionGreaterThan", "11..")

        subKey.SetValue("Version", 1)
        subKey.Close()

        subKey = clsid.CreateSubKey("Description")
        subKey.SetValue(Nothing, "InventorScreenshot")

      Catch ex As Exception
        System.Diagnostics.Trace.Assert(False)
      Finally
        If Not subKey Is Nothing Then subKey.Close()
        If Not clsid Is Nothing Then clsid.Close()
        If Not clssRoot Is Nothing Then clssRoot.Close()
      End Try

    End Sub

    ' Unregisters this class as an AddIn for Inventor.
    ' This function is called when the assembly is unregistered.
    <ComUnregisterFunctionAttribute()> _
    Public Shared Sub Unregister(ByVal t As Type)

      Dim clssRoot As RegistryKey = Registry.ClassesRoot
      Dim clsid As RegistryKey = Nothing

      Try
        clssRoot = Microsoft.Win32.Registry.ClassesRoot
        clsid = clssRoot.OpenSubKey("CLSID\" + AddInGuid(t), True)
        clsid.SetValue(Nothing, "")
        clsid.DeleteSubKeyTree( _
          "Implemented Categories\{39AD2B5C-7A29-11D6-8E0A-0010B541CAA8}")
        clsid.DeleteSubKeyTree("Settings")
        clsid.DeleteSubKeyTree("Description")
      Catch
      Finally
        If Not clsid Is Nothing Then clsid.Close()
        If Not clssRoot Is Nothing Then clssRoot.Close()
      End Try

    End Sub

    ' This property uses reflection to get the value for the GuidAttribute
    ' attached to the class.
    Public Shared ReadOnly Property AddInGuid(ByVal t As Type) As String
      Get
        Dim guid As String = ""
        Try
          Dim customAttributes() As Object = _
            t.GetCustomAttributes(GetType(GuidAttribute), False)
          Dim guidAttribute As GuidAttribute = _
            CType(customAttributes(0), GuidAttribute)
          guid = "{" + guidAttribute.Value.ToString() + "}"
        Finally
          AddInGuid = guid
        End Try
      End Get
    End Property

#End Region

    Private Sub oSSButtonDef_OnExecute( _
      ByVal Context As Inventor.NameValueMap) _
      Handles oSSButtonDef.OnExecute

      If oSSForm Is Nothing Then
        oSSForm = New ScreenshotForm(m_inventorApplication)
        oSSForm.Left = _
          m_inventorApplication.Left + _
            (m_inventorApplication.width - oSSForm.Width) / 2.0
        oSSForm.Top = _
          m_inventorApplication.Top + _
            (m_inventorApplication.height - oSSForm.Height) / 2.0

      End If
      oSSForm.prepareBitmapsBeforeLoadDialog()

      Do
        oSSForm.bTakeSnapShot = False
        oSSForm.ShowDialog()

        If oSSForm.bTakeSnapShot Then

          'delay for specific machine. adjust the value in app.config.
          Try
            System.Threading.Thread.Sleep(My.Settings.delayTime)
          Catch ex As Exception
            System.Threading.Thread.Sleep(m_delaytime)
          End Try

          oSSForm.DoSelect()
        End If
      Loop Until Not (oSSForm.bTakeSnapShot)

    End Sub

    Private Function IsRibbonUI() As Boolean

      ' Ribbon starts from R14 (2010)
      If m_inventorApplication.SoftwareVersion.Major > 13 Then
        Dim oTypeObj As Object = _
          m_inventorApplication.UserInterfaceManager.InterfaceStyle

        If Not oTypeObj Is Nothing Then

          If oTypeObj = 87809 Then 'ribbon
            IsRibbonUI = True
          Else   'you can change to classic UI (by product)
            IsRibbonUI = False
          End If

        Else  'Classic UI
          IsRibbonUI = False
        End If
      End If

    End Function

    Private Sub AddRibbonUI()
      Dim ribNames() As String = _
        {"Drawing", "Part", "Assembly", "Presentation"}

      For Each ribName In ribNames
        Dim oRibbon As Object = _
          m_inventorApplication.UserInterfaceManager.Ribbons(ribName)
        Dim oTab As Object = oRibbon.RibbonTabs("id_TabTools")
        Dim oScreenShotPanel As Object

        Try
          oScreenShotPanel = _
            oTab.RibbonPanels("InventorScreenshot:RibbonPanel")
        Catch ex As Exception
          oScreenShotPanel = _
            oTab.RibbonPanels.Add( _
              "Screenshot", "InventorScreenshot:RibbonPanel", m_ClientId)
        End Try
        oScreenShotPanel.CommandControls.AddButton(oSSButtonDef, True)
      Next
    End Sub

    Private Sub AddClassicUI()

      Dim classicUIToolsMenuNames() As String = _
        {"PartToolsMenu", "AssemblyToolsMenu", "DrawingMangerToolsMenu", _
         "PresentationToolsMenu"}

      For Each classicUIToolsMenuName In classicUIToolsMenuNames

        Try
          Dim oToolsMenu As Object = _
          m_inventorApplication.UserInterfaceManager.CommandBars( _
            classicUIToolsMenuName)

          If Not oToolsMenu Is Nothing Then
            'delete if exists
            Dim oCommandBarCtrl As Object
            For Each oCommandBarCtrl In oToolsMenu.Controls
              Dim oCtrolDef As Object
              oCtrolDef = oCommandBarCtrl.ControlDefinition
              If Not oCtrolDef Is Nothing Then
                If oCtrolDef.InternalName = "InventorScreenshot:SSDef" Then
                  oCommandBarCtrl.Delete()
                End If
              End If
            Next

            'add the button
            oToolsMenu.Controls.AddButton(oSSButtonDef)
          End If

        Catch ex As Exception
        End Try
      Next
    End Sub
  End Class

End Namespace

