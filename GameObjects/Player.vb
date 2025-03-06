Imports System.Security.Cryptography.X509Certificates

Public Class Player

    'properties
    Property ID As String
    Property Color As Color

    Property X As Single

    Property Y As Single

    Property Size As Single

    Property Speed As Single

    Property Vspeed As Single

    'constructor

    Public Sub New(id As String)
        Me.ID = id
    End Sub

    'methods
    Public Sub draw(g As Graphics)
        g.FillRectangle(New SolidBrush(Me.Color), New Rectangle(Me.X, Me.Y, Me.Size, Me.Size))
    End Sub


End Class