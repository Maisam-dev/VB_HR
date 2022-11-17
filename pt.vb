Imports System.Data.OleDb

Public Class pt
    Inherits AccessC


    Public Shared Function put(className As String, funkName As String, parmeters As String, exMesseg As String, Runde As Integer)
        Dim ret As Boolean = False
        exMesseg = exMesseg.Replace(",", "_") ' todo in Patt ([,'"])
        exMesseg = exMesseg.Replace("'", "*")
        exMesseg = exMesseg.Replace(Chr(34), "*")

        parmeters = parmeters.Replace(",", "_") ' todo in Patt ([,'"])
        parmeters = parmeters.Replace("'", "*")
        parmeters = parmeters.Replace(Chr(34), "*")


        Dim sql As String = "insert into PT ( [className], [Funktion], [Parameters], [Messeg] ,[Runde]) values ( '" & className & "','" & funkName & "','" & parmeters & "','" & exMesseg & "','" & Runde & "')"
        Dim cmd As New OleDbCommand()

        Try
            If Acsdb.State = ConnectionState.Closed Then
                Acsdb.Open()
            End If
            cmd.Connection = Acsdb
            cmd.CommandType = CommandType.Text
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()
            ret = True
        Catch ex As Exception
            MsgBox(ex.Message & "// liefer Pars waren: " & sql)
        Finally
            Acsdb.Close()
        End Try
        Return ret

    End Function

End Class
