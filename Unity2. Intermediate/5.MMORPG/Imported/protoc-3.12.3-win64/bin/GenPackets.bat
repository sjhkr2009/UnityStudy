:: 해당 가이드에 따라 작성되었습니다: https://protobuf.dev/getting-started/csharptutorial/
:: protoc 파일 위치도 현재 폴더(./), Protocol.proto 파일도 현재 폴더(./)에 위치하며, 현재 위치에 C# 파일로 변환해 추출한다는 의미.

protoc -I=./ --csharp_out=./ Protocol.proto
IF ERRORLEVEL 1 PAUSE

:: 성공하면 Client, Server 쪽에 코드 파일을 복사

Start ../../PacketGenerator/bin/PacketGenerator.exe ./Protocol.proto
XCopy /Y GenPackets.cs "../../../Client/Assets/Scripts/Packet"
XCopy /Y GenPackets.cs "../../../Server/Server/Packet"
XCopy /Y ClientPacketManager.cs "../../../Client/Assets/Scripts/Packet"
XCopy /Y ServerPacketManager.cs "../../../Server/Server/Packet"