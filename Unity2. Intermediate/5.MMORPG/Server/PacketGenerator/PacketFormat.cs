using System;
using System.Collections.Generic;
using System.Text;

namespace PacketGenerator
{
	class PacketFormat
	{

		// 패킷 매니저 클래스
		// {0} : 멤버 함수 Register() 본문
		public static string managerFormat =
@"using System;
using System.Collections.Generic;
using System.Text;
using ServerCore;

public class PacketManager
{{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance => _instance;

	private PacketManager()
	{{
		Register();
	}}
	#endregion
	
	Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makePktFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
	Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

	public void Register()
	{{
{0}
	}}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onReceiveCallback = null)
	{{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += sizeof(ushort);
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += sizeof(ushort);

		Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
		if (_makePktFunc.TryGetValue(id, out func))
		{{
			IPacket packet = func.Invoke(session, buffer);
			
			// 따로 정의된 콜백 함수가 있으면 실행, 아니라면 패킷을 핸들로 바로 넘겨준다.
			if (onReceiveCallback != null)
				onReceiveCallback.Invoke(session, packet);
			else
				HandlePacket(session, packet);
		}}
	}}

	T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
	{{
		T packet = new T();
		packet.DeSerialize(buffer);

		// MakePacket에서는 패킷 조립만 하고, 실행하는 부분은 아래의 HandlePacket으로 분리한다.

		return packet;
	}}
	public void HandlePacket(PacketSession session, IPacket packet)
	{{
		Action<PacketSession, IPacket> action = null;
		if (_handler.TryGetValue(packet.Protocol, out action))
			action(session, packet);
	}}
}}";

		// 멤버 함수 - Register() (패킷 매니저)
		// {0} : 생성할 패킷 클래스 이름
		public static string managerRegisterFormat =
@"		_makePktFunc.Add((ushort)PacketID.{0}, MakePacket<{0}>);
		_handler.Add((ushort)PacketID.{0}, PacketHandler.{0}Handler);";


		// cs 파일에 최종적으로 들어갈 내용
		// {0} : 패킷 ID들 (enum 원소 선언)
		// {1} : 패킷 클래스
		public static string fileFormat =
@"using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ServerCore;

public enum PacketID 
{{
	{0}
}}

public interface IPacket
{{
	ushort Protocol {{ get; }}
	void DeSerialize(ArraySegment<byte> arr);
	ArraySegment<byte> Serialize();
}}

{1}
";

		
		// Enum 원소 선언부 (패킷 ID)
		// {0} : 패킷 ID 이름
		// {1} : 패킷 ID (정수)
		public static string packetEnumFormat =
@"{0} = {1},";

		#region Packet Class

		// 클래스 (패킷)
		// {0} : 패킷 이름
		// {1} : 멤버 변수들
		// {2} : 멤버 변수 읽기(De-Serialize)
		// {3} : 멤버 변수 쓰기(Serialize)
		public static string packetFormat =
@"
public class {0} : IPacket
{{
	public ushort Protocol => (ushort)PacketID.{0};

	{1}

	public ArraySegment<byte> Serialize()
	{{
		ArraySegment<byte> arr = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.{0}), 0, arr.Array, arr.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		
		{3}

		Array.Copy(BitConverter.GetBytes(count), 0, arr.Array, arr.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}}
	public void DeSerialize(ArraySegment<byte> arr)
	{{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);

		{2}
	}}
}}
";
		// 클래스 일반 멤버 변수 선언 (패킷)
		// {0} : 변수 타입
		// {1} : 변수 이름
		public static string memberFormat =
@"public {0} {1};";

		// 클래스 리스트 멤버 변수 선언 (패킷)
		// {0} : 리스트 멤버 이름 (파스칼 표기)
		// {1} : 리스트 멤버 이름 (카멜 표기)
		// {2} : 구조체 멤버 변수들
		// {3} : 구조체 멤버 변수 읽기(De-Serialize)
		// {4} : 구조체 멤버 변수 쓰기(Serialize)
		public static string memberListFormat =
@"public struct {0}
{{
	{2}

	public bool Serialize(ArraySegment<byte> arr, ref ushort count)
	{{
		bool success = true;

		{4}

		return success;
	}}

	public void DeSerialize(ArraySegment<byte> arr, ref ushort count)
	{{
		{3}
	}}
}}
public List<{0}> {1}s = new List<{0}>();";

#region Read (De-Serialize)

		// {0} : 변수 이름
		// {1} : 변환 함수명 (ToSingle, ToInt64, ToUInt16 등)
		// {2} : 변수 타입
		public static string readFormat =
@"this.{0} = BitConverter.{1}(arr.Array, arr.Offset + count);
count += sizeof({2});";

		// {0} : 변수 이름
		// {1} : 변수 타입 (sbyte에도 대응해야 하니 캐스팅을 해 준다)
		public static string readByteFormat =
@"this.{0} = ({1})arr.Array[arr.Offset + count];
count += sizeof({1});";

		// {0} : 변수 이름
		public static string readStringFormat =
@"ushort {0}Len = BitConverter.ToUInt16(arr.Array, arr.Offset + count);
count += sizeof(ushort);
this.{0} = Encoding.Unicode.GetString(arr.Array, arr.Offset + count, {0}Len);
count += {0}Len;";

		// {0} : 리스트 멤버 이름 (파스칼 표기)
		// {1} : 리스트 멤버 이름 (카멜 표기)
		public static string readListFormat =
@"this.{1}s.Clear();
ushort {1}Len = BitConverter.ToUInt16(arr.Array, arr.Offset + count);
count += sizeof(ushort);
for (int i = 0; i < {1}Len; i++)
{{
	{0} {1} = new {0}();
	{1}.DeSerialize(arr, ref count);
	{1}s.Add({1});
}}";

#endregion

#region Write (Serialize)

		// {0} : 변수 이름
		// {1} : 변수 타입
		public static string writeFormat =
@"Array.Copy(BitConverter.GetBytes({0}), 0, arr.Array, arr.Offset + count, sizeof({1}));
count += sizeof({1});";

		// {0} : 변수 이름
		// {1} : 변수 타입
		public static string writeByteFormat =
@"arr.Array[arr.Offset + count] = (byte)this.{0};
count += sizeof({1});";

		// {0} : 변수 이름
		public static string writeStringFormat =
@"ushort {0}Len = (ushort)Encoding.Unicode.GetBytes({0}, 0, {0}.Length, arr.Array, arr.Offset + count + sizeof(ushort));
Array.Copy(BitConverter.GetBytes({0}Len), 0, arr.Array, arr.Offset + count, sizeof(ushort));
count += sizeof(ushort);
count += {0}Len;";

		// {0} : 리스트 멤버 이름 (파스칼 표기)
		// {1} : 리스트 멤버 이름 (카멜 표기)
		public static string writeListFormat =
@"Array.Copy(BitConverter.GetBytes((ushort){1}s.Count), 0, arr.Array, arr.Offset + count, sizeof(ushort));
count += sizeof(ushort);
foreach ({0} {1} in {1}s)
	{1}.Serialize(arr, ref count);";

#endregion Write

		#endregion
	}
}
