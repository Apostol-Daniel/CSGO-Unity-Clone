# BugTracker

## Project
BugTracker webapp registreert bugs die zijn opgetreden tijdens de development van het project. De gebruikers van het systeem zijn de Admin, Project Manager en Developer en Tester. De Tester logt de bug door details van de bug in te voeren. Project Manager of Developer wijst bugs toe aan Developers, Developers logt in en lost de bug op en werkt de status bij.

### Beschrijving van de applicatie

Roles:
- Admin is de krachtigste gebruiker met volledige toegang tot CRUD-functies van projecten, taken, teamleden
- Project Manager kan projecten bewerken, Developers toevoegen/verwijderen van projecten, taken/bugs toevoegen/verwijderen
- Developers lossen problemen/bugs op, voegen opmerkingen toe, updaten ernst/prioriteit
- Testers test applicatie, maak bugrapporten problemen

Projectonderdelen:
- API(.NET 6 Web API )
- MSSQL databank(EF Core 6 & SQL Server)
- Responsive UI met Bootsraap CSS
- Authentication met Identity en JWT
## Tehnologies
- Blazor WebAssembly
- Bootstrap
- .NET 6 Web API 
- EF Core 6 & SQL Server
- Jwt voor Authentication Met Identity voor Roles

### Blazor WebAssembly & .NET 6
De keuze Blazor is omwille dat u kunt  interactieve web-UI's bouwen met C# in plaats van JavaScript. Blazor-apps zijn samengesteld uit herbruikbare web-UI-componenten die zijn geïmplementeerd met C#, HTML en CSS. Zowel client- als servercode is geschreven in C#, zodat u code en bibliotheken kunt delen.

Het was een moeilijke keuze tussen Angular met Typescript en blazor omdat beide gebruik maken van componenten en vergelijkbare bouwstenen gebruiken, maar blazor hebben gekozen voor de eenvoudige vormvalidatie (Edit Form Component)

### Warrom Blazor WebAssembly en niet Blazor Server
Het enige negatieve van het gebruik van Blazor Web Assembly is de hoge initiële laadtijd van de applicatie.

- na de eerste keer laden reageert de UI sneller op gebruikersverzoeken (behalve externe API-calls), omdat de volledige inhoud van de website aan de clientzijde wordt gedownload.
- omdat de applicatie aan de clientzijde wordt gedownload, is offline ondersteuning mogelijk in geval van netwerkproblemen.


## Bronnenlijst
- https://jwt.io/
- https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor
- https://www.microsoft.com/en-us/sql-server/application-development
- https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-6.0/whatsnew
- https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6