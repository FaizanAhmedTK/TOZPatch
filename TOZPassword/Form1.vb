Imports System.Reflection
Imports System.Data
Imports MySql.Data
Imports MySql.Data.MySqlClient

Public Class Form1
    Dim runTimeResourceSet As Object
    Dim dictEntry As DictionaryEntry
    'Public conn As MySqlConnection()
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        runTimeResourceSet = My.Resources.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, True, False)
        ComboBox1.Items.Clear()
        For Each dictEntry In runTimeResourceSet
            ComboBox1.Items.Add(dictEntry.Key)
        Next
        ComboBox1.Text = "Select from..."
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        'Dim chk As New CheckBox
        For Each dictEntry In runTimeResourceSet
            If dictEntry.Key = ComboBox1.Text Then
                Dim mCrypTo As New SMRHRT.Services.Security.CryptoProvider("smr357951hrdpower312net")
                Dim str = mCrypTo.DecryptString(dictEntry.Value)
                Try
                    Dim conn = New MySql.Data.MySqlClient.MySqlConnection(str)
                    conn.Open()
                    'Dim myCommand As New MySqlCommand("SELECT schema_name FROM information_schema.schemata where schema_name like '%_talentoz%' and schema_name not in ('datazie_talentoz','accounts_talentoz') order by schema_name ;", conn)
                    Dim myCommand As New MySqlCommand("SELECT schema_name FROM information_schema.schemata order by schema_name ;", conn)
                    Dim da As MySqlDataAdapter = New MySqlDataAdapter(myCommand)
                    Dim dt As New DataTable("Client_details")
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        Button1.Visible = True
                        RichTextBox1.Text = ""
                        Dim count As Integer = Me.Controls.Count
                        For k As Integer = count - 1 To 0 Step -1
                            Dim cControl As Control = Me.Controls(k)
                            If (TypeOf cControl Is CheckBox) Then
                                Me.Controls.Remove(cControl)
                            End If
                        Next
                        Dim i = 0
                        Dim j = 40
                        'Dim CheckedList As CheckedListBox = New CheckedListBox()
                        CheckedList.Items.Clear()
                        Do Until i = dt.Rows.Count
                            If dt.Rows(i)("schema_name") = "datazie_talentoz" Or dt.Rows(i)("schema_name") = "mysql" Or dt.Rows(i)("schema_name") = "test" Or dt.Rows(i)("schema_name") = "sys" Or dt.Rows(i)("schema_name") = "information_schema" Or dt.Rows(i)("schema_name") = "performance_schema" Then
                                CheckedList.Items.Add(dt.Rows(i)("schema_name"), False)
                                i += 1
                            Else
                                CheckedList.Items.Add(dt.Rows(i)("schema_name"), True)
                                i += 1
                            End If

                        Loop
                    End If
                    conn.Close()
                Catch ex As Exception
                End Try
            End If
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If RichTextBox1.Text <> "" Then
            If Me.CheckedList.CheckedItems.Count > 0 Then
                'Dim pbs = Me.Controls.OfType(Of CheckBox)()
                Dim txt As String = ""
                'For Each pb In pbs
                '    If pb.Checked = True Then
                '        txt = txt & vbLf & "use `" & pb.Text & "`; " & vbLf & RichTextBox1.Text
                '    End If
                'Next


                For Each pb In Me.CheckedList.CheckedItems
                    txt = txt & vbLf & "use `" & pb & "`; " & vbLf & " set sql_safe_updates = 0; " & vbLf & " Select Case database(); " & vbLf & RichTextBox1.Text
                Next
                txt = "\T " & ComboBox1.Text & "_" & Date.Now.ToString("yyyyMMddHHmmss") & ".log" & vbLf & txt
                If Not System.IO.File.Exists("C:\TozPatchFiles\" & ComboBox1.Text & "_" & Date.Now.ToString("yyyyMMddHHmmss") & ".sql") = True Then
                    Dim file As System.IO.FileStream
                    file = System.IO.File.Create("C:\TozPatchFiles\" & ComboBox1.Text & "_" & Date.Now.ToString("yyyyMMddHHmmss") & ".sql")
                    file.Close()
                End If
                My.Computer.FileSystem.WriteAllText("C:\TozPatchFiles\" & ComboBox1.Text & "_" & Date.Now.ToString("yyyyMMddHHmmss") & ".sql", txt, False)
                RichTextBox1.Text = txt
                Button1.Visible = False
                MessageBox.Show("File generated path as " & "C:\TozPatchFiles\" & ComboBox1.Text & "_" & Date.Now.ToString("yyyyMMddHHmmss") & ".sql", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Schema(s) can not be empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("Query can not be empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text = "PAYGROUP312" Then
            Panel1.Visible = False
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Using fs As IO.FileStream = New IO.FileStream("Resourcees.resx", IO.FileMode.Create)
            Using resx As Resources.ResXResourceWriter = New Resources.ResXResourceWriter(fs)
                resx.AddResource("Key1", "Value1")
                resx.AddResource("Key2", "Value2")
                resx.AddResource("Key3", "Value3")
            End Using
        End Using
    End Sub

   
End Class
