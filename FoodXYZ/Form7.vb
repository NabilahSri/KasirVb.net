Imports CrystalDecisions.CrystalReports.Engine

Public Class Form7
    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim print As New ReportDocument
        print.Load("C:\Users\DPIB\source\repos\FoodXYZ\FoodXYZ\CrystalReport1.rpt")
        CrystalReportViewer1.ReportSource = print
        CrystalReportViewer1.SelectionFormula = "{tbl_transaksi.id_transaksi}='" & Form6.idtrans.Text & "'"
        CrystalReportViewer1.RefreshReport()
    End Sub
End Class