Imports System.Data.SqlClient
Imports Microsoft.Office.Interop.Excel

Public Class Form4
    Dim sql As SqlCommand
    Dim da As SqlDataAdapter
    Dim dr As SqlDataReader
    Dim dt As System.Data.DataTable
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        da = New SqlDataAdapter("select tgl_transaksi as Tanggal_Transaksi,total_bayar as Total_Pembayaran,nama as Nama_kasir from tbl_transaksi inner join tbl_user on tbl_transaksi.id_user=tbl_user.id_user", konek)
        dt = New System.Data.DataTable
        da.Fill(dt)
        dgv.DataSource = dt
    End Sub

    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        da = New SqlDataAdapter("select tgl_transaksi as Tanggal_Transaksi,total_bayar as Total_Pembayaran,nama as Nama_kasir from tbl_transaksi inner join tbl_user on tbl_transaksi.id_user=tbl_user.id_user where tgl_transaksi between '" & awal.Text & "' and '" & akhir.Text & "'", konek)
        dt = New System.Data.DataTable
        da.Fill(dt)
        If dt.Rows.Count < 1 Then
            MessageBox.Show("Data tidak ditemukan", "Kelola Laporan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            dgv.DataSource = dt
        End If
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        sql = New SqlCommand("select format(tbl_transaksi.tgl_transaksi,'yyyy/MM/dd') as date,sum(tbl_transaksi.total_bayar) as harga from tbl_transaksi where tgl_transaksi between '" & awal.Text & "' and '" & akhir.Text & "' group by format(tbl_transaksi.tgl_transaksi,'yyyy/MM/dd')", konek)
        dr = sql.ExecuteReader
        Dim i As Integer
        Chart1.Series.Clear()
        While dr.Read()
            Chart1.Series.Add("Omset")
            Chart1.Series(i).Points.AddXY(dr.Item("date"), dr.Item("harga"))
        End While
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Dim excelApp As New Application
        Dim workbook As Workbook
        Dim worksheet As Worksheet
        workbook = excelApp.Workbooks.Add(Type.Missing)
        worksheet = workbook.ActiveSheet

        worksheet.Cells(1, 1) = "ID Transaksi"
        worksheet.Cells(1, 2) = "No Transaksi"
        worksheet.Cells(1, 3) = "Tanggal Transaksi"
        worksheet.Cells(1, 4) = "Nama Barang"
        worksheet.Cells(1, 5) = "Quantitas"

        sql = New SqlCommand("select * from tbl_transaksi inner join tbl_detail on tbl_transaksi.id_transaksi=tbl_detail.id_transaksi", konek)
        dr = sql.ExecuteReader
        Dim x As Integer = 2
        While dr.Read()
            worksheet.Cells(x, 1) = dr.Item("id_transaksi")
            worksheet.Cells(x, 2) = dr.Item("no_transaksi")
            worksheet.Cells(x, 3) = dr.Item("tgl_transaksi")
            worksheet.Cells(x, 4) = dr.Item("nama_barang")
            worksheet.Cells(x, 5) = dr.Item("jumlah_barang")
            x += 1
        End While
        Dim savedialog As New SaveFileDialog()
        savedialog.Filter = "Excel files (*.xlsx)|*.xlsx"
        savedialog.FilterIndex = 2
        savedialog.RestoreDirectory = True

        If savedialog.ShowDialog = DialogResult.OK Then
            Dim filename As String
            filename = savedialog.FileName
            workbook.SaveAs(filename)
            MsgBox("Excel berhasil disimpan")
        End If
        excelApp.Quit()
    End Sub
End Class