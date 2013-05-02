Public Class frmMain

    Public Enum Methods
        ListExtensions = 0
        ListFiles = 1
        ListDirectories = 2
    End Enum



    Private Sub cmbMethod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMethod.SelectedIndexChanged
        Try
            Select Case cmbMethod.SelectedIndex

                Case Methods.ListFiles, Methods.ListExtensions, Methods.ListDirectories
                    cmdSearch.Enabled = True
                Case Else
                    cmdSearch.Enabled = False
            End Select

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Method Selection Error!")
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Try

            If txtDirectory.Text = String.Empty Then
                Dim exDirectory As New Exception("Error: Directory Not Provided")
                Throw exDirectory
            End If

            txtOutput.Text = String.Empty

            Dim DIR As IO.DirectoryInfo
            DIR = New IO.DirectoryInfo(txtDirectory.Text)
            Dim FI As IO.FileInfo
            Dim SearchOutput As New Hashtable()


            Select Case cmbMethod.SelectedIndex

                Case Methods.ListExtensions
                    If chkSub.Checked Then
                        For Each FI In DIR.GetFiles(txtFilter.Text, IO.SearchOption.AllDirectories)
                            If Not SearchOutput.ContainsValue(FI.Extension.ToLower()) Then
                                SearchOutput.Add(SearchOutput.Count, FI.Extension.ToLower())
                            End If
                        Next
                    Else
                        For Each FI In DIR.GetFiles(txtFilter.Text, IO.SearchOption.TopDirectoryOnly)
                            If Not SearchOutput.ContainsValue(FI.Extension.ToLower()) Then
                                SearchOutput.Add(SearchOutput.Count, FI.Extension.ToLower())
                            End If
                        Next
                    End If

                    Dim i As Integer
                    For i = 0 To SearchOutput.Count - 1
                        txtOutput.Text = txtOutput.Text + CType(SearchOutput.Item(i), String) + vbCrLf
                    Next

                Case Methods.ListFiles
                    If chkSub.Checked Then
                        For Each FI In DIR.GetFiles(txtFilter.Text, IO.SearchOption.AllDirectories)
                            If Not SearchOutput.ContainsValue(FI.FullName) Then
                                SearchOutput.Add(SearchOutput.Count, FI.FullName.ToLower())
                            End If
                        Next
                    Else
                        For Each FI In DIR.GetFiles(txtFilter.Text, IO.SearchOption.TopDirectoryOnly)
                            If Not SearchOutput.ContainsValue(FI.FullName) Then
                                SearchOutput.Add(SearchOutput.Count, FI.FullName.ToLower())
                            End If
                        Next
                    End If

                    Dim i As Integer
                    For i = 0 To SearchOutput.Count - 1
                        txtOutput.Text = txtOutput.Text + CType(SearchOutput.Item(i), String) + vbCrLf
                    Next

                Case Methods.ListDirectories
                    If chkSub.Checked Then
                        For Each item In DIR.GetDirectories(txtFilter.Text, IO.SearchOption.AllDirectories)
                            If Not SearchOutput.ContainsValue(item.FullName) Then
                                SearchOutput.Add(SearchOutput.Count, item.FullName.ToLower())
                            End If
                        Next
                    Else
                        For Each item In DIR.GetDirectories(txtFilter.Text, IO.SearchOption.TopDirectoryOnly)
                            If Not SearchOutput.ContainsValue(item.FullName) Then
                                SearchOutput.Add(SearchOutput.Count, item.FullName.ToLower())
                            End If
                        Next

                    End If

                    Dim i As Integer
                    For i = 0 To SearchOutput.Count - 1
                        txtOutput.Text = txtOutput.Text + CType(SearchOutput.Item(i), String) + vbCrLf
                    Next

                Case Else
                    Dim exSearch As New Exception("Invalid Search")
                    Throw exSearch

            End Select

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Search Error!")
        End Try
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Try
            txtFilter.Text = "*.*"
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Reset Filter Error!")
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            txtOutput.Text = String.Empty
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Clear Output Error!")
        End Try
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            cmbMethod.SelectedIndex = Methods.ListExtensions
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error Loading Form")
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim saveFileDialog1 As New SaveFileDialog
            saveFileDialog1.Filter = "TXT File|*.txt"
            saveFileDialog1.Title = "Save a txt File"
            saveFileDialog1.InitialDirectory = Environment.CurrentDirectory
            saveFileDialog1.FileName = "Shakedown_Output"

            If (saveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.Cancel) Then
                Exit Sub
            End If

            Dim fileWriter As System.IO.StreamWriter
            If (saveFileDialog1.FileName <> "") Then
                fileWriter = My.Computer.FileSystem.OpenTextFileWriter(saveFileDialog1.FileName, False)
                fileWriter.Write(txtOutput.Text)
                fileWriter.Close()
                MsgBox("File Successfully Saved!", MsgBoxStyle.OkOnly, "File Saved")
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Save File Error")
        End Try
    End Sub
End Class
