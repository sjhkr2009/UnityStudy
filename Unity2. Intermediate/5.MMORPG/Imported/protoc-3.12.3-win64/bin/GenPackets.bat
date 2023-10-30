:: �ش� ���̵忡 ���� �ۼ��Ǿ����ϴ�: https://protobuf.dev/getting-started/csharptutorial/
:: protoc ���� ��ġ�� ���� ����(./), Protocol.proto ���ϵ� ���� ����(./)�� ��ġ�ϸ�, ���� ��ġ�� C# ���Ϸ� ��ȯ�� �����Ѵٴ� �ǹ�.

protoc -I=./ --csharp_out=./ Protocol.proto
IF ERRORLEVEL 1 PAUSE

:: �����ϸ� Client, Server �ʿ� �ڵ� ������ ����

Start ../../PacketGenerator/bin/PacketGenerator.exe ./Protocol.proto
XCopy /Y GenPackets.cs "../../../Client/Assets/Scripts/Packet"
XCopy /Y GenPackets.cs "../../../Server/Server/Packet"
XCopy /Y ClientPacketManager.cs "../../../Client/Assets/Scripts/Packet"
XCopy /Y ServerPacketManager.cs "../../../Server/Server/Packet"