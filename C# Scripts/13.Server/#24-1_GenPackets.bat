:: Window ��ġ ����: �����쿡�� ���� ���� �۾����� �� ���� ó���� �� �ֵ��� �ؽ�Ʈ�� �����س��� ����.
:: ��ġ ���Ͽ��� rem �Ǵ� :: ���� �����ϴ� ���� �ּ� ó���ȴ�.
:: ��ɾ�� �빮�ڵ� �ҹ��ڵ� �����ϴ�. (start, Start, START ��)

:: Start [���] [�Ű�����]
:: �ش� ����� ���α׷��� �����Ѵ�. ���� �� �� ��° ��Ұ� ���� ��� main�Լ��� �Ű������� �־� �����Ѵ�.
Start ../../PacketGenerator/bin/PacketGenerator.exe ../../PacketGenerator/PDL.xml
:: XCopy [�ɼ�] [������ ���� ���] [�ٿ��ֱ��� ��ġ ���]
:: ���� ����� ���� �Ǵ� ������ ��� ���� ������ �����Ѵ�. �ɼ��� �ʼ������� �����ؾ� �Ѵ�.
:: [�ɼ�] /Y : �ٿ��ֱ� ��ο� �̹� ��� ������ ������ �����.
::		  /S : �� ������ �������� �ʴ´�.
::		  /E : �� ������ ��� �����Ѵ�.
::		  /H : ������ ���ϵ� �����Ѵ�.
XCopy /Y GenPackets.cs "../../DummyClient/Packet"
XCopy /Y GenPackets.cs "../../Server/Packet"

:: ��Ŷ �Ŵ����� �������ش�.
:: ��Ŷ�� ��κ� ��������� ������ �ʰ�, ������ ���� ����->Ŭ�� �Ǵ� Ŭ��->������ �̵��Ѵ�.
:: (���� ��� ���� ��û�� ������ Ŭ��� �����ų�, ��ȭ ���� ���θ� Ŭ�� ������ ���� ���� ����)
:: ���� ��Ŷ ������ ���� Ŭ��/������ ��Ŷ�� �����ϰ� �Ŵ����� ���� ����� �����Ѵ�.
XCopy /Y ClientPacketManager.cs "../../DummyClient/Packet"
XCopy /Y ServerPacketManager.cs "../../Server/Packet"

:: ����Ƽ�� Client�� ��������� �������ε� �ڵ� �����ϵ��� �����Ѵ�.
XCopy /Y GenPackets.cs "../../Client/Assets/Scripts/Packet"
XCopy /Y ClientPacketManager.cs "../../Client/Assets/Scripts/Packet"