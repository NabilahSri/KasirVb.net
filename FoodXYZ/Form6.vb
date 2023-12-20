Imports System.Data.SqlClient
Public Class Form6
    Dim sql As SqlCommand
    Dim da As SqlDataAdapter
    Dim dr As SqlDataReader
    Dim dt As DataTable
    Dim t As Integer
    Dim value As String
    Private Sub Guna2Button5_Click(sender As Object, e As EventArgs) Handles Guna2Button5.Click
        Dim waktu As String = Format(Now, "yyyy/MM/dd HH:mm:ss")
        sql = New SqlCommand("insert into tbl_log values('" & waktu & "','" & Guna2Button5.Text & "','" & sid & "')", konek)
        sql.ExecuteNonQuery()
        Form1.Show()
        Me.Close()
    End Sub

    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        nmkasir.Text = sn
        tgltrans.Text = Format(Now, "yyyy/MM/dd")
        sql = New SqlCommand("select nama_barang,kode_barang from tbl_barang", konek)
        dr = sql.ExecuteReader
        While dr.Read()
            menu.Items.Add(dr.Item("kode_barang") & "-" & dr.Item("nama_barang"))
        End While
        dgv.Columns.Clear()
    End Sub

    Private Sub menu_SelectedIndexChanged(sender As Object, e As EventArgs) Handles menu.SelectedIndexChanged
        dgv.DataSource = Nothing
        Dim kb As String
        Dim nb As String
        Dim hasil() As String
        hasil = Split(menu.Text, "-")
        kb = hasil(0)
        'nb = hasil(1)
        notrans.Text = Format(Now, "yyyyMMddHHmmss")
        sql = New SqlCommand("select id_transaksi from tbl_transaksi order by id_transaksi desc", konek)
        dr = sql.ExecuteReader
        dr.Read()
        If dr.HasRows Then
            Dim urut As Integer
            urut = Val(Mid(dr.Item("id_transaksi").ToString, 4, 4)) + 1
            idtrans.Text = "TRS" + urut.ToString("000")
        Else
            idtrans.Text = "TRS001"
        End If
        sql = New SqlCommand("select id_barang,harga_satuan from tbl_barang where kode_barang='" & kb & "'", konek)
        dr = sql.ExecuteReader
        dr.Read()
        If dr.HasRows Then
            harga.Text = FormatCurrency(dr.Item("harga_satuan"))
            idbarang.Text = dr.Item("id_barang")
        End If
        With dgv
            .ColumnCount = 6
            .Columns(0).Name = "Kode Barang"
            .Columns(1).Name = "Nama Barang"
            .Columns(2).Name = "Harga Satuan"
            .Columns(3).Name = "Quantitas"
            .Columns(4).Name = "Subtotal"
            .Columns(5).Name = "id barang"
            .Columns(5).Visible = False
        End With
    End Sub

    Private Sub qty_KeyUp(sender As Object, e As KeyEventArgs) Handles qty.KeyUp
        th.Text = FormatCurrency(Val(qty.Text) * harga.Text)
    End Sub

    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        Dim kb As String
        Dim nb As String
        Dim hasil() As String
        hasil = Split(menu.Text, "-")
        kb = hasil(0)
        nb = hasil(1)
        Dim cek = False
        Dim tot As Integer
        For Each row As DataGridViewRow In dgv.Rows
            If row.Cells(0).Value = kb Then
                cek = True
            End If
        Next
        If cek = False Then
            dgv.Rows.Add(1)
            dgv.Rows(dgv.RowCount - 2).Cells(0).Value = kb
            dgv.Rows(dgv.RowCount - 2).Cells(1).Value = nb
            dgv.Rows(dgv.RowCount - 2).Cells(2).Value = harga.Text
            dgv.Rows(dgv.RowCount - 2).Cells(3).Value = qty.Text
            dgv.Rows(dgv.RowCount - 2).Cells(4).Value = th.Text
            dgv.Rows(dgv.RowCount - 2).Cells(5).Value = idbarang.Text
        End If
        For Each row As DataGridViewRow In dgv.Rows
            If row.Cells(0).Value = kb And cek = True Then
                row.Cells(3).Value = Val(qty.Text)
                row.Cells(4).Value = harga.Text
            End If
            tot += row.Cells(4).Value
        Next
        tb.Text = FormatCurrency(tot)
        t = tot
        MsgBox("Data ditambahkan")
        idbarang.Clear()
        menu.Text = Nothing
        harga.Clear()
        qty.Clear()
        th.Clear()
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        idbarang.Clear()
        menu.Text = Nothing
        harga.Clear()
        qty.Clear()
        th.Clear()
        notrans.Clear()
        idtrans.Clear()
        tb.Text = Nothing
        kembalian.Text = Nothing
        dgv.Columns.Clear()
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        If Val(value) >= Val(t) Then
            For Each row As DataGridViewRow In dgv.Rows
                sql = New SqlCommand("update tbl_barang set jumlah_barang=jumlah_barang-" & Val(row.Cells(3).Value) & " where id_barang='" & row.Cells(5).Value & "'", konek)
                sql.ExecuteNonQuery()
            Next
            kembalian.Text = FormatCurrency(Val(value) - Val(t))
            MsgBox("Transkasi berhasil")
        Else
            MsgBox("Transkasi gagal")
        End If
    End Sub

    Private Sub bayar_KeyUp(sender As Object, e As KeyEventArgs) Handles bayar.KeyUp
        value = bayar.Text.Replace("Rp. ", "")
        value = value.Replace(",00", "")
        value = value.Replace(".", "")
        bayar.Text = "Rp. " & Val(value).ToString("N0")
        bayar.SelectionStart = bayar.TextLength
    End Sub

    Private Sub Guna2Button6_Click(sender As Object, e As EventArgs) Handles Guna2Button6.Click
        Form7.Show()
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        sql = New SqlCommand("insert into tbl_transaksi values('" & idtrans.Text & "','" & notrans.Text & "','" & tgltrans.Text & "','" & sn & "','" & t & "','" & sid & "')", konek)
        sql.ExecuteNonQuery()
        For i As Integer = 0 To dgv.Rows.Count - 2
            sql = New SqlCommand("insert into tbl_detail values('" & dgv.Rows(i).Cells(1).Value & "','" & dgv.Rows(i).Cells(3).Value & "','" & dgv.Rows(i).Cells(5).Value & "','" & idtrans.Text & "')", konek)
            sql.ExecuteNonQuery()
        Next
        MsgBox("Data disimpan")
    End Sub

    Private Sub Guna2TextBox1_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox1.TextChanged
        If Guna2TextBox1.Text.Length = 0 Then
            dgv.DataSource = Nothing
            dgv.Columns.Clear()
        Else
            sql = New SqlCommand("select*from tbl_transaksi inner join tbl_detail on tbl_transaksi.id_transaksi=tbl_detail.id_transaksi inner join tbl_barang on tbl_detail.id_barang=tbl_barang.id_barang where no_transaksi like '%" & Guna2TextBox1.Text & "%'", konek)
            dr = sql.ExecuteReader
            dr.Read()
            If dr.HasRows Then
                With dgv
                    .ColumnCount = 6
                    .Columns(0).Name = "Kode Barang"
                    .Columns(1).Name = "Nama Barang"
                    .Columns(2).Name = "Harga Satuan"
                    .Columns(3).Name = "Quantitas"
                    .Columns(4).Name = "Subtotal"
                    .Columns(5).Name = "id barang"
                    .Columns(5).Visible = False
                End With
                dgv.Rows.Add(1)
                dgv.Rows(dgv.RowCount - 2).Cells(0).Value = dr.Item("kode_barang")
                dgv.Rows(dgv.RowCount - 2).Cells(1).Value = dr.Item("nama_barang")
                dgv.Rows(dgv.RowCount - 2).Cells(2).Value = dr.Item("harga_satuan")
                dgv.Rows(dgv.RowCount - 2).Cells(3).Value = dr.Item("jumlah_barang")
                dgv.Rows(dgv.RowCount - 2).Cells(4).Value = Val(dr.Item("harga_satuan")) * Val(dr.Item("jumlah_barang"))
                dgv.Rows(dgv.RowCount - 2).Cells(5).Value = dr.Item("id_barang")
            End If
        End If
    End Sub
End Class