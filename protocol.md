
# <center>Protokoll</center>

## design / lessons learned

    Ich habe mir sehr schwer getan den Server zu designen. Ich wusste nicht wie man am besten eine Request behandelt. Außerdem habe ich mir mit der Spezifikation schwer getan, da ich lange gebraucht habe aus dieser ein vernünftiges Datenbankmodell zu entwickeln. Zuerst wollte ich mit Custom Types ein Card Type erstellen, der dann die ID, den Namen und den Damage enthält und dem Stack, Deck und Packages Table ein Array aus diesem Type geben. Das hat sich dann als eher schwierig  herausgestellt, da das inserten mit arrays und customtypes eine Herausforderung ist, die ich mir in diesem Projekt nicht stellen wollte. Jetzt arbeite ich nur mit der Card_id, um die Karten sinngemäß zu speichern. 

    Meinen Server habe ich sehr auf die refactored HTTPServerDemo angelehnt, die im MoodleKurs verlinkt ist. Der Server besitzt ein Endpoints Dictionary, dass beim Start des Programs befüllt wird mit allen Endpunkten, die es gibt. Wenn eine Request kommt, wird sie geparsed und jede Art von Information wird gespeichert. Sei es der Pfad, Headers oder auch QueryParameters. Die Response beinhaltet den Response Code mit dem dazugeörigen Response Text und den Content. Durch Exceptions werden Sonderfälle abgefangen und der Server schickt demnach auch eine negative Response zurück. Von einem Endpoint aus wird dann das jeweilige Repository, für den Daten Zugriff, eröffnet.

    Am Ende habe ich gelernt, dass es am besten ist sich die Angabe sehr gut durchzulesen und zu verstehen, damit man das Problem, dass man lösen will auch wirklich erkennt und versteht. Ich habe viel zu sehr versucht die Karten schön anfertigen zu lassen, aber im Endeffekt werden diese ja nicht random von uns Entwickler gesetzt sondern von den HTTP Anfragen. Da hätte ich mich besser mit der Spezifikation auseinandersetzen müssen. Außerdem ist es besser sich Stück für Stück vorzuarbeiten, als das ganze Projekt als ein großes Problem zu sehen.
 
## unit test design

    Meine Unit Tests sind größtenteils dazu ausgelegt ein Battle richtig ausführen zu lassen. Weil wenn da etwas schief läuft kann ein User ein ganzes Deck verlieren. Trotzdem habe ich Unit Tests geschrieben, die sicherstellen sollen, dass eine request im Endpoint richtig verarbeitet wird und eine eigene Datenbank erstellt, die genau gleich aussieht, wie die Richtige. Hier habe ich Tests geschrieben, die sicher stellen sollen, dass die Datenbanzugriffe das zuwünschende Ergebniss liefern. Also zum Beispiel einen User aus der Datenbank holen oder zum Beispiel Karten aus der Datenbank holen. 

## time spent

    Ich habe nach meiner ersten Präsentation wirklich wenig für dieses Projekt gemacht. Somit hatte ich hab Ende Dezember sehr viel zu tun. Aber in diesen 3-4 Wochen hat dieses Projekt dann doch Spaß gemacht und ich habe jeden Tag 3-4 Stunden, manchmal auch mehr, verbracht mit diesem Projekt. Geschätzt, habe ich ca. 90 Stunden mit diesem Projekt verbracht. Dazu zählt natürlich nicht nur das programmieren, sondern auch die Datenbankmodellierung, die sehr viel Zeit in Anspruch genommen hat und das designen des Servers.

## git link

    https://github.com/if22b151/MonsterTradingCardGame.git
