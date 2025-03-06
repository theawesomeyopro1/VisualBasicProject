Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify

Public Class Enemy : Inherits GameObject

    'properties

    Property ID As String

    Property Color As Color

    Property X As Single

    Property Y As Single

    Property Size As Single

    Property Speed As Single

    Property Vspeed As Single

    Property Hspeed As Single

    'constructor

    Public Sub New(id As String)

        Me.ID = id

    End Sub

    'method

    Public Overrides Sub Draw(g As Graphics)




        'If (Me.X >= GameWindow.displaySize.Width - 120) Then

        '    Me.Hspeed = -1 * Me.Hspeed

        '    'Me.X = GameWindow.DisplaySize.Width - 10

        'End If

        'If (Me.X <= 0) Then

        '    Me.X = Me.X + Me.Hspeed

        '    'Me.X = GameWindow.DisplaySize.Width - 10

        'End If

        'Me.Y = Me.Y + Me.Vspeed


        'If (Me.Y >= GameWindow.displaySize.Height - 125) Then

        '    Me.Vspeed = -1 * Me.Vspeed

        'Me.X = GameWindow.DisplaySize.Height - 10

        'End If

        'If (Me.Y <= 0) Then

        '    Me.Y = Me.Y + Me.Vspeed

        'Me.X = GameWindow.DisplaySize.Height - 10

        'End If


        'If (Me.Y >= 775) Then

        '    Me.Y = -100

        'End If

        g.FillRectangle(New SolidBrush(Me.Color), New Rectangle(Me.X, Me.Y, Me.Size, Me.Size))

        Me.X = Me.X + Me.Hspeed
        Me.Y = Me.Y + Me.Vspeed

        'Dim enemy_pic As Bitmap = Image.FromFile("enemy.png")


    End Sub


End Class



' spare code
