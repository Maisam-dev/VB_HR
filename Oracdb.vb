Imports System.Data.OleDb
Imports Oracle.ManagedDataAccess.Client

Public Class Oracdb
    Private oradb As String = "Data Source = svatlihrl03.vog.local:1521/SAT.voglinz-test.wamas.com;Persist Security Info=True;User ID=wamas;Password=wamas"
    Private conn As New OracleConnection(oradb)

    '  Public Sub New()

    '  End Sub


    Public Function insertArtikelIn(table As Hashtable) As Boolean
        If table.Count < 1 Then
            Return False
        End If
        Dim query As String = " insert all "
        Dim values As String = Nothing
        Dim into As String = Nothing
        Dim cmd As New OracleCommand
        Dim Trans As OracleTransaction = Nothing

        For row As Integer = 0 To table.Count - 1
            values = "('" &
                      table(row)("ID") & "','" &
                      table(row)("BIB") & "','" &
                      table(row)("ARTIKELID") & "','" &
                      table(row)("TELEGRAMTYPE") & "','" &
                      "0'," &
                      "sysdate," &
                      "sysdate)"

            If Not table(row)("TELEGRAMTYPE").Equals("LÖSCHUNG") Then
                values = values.Substring(0, values.Length - 1) & ",'"
                values += table(row)("BEZEICHNUNG") & "','" &
                      table(row)("VECOLLI") & "','" &
                      table(row)("COLLIPALETTE") & "','" &
                      table(row)("COLLIEAN") & "','" &
                      table(row)("FIFO") & "','" &
                      table(row)("GEWICHT_KARTON") & "','" &
                      table(row)("MHDPFLICHT") & "','" &
                      table(row)("AVISOPFLICHT") & "','" &
                      table(row)("CHARGENPFLICHT") & "','" &
                      table(row)("HOCHREGALFAEHIG") & "','" &
                      table(row)("LAGEROPTION") & "'" &
                      ")"
            End If

            into = "
                     INTO ARTIKELIN ( 
                                     MESSAGEID,
                                     BIB,
                                     ARTIKELID,
                                     TELEGRAMTYPE,
                                     TELEGRAMSTATE,
                                     CREATED,
                                     UPDATED)"

            If Not table(row)("TELEGRAMTYPE").Equals("LÖSCHUNG") Then
                into = into.Substring(0, into.Length - 1) & ","
                into += "
                                     BEZEICHNUNG,
                                     VECOLLI ,
                                     COLLIPALETTE,
                                     COLLIEAN,
                                     FIFO,
                                     GEWICHT_KARTON,
                                     MHDPFLICHT,
                                     AVISOPFLICHT,
                                     CHARGENPFLICHT,
                                     HOCHREGALFAEHIG,
                                     lageroption
                                     )"
            End If

            into += " VALUES " & values
            query += into
        Next

        query += "select 1 from Dual"

        Try
            conn.Open()
            Trans = conn.BeginTransaction()
            cmd.Connection = conn
            cmd.CommandText = query
            cmd.CommandType = CommandType.Text
            cmd.ExecuteNonQuery()
            Trans.Commit()
            conn.Close()
        Catch ex As Exception
            Trans.Rollback()
            conn.Close()
            ' MsgBox(ex.Message)
            Console.WriteLine(ex.Message)
            Return False
        End Try
        Return True
    End Function


    Public Function getTable(query As String) As Hashtable

        Dim cmd As New OracleCommand
        Dim reader As OracleDataReader
        Dim ht As New Hashtable()
        Dim htrow As New Hashtable()
        Dim rowNr As Integer = 0
        Dim ret = Nothing
        If Not String.IsNullOrEmpty(query) Then
            Try
                conn.Open()
                cmd.Connection = conn
                cmd.CommandText = query
                cmd.CommandType = CommandType.Text
                reader = cmd.ExecuteReader()
                If reader.HasRows Then
                    While reader.Read
                        For i As Integer = 0 To reader.FieldCount - 1
                            htrow.Add(reader.GetName(i), reader.Item(i))
                        Next
                        ht.Add(rowNr, htrow.Clone)
                        htrow.Clear()
                        rowNr += 1
                    End While
                    conn.Close()
                End If
            Catch ex As Exception
                'MsgBox(ex.Message)
                Console.WriteLine(ex.Message)
                conn.Close()
                ret = Nothing
            Finally
                conn.Close()
            End Try
        End If
        ret = ht
        Return ret

    End Function

    Public Function updateTable(TableName As String, spalteStatus As String, wert As Integer, values As String, Optional cu_Time As Boolean = False) As Boolean ' todo update  set updated spalte = sysdate

        Dim cmd As New OracleCommand("update " & TableName & " set " & spalteStatus & " = " & wert & " where MESSAGEID in (" & values & ")", conn)

        If Not String.IsNullOrEmpty(values) Then
            Try
                cmd.CommandType = CommandType.Text
                conn.Open()
                cmd.ExecuteNonQuery()
                conn.Close()
            Catch ex As Exception
                Console.Beep()
                Console.WriteLine(ex.Message)
                conn.Close()
                Return False
            End Try
            Return True
        End If

    End Function


    Public Function getOrderIdUndMessegId(Table As Hashtable) As Hashtable

        Dim MESSAGEIDFehle As String = " "
        Dim ORDERIDFehle As String = " "
        Dim MESSAGEIDOhneFehle As String = " "
        Dim ORDERIDOhneFehle As String = " "

        Dim messegIDTable As New Hashtable()

        If Not IsNothing(Table) Then
            For row As Integer = 0 To Table.Count - 1
                If Not String.IsNullOrEmpty(Table(row)("ERRORID").ToString) Then
                    MESSAGEIDFehle += Table(row)("MESSAGEID") & ","
                    ORDERIDFehle += Table(row)("ORDERID") & ","
                Else
                    MESSAGEIDOhneFehle += Table(row)("MESSAGEID") & ","
                    ORDERIDOhneFehle += Table(row)("ORDERID") & ","
                End If
            Next

            messegIDTable.Add("MESSAGEIDFehle", MESSAGEIDFehle.Substring(0, MESSAGEIDFehle.Length - 1))
            messegIDTable.Add("ORDERIDFehle", ORDERIDFehle.Substring(0, ORDERIDFehle.Length - 1))
            messegIDTable.Add("MESSAGEIDOhneFehle", MESSAGEIDOhneFehle.Substring(0, MESSAGEIDOhneFehle.Length - 1))
            messegIDTable.Add("ORDERIDOhneFehle", ORDERIDOhneFehle.Substring(0, ORDERIDOhneFehle.Length - 1))
            Return messegIDTable
        End If

    End Function

    Public Function insertInTable(fromTable As Hashtable, ToTable As String, Optional cu_Time As Boolean = False) As Boolean

        If IsNothing(fromTable) Then
            Return False
        End If
        Dim Trans As OracleTransaction = Nothing
        Dim cmd As New OracleCommand()
        Dim sql = " insert All "
        Dim ret = False
        Try
            For row As Integer = 0 To fromTable.Count - 1
                Dim col = " into  " & ToTable & " ( "
                Dim values = " values ('"

                For Each ModKey As DictionaryEntry In fromTable(row)
                    col += ModKey.Key.ToString & ","
                    values += fromTable(row)(ModKey.Key.ToString) & "','"
                Next
                If cu_Time Then
                    col += " created, updated,"
                    values = values.Substring(0, values.Length - 1) & " sysdate, sysdate ,"
                End If

                col = col.Substring(0, col.Length - 1) & ")"
                values = values.Substring(0, values.Length - 2) & ")"

                sql += col & values
                col = ""
                values = ""
            Next

            sql += " SELECT 1 FROM Dual "

            'schreib in DB
            conn.Open()
            Trans = conn.BeginTransaction()
            cmd.Connection = conn
            cmd.CommandText = sql
            cmd.CommandType = CommandType.Text
            cmd.ExecuteNonQuery()
            Trans.Commit()
            conn.Close()
            ret = True
        Catch ex As Exception
            Trans.Rollback()
            conn.Close()
            ' MsgBox(ex.Message)
            Console.WriteLine(ex.Message)
            ret = False
        Finally
            conn.Close()
        End Try
        Return ret

    End Function


End Class
