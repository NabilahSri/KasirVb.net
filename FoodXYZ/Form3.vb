Imports System.Data.SqlClient
Public Class Form3
    Dim sql As SqlCommand
    Dim da As SqlDataAdapter
    Dim dr As SqlDataReader
    Dim dt As DataTable
    Dim upload = False
    Dim fname As String
    Dim fs As System.IO.FileStream
    Sub tampil()
        da = New SqlDataAdapter("select * from tbl_user", konek)
        dt = New DataTable
        da.Fill(dt)
        dgv.DataSource = dt
    End Sub
    Sub id()
        sql = New SqlCommand("select id_user from tbl_user order by id_user desc", konek)
        dr = sql.ExecuteReader
        dr.Read()
        If dr.HasRows Then
            Dim urut As Integer
            urut = Val(dr.Item("id_user")) + 1
            iduser.Text = urut
        Else
            iduser.Text = "1"
        End If
    End Sub
    Sub clear()
        iduser.Clear()
        nama.Clear()
        alamat.Clear()
        telp.Clear()
        user.Clear()
        pass.Clear()
        tipe.Text = Nothing
        img.Image = Nothing
    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        If tipe.Text.Length = 0 Then
            MessageBox.Show("Pilih Tipe", "Kelola User", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf nama.Text.Length = 0 Then
            MessageBox.Show("Masukan Nama", "Kelola User", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf telp.Text.Length = 0 Then
            MessageBox.Show("Masukan No telepon", "Kelola User", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf alamat.Text.Length = 0 Then
            MessageBox.Show("Masukan Alamat", "Kelola User", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf user.Text.Length = 0 Then
            MessageBox.Show("Masukan Username", "Kelola User", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf pass.Text.Length = 0 Then
            MessageBox.Show("Masukan Password", "Kelola User", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf img.Image Is Nothing Then
            MessageBox.Show("Pilih Foto", "Kelola User", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            fname = iduser.Text & ".jpg"
            Dim folder As String = "C:\Users\DPIB\source\repos\FoodXYZ\FoodXYZ\img"
            Dim path As String = System.IO.Path.Combine(folder, fname)
            img.Image.Save(path)
            sql = New SqlCommand("select username from tbl_user where username='" & user.Text & "'", konek)
            dr = sql.ExecuteReader
            dr.Read()
            If dr.HasRows Then
                MsgBox("Username telah digunaka")
            Else
                sql = New SqlCommand("insert into tbl_user values('" & tipe.Text & "','" & nama.Text & "','" & alamat.Text & "','" & user.Text & "','" & telp.Text & "','" & password(pass.Text) & "','" & fname & "')", konek)
                sql.ExecuteNonQuery()
                tampil()
                MsgBox("Data berhasil disimpan")
                clear()
                id()
            End If
        End If
    End Sub

    Private Sub dgv_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgv.CellMouseClick
        iduser.Text = dgv.Rows(e.RowIndex).Cells("id_user").Value
        nama.Text = dgv.Rows(e.RowIndex).Cells("nama").Value
        alamat.Text = dgv.Rows(e.RowIndex).Cells("alamat").Value
        telp.Text = dgv.Rows(e.RowIndex).Cells("telepon").Value
        user.Text = dgv.Rows(e.RowIndex).Cells("username").Value
        tipe.Text = dgv.Rows(e.RowIndex).Cells("tipe_user").Value
        img.Text = dgv.Rows(e.RowIndex).Cells("gambar").Value.ToString
        If Not IsDBNull(dgv.Rows(e.RowIndex).Cells("gambar").Value) Then
            fname = iduser.Text & ".jpg"
            Dim folder As String = "C:\Users\DPIB\source\repos\FoodXYZ\FoodXYZ\img"
            Dim path As String = System.IO.Path.Combine(folder, fname)
            If (System.IO.File.Exists(path)) Then
                fs = New System.IO.FileStream(path, IO.FileMode.Open, IO.FileAccess.Read)
                img.Image = System.Drawing.Image.FromStream(fs)
                fs.Close()
            End If
        End If
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        fname = iduser.Text & ".jpg"
        If upload = True Then
            Dim folder As String = "C:\Users\DPIB\source\repos\FoodXYZ\FoodXYZ\img"
            Dim path As String = System.IO.Path.Combine(folder, fname)
            img.Image.Save(path)
        End If
        upload = False
        If pass.Text.Length = 0 Then
            sql = New SqlCommand("update tbl_user set tipe_user='" & tipe.Text & "',nama='" & nama.Text & "',alamat='" & alamat.Text & "',username='" & user.Text & "',telepon='" & telp.Text & "',gambar='" & fname & "' where id_user='" & iduser.Text & "'", konek)
        Else
            sql = New SqlCommand("update tbl_user set tipe_user='" & tipe.Text & "',nama='" & nama.Text & "',alamat='" & alamat.Text & "',username='" & user.Text & "',telepon='" & telp.Text & "',password='" & password(pass.Text) & "',gambar='" & fname & "' where id_user='" & iduser.Text & "'", konek)
        End If
        sql.ExecuteNonQuery()
        tampil()
        MsgBox("Data berhasil diedit")
        clear()
        id()
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        If MsgBox("Yakin Hapus data ini ?", vbYesNo + 32, "Hapus") = vbYes Then
            fname = iduser.Text & ".jpg"
            Dim folder As String = "C:\Users\DPIB\source\repos\FoodXYZ\FoodXYZ\img"
            Dim path As String = System.IO.Path.Combine(folder, fname)
            If (System.IO.File.Exists(path)) Then
                System.IO.File.Delete(path)
            End If
            sql = New SqlCommand("delete from tbl_user where id_user='" & iduser.Text & "'", konek)
            sql.ExecuteNonQuery()
            tampil()
            MsgBox("Data berhasil dihapus")
            clear()
            id()
        End If
    End Sub

    Private Sub Guna2TextBox6_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox6.TextChanged
        da = New SqlDataAdapter("select * from tbl_user where nama like '" & Guna2TextBox6.Text & "%'", konek)
        dt = New DataTable
        da.Fill(dt)
        dgv.DataSource = dt
    End Sub

    Private Sub telp_KeyPress(sender As Object, e As KeyPressEventArgs) Handles telp.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then
            e.Handled = True
        End If
    End Sub

    Private Sub tipe_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tipe.SelectedIndexChanged
        nama.Focus()
    End Sub
End Class