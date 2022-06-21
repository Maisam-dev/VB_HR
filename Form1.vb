Imports System.Data
Imports System.Data.OleDb

Imports Oracle.ManagedDataAccess.Client ' ODP.NET, Managed Driver
Imports Oracle.ManagedDataAccess.Types

Public Class Form1

    Private Sub Start_Click(sender As Object, e As EventArgs) Handles Start.Click

        Start.BackColor = Color.Green

        Dim ACSdb As New AccessC()
        Dim orcdb As New Oracdb()
        Dim ArtikelAnWamas = ACSdb.gettable("select * from tblArtikelAnwamas where artstatus = 0 ") ' lesen ArtikelAnWamas
        Dim ArtikelOut = Nothing
        Dim ArtikelAnWamasid = ACSdb.getIdlistFromTable(ArtikelAnWamas, "ID") 'gib artikelAnWamasID als String () zurück
        Dim messegIDTable = Nothing
        Dim MOVEMENTORDEROUT = Nothing
        Dim MOVEMENTORDEROUTIdList = Nothing
        ' schreiben im Oracle ArtikelIn

        If orcdb.insertArtikelIn(ArtikelAnWamas) = True Then

            'update tblArtikelAnwamas
            ACSdb.updateTable("tblArtikelAnWamas", "artStatus", 5, "ID", ArtikelAnWamasid)
        End If

        'Artikelout 
        ArtikelOut = orcdb.getTable(" SELECT * FROM Artikelout WHERE TELEGRAMSTATE =0 ") 'lesen oracle ArtikleOut
        messegIDTable = orcdb.getOrderIdUndMessegId(ArtikelOut) ' gib Artikelout orderid als table zurück
        If ACSdb.updateTable("tblArtikelAnWamas", "artStatus", 12, " ID ", messegIDTable("ORDERIDFehle")) Then
            orcdb.updateTable("ArtikelOut", "TELEGRAMSTATE", 1, messegIDTable("MESSAGEIDFehle"))
        End If

        If ACSdb.updateTable("tblArtikelAnWamas", "artStatus", 10, " ID ", messegIDTable("ORDERIDOhneFehle")) Then
            orcdb.updateTable("ArtikelOUt", "TELEGRAMSTATE", 1, messegIDTable("MESSAGEIDOhneFehle"))
        End If

        'TblAnWamas
        Dim tbAnWamas = ACSdb.gettable(Definition.selectTblAnwamas.ToUpper) ' todo auf 1 
        If orcdb.insertInTable(tbAnWamas, "MOVEMENTORDERIN") Then
            ACSdb.updateTable("tblAnWamas", "AHStat", 5, "MESSAGEID", ACSdb.getIdlistFromTable(tbAnWamas, "ID"))
        End If

        'lesen MoventOrderOut
        MOVEMENTORDEROUT = orcdb.getTable(Definition.selectMOVEMENTORDEROUt)
        MOVEMENTORDEROUTIdList = orcdb.getOrderIdUndMessegId(MOVEMENTORDEROUT) 'gib MOVEMENTORDEROUT orderid als table zurück

        If ACSdb.insertInTable(MOVEMENTORDEROUT, "TblvonWamas") Then

            orcdb.updateTable("MOVEMENTORDEROUT", "TELEGRAMSTATE", 1, MOVEMENTORDEROUTIdList("MESSAGEIDOhneFehle"))
            orcdb.updateTable("MOVEMENTORDEROUT", "TELEGRAMSTATE", 1, MOVEMENTORDEROUTIdList("MESSAGEIDFehle"))
            'update tblAnwamas
            ACSdb.updateTable("tblAnWamas", "AHStat", 10, "MESSAGEID", MOVEMENTORDEROUTIdList("MESSAGEIDOhneFehle"))
            ACSdb.updateTable("tblAnWamas", "AHStat", 12, "MESSAGEID", MOVEMENTORDEROUTIdList("MESSAGEIDFehle"))

        End If

        ' AvisoAnWamas lesen
        Dim AvisoAnWamas = ACSdb.gettable(Definition.selectAvisoAnWamas)
        If orcdb.insertInTable(AvisoAnWamas, "AVISIN") Then
            Dim vals = ACSdb.getIdlistFromTable(AvisoAnWamas, "MESSAGEID")
            ACSdb.updateTable("AvisoAnWamas", "AVStatus", 5, "MESSAGEID", vals)
        End If
        Break(sender, e)

    End Sub


    Private Sub Break(sender As Object, e As EventArgs)

        Start.BackColor = Color.Red
        Threading.Thread.Sleep(10000)
        Start_Click(sender, e)

    End Sub

End Class


