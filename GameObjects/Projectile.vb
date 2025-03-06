Public Class Projectile
    Inherits GameObject

    Property ID As String
    Property Size As Single
    Property X As Single
    Property Y As Single
    Property Hspeed As Single
    Property Vspeed As Single
    Property Speed As Single = 5 ' Adjust speed for smoother motion
    Property Gravity As Single = 0.5 ' Simulate gravity
    Property Color As Color

    Private WindowWidth As Integer
    Private WindowHeight As Integer

    Public Sub New(id As String, x As Single, y As Single, destX As Single, destY As Single, width As Integer, height As Integer)
        Me.ID = id
        Me.Color = Color.Red
        Me.Size = 10
        Me.X = x
        Me.Y = y
        Me.WindowWidth = width
        Me.WindowHeight = height

        ' Calculate angle and initial velocities
        Dim angle As Double = Math.Atan2(destY - Me.Y, destX - Me.X)
        Me.Vspeed = Me.Speed * Math.Sin(angle)
        Me.Hspeed = Me.Speed * Math.Cos(angle)
    End Sub

    Public Sub UpdatePosition()
        ' Update position
        Me.X += Me.Hspeed
        Me.Y += Me.Vspeed

        ' Bounce off left and right walls
        If Me.X <= 0 OrElse Me.X + Me.Size >= WindowWidth Then
            Me.Hspeed *= -1 ' Reverse horizontal speed
        End If

        ' Bounce off top and bottom walls
        If Me.Y <= 0 OrElse Me.Y + Me.Size >= WindowHeight Then
            Me.Vspeed *= -1 ' Reverse vertical speed
        End If
    End Sub

    Public Sub Draw(g As Graphics)
        g.FillEllipse(New SolidBrush(Me.Color), New RectangleF(Me.X, Me.Y, Me.Size, Me.Size))
    End Sub
End Class
