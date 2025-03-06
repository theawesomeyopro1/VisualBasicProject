Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text

Public Class GameWindow

#Region "GameWindowSetup"

    Private antialiasing = False
    Private backBuffer As Image
    Private bufferDisp As Graphics
    Private gDisplay As Graphics
    Private gRefresh As Timer
    Dim frequency As Integer = 10
    Private projs As List(Of Projectile) = New List(Of Projectile)
    Private r = New Random()

    Private Shadows Sub Paint(g As Graphics)
        GameDraw(g)
    End Sub

    Private Sub initGDI(width As Integer, height As Integer)
        Dim dispsize As Size = New Size(width, height)
        backBuffer = New Bitmap(width, height)
        bufferDisp = Graphics.FromImage(backBuffer)
        gDisplay = Me.CreateGraphics()
        gDisplay.CompositingMode = CompositingMode.SourceCopy
        gDisplay.CompositingQuality = CompositingQuality.AssumeLinear
        gDisplay.InterpolationMode = InterpolationMode.NearestNeighbor
        gDisplay.TextRenderingHint = TextRenderingHint.SystemDefault
        gDisplay.PixelOffsetMode = PixelOffsetMode.HighSpeed

        If antialiasing Then
            bufferDisp.SmoothingMode = SmoothingMode.AntiAlias And SmoothingMode.HighSpeed
            bufferDisp.TextRenderingHint = TextRenderingHint.AntiAlias And TextRenderingHint.SystemDefault
        End If

        bufferDisp.CompositingMode = CompositingMode.SourceOver
        bufferDisp.CompositingQuality = CompositingQuality.HighSpeed
        bufferDisp.InterpolationMode = InterpolationMode.Low
        bufferDisp.PixelOffsetMode = PixelOffsetMode.Half

        gDisplay.Clear(Color.SlateGray)
    End Sub

    Private Sub GameWindow_Init()
        Dim dispsize As Size = displaySize
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.UserPaint, False)
        Me.Size = dispsize
        Me.ClientSize = dispsize
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.SetStyle(ControlStyles.FixedHeight, True)
        Me.SetStyle(ControlStyles.FixedWidth, False)
        Me.MinimumSize = displaySize
        Me.MaximumSize = displaySize
        Me.Update()
        Me.Location = New Point(0, 0)
        initGDI(dispsize.Width, displaySize.Height)

        gRefresh = New Timer()
        With (gRefresh)
            .Interval = frequency
            .Enabled = True
        End With

        AddHandler gRefresh.Tick, Sub() GameLoop()
    End Sub

    Private Sub GameLoop()
        GameLogic()
        If Me.Disposing = False And Me.IsDisposed = False And Me.Visible Then
            Try
                Paint(bufferDisp)
                gDisplay.DrawImageUnscaled(backBuffer, New Point(0, 0))
            Catch ex As Exception
                Console.WriteLine(ex)
            End Try
        End If
    End Sub

