Imports System.Data.SqlClient
Public Class Form2
    Dim sql As SqlCommand
    Dim da As SqlDataAdapter
    Dim dr As SqlDataReader
    Dim dt As DataTable
    Dim waktu As String = Format(Now, "yyyy/MM/dd HH:mm:ss")
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        da = New SqlDataAdapter("select username as Username,waktu as Waktu,aktivitas as Aktivitas from tbl_log inner join tbl_user on tbl_log.id_user=tbl_user.id_user", konek)
        dt = New DataTable
        da.Fill(dt)
        dgv.DataSource = dt
    End Sub

    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        Dim waktu As String = Format(dtp.Value, "yyyy/MM/dd")
        da = New SqlDataAdapter("select username as Username,waktu as Waktu,aktivitas as Aktivitas from tbl_log inner join tbl_user on tbl_log.id_user=tbl_user.id_user where format(tbl_log.waktu,'yyyy/MM/dd')='" & waktu & "'", konek)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count < 1 Then
            MessageBox.Show("Data tidak ditemukan", "Form Admin", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            dgv.DataSource = dt
        End If
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Form3.TopLevel = False
        Guna2Panel2.Controls.Add(Form3)
        Form3.BringToFront()
        Form3.Show()
        Form4.Close()
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Form4.TopLevel = False
        Guna2Panel2.Controls.Add(Form4)
        Form4.BringToFront()
        Form4.Show()
        Form3.Close()
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        sql = New SqlCommand("insert into tbl_log values('" & waktu & "','" & Guna2Button3.Text & "','" & sid & "')", konek)
        sql.ExecuteNonQuery()
        Form1.Show()
        Me.Close()
    End Sub
End Class