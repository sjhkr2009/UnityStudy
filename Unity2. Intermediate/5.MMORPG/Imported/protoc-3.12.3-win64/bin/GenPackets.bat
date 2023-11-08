:: �ش� ���̵忡 ���� �ۼ��Ǿ����ϴ�: https://protobuf.dev/getting-started/csharptutorial/
:: protoc ���� ��ġ�� ���� ����(./), Protocol.proto ���ϵ� ���� ����(./)�� ��ġ�ϸ�, ���� ��ġ�� C# ���Ϸ� ��ȯ�� �����Ѵٴ� �ǹ�.

protoc.exe -I=./ --csharp_out=./ ./Protocol.proto
IF ERRORLEVEL 1 PAUSE

:: �����ϸ� Client, Server �ʿ� �ڵ� ������ ����

START ../../../Server/PacketGenerator/bin/PacketGenerator.exe ./Protocol.proto
XCOPY /Y Protocol.cs "../../../Client/Assets/Scripts/Packet"
XCOPY /Y Protocol.cs "../../../Server/Server/Packet"
XCOPY /Y ClientPacketManager.cs "../../../Client/Assets/Scripts/Packet"
XCOPY /Y ServerPacketManager.cs "../../../Server/Server/Packet"