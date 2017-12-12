'<------------------>
'Author: WithCreative (JordanBelford, mRobin)
'Created: 12.12.2017
'Category: Game Hacking
'Description: Wolfteam Cold Hack D3D9 Menu UI
'<------------------>

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Reflection
Imports System.ComponentModel

Public Enum MouseState As Byte
    None = 0
    Hover = 1
    Down = 2
    Block = 3
End Enum
Module Functions
    Function CenterText(Text As String, Font As Font, Width As Integer, Height As Integer) As Point
        Dim W As Integer = (Width / 2) - (TextRenderer.MeasureText(Text, Font).Width / 2)
        Dim H As Integer = (Height / 2) - (TextRenderer.MeasureText(Text, Font).Height / 2)
        Dim P As New Point(W, H)
        Return P
    End Function
    Function LeftText(Text As String, Font As Font, Width As Integer, Height As Integer) As Point
        Dim W As Integer = 3
        Dim H As Integer = (Height / 2) - (TextRenderer.MeasureText(Text, Font).Height / 2)
        Dim P As New Point(W, H - 2)
        Return P
    End Function
    Function DrawGradient(Width As Integer, Height As Integer, Optional State As MouseState = MouseState.None) As LinearGradientBrush
        Dim myBrush As LinearGradientBrush
        If State = MouseState.None Then
            myBrush = New LinearGradientBrush(New Point(0, Height), New Point(Width, Height), Color.FromArgb(255, 202, 18), Color.FromArgb(210, 160, 39))
        ElseIf State = MouseState.Hover Then
            myBrush = New LinearGradientBrush(New Point(0, Height), New Point(Width, Height), Color.FromArgb(220, 255, 202, 18), Color.FromArgb(220, 210, 160, 39))
        ElseIf State = MouseState.Down Then
            myBrush = New LinearGradientBrush(New Point(0, Height), New Point(Width, Height), Color.FromArgb(210, 160, 39), Color.FromArgb(255, 202, 18))
        Else
            myBrush = New LinearGradientBrush(New Point(0, Height), New Point(Width, Height), Color.FromArgb(255, 202, 18), Color.FromArgb(210, 160, 39))
        End If
        myBrush.RotateTransform(90)
        Return myBrush
    End Function
End Module
Class ColdForm
    Inherits ContainerControl
    Dim Down As Boolean = False
    Dim MoveHeight As Integer = 30
    Dim MousePoint As New Point(0, 0)

    Protected Overrides Sub OnCreateControl()
        ParentForm.FormBorderStyle = FormBorderStyle.None
        ParentForm.AllowTransparency = True
        ParentForm.TransparencyKey = Color.Fuchsia
        Dock = DockStyle.Fill
        Invalidate()
    End Sub
#Region "MouseStates"
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, MoveHeight).Contains(e.Location) Then
            Down = True
            MousePoint = e.Location
        End If
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        Down = False
    End Sub
    Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        If Down Then
            Parent.Location = MousePosition - MousePoint
        End If
    End Sub
#End Region
    Public mFont As New Font("Segoe UI", 10)
    Sub New()
        Me.Font = mFont
    End Sub
    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Using g = e.Graphics
            'Form Background and Topbar
            g.Clear(Color.White)
            g.FillRectangle(DrawGradient(50, Height), New Rectangle(New Point(0, 0), New Size(Width, 25)))
            g.FillRectangle(New SolidBrush(Color.FromArgb(218, 218, 218)), New Rectangle(0, 25, Width, Height - 25))
            g.DrawString(Me.Text, mFont, New SolidBrush(Color.FromArgb(0, 0, 0)), LeftText(Me.Text, mFont, Width, MoveHeight))
            'Border
            g.DrawLine(Pens.Black, New Point(Width, 25), New Point(0, 25)) 'Topbar bottom
            g.DrawLine(Pens.Black, New Point(Width, 0), New Point(0, 0)) 'Form Top
            g.DrawLine(Pens.Black, New Point(0, Height), New Point(0, 0)) 'Form Left
            g.DrawLine(Pens.Black, New Point(Width - 1, 0), New Point(Width - 1, Height)) 'Form Right
            g.DrawLine(Pens.Black, New Point(Width, Height - 1), New Point(0, Height - 1)) 'Form Bottom
        End Using
    End Sub
    Protected Overrides Sub OnSizeChanged(e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        MyBase.Refresh()
    End Sub
    Protected Overrides Sub OnEnter(e As System.EventArgs)
        MyBase.OnLocationChanged(e)
    End Sub
End Class
Class ColdButton
    Inherits Control
    Dim Down As Boolean = False
    Dim MoveHeight As Integer = 30
    Dim MousePoint As New Point(0, 0)
#Region "MouseStates"
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)

    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)

        Down = False
    End Sub
    Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)

    End Sub
