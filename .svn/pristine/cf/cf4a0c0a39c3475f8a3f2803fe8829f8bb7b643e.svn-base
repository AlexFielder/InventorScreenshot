Public Class ProgressForm

  Private Sub ProgressForm_Load( _
    ByVal sender As System.Object, _
    ByVal e As System.EventArgs) Handles MyBase.Load

    ProgressBar1.Minimum = 0
    ProgressBar1.Maximum = 100
    ProgressBar1.Value = 0
    ProgressBar1.Visible = True

  End Sub

  Public Sub setProgress( _
    ByVal progress As Integer, _
    ByVal tol As Integer)

    ProgressBar1.Value = progress / tol * 100
  End Sub
 
End Class