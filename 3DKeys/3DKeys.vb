Imports System.Xml
Imports System.IO
Imports System.Text

' 3DKeys - by Ed Sellers <ed.sellers@gmx.com>
'
'This work is licensed under a Creative Commons Attribution 3.0 Unported License.
'...basically you can steal it, use it, even sell it - but give me some credit. 

Public Class frm3dKeys

    Public fileLoc As String = ""
    Public FileToCopy As String
    Public LocalFile As String = ""
    Public KeyBlank As String
    Public MACS As String
    Public CUTS As String
    Public REVERSE As Boolean
    Public ROTATE As Boolean

    Public Depth0 As Single
    Public Depth1 As Single
    Public Depth2 As Single
    Public Depth3 As Single
    Public Depth4 As Single
    Public Depth5 As Single
    Public Depth6 As Single
    Public Depth7 As Single
    Public Depth8 As Single
    Public Depth9 As Single

    Public depth(9) As Single

    Public minCode As Integer
    Public maxCode As Integer

    Function getDepth(ByVal thisCode As String) As Single
        Select Case thisCode
            Case "0"
                Return Depth0
            Case "1"
                Return Depth1
            Case "2"
                Return Depth2
            Case "3"
                Return Depth3
            Case "4"
                Return Depth4
            Case "5"
                Return Depth5
            Case "6"
                Return Depth6
            Case "7"
                Return Depth7
            Case "8"
                Return Depth8
            Case "9"
                Return Depth9

        End Select
    End Function

    Function getCode(ByVal thisDepth As Single) As Integer
        Dim lclCode As Integer
        Dim i As Integer
        lclCode = minCode

        For i = minCode To maxCode
            If thisDepth > depth(i) Then
                lclCode = i
                Exit For
            End If
            lclCode = i
        Next

        If (lclCode <> minCode) And (lclCode <> maxCode) Then
            If (depth(i - 1) - thisDepth) < (thisDepth - depth(i)) Then
                lclCode = lclCode -1:
            End If
        End If

        Return lclCode
    End Function

    Private Sub lbxBlank_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbxBlank.SelectedIndexChanged

        pnlCodes.Enabled = True
        pnlKeys.Enabled = True
        btnRender.Enabled = True
        btnPrint.Enabled = True

        btnClear_Click(sender, e)

        'set defaults
        REVERSE = False
        ROTATE = False

        pictBlank.Load(Application.StartupPath & "\images\unknown.PNG")
        On Error Resume Next
        pictBlank.Load(Application.StartupPath & "\images\" + lbxBlank.SelectedItem.ToString & ".PNG")
        KeyBlank = lbxBlank.SelectedItem.ToString

        If KeyBlank = "Handcuff" Then
            radBump.Enabled = False
            radCode.Enabled = False
            radBlank.Enabled = False
            pnlCodes.Enabled = False
        Else
            radSpecial.Enabled = False
            radCode.Checked = True
            radBump.Enabled = True
            radCode.Enabled = True
            radBlank.Enabled = True
            pnlCodes.Enabled = radCode.Enabled

        End If

        Dim want As Boolean
        want = False
        minCode = 1
        maxCode = 0

        Dim i As Integer
        For i = 0 To 9
            depth(i) = 0.0
        Next

        txtCode1.Text = "" : txtDepth1.Text = "0"
        txtCode2.Text = "" : txtDepth2.Text = "0"
        txtCode3.Text = "" : txtDepth3.Text = "0"
        txtCode4.Text = "" : txtDepth4.Text = "0"
        txtCode5.Text = "" : txtDepth5.Text = "0"
        txtcode6.Text = "" : txtDepth6.Text = "0"

        'Dim xmlFile As XmlReader
        'xmlFile = XmlReader.Create("keyconfig2.xml")
        'Dim ds As New DataSet
        'Dim dv As DataView
        'ds.ReadXml(xmlFile)
        'dv = New DataView(ds.Tables(0))
        'dv.Sort = "keyid"
        ''dv.RowFilter = "keyid='" + KeyBlank + "'"
        'dv.RowFilter = "keyid='KW1'"
        'Console.Write(dv.Item(0).Item("keyid").ToString())

        'Dim tmpStr As String
        'tmpStr = dv.Item(0).Item("keyid").ToString
        'tmpStr = dv.Item(0).Item("keyid").ToString
        'tmpStr = dv.Item(0).Item("MACS").ToString
        'tmpStr = dv.Item(0).Item("keyid").ToString
        'tmpStr = dv.Item(0).Item("keyid").ToString


        Dim tmpStr As String
        want = False

        ' Create an XML reader.
        Using reader As XmlReader = XmlReader.Create("keyconfig.xml")
            While reader.Read()
                ' Check for start elements.
                'If reader.IsStartElement() Then
                tmpStr = reader.Name

                If tmpStr = "keyid" Then
                    tmpStr = reader.ReadElementContentAsString()
                    want = (tmpStr = KeyBlank)
                    'reader.ReadToFollowing("keyid")
                End If

                If want Then
                    Select Case reader.Name
                        Case "ROTATE"
                            ROTATE = True
                        Case "REVERSE"
                            REVERSE = True
                        Case "MACS"
                            MACS = reader.ReadElementContentAsString()
                        Case "CUTS"
                            CUTS = reader.ReadElementContentAsString()
                        Case "Code0"
                            Depth0 = reader.ReadElementContentAsString() : depth(0) = Depth0
                            minCode = 0
                            maxCode = 0
                        Case "Code1"
                            Depth1 = reader.ReadElementContentAsString() : depth(1) = Depth1
                            maxCode = 1
                        Case "Code2"
                            Depth2 = reader.ReadElementContentAsString() : depth(2) = Depth2
                            maxCode = 2
                        Case "Code3"
                            Depth3 = reader.ReadElementContentAsString() : depth(3) = Depth3
                            maxCode = 3
                        Case "Code4"
                            Depth4 = reader.ReadElementContentAsString() : depth(4) = Depth4
                            maxCode = 4
                        Case "Code5"
                            Depth5 = reader.ReadElementContentAsString() : depth(5) = Depth5
                            maxCode = 5
                        Case "Code6"
                            Depth6 = reader.ReadElementContentAsString() : depth(6) = Depth6
                            maxCode = 6
                        Case "Code7"
                            Depth7 = reader.ReadElementContentAsString() : depth(7) = Depth7
                            maxCode = 7
                        Case "Code8"
                            Depth8 = reader.ReadElementContentAsString() : depth(8) = Depth8
                            maxCode = 8
                        Case "Code9"
                            Depth9 = reader.ReadElementContentAsString() : depth(9) = Depth9
                            maxCode = 9
                    End Select
                End If

            End While
        End Using

        Label9.Visible = (CUTS = "6")
        txtcode6.Visible = (CUTS = "6")
        txtDepth6.Visible = (CUTS = "6")
        Label9.Enabled = (CUTS = "6")
        txtcode6.Enabled = (CUTS = "6")
        'txtDepth6.Enabled = (CUTS = "6")

        If REVERSE Then
            lblCutsMsg.Text = "(Tip to Bow)"
            lblCodesMsg.Text = "Zero for DEEPEST cut!"
            MsgBox("Note that the code depths are REVERSED and the spacing is TIP-to-BOW for this key!", MsgBoxStyle.Information, "ATTENTION!")

            maxCode = 0
        Else
            lblCutsMsg.Text = "(Bow to Tip)"
            lblCodesMsg.Text = ""
        End If

        radCode.Select()
        txtCode1.Focus()

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()

        Dim i As Integer
        SplashForm.Show()
        For i = 1 To 2000000000
        Next i
        SplashForm.Close()
        End

    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtCode1.Text = "" : txtDepth1.Text = 0
        txtCode2.Text = "" : txtDepth2.Text = 0
        txtCode3.Text = "" : txtDepth3.Text = 0
        txtCode4.Text = "" : txtDepth4.Text = 0
        txtCode5.Text = "" : txtDepth5.Text = 0
        txtcode6.Text = "" : txtDepth6.Text = 0
        If txtCode1.Enabled Then txtCode1.Focus() Else txtDepth1.Focus()
    End Sub


    Private Sub radBlank_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radBlank.CheckedChanged
        pnlCodes.Enabled = False

    End Sub

    Private Sub radBump_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radBump.CheckedChanged
        pnlCodes.Enabled = False
        txtCode1.Text = maxCode
        txtCode2.Text = maxCode
        txtCode3.Text = maxCode
        txtCode4.Text = maxCode
        txtCode5.Text = maxCode
        txtcode6.Text = maxCode

    End Sub

    Private Sub radSpecial_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radSpecial.CheckedChanged
        pnlCodes.Enabled = False

    End Sub

    Private Sub radCode_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radCode.CheckedChanged
        pnlCodes.Enabled = True

    End Sub


    Private Sub rbtnCodes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtnCodes.CheckedChanged
        txtCode1.BackColor = Color.White : txtDepth1.BackColor = Color.LightGray : txtCode1.Enabled = True : txtDepth1.Enabled = False
        txtCode2.BackColor = Color.White : txtDepth2.BackColor = Color.LightGray : txtCode2.Enabled = True : txtDepth2.Enabled = False
        txtCode3.BackColor = Color.White : txtDepth3.BackColor = Color.LightGray : txtCode3.Enabled = True : txtDepth3.Enabled = False
        txtCode4.BackColor = Color.White : txtDepth4.BackColor = Color.LightGray : txtCode4.Enabled = True : txtDepth4.Enabled = False
        txtCode5.BackColor = Color.White : txtDepth5.BackColor = Color.LightGray : txtCode5.Enabled = True : txtDepth5.Enabled = False
        txtcode6.BackColor = Color.White : txtDepth6.BackColor = Color.LightGray : txtcode6.Enabled = True : txtDepth6.Enabled = False

    End Sub

    Private Sub rbtnDepths_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtnDepths.CheckedChanged
        txtDepth1.BackColor = Color.White : txtCode1.BackColor = Color.LightGray : txtCode1.Enabled = False : txtDepth1.Enabled = True
        txtDepth2.BackColor = Color.White : txtCode2.BackColor = Color.LightGray : txtCode2.Enabled = False : txtDepth2.Enabled = True
        txtDepth3.BackColor = Color.White : txtCode3.BackColor = Color.LightGray : txtCode3.Enabled = False : txtDepth3.Enabled = True
        txtDepth4.BackColor = Color.White : txtCode4.BackColor = Color.LightGray : txtCode4.Enabled = False : txtDepth4.Enabled = True
        txtDepth5.BackColor = Color.White : txtCode5.BackColor = Color.LightGray : txtCode5.Enabled = False : txtDepth5.Enabled = True
        txtDepth6.BackColor = Color.White : txtcode6.BackColor = Color.LightGray : txtcode6.Enabled = False : txtDepth6.Enabled = True
        txtDepth1.Focus()


    End Sub


    Private Sub OpenToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem1.Click
        openKeys.InitialDirectory = Application.StartupPath & "\blanks\"
        openKeys.ShowDialog()

    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        openKeys.InitialDirectory = Application.StartupPath & "\keys\"
        openKeys.ShowDialog()

    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click

        ' Create a Text File
        Dim fs As FileStream = Nothing

        fileLoc = Application.StartupPath & "\keys\" & lbxBlank.SelectedItem.ToString & ".txt"

        If (Not File.Exists(fileLoc)) Then
            fs = File.Create(fileLoc)
            Using fs

            End Using
        End If


    End Sub

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        End


    End Sub

    Public Function checkMACS() As Boolean
        Dim lclResult As MsgBoxResult
        checkMACS = False
        If (Math.Abs(Val(txtCode1.Text) - Val(txtCode2.Text)) > MACS) Or _
           (Math.Abs(Val(txtCode2.Text) - Val(txtCode3.Text)) > MACS) Or _
           (Math.Abs(Val(txtCode3.Text) - Val(txtCode4.Text)) > MACS) Or _
           (Math.Abs(Val(txtCode4.Text) - Val(txtCode5.Text)) > MACS) Or _
           (txtcode6.Visible And Math.Abs(Val(txtCode5.Text) - Val(txtcode6.Text)) > MACS) Then

            lclResult = MsgBox("MACS = " + MACS + vbCrLf + _
                               "The Codes (or Dapths) entered exceed the maximum adjacent cut rule!" + vbCrLf + vbCrLf + _
                               "Do you wish to correct these values?", MsgBoxStyle.YesNo, "MACS WARNING")
            checkMACS = (lclResult = 6)
        End If

    End Function

    Private Sub btnRender_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRender.Click

        If checkMACS() Then
            btnClear_Click(sender, e)

        Else
            MsgBox("Hit F6 to render object after window appears", MsgBoxStyle.Information)

            'scadBuild()
            FileToCopy = "blanks\" + KeyBlank + ".scad"
            LocalFile = KeyBlank + ".scad"

            If System.IO.File.Exists(FileToCopy) = True Then
                System.IO.File.Copy(FileToCopy, LocalFile, True)
            End If

            Dim objWriter As New System.IO.StreamWriter(LocalFile, True)
            Dim codes As String
            Dim params As String

            If radSpecial.Checked Then
                Select Case KeyBlank
                    Case "Handcuff"
                        codes = "// Handcuff Key!"
                End Select
            Else
                If radBlank.Checked Then
                    If CUTS = "5" Then
                        codes = KeyBlank + "(" + Chr(34) + "blank" + Chr(34) + ",[0,0,0,0,0]);"
                    Else
                        codes = KeyBlank + "(false,[0,0,0,0,0,0]);"
                    End If
                ElseIf radBump.Checked Then
                    codes = KeyBlank + "(" + Chr(34) + "bump" + Chr(34) + ",[" + _
                    txtCode1.Text + "," + _
                    txtCode2.Text + "," + _
                    txtCode3.Text + "," + _
                    txtCode4.Text + "," + _
                    txtCode5.Text + "," + _
                    txtCode5.Text + "]);"
                Else
                    If CUTS = "5" Then
                        codes = KeyBlank + "(" + Chr(34) + "code" + Chr(34) + ",[" + txtCode1.Text + "," + _
                        txtCode2.Text + "," + _
                        txtCode3.Text + "," + _
                        txtCode4.Text + "," + _
                        txtCode5.Text + "]);"
                    Else
                        codes = KeyBlank + "(" + Chr(34) + "code" + Chr(34) + ",[" + txtCode1.Text + "," + _
                        txtCode2.Text + "," + _
                        txtCode3.Text + "," + _
                        txtCode4.Text + "," + _
                        txtCode5.Text + "," + _
                        txtcode6.Text + "]);"
                    End If

                End If
            End If

            'rotate key blank depending on xml data
            If ROTATE Then codes = "rotate([180, 0, 0]) " + codes

            objWriter.WriteLine(codes)
            objWriter.Close()

            'Dim params As String
            params = " --render " + KeyBlank + ".scad"

            Process.Start("openscad", params)

        End If

    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        checkMACS()
        'scadBuild()
        FileToCopy = "blanks\" + KeyBlank + ".scad"
        LocalFile = KeyBlank + ".scad"

        If System.IO.File.Exists(FileToCopy) = True Then
            System.IO.File.Copy(FileToCopy, LocalFile, True)
        End If

        Dim objWriter As New System.IO.StreamWriter(LocalFile, True)
        Dim codes As String
        Dim params As String

        If radSpecial.Checked Then
            Select Case KeyBlank
                Case "Handcuff"
                    codes = "// Handcuff Key!"
            End Select
        Else
            If radBlank.Checked Then
                If CUTS = "5" Then
                    codes = KeyBlank + "(" + Chr(34) + "blank" + Chr(34) + ",[0,0,0,0,0]);"
                Else
                    codes = KeyBlank + "(false,[0,0,0,0,0,0]);"
                End If
            ElseIf radBump.Checked Then
                codes = KeyBlank + "(" + Chr(34) + "bump" + Chr(34) + ",[" + _
                txtCode1.Text + "," + _
                txtCode2.Text + "," + _
                txtCode3.Text + "," + _
                txtCode4.Text + "," + _
                txtCode5.Text + "," + _
                txtCode5.Text + "]);"
            Else
                If CUTS = "5" Then
                    codes = KeyBlank + "(" + Chr(34) + "code" + Chr(34) + ",[" + txtCode1.Text + "," + _
                    txtCode2.Text + "," + _
                    txtCode3.Text + "," + _
                    txtCode4.Text + "," + _
                    txtCode5.Text + "]);"
                Else
                    codes = KeyBlank + "(" + Chr(34) + "code" + Chr(34) + ",[" + txtCode1.Text + "," + _
                    txtCode2.Text + "," + _
                    txtCode3.Text + "," + _
                    txtCode4.Text + "," + _
                    txtCode5.Text + "," + _
                    txtcode6.Text + "]);"
                End If

            End If
        End If

        'rotate key blank depending on xml data
        If ROTATE Then codes = "rotate([180, 0, 0]) " + codes

        objWriter.WriteLine(codes)
        objWriter.Close()

        'openscad -o my_model_production.stl -D 'quality="production"' my_model.scad
        'params = "-o " + KeyBlank + ".stl -D 'quality=" + Chr(34) + "production" + Chr(34) + "' " + KeyBlank + ".scad"
        params = "-o " + KeyBlank + ".stl -D quality=" + Chr(34) + "production" + Chr(34) + " " + KeyBlank + ".scad"
        Process.Start("openscad", params)


    End Sub

    Private Sub frm3dKeys_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim directoryInfo As New System.IO.DirectoryInfo("blanks")
        Dim fileInfos() As System.IO.FileInfo

        Dim i As Integer
        SplashForm.Show()
        For i = 1 To 1500000000
        Next i
        SplashForm.Close()

        Me.Show()

        fileInfos = directoryInfo.GetFiles()
        For Each fileInfo As System.IO.FileInfo In fileInfos
            Dim lclname As String
            lclname = fileInfo.Name
            lclname = Mid(lclname, 1, Len(fileInfo.Name) - 5)
            lbxBlank.Items.Add(lclname)
        Next

        pnlCodes.Enabled = False
        pnlKeys.Enabled = False
        btnRender.Enabled = False
        btnPrint.Enabled = False

    End Sub

    Public Function stripAlphaChars(ByVal input As TextBox) As Boolean
        Dim rx As New Regex("[0-9]")
        stripAlphaChars = rx.IsMatch(input.Text)
        If Not (stripAlphaChars) Then
            input.Text = ""
        End If

    End Function

    Private Sub txtCode1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCode1.TextChanged
        If txtCode1.Enabled Then
            If stripAlphaChars(txtCode1) Then
                txtDepth1.Text = getDepth(txtCode1.Text).ToString
                txtCode2.Focus()
            End If
        End If

    End Sub

    Private Sub txtCode2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCode2.TextChanged
        If txtCode2.Enabled Then
            If stripAlphaChars(txtCode2) Then
                txtDepth2.Text = getDepth(txtCode2.Text).ToString
                txtCode3.Focus()
            End If
        End If

    End Sub

    Private Sub txtCode3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCode3.TextChanged
        If txtCode3.Enabled Then
            If stripAlphaChars(txtCode3) Then
                txtDepth3.Text = getDepth(txtCode3.Text).ToString
                txtCode4.Focus()
            End If
        End If
    End Sub

    Private Sub txtCode4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCode4.TextChanged
        If txtCode4.Enabled Then
            If stripAlphaChars(txtCode4) Then
                txtDepth4.Text = getDepth(txtCode4.Text).ToString
                txtCode5.Focus()
            End If
        End If
    End Sub

    Private Sub txtCode5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCode5.TextChanged
        If txtCode5.Enabled Then
            If stripAlphaChars(txtCode5) Then
                txtDepth5.Text = getDepth(txtCode5.Text).ToString
                txtcode6.Focus()
            End If
        End If
    End Sub

    Private Sub txtcode6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtcode6.TextChanged
        If txtcode6.Enabled Then
            If stripAlphaChars(txtcode6) Then
                txtDepth6.Text = getDepth(txtcode6.Text).ToString
                btnPrint.Focus()
            End If
        End If


    End Sub


    Private Sub txtDepth1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDepth1.KeyDown
        If txtDepth1.Enabled Then
            If (e.KeyData = Keys.Return) Or (e.KeyCode = Keys.Tab) Then
                txtCode1.Text = getCode(txtDepth1.Text).ToString
                txtDepth2.Focus()
            End If

        End If

    End Sub

    Private Sub txtDepth2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDepth2.KeyDown
        If txtDepth2.Enabled Then
            If (e.KeyData = Keys.Return) Or (e.KeyCode = Keys.Tab) Then
                txtCode2.Text = getCode(txtDepth2.Text).ToString
                txtDepth3.Focus()
            End If

        End If

    End Sub

    Private Sub txtDepth3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDepth3.KeyDown
        If txtDepth3.Enabled Then
            If (e.KeyData = Keys.Return) Or (e.KeyCode = Keys.Tab) Then
                txtCode3.Text = getCode(txtDepth3.Text).ToString
                txtDepth4.Focus()
            End If

        End If

    End Sub

    Private Sub txtDepth4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDepth4.KeyDown
        If txtDepth4.Enabled Then
            If (e.KeyData = Keys.Return) Or (e.KeyCode = Keys.Tab) Then
                txtCode4.Text = getCode(txtDepth4.Text).ToString
                txtDepth5.Focus()
            End If

        End If

    End Sub

    Private Sub txtDepth5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDepth5.KeyDown
        If txtDepth5.Enabled Then
            If (e.KeyData = Keys.Return) Or (e.KeyCode = Keys.Tab) Then
                txtCode5.Text = getCode(txtDepth5.Text).ToString
                txtDepth6.Focus()
            End If

        End If

    End Sub
    Private Sub txtDepth6_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDepth6.KeyDown
        If txtDepth6.Enabled Then
            If (e.KeyData = Keys.Return) Or (e.KeyCode = Keys.Tab) Then
                txtcode6.Text = getCode(txtDepth6.Text).ToString
                btnPrint.Focus()
            End If
        End If

    End Sub

   

End Class
