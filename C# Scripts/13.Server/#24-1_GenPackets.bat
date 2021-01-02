:: Window 배치 파일: 윈도우에서 여러 가지 작업들을 한 번에 처리할 수 있도록 텍스트로 작정해놓은 문서.
:: 배치 파일에서 rem 또는 :: 으로 시작하는 행은 주석 처리된다.
:: 명령어는 대문자든 소문자든 무관하다. (start, Start, START 등)

:: Start [경로] [매개변수]
:: 해당 경로의 프로그램을 실행한다. 띄어쓰기 후 두 번째 요소가 있을 경우 main함수의 매개변수로 넣어 실행한다.
Start ../../PacketGenerator/bin/PacketGenerator.exe ../../PacketGenerator/PDL.xml
:: XCopy [옵션] [복사할 파일 경로] [붙여넣기할 위치 경로]
:: 지정 경로의 파일 또는 폴더와 모든 하위 폴더를 복사한다. 옵션은 필수적으로 지정해야 한다.
:: [옵션] /Y : 붙여넣기 경로에 이미 대상 파일이 있으면 덮어쓴다.
::		  /S : 빈 폴더는 복사하지 않는다.
::		  /E : 빈 폴더도 모두 복사한다.
::		  /H : 숨겨진 파일도 복사한다.
XCopy /Y GenPackets.cs "../../DummyClient/Packet"
XCopy /Y GenPackets.cs "../../Server/Packet"

:: 패킷 매니저도 복사해준다.
:: 패킷은 대부분 양방향으로 쓰이지 않고, 종류에 따라 서버->클라 또는 클라->서버로 이동한다.
:: (예를 들어 공격 요청을 서버가 클라로 보내거나, 강화 성공 여부를 클라가 서버로 보낼 일은 없다)
:: 따라서 패킷 종류에 따라 클라/서버용 패킷을 구분하고 매니저를 따로 만들어 구현한다.
XCopy /Y ClientPacketManager.cs "../../DummyClient/Packet"
XCopy /Y ServerPacketManager.cs "../../Server/Packet"

:: 유니티로 Client를 만들었으니 그쪽으로도 자동 복사하도록 지정한다.
XCopy /Y GenPackets.cs "../../Client/Assets/Scripts/Packet"
XCopy /Y ClientPacketManager.cs "../../Client/Assets/Scripts/Packet"