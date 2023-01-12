Public Class Definition
    Public Shared selectTblAnwamas As String = "
SELECT 
        [KDNArtnr], 
        [Restmenge], 
        [GesamtColli], 
        [WKdLand], 
        [Zone], 
        [ProduktionsDatum], 
        [KundenReferenzdatum], 
        [KDPlz], 
        [TelegramType],  
        [WKDOrt], 
        [BibText], 
        [MESSAGEID], 
        [Strasse], 
        [WKdPlz], 
        [SdgNR], 
        [BewInfo], 
        [DisNr], 
        [MandantenGLN], 
        [FlagEP], 
        [Haltbar], 
        [Sperrung], 
        [Prioritaet],
        [LSKunde], 
        [WKunde], 
        [MHD], 
        [Anschrift], 
        [VarText2], 
        [VarText4], 
        [Kunde], 
        [PalletID], 
        [Colli_Pro_Palette], 
        [KundenReferenz2], 
        [Artbez], 
        [WStr], 
        [WKDZusn], 
        [Bib], 
        [VarText3], 
        [VarText1], 
        [ZielPosition], 
        [Charge], 
        [KDOrt], 
        [KDStr], 
        [Split], 
        [VOG_LS], 
        [Ort], 
        [KundenReferenz], 
        [Gewicht], 
        [Artnr], 
        [Kdnr]
from TblAnWamas where AHStat = 0 
"


    Public Shared selectMOVEMENTORDEROUt As String = "
SELECT
        MESSAGEID,
        BEWINFO ,
        CHARGE ,
        ERRORID ,
        ERRORTEXT ,
        ORDERID,
        PALLETID,
        PRODUKTIONSDATUM, 
        ABGABEPLATZ
        FROM MOVEMENTORDEROUT
 WHERE TELEGRAMSTATE = 0
"

    Public Shared selectAvisoAnWamas As String = "
SELECT
        [MESSAGEID], 
        [TELEGRAMTYPE], 
        [ARTIKELID], 
        [BIB], 
        [DISNR], 
        [KUNDENREFERENZ], 
        [KUNDENREFERENZ2], 
        [KUNDENREFERENZDATUM], 
        [MANDANTENGLN],
        [PALLETID], 
        [SATZART], 
        [SDGNR], 
        [ZIEL]
FROM AvisoAnWamas
 WHERE TELEGRAMSTATE = 0
"


    Public Shared selectEINLAGERUNGSMELDUNG As String = "
select 
         MESSAGEID,
        TELEGRAMTYPE,
        ARTBEZ,
        ARTIKELNR,
        BIB,
        CHARGE,
        COLLIANZAHL,
        CROSSDOCKING,
        DISNR,
        EAN,
        HALTBAR,
        HOEHENKLASSE,
        KUNDENREFERENZ,
        KUNDENREFERENZ2,
        KUNDENREFERENZDATUM,
        LOKATION,
        MANDANTENGLN,
        PALLETID,
        PRODUKTIONSDATUM,
        SDGNR,
        BEWEGUNGSART
from EINLAGERUNGSMELDUNG 
 WHERE TELEGRAMSTATE = 0
"

    Public Shared selectTblAuslqagerungssperren As String = "
select 
   MESSAGEID,
   TELEGRAMSTATE,
   TELEGRAMTYPE,
   ARTIKELID,
   BIB,
   CHARGE,
   INFOTEXT,
   LOCATION,
   PALLETID,
   USER_VOG
From  tblAuslagerungssperren
 WHERE TELEGRAMSTATE = 0
"

    Public Shared selectLOCKOUT As String = "
SELECT 
          TELEGRAMTYPE,
          ARTIKELID,
          BIB,
          CHARGE,
          ORDERID,
          MESSAGEID,
          ERRORID ,
          ERRORTEXT ,
          INFOTEXT,
          LOCATION,
          PALLETID,
          USER_VOG 
FROM LOCKOUt WHERE TELEGRAMSTATE = 0  
"

    Public Shared selectEinstellung As String = "
 SELECT  
       oracleServer, 
       oracleUser, 
       oraclepass 
FROM einstellung
"

    Public Shared selectAvisoOut As String = "
         select 
            MESSAGEID,
            ORDERID,
            ERRORID,
            ERRORTEXT
         from Avisout
         WHERE TELEGRAMSTATE = 0            
"

End Class
