<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Start = New System.Windows.Forms.Button()
        Me.Timer = New System.Windows.Forms.Timer(Me.components)
        Me.lb1 = New System.Windows.Forms.Label()
        Me.lb2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Start
        '
        Me.Start.BackColor = System.Drawing.SystemColors.Control
        Me.Start.Location = New System.Drawing.Point(55, 32)
        Me.Start.Name = "Start"
        Me.Start.Size = New System.Drawing.Size(219, 43)
        Me.Start.TabIndex = 0
        Me.Start.Text = "Start"
        Me.Start.UseVisualStyleBackColor = False
        '
        'Timer
        '
        Me.Timer.Enabled = True
        Me.Timer.Interval = 15000
        '
        'lb1
        '
        Me.lb1.AutoSize = True
        Me.lb1.Location = New System.Drawing.Point(5, 110)
        Me.lb1.Name = "lb1"
        Me.lb1.Size = New System.Drawing.Size(0, 15)
        Me.lb1.TabIndex = 1
        '
        'lb2
        '
        Me.lb2.AutoSize = True
        Me.lb2.Location = New System.Drawing.Point(5, 82)
        Me.lb2.Name = "lb2"
        Me.lb2.Size = New System.Drawing.Size(0, 15)
        Me.lb2.TabIndex = 2
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(311, 146)
        Me.Controls.Add(Me.lb2)
        Me.Controls.Add(Me.lb1)
        Me.Controls.Add(Me.Start)
        Me.Name = "Form1"
        Me.Text = "Schnittstelle"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Start As Button
    Friend WithEvents Timer As Timer
    Friend WithEvents lb1 As Label
    Friend WithEvents lb2 As Label
End Class
