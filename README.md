# Movesion Challenge 
Il candidato dovrà sviluppare delle api in .NET per la gestione di una applicazione senza sviluppare la parte front-end prevedendone però le api necessarie per il successivo sviluppo. 
In particolare il back-end dovrà gestire le operazioni CRUD per la gestione di compagnie e dei loro relativi prodotti (relazione master detail). 
I dati della compagnia devono essere “nome compagnia”, “indirizzi”, “mezzi di contatto”, “prodotti” 
Ogni prodotto è composto da “nome prodotto”, “categoria merceologica”, “quantità” 
Prevedere per compagnie e prodotti altri campi che si ritengono necessari 
# Utilizzare i seguenti framework/librerie  
- .NET Framework 4.8 o .NET Core 
- DB: SQLITE o SQLSERVER o POSTGRESQL 
# Plus
Se si riesce inoltre a fare in modo che tali chiamate siano autenticate viene considerato un plus. 

# Note sulla realizzazione: 
L'applicazione demo è stata realizzata utilizzando .Net 6 + EntityframeworkCore 6 per la gestione della banca dati __Sqlite__ (contenuta nel progetto .\Data\localdb.sqlite). Le interrogazioni fanno uso di un _datacontext_, generico, che poi viene derivato e inniettato sui singoli servizi. Non è stato fatto uso di ADO.Net, per l'interrogazione della banca dati. La validazione è effettuata tramite libreria __FluentValidation__. All'interno del codice sono presenti diversi TODO, per ipotetiche ulteriori implentazioni o affinamento dei controlli. La persistenza delle foto prodotto non è stata implementata, anche se l'idea di base era quella di salvare, per ogni chiave prodotto il nome guid, del file memorizzato su disco, per un'ipotetica memorizzazione di un server CDN.

L'applicazione demo in modalità sviluppo, abilita in automatico l'interfaccia di __swagger__. 
All'interno c'è un semplice sistema di autenticazione con ApiKey, impostata fissa nel file di configurazione:
_8hZqPJK/JU++Gem2rVOJBg8CMIu7hyx9weW+DWyQ+kALJ0fjhccGOJYzBPiny52gx9U98zFTM3dKUJGXgUOhVA_
Per testarlo con swagger, ricordare di inserire la chiave, utilizzando il pulsante in alto a destra di autenticazione.