#End Region
    Protected Overrides Sub OnCreateControl()
        Dock = DockStyle.None
        Invalidate()
    End Sub
    Private mFont As New Font("Segoe UI", 10)
    Sub New()
        BackColor = Color.FromArgb(51, 51, 51)
        Width = 100
        Height = 30
        Me.Font = mFont
    End Sub
    Dim isMouseEnter As Boolean = False
    Dim isMousePress As Boolean = False
    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        Using g = e.Graphics
            g.Clear(Color.White)
            If Not isMouseEnter And Not isMousePress Then
                g.FillRectangle(DrawGradient(Me.Height, Me.Width), New Rectangle(New Point(0, 0), New Size(Me.Width, Me.Height)))
            ElseIf isMousePress Then
                g.FillRectangle(DrawGradient(Me.Height, Me.Width, MouseState.Down), New Rectangle(New Point(0, 0), New Size(Me.Width, Me.Height)))
            Else
                g.FillRectangle(DrawGradient(Me.Height, Me.Width, MouseState.Hover), New Rectangle(New Point(0, 0), New Size(Me.Width, Me.Height)))
            End If
            g.DrawLine(Pens.Black, New Point(0, 0), New Point(Me.Width, 0)) 'Top
            g.DrawLine(Pens.Black, New Point(0, Me.Height), New Point(0, 0)) 'Left
            g.DrawLine(Pens.Black, New Point(Width - 1, 0), New Point(Width - 1, Me.Height)) 'Right
            g.DrawLine(Pens.Black, New Point(0, Me.Height - 1), New Point(Me.Width, Me.Height - 1)) 'Bottom
            g.DrawString(Me.Text, Me.Font, Brushes.Black, New Point(CenterText(Me.Text, Me.Font, Me.Width, Me.Height)))
        End Using
    End Sub
    Protected Overrides Sub OnMouseEnter(e As System.EventArgs)
        isMouseEnter = True
        MyBase.OnMouseEnter(e)
        Me.Refresh()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As System.EventArgs)
        isMouseEnter = False
        isMousePress = False
        MyBase.OnMouseEnter(e)
        Me.Refresh()
    End Sub
    Protected Overrides Sub OnSizeChanged(e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        MyBase.Refresh()
    End Sub

End Class
Class ColdTabPage
    Inherits TabControl
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                 ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        ItemSize = New Size(0, 30)
        Font = New Font("Verdana", 8)
    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Top
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        Dim borderPen As New Pen(Color.FromArgb(0, 0, 0))
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Color.FromArgb(218, 218, 218))
        Dim fillRect As New Rectangle(2, ItemSize.Height + 2, Width - 6, Height - ItemSize.Height - 3)
        G.FillRectangle(New SolidBrush(Color.FromArgb(218, 218, 218)), fillRect)
        G.DrawRectangle(borderPen, fillRect)
        Dim FontColor As New Color
        For i = 0 To TabCount - 1
            Dim mainRect As Rectangle = GetTabRect(i)
            mainRect.Height = mainRect.Height - 6
            mainRect.Width = mainRect.Width - 5
            If i = SelectedIndex Then
                G.FillRectangle(DrawGradient(mainRect.Height + 5, 0), mainRect)
                G.DrawRectangle(borderPen, mainRect)
                FontColor = Color.FromArgb(0, 0, 0)
            Else
                G.FillRectangle(New SolidBrush(Color.FromArgb(218, 218, 218)), mainRect)
                G.DrawRectangle(borderPen, mainRect)
                FontColor = Color.FromArgb(0, 0, 0)
            End If
            Dim titleX As Integer = (mainRect.Location.X + mainRect.Width / 2) - (G.MeasureString(TabPages(i).Text, Font).Width / 2)
            Dim titleY As Integer = (mainRect.Location.Y + mainRect.Height / 2) - (G.MeasureString(TabPages(i).Text, Font).Height / 2)
            G.DrawString(TabPages(i).Text, Font, New SolidBrush(FontColor), New Point(titleX, titleY))
            Try : TabPages(i).BackColor = Color.FromArgb(218, 218, 218) : Catch : End Try
        Next
    End Sub
End Class
<DefaultEvent("CheckedChanged")> Class ColdCheckBox
    Inherits Control
    Event CheckedChanged(ByVal sender As Object)
    Private _checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _checked
        End Get
        Set(ByVal value As Boolean)
            _checked = value
            Invalidate()
        End Set
    End Property
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw, True)
        Size = New Size(140, 20)
        Font = New Font("Segoe UI", 8)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        'Variable
        Dim G As Graphics = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)
        'Shape and Colors
        Dim box As New Rectangle(0, 0, Height, Height - 1)
        G.FillRectangle(DrawGradient(box.Height + 5, 0), box)
        G.DrawRectangle(New Pen(Color.FromArgb(218, 218, 218)), box)
        box.Width = 15
        box.Height = 15
        'Border
        G.DrawLine(Pens.Black, New Point(0, 0), New Point(box.Width, 0)) 'Top
        G.DrawLine(Pens.Black, New Point(0, 0), New Point(0, box.Height)) 'Left
        G.DrawLine(Pens.Black, New Point(box.Width, 0), New Point(box.Width, box.Height)) 'Right
        G.DrawLine(Pens.Black, New Point(0, box.Height), New Point(box.Width, box.Height)) 'Bottom
        'Inside
        Dim markPen As New Pen(Color.FromArgb(218, 218, 218))
        Dim lightMarkPen As New Pen(Color.FromArgb(0, 0, 0))
        If _checked Then G.DrawString("a", New Font("Marlett", 13), Brushes.Black, New Point(-3, -1.2))
        'Text
        Dim textY As Integer = (Height / 2) - (G.MeasureString(Text, Font).Height / 2)
        G.DrawString(Text, Font, Brushes.Black, New Point(24, textY))
        Me.Size = New Size(28 + G.MeasureString(Text, Font).Width, 16)
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If _checked Then
            _checked = False
        Else
            _checked = True
        End If
        RaiseEvent CheckedChanged(Me)
        Invalidate()
    End Sub
End Class
