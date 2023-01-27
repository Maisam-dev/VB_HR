Imports System.Data.OleDb
Imports Oracle.ManagedDataAccess.Client

Public Class Oracdb
    Private oradb As String
    Public conn
    '
    Public Sub New(server As String, user As String, pass As String)
        oradb = "Data Source = " & server & ";Persist Security Info=True;User ID=" & user & ";Password=" & pass
        conn = New OracleConnection(oradb)
    End Sub

    '
    ' 
    '
    Public Function insertArtikelIn(table As Hashtable) As Boolean
        Dim ret As Boolean = False
        Dim rund As Integer = Math.Round(table.Count / 1000 + 0.5)
        Dim start As Integer = 0
        Dim finsh As Integer = 999
        If table.Count < 1 Then
            Return ret
        End If
        Dim query As String = Nothing
        Dim values As String = Nothing
        Dim into As String = Nothing
        Dim cmd As New OracleCommand
        Dim Trans As OracleTransaction = Nothing

        ' Begin:Artikel count teilen
        Try
            conn.Open()
            Trans = conn.BeginTransaction()
            For r As Integer = 0 To rund
                rund -= 1
                If rund < 0 Then
                    Exit For
                End If
                query = " insert all "
                If rund = 0 Then
                    finsh = table.Count - 1
                End If
                'End: Artikel count teilen
                For row As Integer = start To finsh
                    values = "('" &
                      table(row)("ID") & "','" &
                      table(row)("BIB") & "','" &
                      table(row)("ARTIKELID") & "','" &
                      table(row)("TELEGRAMTYPE") & "','" &
                      table(row)("ABCKLASSE") & "','" &
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
                      table(row)("LAGEROPTION") & "','" &
                      table(row)("ABCKLASSE") & "'" &
                      ")"
                    End If

                    into = "
                     INTO ARTIKELIN ( 
                                     MESSAGEID,
                                     BIB,
                                     ARTIKELID,
                                     TELEGRAMTYPE,
                                     ABCKLASSE,
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
                                     lageroption,
                                     ABCKLASSE
                                     )"
                    End If

                    into += " VALUES " & values
                    query += into
                Next

                query += "select 1 from Dual"
                cmd.Connection = conn
                cmd.CommandText = query
                cmd.CommandType = CommandType.Text
                cmd.ExecuteNonQuery()
                ' Begin:Artikel count teilen
                start = finsh + 1
                finsh += 1000
            Next
            Trans.Commit()
            conn.Close()
            ret = True
        Catch ex As Exception
            Trans.Rollback()
            conn.Close()
            If pt.put("Oracdb", "insertArtikelIn", query, ex.Message, Form1.loopCounter) = 2 Then
                ret = True
            End If
        End Try
        'End: Artikel count teilen
        Return ret
    End Function

    '
    '
    '
    '
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
                conn.Close()
                ret = Nothing
                pt.put("Oracdb", "getTable", query, ex.Message, Form1.loopCounter)
            Finally
                conn.Close()
            End Try
        End If
        ret = ht
        Return ret

    End Function
    '
    '
    '
    '
    '
    Public Function updateTable(TableName As String, spalteStatus As String, wert As Integer, values As String) As Boolean

        Dim Sql = "update " & TableName & " set " & spalteStatus & " = " & wert & " where MESSAGEID in " & values
        Dim cmd As New OracleCommand(Sql, conn)
        Dim ret As Boolean = False
        If Not String.IsNullOrEmpty(values) And Not values.Equals("()") And Not values.Contains("()") Then
            Try
                cmd.CommandType = CommandType.Text
                conn.Open()
                cmd.ExecuteNonQuery()
                conn.Close()
                ret = True
            Catch ex As Exception
                Console.Beep()
                Console.WriteLine(ex.Message)
                conn.Close()
                pt.put("Oracdb", "updateTable", Sql, ex.Message, Form1.loopCounter)
            End Try
        End If
        Return ret
    End Function

    '
    '
    '
    '
    '
    Public Function insertInTable(fromTable As Hashtable, ToTable As String, Optional cu_Time As Boolean = False) As Boolean

        If IsNothing(fromTable) Or fromTable.Count < 1 Then
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
            If pt.put("Oracdb", "insertInTable", sql, ex.Message, Form1.loopCounter) = 2 Then 'Unique Error ignorieren 
                ret = True
            End If
        Finally
            conn.Close()
        End Try
        Return ret

    End Function



    '
    '
    'status_8 bei True wird prüfen ob die Spalte "PalletId" bei Fehler nicht leer ist, wenn ja wird die Status auf 8 erstezen statt 12
    '
    Public Function getMsgIdAndOrderId(table As Hashtable, Optional status_8 As Boolean = False) As Hashtable

        Dim Linst As New Hashtable()
        Dim MESSAGEIDFehle As String = ""
        Dim ORDERIDFehle As String = ""
        Dim ORDERID As String = ""
        Dim MESSAGEID As String = ""
        Dim MESSAGEID_8 As String = ""
        Dim ORDERID_8 As String = ""
        Dim start As Integer = 0
        Dim end_ As Integer = 999
        Dim round As Integer = 0
        Dim orValcounter As Integer = 0


        If Not IsNothing(table) And Not table.Count < 1 Then

            MESSAGEIDFehle = "( "
            ORDERIDFehle = "( "
            MESSAGEID_8 = "( "
            ORDERID_8 = "( "
            ORDERID = "( "
            MESSAGEID = "( "
            round = Math.Round((table.Count / 1000) + 0.5)
            orValcounter = round

            For R As Integer = 1 To round
                orValcounter -= 1
                If orValcounter = 0 Then
                    end_ = table.Count - 1
                End If

                For row As Integer = start To end_

                    If table(row).ContainsKey("ERRORID") Then

                        If Not String.IsNullOrEmpty(table(row)("ERRORID").ToString) Then
                            ' prüfen die Palte id muss prüfen Table Name Move
                            If status_8 Then
                                If isPalletID(table(row)("PALLETID").ToString) Then
                                    MESSAGEID_8 += table(row)("MESSAGEID") & ","
                                    ORDERID_8 += table(row)("ORDERID") & ","
                                Else
                                    MESSAGEIDFehle += table(row)("MESSAGEID") & ","
                                    ORDERIDFehle += table(row)("ORDERID") & ","
                                End If
                            Else
                                MESSAGEIDFehle += table(row)("MESSAGEID") & ","
                                ORDERIDFehle += table(row)("ORDERID") & ","
                            End If
                        Else
                            MESSAGEID += table(row)("MESSAGEID") & ","
                            ORDERID += table(row)("ORDERID") & ","
                        End If
                    Else
                        MESSAGEID += table(row)("MESSAGEID") & ","
                        ORDERID += table(row)("ORDERID") & ","
                    End If
                Next

                MESSAGEIDFehle = closeVal(MESSAGEIDFehle)
                MESSAGEID = closeVal(MESSAGEID)
                MESSAGEID_8 = closeVal(MESSAGEID_8)
                ORDERIDFehle = closeVal(ORDERIDFehle)
                ORDERID = closeVal(ORDERID)
                ORDERID_8 = closeVal(ORDERID_8)

                If orValcounter > 0 Then
                    start += 1001
                    end_ += 1001
                    MESSAGEIDFehle += get_ORVal(MESSAGEIDFehle)
                    MESSAGEID += get_ORVal(MESSAGEID)
                    ORDERIDFehle += get_ORVal(ORDERIDFehle)
                    ORDERID += get_ORVal(ORDERID)
                    MESSAGEID_8 += get_ORVal(MESSAGEID_8)
                    ORDERID_8 += get_ORVal(ORDERID_8)
                End If
            Next
        End If

        Linst.Add("MESSAGEIDFehle", MESSAGEIDFehle)
        Linst.Add("ORDERIDFehle", ORDERIDFehle)
        Linst.Add("MESSAGEID", MESSAGEID)
        Linst.Add("ORDERID", ORDERID)
        Linst.Add("MESSAGEID_8", MESSAGEID_8)
        Linst.Add("ORDERID_8", ORDERID_8)
        Return Linst

    End Function
    '
    '
    '
    Public Function get_ORVal(Str As String) As String
        Dim ret As String = ""
        If Str.Equals("()") Then
            ret = "( "
        Else
            ret = " or MESSAGEID in ( "
        End If
        Return ret
    End Function
    '
    '
    '
    Public Function closeVal(str As String) As String
        Dim ret As String = ""
        If Not String.IsNullOrEmpty(str) Then
            ret = str.Substring(0, str.Length - 1) & ")"
        End If
        Return ret
    End Function
    '
    '
    '
    Public Function isPalletID(ID As String) As Boolean
        Return Not String.IsNullOrWhiteSpace(ID)
    End Function

End Class
