Imports System.Data
Imports System.Data.OleDb
Imports Oracle.ManagedDataAccess.Client ' ODP.NET, Managed Driver
Imports Oracle.ManagedDataAccess.Types

Public Class Form1
    Public loopCounter As Integer = 0
    Public ActKz As Boolean = False
    Public oracleConnectDaten

    Function DatenSchnittstelle() As Boolean

        Start.BackColor = Color.Green
        Start.Refresh()
        ActKz = True

        Dim ret As Boolean = False
        Dim ACSdb As New AccessC()
        Dim ArtikelAnWamas = Nothing
        Dim ArtikelOut = Nothing
        Dim ArtikelAnWamasid = Nothing
        Dim messegIDTable = Nothing
        Dim MOVEMENTORDEROUT = Nothing
        Dim MOVEMENTORDEROUTIdList = Nothing
        Dim EINLAGERUNGSMELDUNG = Nothing
        Dim LOCKOUT = Nothing
        Dim LOCKOUTLIST = Nothing
        Dim wr As New WR(Me.oracleConnectDaten("Server"), Me.oracleConnectDaten("User"), Me.oracleConnectDaten("Pass"))

        'Artikelout 
        wr.readArtikelout_AndUpdateTBlArtikelAnWamas()


        'schreiben im Oracle ArtikelIn
        wr.insertinArtikelInFromArtikelAnWamas()


        'lesen MoventOrderOut
        wr.insertTblvonWamasFromMoventorderOut()


        'TblAnWamas
        wr.insertMovenentorderinFromTBlAnwamas()


        ' lesen AvisoOut
        wr.readAvisOutAnd_updateAvisoAnWamas()


        ' AvisoAnWamas lesen
        wr.insertAvisinFromAvisoAnWamas()


        ''einlegerungen
        wr.insertTblWEVonWamasFromEinlagerungsmeldung()


        'AuslagerungsSpern lesen
        wr.insertLockInFromTblAuslqagerungssperren()


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
        Me.oracleConnectDaten = ACSdb.getOracleConnectDaten(Definition.selectEinstellung)
        lb2.Text = " USER: " & Me.oracleConnectDaten("User")
        lb1.Text = "SERVER: " & Me.oracleConnectDaten("Server")
    End Sub
End Class


