
# CatSync 

CatSync is a C# Windows Application that syncs the frequency between two Amateur Radio Tansceivers or Receivers.
This app is intended at the Amateur Radio Operators community.

The current version supports the following CAT (Computer Aided Transceiver) protocols:

- CI-V, Communications Interface 5
- KSI, Kenwood Serial Protocol

CI-V is used by ICOM and Xiegu Transceivers. KSI is used by Kenwood Transceivers.
If there's enough interest, support for Yeasu Transceivers CAT protocol and/or support for Elecraft K3 protocol can easly be added. Please contact m if interested.


## Demo

Here, you can watch a quick video of CatSync being tested with a Kenwood TS-590S Transceiver and a Xiegu X6100 Transceiver:

[![CatSync v0.0.2-alpha Demo](https://img.youtube.com/vi/1GEGxY4AOv4/sddefault.jpg)](https://www.youtube.com/watch?v=1GEGxY4AOv4)

## Documentation

To use the application you shoud edit the Xcvrs.json configuration file located at:

CatSyncPoc\Xcvr\Config\Xcvrs.json

The configuration file uses the JSON format, so after any change please make sure your file is compliant (I suggest using a free JSON validator service like [JSONLint](https://jsonlint.com/)

Here's an explanation on all parameters in the config file (for one Transceiver):

"Id": 1 for Transceiver #1, 2 for Transceiver #2. Please do not edit.

"Manufacturer": Free text for your Transceiver manufacturer. Edit at will.

"Model": Free text for your Transceiver model. Edit at will.

"Protocol": This is the CAT protocol used by your Transceiver. Currently supported values are "CIV" and "KSI".

"Timeout": This is the time period, in miliseconds, a Transceiver needs to process a CAT request and reply to it. Lower limit is about 100 miliseconds for most Transceivers. I sugest a value around 200 miliseconds.

Commands:

"Read": The CAT command your Transceiver uses to read a frequency.

"ReadPrefix: The string prefixing a reply to a Read command.

"ReadSuffix: The string sufixing a reply to a Read command.

"Write: The CAT command your Transceiver uses to set a frequency.

"WritePrefix: The string prefixing a reply to a Write command.

"WriteSuffix: The string sufixing a reply to a Write command.


PortSettings:

"PortName": Use "COMx" where x is the CAT port for your Transceiver.

"BaudRate": Use the recommended port speed for serial communication with your Transceiver.

"Parity": Use the recommended parity for serial communication with your Transceiver. Usually this value is "None".

"DataBits": Use the recommended databits for serial communication with your Transceiver. Usually this value is 8.

"StopBits": Use the recommended stopbits for serial communication with your Transceiver. Usually this value is "One".

"Handshake": Use the recommended stopbits for serial communication with your Transceiver. Usually this value is "RequestToSend".


The provied configuration file is set to work with a KENWOOOD Model TS-590S and a Xiegu Model X6100. Adjust accordingly to fit your Transceivers/Receivers.


## Functionalities

- Supports Transceivers/Receivers that can be CAT controlled via CI-V or KSI protocols.
- Widely Configurable.
- Low Latency.
- Tested with a Kenwood TS-590S and a Xiegu X6100.


## Limitations

- Code has scarse use of Try/Catch blocks.
- CI-V and KSI only (at this time).
- Does not recover from any code Exception.


## Autores

- [@joaocunhalopes](https://www.github.com/joaocunhalopes)
