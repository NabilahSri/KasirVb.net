Imports System.Data.SqlClient
Public Class Form5
    Dim sql As SqlCommand
    Dim da As SqlDataAdapter
    Dim dr As SqlDataReader
    Dim dt As DataTable
    Dim value As String
    Dim upload = False
    Dim fname As String
    Dim fs As System.IO.FileStream
    Sub tampil()
        da = New SqlDataAdapter("select * from tbl_barang", konek)
        dt = New DataTable
        da.Fill(dt)
        dgv.DataSource = dt
    End Sub
    Sub id()
        sql = New SqlCommand("select id_barang from tbl_barang order by id_barang desc", konek)
        dr = sql.ExecuteReader
        dr.Read()
        If dr.HasRows Then
            Dim urut As Integer
            urut = Val(Mid(dr.Item("id_barang").ToString, 4, 4)) + 1
            idbarang.Text = "BRG" + urut.ToString("000")
        Else
            idbarang.Text = "BRG001"
        End If
    End Sub
    Sub clear()
        idbarang.Clear()
        nama.Clear()
        kode.Clear()
        jumlah.Clear()
        harga.Clear()
        satuan.Text = Nothing
        img.Image = Nothing
    End Sub

    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tampil()
        id()
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            img.Image = Image.FromFile(OpenFileDialog1.FileName)
            upload = True
        End If
    End Sub

    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        If kode.Text.Length = 0 Then
            MessageBox.Show("Masukan Kode", "Kelola Barang", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf nama.Text.Length = 0 Then
            MessageBox.Show("Masukan Nama", "Kelola Barang", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf jumlah.Text.Length = 0 Then
            MessageBox.Show("Masukan Jumlah", "Kelola Barang", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf harga.Text.Length = 0 Then
            MessageBox.Show("Masukan Harga", "Kelola Barang", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf satuan.Text.Length = 0 Then
            MessageBox.Show("Pilih Satuan", "Kelola Barang", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf img.Image Is Nothing Then
            MessageBox.Show("Pilih Foto", "Kelola Barang", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            fname = idbarang.Text & ".jpg"
            Dim folder As String = "C:\Users\DPIB\source\repos\FoodXYZ\FoodXYZ\barang"
            Dim path As String = System.IO.Path.Combine(folder, fname)
            img.Image.Save(path)
            sql = New SqlCommand("select kode_barang from tbl_barang where kode_barang='" & kode.Text & "'", konek)
            dr = sql.ExecuteReader
            dr.Read()
            If dr.HasRows Then
                MsgBox("kode telah digunakan")
            Else
                sql = New SqlCommand("insert into tbl_barang values('" & idbarang.Text & "','" & kode.Text & "','" & nama.Text & "','" & exp.Text & "','" & jumlah.Text & "','" & satuan.Text & "','" & value & "','" & fname & "')", konek)
                sql.ExecuteNonQuery()
                tampil()
                MsgBox("Data berhasil disimpan")
                clear()
                id()
            End If
        End If
    End Sub

    Private Sub dgv_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgv.CellMouseClick
        idbarang.Text = dgv.Rows(e.RowIndex).Cells("id_barang").Value
        nama.Text = dgv.Rows(e.RowIndex).Cells("nama_barang").Value
        kode.Text = dgv.Rows(e.RowIndex).Cells("kode_barang").Value
        exp.Text = dgv.Rows(e.RowIndex).Cells("expired_date").Value
        jumlah.Text = dgv.Rows(e.RowIndex).Cells("jumlah_barang").Value
        satuan.Text = dgv.Rows(e.RowIndex).Cells("satuan").Value
        harga.Text = "Rp. " & Val(dgv.Rows(e.RowIndex).Cells("harga_satuan").Value).ToString("N0")
        value = dgv.Rows(e.RowIndex).Cells("harga_satuan").Value
        img.Text = dgv.Rows(e.RowIndex).Cells("gambar").Value.ToString
        If Not IsDBNull(dgv.Rows(e.RowIndex).Cells("gambar").Value) Then
            fname = idbarang.Text & ".jpg"
            Dim folder As String = "C:\Users\DPIB\source\repos\FoodXYZ\FoodXYZ\barang"
            Dim path As String = System.IO.Path.Combine(folder, fname)
            If (System.IO.File.Exists(path)) Then
                fs = New System.IO.FileStream(path, IO.FileMode.Open, IO.FileAccess.Read)
                img.Image = System.Drawing.Image.FromStream(fs)
                fs.Close()
            End If
        End If
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        fname = idbarang.Text & ".jpg"
        If upload = True Then
            Dim folder As String = "C:\Users\DPIB\source\repos\FoodXYZ\FoodXYZ\barang"
            Dim path As String = System.IO.Path.Combine(folder, fname)
            img.Image.Save(path)
        End If
        upload = False
        sql = New SqlCommand("update tbl_barang set satuan='" & satuan.Text & "',nama_barang='" & nama.Text & "',harga_satuan='" & value & "',kode_barang='" & kode.Text & "',jumlah_barang='" & jumlah.Text & "',expired_date='" & exp.Text & "',gambar='" & fname & "' where id_barang='" & idbarang.Text & "'", konek)
        sql.ExecuteNonQuery()
        tampil()
        MsgBox("Data berhasil diedit")
        clear()
        id()
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        If MsgBox("Yakin Hapus data ini ?", vbYesNo + 32, "Hapus") = vbYes Then
            fname = idbarang.Text & ".jpg"
            Dim folder As String = "C:\Users\DPIB\source\repos\FoodXYZ\FoodXYZ\barang"
            Dim path As String = System.IO.Path.Combine(folder, fname)
            If (System.IO.File.Exists(path)) Then
                System.IO.File.Delete(path)
            End If
            sql = New SqlCommand("delete from tbl_barang where id_barang='" & idbarang.Text & "'", konek)
            sql.ExecuteNonQuery()
            tampil()
            MsgBox("Data berhasil dihapus")
            clear()
            id()
        End If
    End Sub

    Private Sub Guna2TextBox6_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox6.TextChanged
        da = New SqlDataAdapter("select * from tbl_barang where nama_barang like '" & Guna2TextBox6.Text & "%'", konek)
        dt = New DataTable
        da.Fill(dt)
        dgv.DataSource = dt
    End Sub

    Private Sub jumlah_KeyPress(sender As Object, e As KeyPressEventArgs) Handles jumlah.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then
            e.Handled = True
        End If
    End Sub

    Private Sub harga_KeyPress(sender As Object, e As KeyPressEventArgs) Handles harga.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then
            e.Handled = True
        End If
    End Sub

    Private Sub satuan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles satuan.SelectedIndexChanged
        harga.Focus()
    End Sub

    Private Sub Guna2Button5_Click(sender As Object, e As EventArgs) Handles Guna2Button5.Click
        Dim waktu As String = Format(Now, "yyyy/MM/dd HH:mm:ss")
        sql = New SqlCommand("insert into tbl_log values('" & waktu & "','" & Guna2Button5.Text & "','" & sid & "')", konek)
        sql.ExecuteNonQuery()
        Form1.Show()
        Me.Close()
    End Sub

    Private Sub harga_KeyUp(sender As Object, e As KeyEventArgs) Handles harga.KeyUp
        value = harga.Text.Replace("Rp. ", "")
        value = value.Replace(",00", "")
        value = value.Replace(".", "")
        harga.Text = "Rp. " & Val(value).ToString("N0")
        harga.SelectionStart = harga.TextLength
    End Sub
End Class