# Diplomarbeit 
## HTL-Mensa-App\Mittagsmenüs mittels App Online bestellen

### Kurzfassung
Zur Mittagszeit entsteht in der Mensa immer eine lange Warteschlange, da mehr und mehr Schüler es bevorzugen die Feinkost der Mensa zu genießen. Im Rahmen der Diplomarbeit versuchen wir den Kaufvorgang der Mittagsmenüs zu digitalisieren und dadurch die Wartezeit zu verkürzen. Dafür wird nach der Bezahlung ein QR-Code generiert, dieser kann in der App oder über Mail aufgerufen werden. Diese QR-Codes können anschließend von dem Mensapersonal gescannt werden und zeigen ausgewählte Daten zur Bestellung an. Zusätzlich hat die Mensa, die Möglichkeit auf einem Terminal, die Statistiken der letzten Tage, Wochen und Monaten abzurufen und die Daten grafisch darstellen zu lassen.

### Abstract
There is always a long queue in the canteen at lunchtime, as more and more pupils prefer to enjoy the canteen’s food. As part of the diploma thesis, we are trying to digitise the process of buying lunch menus and thus reduce the waiting time. To do this, a QR code is generated after payment, which can be called up in the app or via e-mail. These QR codes can then be scanned by the canteen staff and display selected data about the order. In addition, the canteen has the option of calling up the statistics for the last few days, weeks and months on a terminal and displaying the data graphically.

### Für den Start benötigt
* Einfüllen der Url für Ihren Webserver in jeder ViewModel-Datei.

* Im LdapAPIController.cs müssen diese Felder befüllt werden:
    string ldapServer = "ldap_server_ip";
    string ldap_password = "ldap_server_password";
    string baseDnLehrer = "ldap_baseDN_teacher";
    string baseDnSchüler = "ldap_baseDN_student";
* Das PayPal Projekt (PayPalWebsiteTeil) wurde aufgrund von Zeitmangel nicht komplett implementiert
