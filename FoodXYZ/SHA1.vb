Imports System.Security.Cryptography
Imports System.Text

Module SHA1
    Public Function password(ByVal input As String) As String
        Dim sha1 As New SHA1CryptoServiceProvider
        Dim inputByte As Byte() = Encoding.UTF8.GetBytes(input)
        Dim hashByte As Byte() = sha1.ComputeHash(inputByte)
        Dim hashString As String = BitConverter.ToString(hashByte).Replace("-", "")
        Return hashString
    End Function
End Module
