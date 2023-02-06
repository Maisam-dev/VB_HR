Imports System.Data.OleDb
Imports FxResources.System.Data

Public Class WR

    Private ret As Boolean
    Private ACSdb
    Private oracleConnectDaten
    Private orcdb
    Private ArtikelAnWamas
    Private ArtikelOut
    Private ArtikelAnWamasid
    Private messegIDTable
    Private MOVEMENTORDEROUT
    Private MOVEMENTORDEROUTIdList
    Private EINLAGERUNGSMELDUNG
    Private LOCKOUT
    Private LOCKOUTLIST

    Public Sub New(server, user, pass)
        ret = False
        ACSdb = New AccessC()
        orcdb = New Oracdb(server, user, pass)
        ArtikelAnWamas = Nothing
        ArtikelOut = Nothing
        ArtikelAnWamasid = Nothing
        messegIDTable = Nothing
        MOVEMENTORDEROUT = Nothing
        MOVEMENTORDEROUTIdList = Nothing
        EINLAGERUNGSMELDUNG = Nothing
        LOCKOUT = Nothing
        LOCKOUTLIST = Nothing
    End Sub

    'read and update Artikelout and update ´TblArtikelAnWamas 
    '
    Public Function readArtikelout_AndUpdateTBlArtikelAnWamas()
        Dim ret As Boolean = False
        ArtikelOut = orcdb.getTable(" SELECT * FROM Artikelout WHERE TELEGRAMSTATE =0 ") 'lesen oracle ArtikleOut
        messegIDTable = orcdb.getMsgIdAndOrderId(ArtikelOut) 'gib Artikelout orderid als table zurück
        If ACSdb.updateTable("tblArtikelAnWamas", "artStatus", 12, " ID ", messegIDTable("ORDERIDFehle")) Then
            orcdb.updateTable("ArtikelOut", "TELEGRAMSTATE", 1, messegIDTable("MESSAGEIDFehle"))
        End If
        If ACSdb.updateTable("tblArtikelAnWamas", "artStatus", 10, " ID ", messegIDTable("ORDERID")) Then
            orcdb.updateTable("ArtikelOUt", "TELEGRAMSTATE", 1, messegIDTable("MESSAGEID"))
        End If
        ret = True
        Return ret
    End Function
    '
    'schreib in orcdb.Artiklin and update ACSdb.tblArtikelAnWamas
    '
    Public Function insertinArtikelInFromArtikelAnWamas()
        'schreiben im Oracle ArtikelIn
        Dim ret As Boolean = False
        ArtikelAnWamas = ACSdb.gettable(Definition.selectTblArtikelAnwamas)    'lesen ArtikelAnWamas
        ArtikelAnWamasid = ACSdb.getIdlistFromTable(ArtikelAnWamas, "ID")
        If orcdb.insertArtikelIn(ArtikelAnWamas) = True Then ' todo 
            ACSdb.updateTable("tblArtikelAnWamas", "artStatus", 5, "ID", ArtikelAnWamasid) 'update tblArtikelAnwamas
        End If
        ret = True
        Return ret
    End Function
    '
    '
    '
    Public Function insertTblvonWamasFromMoventorderOut()
        Dim ret As Boolean = False
        MOVEMENTORDEROUT = orcdb.getTable(Definition.selectMOVEMENTORDEROUt)
        MOVEMENTORDEROUTIdList = orcdb.getMsgIdAndOrderId(MOVEMENTORDEROUT, True) 'gib MOVEMENTORDEROUT orderid als table zurück
        If ACSdb.insertInTable(MOVEMENTORDEROUT, "TblvonWamas") Then
            orcdb.updateTable("MOVEMENTORDEROUT", "TELEGRAMSTATE", 1, MOVEMENTORDEROUTIdList("MESSAGEID"))
            orcdb.updateTable("MOVEMENTORDEROUT", "TELEGRAMSTATE", 1, MOVEMENTORDEROUTIdList("MESSAGEIDFehle"))
            orcdb.updateTable("MOVEMENTORDEROUT", "TELEGRAMSTATE", 1, MOVEMENTORDEROUTIdList("MESSAGEID_8"))
            'update tblAnwamas
            ACSdb.updateTable("tblAnWamas", "AHStat", 10, "MESSAGEID", MOVEMENTORDEROUTIdList("ORDERID"))
            ACSdb.updateTable("tblAnWamas", "AHStat", 12, "MESSAGEID", MOVEMENTORDEROUTIdList("ORDERIDFehle"))
            ACSdb.updateTable("tblAnWamas", "AHStat", 8, "MESSAGEID", MOVEMENTORDEROUTIdList("ORDERID_8"))
        End If
        ret = True
        Return ret
    End Function


    Public Function insertAvisinFromAvisoAnWamas()
        Dim ret As Boolean = False
        Dim AvisoAnWamas = ACSdb.gettable(Definition.selectAvisoAnWamas)
        If orcdb.insertInTable(AvisoAnWamas, "AVISIN") Then
            Dim vals = ACSdb.getIdlistFromTable(AvisoAnWamas, "MESSAGEID")
            ACSdb.updateTable("AvisoAnWamas", "TELEGRAMSTATE", 5, "MESSAGEID", vals)
        End If
        ret = True
        Return ret
    End Function

    Public Function insertMovenentorderinFromTBlAnwamas()
        Dim ret As Boolean = False
        Dim tbAnWamas = ACSdb.gettable(Definition.selectTblAnwamas.ToUpper) 'todo auf 1 
        If orcdb.insertInTable(tbAnWamas, "MOVEMENTORDERIN") Then
            ACSdb.updateTable("tblAnWamas", "AHStat", 5, "MESSAGEID", ACSdb.getIdlistFromTable(tbAnWamas, "MESSAGEID"))
        End If
        ret = True
        Return ret
    End Function



    Public Function readAvisOutAnd_updateAvisoAnWamas()
        Dim ret As Boolean = False
        Dim AvisoOut = orcdb.getTable(Definition.selectAvisoOut)
        Dim AvisoOutIdList = orcdb.getMsgIdAndOrderId(AvisoOut)
        If ACSdb.updateTable("AvisoAnWamas", "TELEGRAMSTATE", 10, "MESSAGEID", AvisoOutIdList("ORDERID")) Then
            orcdb.updateTable("Avisout", "TELEGRAMSTATE", 1, AvisoOutIdList("MESSAGEID"))
        End If
        If ACSdb.updateTable("AvisoAnWamas", "TELEGRAMSTATE", 12, "MESSAGEID", AvisoOutIdList("ORDERIDFehle")) Then
            orcdb.updateTable("Avisout", "TELEGRAMSTATE", 1, AvisoOutIdList("MESSAGEIDFehle"))
        End If
        ret = True
        Return ret
    End Function

    Public Function insertTblWEVonWamasFromEinlagerungsmeldung()
        Dim ret As Boolean = False
        EINLAGERUNGSMELDUNG = orcdb.getTable(Definition.selectEINLAGERUNGSMELDUNG)
        If ACSdb.insertInTable(EINLAGERUNGSMELDUNG, "TblWEVonWamas") Then
            Dim EINLAGERUNGSMELDUNGList = orcdb.getMsgIdAndOrderId(EINLAGERUNGSMELDUNG)
            orcdb.updateTable("EINLAGERUNGSMELDUNG", "TELEGRAMSTATE", 1, EINLAGERUNGSMELDUNGList("MESSAGEID"))
        End If
        ret = True
        Return ret
    End Function

    Public Function insertLockInFromTblAuslqagerungssperren()
        Dim ret As Boolean = False
        Dim tblAuslagerungssperren = ACSdb.gettable(Definition.selectTblAuslqagerungssperren)
        If orcdb.insertInTable(tblAuslagerungssperren, "LOCKIN") Then
            Dim vals = ACSdb.getIdlistFromTable(tblAuslagerungssperren, "MESSAGEID")
            ACSdb.updateTable("tblAuslagerungssperren", "TELEGRAMSTATE", 5, "MESSAGEID", vals)
        End If
        ret = True
        Return ret
    End Function

    Public Function insertTBlAuslagerungssperrenRückmeldungenFromLockout()
        Dim ret As Boolean = False
        LOCKOUT = orcdb.getTable(Definition.selectLOCKOUT)
        LOCKOUTLIST = orcdb.getMsgIdAndOrderId(LOCKOUT) 'gib Lockout orderid als table zurück
        If ACSdb.insertInTable(LOCKOUT, "tblAuslagerungssperrenRückmeldungen") Then
            orcdb.updateTable("LOCKOUt", "TELEGRAMSTATE", 1, LOCKOUTLIST("MESSAGEID"))
            orcdb.updateTable("LOCKOUt", "TELEGRAMSTATE", 1, LOCKOUTLIST("MESSAGEIDFehle"))
            'update tblAuslagerungssperrenRückmeldungen
            ACSdb.updateTable("tblAuslagerungssperrenRückmeldungen", "TELEGRAMSTATE", 10, "ORDERID", LOCKOUTLIST("ORDERID"))
            ACSdb.updateTable("tblAuslagerungssperrenRückmeldungen", "TELEGRAMSTATE", 12, "ORDERID", LOCKOUTLIST("ORDERIDFehle"))
        End If
        ret = True
        Return ret
    End Function

End Class
