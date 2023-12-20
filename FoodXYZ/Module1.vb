Imports System.Data.SqlClient
Module Module1
    Dim conn As SqlConnection
    Public sn As String
    Public sid As Integer
    Public Function konek() As SqlConnection
        conn = New SqlConnection("data source=DESKTOP-0PCDDBH\SQLEXPRESS;initial catalog=FoodXYZ;integrated security=true")
        conn.Open()
        Return conn
    End Function
End Module
