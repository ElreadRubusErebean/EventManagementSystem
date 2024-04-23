# Projektbeschreibung

Dieses Projekt ist im Zusammenhang mit dem Softwaretechnik-Projekt der BA Sachsen im Kurs MI22-1 entstanden. Daher darf das Projekt nicht der Öffentlichkeit zur Verfügung gestellt werden. 
Ebenso sind die Schwierigkeiten des Datenschutzes zu bedenken.


## Verantwortlich:
**Verantwortlich sind:**  
Abdul Hadi AlQawas,Carolin Feurich, Roman Kosovtsev, Jennifer Schön und Yannick Rammelt


## Anmerkungen zu dem Projekt:

Dokumentation für dieses Projekt finden Sie hier: [Google Docs](https://docs.google.com/document/d/1dvAZjHOX3Jc-2bVEI-lzGPPl5FUN-PChL9QjNTp43Xo/edit?usp=sharing)

Dokument zum Vermerken von Änderungswünschen: [Google Docs](https://docs.google.com/document/d/1xLHEQCvM5F307nuTzwwLtv-PhNp3Nhn9RlOVhgtx1xA/edit?usp=sharing)

Design-Planung für dieses Projekt finden Sie hier: [Figma](https://www.figma.com/team_invite/redeem/RQD2Pc5UUmaQnnjADXB8BA)


## Vorbereitung für das Projekt

Für dieses Projekt benötigt es eine Datenanbindung. Das Projekt selbst basiert auf einer lokalen Datenbank. Der zugehörige Datensatz lässt sich mit dem Projekt zusammen herunterladen. Nun gibt es zwei Möglichkeiten die Datenbank für das Projekt zu initialisieren: Indem Sie sich Microsoft SQL Server 2019 herunterladen und initialisieren oder indem Sie einen entsprechenden Docker-Container aufsetzen.

### Microsoft SQL Server 2019

1.) Microsoft SQL herunterladen 

2.) Microsoft SQL starten, Profil kopieren **(wird noch in Schritt 4 notwendig werden)** und verbinden   

3.) Pfad anpassen:  
> - Servername: appsettings.json: Event zu Eventdatenbank ändern    
> - Console, folgende Befehle anwenden: **add-migration [Datenbankname]** und **update-database**     
        
      
4.) Zu Bedenken ist, dass die appsettings.development.json immer richtig konfiguriert sein muss

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=[Profil von Microsoft SQL eintragen];Database=[Datenbank eintragen];Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```
