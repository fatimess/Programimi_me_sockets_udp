# Programimi_me_sockets_udp

Ky projekt implementon një sistem klient-server duke përdorur UDP sockets, si dhe një HTTP server për monitorimin e serverit.
Serveri UDP menaxhon komunikimin me klientët dhe komandat për manipulimin e file-ve, ndërsa HTTP serveri ofron një endpoint për shikimin e statistikave në kohë reale.

Teknologjitë e përdorura:

-C#
-.NET
-UDP Sockets
-TCP (për HTTP server)
-JSON


Serveri (UDP)
Pranon kërkesa nga klientët
Menaxhon klientët dhe mesazhet
Ekzekuton komanda për file:
/list
/read <file>
/delete <file>
/search <keyword>
/info <file>
/upload (simulim)
/download (simulim)
