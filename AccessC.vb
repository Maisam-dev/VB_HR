Imports System.Data.OleDb
Public Class AccessC

    Private query As String = Nothing
    Private Acsdb As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\10.41.1.40\lagerverwaltung\VogHrl.accdb") 'TODO 



    'Public Sub New(sql As String)
    '    query = sql
    'End Sub



    Public Function gettable(query As String) As Hashtable
        Dim ret = Nothing
        Dim ht As New Hashtable()
        Dim htrow As New Hashtable()
        Dim rowNr As Integer = 0
        If Not String.IsNullOrWhiteSpace(query) Then
            Try
                Acsdb.Open()
                Dim Commd As New OleDbCommand(query, Acsdb)
                Dim reader As OleDbDataReader
                reader = Commd.ExecuteReader()
                If reader.HasRows Then

                    While reader.Read
                        For i As Integer = 0 To reader.FieldCount - 1
                            htrow.Add(reader.GetName(i), reader.Item(i))
                        Next
                        ht.Add(rowNr, htrow.Clone)
                        htrow.Clear()
                        rowNr += 1
                    End While
                End If
                ret = ht
                Acsdb.Close()
            Catch ex As Exception
                'MsgBox(ex.Message)
                Console.WriteLine(ex.Message)
                Acsdb.Close()
                ret = Nothing
            End Try
        End If
        Return ret

    End Function


    Public Function updateTable(table As String, statusSpalte As String, status As Integer, whereSpalte As String, values As String) As Boolean

        Dim ret = False
        If Not String.IsNullOrWhiteSpace(values) Then
            Try
                Acsdb.Open()
                Dim query As String = "update " & table & " set " & statusSpalte & " = " & status & " where " & whereSpalte & " in (" & values & ") and " & statusSpalte & " <> 12 " ' TODO
                Dim Commd As New OleDbCommand(query, Acsdb)
                Commd.ExecuteNonQuery()
                ret = True
                Acsdb.Close()
            Catch ex As Exception
                'MsgBox(ex.Message)
                Console.WriteLine(ex.Message)
                Acsdb.Close()
                ret = False
            Finally
                Acsdb.Close()
            End Try
        End If
        Return ret

    End Function

    Public Function getIdlistFromTable(Table As Hashtable, IsSpalte As String) As String
        Dim IdList As String = " "

        If Table.Count > 0 Then
            For row As Integer = 0 To Table.Count - 1
                IdList += Table(row)(IsSpalte) & ","
            Next
            IdList = IdList.Substring(0, IdList.Length - 1)
        End If

        Return IdList
    End Function

    'Public Function insertInTable(fromTable As Hashtable, ToTable As String) As Boolean 'todo
    '    If IsNothing(fromTable) Then
    '        Return False
    '    End If
    '    Dim Trans As OleDbTransaction = Nothing
    '    Dim cmd As New OleDbCommand()
    '    Dim sql = " insert all "
    '    Dim ret = False
    '    Try
    '        For row As Integer = 0 To fromTable.Count - 1
    '            Dim col = " into  " & ToTable & " ( "
    '            Dim values = " values ('"

    '            For Each ModKey As DictionaryEntry In fromTable(row)
    '                col += ModKey.Key.ToString & ","
    '                values += fromTable(row)(ModKey.Key.ToString) & "','"
    '            Next

    '            col = col.Substring(0, col.Length - 1) & ")"
    '            values = values.Substring(0, values.Length - 2) & ")"
    '            sql += col & values
    '            col = ""
    '            values = ""
    '        Next

    '        sql += " SELECT 1 FROM Dual "

    '        'schreib in DB
    '        Acsdb.Open()
    '        ' Trans = Acsdb.BeginTransaction()
    '        cmd.Connection = Acsdb
    '        cmd.CommandText = sql
    '        cmd.CommandType = CommandType.Text
    '        cmd.ExecuteNonQuery()
    '        ' Trans.Commit()

    '        ret = True
    '    Catch ex As Exception
    '        '  Trans.Rollback()
    '        'MsgBox(ex.Message)
    '        Console.WriteLine(ex.Message)
    '        ret = False
    '    Finally
    '        Acsdb.Close()
    '    End Try
    '    Return ret

    'End Function



    Public Function insertInTable(fromTable As Hashtable, ToTable As String) As Boolean

        If IsNothing(fromTable) Or Not fromTable.Count > 0 Then
            Return False
        End If
        Dim Trans As OleDbTransaction = Nothing
        Dim cmd As New OleDbCommand()
        Dim sql = " insert into " & ToTable
        Dim col As String = " (["
        Dim values As String = "(@"
        Dim ret = False
        Try
            'schreib in DB
            Acsdb.Open()
            cmd.Connection = Acsdb
            cmd.CommandType = CommandType.Text
            Trans = Acsdb.BeginTransaction()
            For row As Integer = 0 To fromTable.Count - 1

                For Each ModKey As DictionaryEntry In fromTable(row)
                    If row = 0 Then
                        col += ModKey.Key.ToString & "],["
                        values += ModKey.Key.ToString & ", @"
                    End If

                    cmd.Parameters.AddWithValue("@" & ModKey.Key.ToString, fromTable(row)(ModKey.Key.ToString))
                Next
                If row = 0 Then
                    col = col.Substring(0, col.Length - 2) & ")"
                    values = values.Substring(0, values.Length - 3) & ")"

                    sql += col & " values " & values
                    cmd.CommandText = sql
                    cmd.Transaction = Trans
                End If

                cmd.ExecuteNonQuery()
            Next

            Trans.Commit()
            ret = True

        Catch ex As Exception
            Trans.Rollback()
            'MsgBox(ex.Message)
            Console.WriteLine(ex.Message)
            ret = False
        Finally
            Acsdb.Close()
        End Try
        Return ret

    End Function

End Class
