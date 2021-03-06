﻿Imports Inventor
Imports System.Drawing

Public Class InteractionEventsManager

  Dim m_inventorApplication As Inventor.Application

  Dim oCornerPt As Point2d
  Dim oSize As Size

  'Interaction Events
  Private WithEvents m_InteractionEvents As InteractionEvents

  'Mouse event
  Private WithEvents m_MouseEvents As MouseEvents

  'Pick event for selecting object
  Dim WithEvents m_SelectEvents As SelectEvents

  'flag to indicate the mouse is down
  Private m_flagMouseDown As Boolean

  ' start point,  for screenshot
    Private m_MouseStartViewPt As Inventor.Point2d

    'start point, for temporary selecting rectangle
    Private m_StartModelPt As Inventor.Point

    'flag to indicate screenshot is running 
    Dim isRunningScreenshot As Boolean

    'graphics node
    Dim oGiNode As GraphicsNode
    'coordinate set for graphics node
    Dim oCoordSet As GraphicsCoordinateSet
    'color set for graphics node
    Dim oColorSet As GraphicsColorSet
    'Line strip Graphics
    Dim oGiLineStripG As LineStripGraphics
    'Line  Graphics (for Inventor 2009 only)
    Dim oGiLineG As LineGraphics



    Public Sub New(ByVal oApp As Inventor.Application)
        m_inventorApplication = oApp
        oCornerPt = _
        m_inventorApplication.TransientGeometry.CreatePoint2d(0, 0)
        oSize = New Size()
    End Sub

    Public Sub DoSelectRegion( _
      ByRef bmSize As Size, _
      ByRef bmCornetPt As Inventor.Point2d)

        StartEvent(True)
        bmSize = oSize
        bmCornetPt = oCornerPt
    End Sub

    Public Sub DoSelectObject( _
      ByRef bmSize As Size, _
      ByRef bmCornetPt As Inventor.Point2d)

        StartEvent(False)
        bmSize = oSize
        bmCornetPt = oCornerPt
    End Sub

    Private Sub StartEvent(ByVal region_or_object As Boolean)

        'start interaction event
        If m_InteractionEvents Is Nothing Then
            m_InteractionEvents = _
              m_inventorApplication.CommandManager.CreateInteractionEvents()
        Else
            m_InteractionEvents.Stop()
        End If

        m_InteractionEvents.InteractionDisabled = False

        If region_or_object Then
            'get mouse event
            If m_MouseEvents Is Nothing Then
                m_MouseEvents = m_InteractionEvents.MouseEvents
                m_MouseEvents.MouseMoveEnabled = True
                m_MouseStartViewPt = _
                  m_inventorApplication.TransientGeometry.CreatePoint2d(0, 0)
                m_flagMouseDown = False
            End If
        Else
            'get select event
            If m_SelectEvents Is Nothing Then
                m_SelectEvents = m_InteractionEvents.SelectEvents
                m_SelectEvents.SingleSelectEnabled = False
                m_SelectEvents.WindowSelectEnabled = True
            End If
        End If

        m_InteractionEvents.Name = "MyScreenshot"

        'start
        m_InteractionEvents.Start()

        Do While Not m_InteractionEvents Is Nothing
            If m_inventorApplication.SoftwareVersion.Major > 13 Then
                m_inventorApplication.UserInterfaceManager.DoEvents()
            Else
                System.Windows.Forms.Application.DoEvents()
            End If
        Loop
    End Sub

    Private Sub m_InteractionEvents_OnTerminate() _
      Handles m_InteractionEvents.OnTerminate

        m_InteractionEvents.InteractionGraphics.PreviewClientGraphics.Delete()
        m_inventorApplication.ActiveView.Update()
        m_flagMouseDown = False
        m_InteractionEvents.Stop()
        m_MouseEvents = Nothing
        m_SelectEvents = Nothing
        m_InteractionEvents = Nothing
    End Sub

    Private Sub m_MouseEvents_OnMouseDown( _
      ByVal Button As Inventor.MouseButtonEnum, _
      ByVal ShiftKeys As Inventor.ShiftStateEnum, _
      ByVal ModelPosition As Inventor.Point, _
      ByVal ViewPosition As Inventor.Point2d, _
      ByVal View As Inventor.View) Handles m_MouseEvents.OnMouseDown

        'if the interaction event is MyScreenshot,
        'then get the view position and model position

        If m_InteractionEvents.Name = "MyScreenshot" Then
            m_MouseStartViewPt = ViewPosition
            m_StartModelPt = ModelPosition
            m_flagMouseDown = True

            'clean the last graphics
            m_InteractionEvents.InteractionGraphics.PreviewClientGraphics.Delete()
            m_inventorApplication.ActiveView.Update()

            'gi node
            oGiNode = m_InteractionEvents.InteractionGraphics.PreviewClientGraphics.AddNode(1)
            oCoordSet = m_InteractionEvents.InteractionGraphics.GraphicsDataSets.CreateCoordinateSet(1)

            'color set
            oColorSet = m_InteractionEvents.InteractionGraphics.GraphicsDataSets.CreateColorSet(1)
            Call oColorSet.Add(1, 255, 0, 0)

            Dim tg As TransientGeometry = m_inventorApplication.TransientGeometry
            Dim tempP As Inventor.Point = tg.CreatePoint(ViewPosition.X, ViewPosition.Y, 0)

            oCoordSet.Add(1, tempP)
            oCoordSet.Add(2, tempP)
            oCoordSet.Add(3, tempP)
            oCoordSet.Add(4, tempP)
            oCoordSet.Add(5, tempP)

            Try
                If Not oGiLineStripG Is Nothing Then
                    oGiLineStripG.Delete()
                    oGiLineStripG = Nothing
                End If
                oGiLineStripG = oGiNode.AddLineStripGraphics()
                oGiLineStripG.CoordinateSet = oCoordSet
                oGiLineStripG.ColorSet = oColorSet
                oGiLineStripG.BurnThrough = True
            Catch ex As Exception
                'a problem in Inventor 2009( R13 ) with 
                'LineStripGraphics.BurnThrough. Use LineGraphics as workaround

                If Not oGiLineG Is Nothing Then
                    oGiLineG.Delete()
                    oGiLineG = Nothing
                End If

                oGiLineG = oGiNode.AddLineGraphics()
                oGiLineG.CoordinateSet = oCoordSet
                oGiLineG.ColorSet = oColorSet
                oGiLineG.BurnThrough = True
            End Try


        End If
    End Sub

    ' version 2010-05-23: to solve the issue in Perspective View
    Private Sub m_MouseEvents_OnMouseMove( _
      ByVal Button As Inventor.MouseButtonEnum, _
      ByVal ShiftKeys As Inventor.ShiftStateEnum, _
      ByVal ModelPosition As Inventor.Point, _
      ByVal ViewPosition As Inventor.Point2d, _
      ByVal View As Inventor.View) Handles m_MouseEvents.OnMouseMove

        'if the interaction event is MyScreenshot, draw selecting rectangle.
        If m_InteractionEvents.Name = "MyScreenshot" And m_flagMouseDown Then

            Dim tg As TransientGeometry = m_inventorApplication.TransientGeometry

            Dim P1 As Inventor.Point = tg.CreatePoint(m_MouseStartViewPt.X, -m_MouseStartViewPt.Y, 0)
            Dim P3 As Inventor.Point = tg.CreatePoint(ViewPosition.X, -ViewPosition.Y, 0)
            Dim P4 As Inventor.Point = tg.CreatePoint(P1.X, P3.Y, 0)
            Dim P2 As Inventor.Point = tg.CreatePoint(P3.X, P1.Y, 0)

            'update coordinates

            oCoordSet(1) = P1
            oCoordSet(2) = P2
            oCoordSet(3) = P3
            oCoordSet(4) = P4
            oCoordSet(5) = P1

            'add line strip 
            If Not oGiLineStripG Is Nothing Then
                'SetTransformBehavior, default value for PixelScale is 1
                oGiLineStripG.SetTransformBehavior(P1, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling, 1)
                oGiLineStripG.SetViewSpaceAnchor(P1, m_MouseStartViewPt, ViewLayoutEnum.kTopLeftViewCorner)
            ElseIf Not oGiLineG Is Nothing Then
                'SetTransformBehavior, default value for PixelScale is 1
                oGiLineG.SetTransformBehavior(P1, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling, 1)
                oGiLineG.SetViewSpaceAnchor(P1, m_MouseStartViewPt, ViewLayoutEnum.kTopLeftViewCorner)
            End If


            m_inventorApplication.ActiveView.Update()
 
        End If
    End Sub


    Private Sub m_MouseEvents_OnMouseUp( _
      ByVal Button As Inventor.MouseButtonEnum, _
      ByVal ShiftKeys As Inventor.ShiftStateEnum, _
      ByVal ModelPosition As Inventor.Point, _
      ByVal ViewPosition As Inventor.Point2d, _
      ByVal View As Inventor.View) Handles m_MouseEvents.OnMouseUp

        'if the interaction event is MyScreenshot, create the image
        If m_InteractionEvents.Name = "MyScreenshot" Then

            m_InteractionEvents.InteractionGraphics.PreviewClientGraphics.Delete()
            m_inventorApplication.ActiveView.Update()

            If Not oGiLineStripG Is Nothing Then
                oGiLineStripG.Delete()
                oGiLineStripG = Nothing
            End If

            If Not oGiLineG Is Nothing Then
                oGiLineG.Delete()
                oGiLineG = Nothing
            End If

            'stop interaction event
            m_InteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow)
            m_flagMouseDown = False
            m_InteractionEvents.Stop()
            m_MouseEvents = Nothing
            m_InteractionEvents = Nothing

            'prepare size of the image region, in pixel

            oSize = _
              New Size( _
                Math.Abs( _
                  ViewPosition.X - m_MouseStartViewPt.X), _
                  Math.Abs(ViewPosition.Y - m_MouseStartViewPt.Y))

            If ViewPosition.X > m_MouseStartViewPt.X And _
              ViewPosition.Y > m_MouseStartViewPt.Y Then

                oCornerPt = m_MouseStartViewPt
            ElseIf ViewPosition.X > m_MouseStartViewPt.X And _
              ViewPosition.Y < m_MouseStartViewPt.Y Then

                oCornerPt.X = m_MouseStartViewPt.X
                oCornerPt.Y = ViewPosition.Y
            ElseIf ViewPosition.X < m_MouseStartViewPt.X And _
              ViewPosition.Y > m_MouseStartViewPt.Y Then

                oCornerPt.X = ViewPosition.X
                oCornerPt.Y = m_MouseStartViewPt.Y
            Else
                oCornerPt = ViewPosition
            End If

            'take the view position in screen, calculate
            'the real pixel data of the corners

            oCornerPt.X = View.Left + oCornerPt.X
            oCornerPt.Y = View.Top + oCornerPt.Y
        End If

    End Sub

  Private Sub m_SelectEvents_OnSelect( _
    ByVal JustSelectedEntities As Inventor.ObjectsEnumerator, _
    ByVal SelectionDevice As Inventor.SelectionDeviceEnum, _
    ByVal ModelPosition As Inventor.Point, _
    ByVal ViewPosition As Inventor.Point2d, _
    ByVal View As Inventor.View) Handles m_SelectEvents.OnSelect

    'reserved for future
  End Sub

End Class