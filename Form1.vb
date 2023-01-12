Imports System.Data
Imports System.Data.OleDb
Imports Oracle.ManagedDataAccess.Client ' ODP.NET, Managed Driver
Imports Oracle.ManagedDataAccess.Types

Public Class Form1
    Public loopCounter As Integer = 0
    Public ActKz As Boolean = False

    Function DatenSchnittstelle() As Boolean

        Start.BackColor = Color.Green
        Start.Refresh()
        ActKz = True

        Dim ret As Boolean = False
        Dim ACSdb As New AccessC()
        Dim oracleConnectDaten = ACSdb.getOracleConnectDaten(Definition.selectEinstellung)
        Dim orcdb As New Oracdb(oracleConnectDaten("Server"), oracleConnectDaten("User"), oracleConnectDaten("Pass"))
        Dim ArtikelAnWamas = Nothing
        Dim ArtikelOut = Nothing
        Dim ArtikelAnWamasid = Nothing
        Dim messegIDTable = Nothing
        Dim MOVEMENTORDEROUT = Nothing
        Dim MOVEMENTORDEROUTIdList = Nothing
        Dim EINLAGERUNGSMELDUNG = Nothing
        Dim LOCKOUT = Nothing
        Dim LOCKOUTLIST = Nothing
        Dim wr As New WR

        ''ArtikelOut
        'ArtikelOut = orcdb.getTable(" SELECT * FROM Artikelout WHERE TELEGRAMSTATE =0 ") 'lesen oracle ArtikleOut
        'messegIDTable = orcdb.getMsgIdAndOrderId(ArtikelOut) 'gib Artikelout orderid als table zurück
        'If ACSdb.updateTable("tblArtikelAnWamas", "artStatus", 12, " ID ", messegIDTable("ORDERIDFehle")) Then
        '    orcdb.updateTable("ArtikelOut", "TELEGRAMSTATE", 1, messegIDTable("MESSAGEIDFehle"))
        'End If
        'If ACSdb.updateTable("tblArtikelAnWamas", "artStatus", 10, " ID ", messegIDTable("ORDERID")) Then
        '    orcdb.updateTable("ArtikelOUt", "TELEGRAMSTATE", 1, messegIDTable("MESSAGEID"))
        'End If



        'Artikelout 
        wr.readArtikelout_AndUpdateTBlArtikelAnWamas()


        ''schreiben im Oracle ArtikelIn
        'ArtikelAnWamas = ACSdb.gettable("select * from tblArtikelAnwamas where artstatus = 0 ")    'lesen ArtikelAnWamas
        'ArtikelAnWamasid = ACSdb.getIdlistFromTable(ArtikelAnWamas, "ID")
        'If orcdb.insertArtikelIn(ArtikelAnWamas) = True Then ' todo 
        '    ACSdb.updateTable("tblArtikelAnWamas", "artStatus", 5, "ID", ArtikelAnWamasid) 'update tblArtikelAnwamas
        'End If


        'schreiben im Oracle ArtikelIn
        wr.insertinArtikelInFromArtikelAnWamas()


        'lesen MoventOrderOut
        'MOVEMENTORDEROUT = orcdb.getTable(Definition.selectMOVEMENTORDEROUt)
        'MOVEMENTORDEROUTIdList = orcdb.getMsgIdAndOrderId(MOVEMENTORDEROUT) 'gib MOVEMENTORDEROUT orderid als table zurück
        'If ACSdb.insertInTable(MOVEMENTORDEROUT, "TblvonWamas") Then
        '    orcdb.updateTable("MOVEMENTORDEROUT", "TELEGRAMSTATE", 1, MOVEMENTORDEROUTIdList("MESSAGEID"))
        '    orcdb.updateTable("MOVEMENTORDEROUT", "TELEGRAMSTATE", 1, MOVEMENTORDEROUTIdList("MESSAGEIDFehle"))
        '    orcdb.updateTable("MOVEMENTORDEROUT", "TELEGRAMSTATE", 1, MOVEMENTORDEROUTIdList("MESSAGEID_8"))
        '    'update tblAnwamas
        '    ACSdb.updateTable("tblAnWamas", "AHStat", 10, "MESSAGEID", MOVEMENTORDEROUTIdList("ORDERID"))
        '    ACSdb.updateTable("tblAnWamas", "AHStat", 12, "MESSAGEID", MOVEMENTORDEROUTIdList("ORDERIDFehle"))
        '    ACSdb.updateTable("tblAnWamas", "AHStat", 12, "MESSAGEID", MOVEMENTORDEROUTIdList("ORDERID_8"))
        'End If


        'lesen MoventOrderOut
        wr.insertTblvonWamasFromMoventorderOut()


        ''TblAnWamas
        'Dim tbAnWamas = ACSdb.gettable(Definition.selectTblAnwamas.ToUpper) 'todo auf 1 
        'If orcdb.insertInTable(tbAnWamas, "MOVEMENTORDERIN") Then
        '    ACSdb.updateTable("tblAnWamas", "AHStat", 5, "MESSAGEID", ACSdb.getIdlistFromTable(tbAnWamas, "MESSAGEID"))
        'End If


        'TblAnWamas
        wr.insertMovenentorderinFromTBlAnwamas()


        '' lesen AvisoOut
        'Dim AvisoOut = orcdb.getTable(Definition.selectAvisoOut)
        'Dim AvisoOutIdList = orcdb.getMsgIdAndOrderId(AvisoOut)
        'If ACSdb.updateTable("AvisoAnWamas", "TELEGRAMSTATE", 10, "MESSAGEID", AvisoOutIdList("ORDERID")) Then
        '    orcdb.updateTable("Avisout", "TELEGRAMSTATE", 1, AvisoOutIdList("MESSAGEID"))
        'End If
        'If ACSdb.updateTable("AvisoAnWamas", "TELEGRAMSTATE", 12, "MESSAGEID", AvisoOutIdList("ORDERIDFehle")) Then
        '    orcdb.updateTable("Avisout", "TELEGRAMSTATE", 1, AvisoOutIdList("MESSAGEIDFehle"))
        'End If


        ' lesen AvisoOut
        wr.readAvisOutAnd_updateAvisoAnWamas()


        ''' AvisoAnWamas lesen
        ''Dim AvisoAnWamas = ACSdb.gettable(Definition.selectAvisoAnWamas)
        ''If orcdb.insertInTable(AvisoAnWamas, "AVISIN") Then
        ''    Dim vals = ACSdb.getIdlistFromTable(AvisoAnWamas, "MESSAGEID")
        ''    ACSdb.updateTable("AvisoAnWamas", "TELEGRAMSTATE", 5, "MESSAGEID", vals)
        ''End If

        ' AvisoAnWamas lesen
        wr.insertAvisinFromAvisoAnWamas()


        ''einlegerungen
        'EINLAGERUNGSMELDUNG = orcdb.getTable(Definition.selectEINLAGERUNGSMELDUNG)
        'If ACSdb.insertInTable(EINLAGERUNGSMELDUNG, "TblWEVonWamas") Then
        '    Dim EINLAGERUNGSMELDUNGList = orcdb.getMsgIdAndOrderId(EINLAGERUNGSMELDUNG)
        '    orcdb.updateTable("EINLAGERUNGSMELDUNG", "TELEGRAMSTATE", 1, EINLAGERUNGSMELDUNGList("MESSAGEID"))
        'End If


        ''einlegerungen
        wr.insertTblWEVonWamasFromEinlagerungsmeldung()


        ''AuslagerungsSpern lesen
        'Dim tblAuslagerungssperren = ACSdb.gettable(Definition.selectTblAuslqagerungssperren)
        'If orcdb.insertInTable(tblAuslagerungssperren, "LOCKIN") Then
        '    Dim vals = ACSdb.getIdlistFromTable(tblAuslagerungssperren, "MESSAGEID")
        '    ACSdb.updateTable("tblAuslagerungssperren", "TELEGRAMSTATE", 5, "MESSAGEID", vals)
        'End If


        'AuslagerungsSpern lesen
        wr.insertLockInFromTblAuslqagerungssperren()


        ''lockout lesen 
        'LOCKOUT = orcdb.getTable(Definition.selectLOCKOUT)
        'LOCKOUTLIST = orcdb.getMsgIdAndOrderId(LOCKOUT) 'gib Lockout orderid als table zurück
        'If ACSdb.insertInTable(LOCKOUT, "tblAuslagerungssperrenRückmeldungen") Then
        '    orcdb.updateTable("LOCKOUt", "TELEGRAMSTATE", 1, LOCKOUTLIST("MESSAGEID"))
        '    orcdb.updateTable("LOCKOUt", "TELEGRAMSTATE", 1, LOCKOUTLIST("MESSAGEIDFehle"))
        '    'update tblAuslagerungssperrenRückmeldungen
        '    ACSdb.updateTable("tblAuslagerungssperrenRückmeldungen", "TELEGRAMSTATE", 10, "ORDERID", LOCKOUTLIST("ORDERID"))
        '    ACSdb.updateTable("tblAuslagerungssperrenRückmeldungen", "TELEGRAMSTATE", 12, "ORDERID", LOCKOUTLIST("ORDERIDFehle"))
        'End If


        'lockout lesen 
        wr.insertTBlAuslagerungssperrenRückmeldungenFromLockout()



        ActKz = False
        Start.BackColor = Color.Red
        Start.Refresh()
        ret = True
        Return ret

    End Function


    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer.Tick
        If ActKz = False Then
            DatenSchnittstelle()
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim ACSdb As New AccessC()
        Dim oracleConnectDaten = ACSdb.getOracleConnectDaten(Definition.selectEinstellung)
        lb2.Text = " USER: " & oracleConnectDaten("User")
        lb1.Text = "SERVER: " & oracleConnectDaten("Server")
    End Sub

End Class