#End Region

    Public Shared displaySize As Size = New Size(700, 700)
    Dim rand As Random = New Random()

    Private Sub GameWindowLoad(sender As Object, e As EventArgs) Handles MyBase.Load
        GameWindow_Init()

        ' Initialize enemies
        enemy1.X = 200
        enemy1.Y = 200
        enemy1.Size = 40
        enemy1.Color = Color.Blue
        enemy1.Hspeed = 4
        enemy1.Vspeed = 4

        enemy2.X = 200
        enemy2.Y = 100
        enemy2.Size = 70
        enemy2.Color = Color.Red
        enemy2.Hspeed = 4
        enemy2.Vspeed = 4
    End Sub

    Private Sub GameLogic()
        ' Update enemies' positions
        enemy1.X += enemy1.Hspeed
        enemy2.X += enemy2.Hspeed

        ' Reverse direction when hitting screen edges
        If enemy1.X > Me.ClientSize.Width - enemy1.Size Or enemy1.X < 0 Then
            enemy1.Hspeed = -enemy1.Hspeed
        End If

        If enemy2.X > Me.ClientSize.Width - enemy2.Size Or enemy2.X < 0 Then
            enemy2.Hspeed = -enemy2.Hspeed
        End If

        ' Check for collisions with projectiles and reverse speed if a collision occurs
        For Each proj As Projectile In projs
            If CheckCollision(proj, enemy1) Then
                enemy1.Hspeed = -enemy1.Hspeed ' Reverse direction on collision with projectile
            End If

            If CheckCollision(proj, enemy2) Then
                enemy2.Hspeed = -enemy2.Hspeed ' Reverse direction on collision with projectile
            End If
        Next
    End Sub

    Private Sub GameDraw(g As Graphics)
        g.Clear(Color.Black)
        enemy1.draw(g)
        enemy2.draw(g)
    End Sub

    ' --- Projectile Logic ---
    ' Constants for physics
    Dim gravity As Double = 9.8 ' m/s²
    Dim time As Double = 0 ' Time in seconds
    Dim velocity As Double = 50 ' Initial velocity (m/s)
    Dim angle As Double = 45 ' Launch angle (degrees)
    Dim vx As Double
    Dim vy As Double
    Dim startX As Integer = 50 ' Starting X position
    Dim startY As Integer = 300 ' Starting Y position
    Dim projectile As Panel
    Dim timer As Timer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Create a projectile (a small panel)
        projectile = New Panel With {
            .Size = New Size(10, 10),
            .BackColor = Color.Red,
            .Location = New Point(startX, startY)
        }
        Me.Controls.Add(projectile)

        ' Create a timer
        timer = New Timer With {
            .Interval = 50 ' Update every 50ms
        }
        AddHandler timer.Tick, AddressOf UpdateProjectile
    End Sub

    Private Sub UpdateProjectile(sender As Object, e As EventArgs)
        ' Update time
        time += 0.05

        ' Update projectile position using motion equations
        Dim x As Integer = startX + CInt(vx * time)
        Dim y As Integer = startY - CInt(vy * time + 0.5 * gravity * time ^ 2)  ' Invert y to reflect gravity

        ' Move the projectile
        projectile.Location = New Point(x, y)

        ' Check for collision with enemy1 (stop at first collision)
        If CheckCollision(projectile, enemy1) Then
            timer.Stop()
            Me.Controls.Remove(projectile) ' Remove projectile on collision
            Exit Sub
        End If

        ' Check for collision with enemy2 (stop at first collision)
        If CheckCollision(projectile, enemy2) Then
            timer.Stop()
            Me.Controls.Remove(projectile) ' Remove projectile on collision
            Exit Sub
        End If

        ' Stop the timer if projectile reaches the ground (stop falling once it touches the bottom)
        If y >= Me.ClientSize.Height - projectile.Height Then
            timer.Stop()
        End If
    End Sub

    ' Collision detection function
    Private Function CheckCollision(proj As Panel, enemy As Enemy) As Boolean
        Dim projRect As New Rectangle(proj.Location, proj.Size)
        Dim enemyRect As New Rectangle(New Point(enemy.X, enemy.Y), New Size(enemy.Size, enemy.Size))

        ' Return true if the projectile's rectangle intersects the enemy's rectangle
        Return projRect.IntersectsWith(enemyRect)
    End Function

    ' Handle Mouse Clicks to launch a new projectile
    Private Sub Form1_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        ' Reset time
        time = 0

        ' Set new start position to mouse click position
        startX = e.X
        startY = e.Y
        projectile.Location = New Point(startX, startY)

        ' Calculate velocity components based on click position
        Dim targetX As Integer = e.X
        Dim targetY As Integer = e.Y
        Dim dx As Integer = targetX - startX
        Dim dy As Integer = targetY - startY
        Dim distance As Double = Math.Sqrt(dx * dx + dy * dy)

        ' Set velocity dynamically based on distance
        velocity = distance / 10
        angle = Math.Atan2(-dy, dx) * 180 / Math.PI ' Calculate angle from click direction

        ' Recalculate velocity components
        vx = velocity * Math.Cos(angle * Math.PI / 180)
        vy = -velocity * Math.Sin(angle * Math.PI / 180)

        ' Restart the timer to move the projectile
        timer.Start()
    End Sub

End Class
