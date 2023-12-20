Imports System.Data.SqlClient
Public Class Form1
    Dim sql As SqlCommand
    Dim da As SqlDataAdapter
    Dim dr As SqlDataReader
    Dim dt As DataTable
    Dim waktu As String = Format(Now, "yyyy/MM/dd HH:mm:ss")
    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        If user.Text.Length = 0 Then
            MessageBox.Show("Masukan username anda", "Form Login", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf pass.Text.Length = 0 Then
            MessageBox.Show("Masukan password anda", "Form Login", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            sql = New SqlCommand("select *from tbl_user where username='" & user.Text & "' and password='" & password(pass.Text) & "'", konek)
            dr = sql.ExecuteReader
            dr.Read()
            If dr.HasRows Then
                If dr.Item("tipe_user") = "Admin" Then
                    sid = dr.Item("id_user")
                    Form2.Show()
                    Me.Hide()
                ElseIf dr.Item("tipe_user") = "Gudang" Then
                    sid = dr.Item("id_user")
                    Form5.Show()
                    Me.Hide()
                Else
                    sid = dr.Item("id_user")
                    sn = dr.Item("nama")
                    Form6.Show()
                    Me.Hide()
                End If
                sql = New SqlCommand("insert into tbl_log values('" & waktu & "','" & Guna2Button1.Text & "','" & sid & "')", konek)
                sql.ExecuteNonQuery()
            Else
                MessageBox.Show("Username atau password tidak sesuai", "Form Login", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        pass.Clear()
        user.Clear()
    End Sub
End Class
