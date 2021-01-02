using System;
using System.Collections.Generic;
using System.Text;

namespace PacketGenerator
{
	class PacketFormat
	{
		// {0} : 패킷 ID들 (enum)
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

{1}
";

		// {0} : 패킷 ID 이름
		// {1} : 패킷 ID (정수)
		public static string packetEnumFormat =
@"{0} = {1},";


		/* 
		 * {0} : 패킷 이름
		 * {1} : 멤버 변수들
		 * {2} : 멤버 변수 읽기(De-Serialize)
		 * {3} : 멤버 변수 쓰기(Serialize)
		*/
		public static string packetFormat =
@"
class {0}
{{
	{1}

	public ArraySegment<byte> Serialize()
	{{
		ArraySegment<byte> arr = SendBufferHelper.Open(4096);
		Span<byte> span = new Span<byte>(arr.Array, arr.Offset, arr.Count);

		ushort count = 0;
		bool success = true;

		count += sizeof(ushort);
		success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), (ushort)PacketID.{0});
		count += sizeof(ushort);
		
		{3}

		success &= BitConverter.TryWriteBytes(span, count);

		if (!success)
			return null;

		return SendBufferHelper.Close(count);
	}}
	public void DeSerialize(ArraySegment<byte> arr)
	{{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);

		ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(arr.Array, arr.Offset, arr.Count);

		{2}
	}}
}}
";
		// {0} : 변수 타입
		// {1} : 변수 이름
		public static string memberFormat =
@"public {0} {1};";

		// {0} : 리스트 멤버 이름 (파스칼 표기)
		// {1} : 리스트 멤버 이름 (카멜 표기)
		// {2} : 구조체 멤버 변수들
		// {3} : 구조체 멤버 변수 읽기(De-Serialize)
		// {4} : 구조체 멤버 변수 쓰기(Serialize)
		public static string memberListFormat =
@"public struct {0}
{{
	{2}

	public bool Serialize(Span<byte> span, ref ushort count)
	{{
		bool success = true;

		{4}

		return success;
	}}

	public void DeSerialize(ReadOnlySpan<byte> span, ref ushort count)
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
@"this.{0} = BitConverter.{1}(span.Slice(count, span.Length - count));
count += sizeof({2});";

		// {0} : 변수 이름
		// {1} : 변수 타입 (sbyte에도 대응해야 하니 캐스팅을 해 준다)
		public static string readByteFormat =
@"this.{0} = ({1})arr.Array[arr.Offset + count];
count += sizeof({1});";

		// {0} : 변수 이름
		public static string readStringFormat =
@"ushort {0}Len = BitConverter.ToUInt16(span.Slice(count, span.Length - count));
count += sizeof(ushort);
this.{0} = Encoding.Unicode.GetString(span.Slice(count, {0}Len));
count += {0}Len;";

		// {0} : 리스트 멤버 이름 (파스칼 표기)
		// {1} : 리스트 멤버 이름 (카멜 표기)
		public static string readListFormat =
@"this.{1}s.Clear();
ushort {1}Len = BitConverter.ToUInt16(span.Slice(count, span.Length - count));
count += sizeof(ushort);
for (int i = 0; i < {1}Len; i++)
{{
	{0} {1} = new {0}();
	{1}.DeSerialize(span, ref count);
	{1}s.Add({1});
}}";

#endregion

#region Write (Serialize)

		// {0} : 변수 이름
		// {1} : 변수 타입
		public static string writeFormat =
@"success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), {0});
count += sizeof({1});";

		// {0} : 변수 이름
		// {1} : 변수 타입
		public static string writeByteFormat =
@"arr.Array[arr.Offset + count] = (byte)this.{0};
count += sizeof({1});";

		// {0} : 변수 이름
		public static string writeStringFormat =
@"ushort {0}Len = (ushort)Encoding.Unicode.GetBytes({0}, 0, {0}.Length, arr.Array, arr.Offset + count + sizeof(ushort));
success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), {0}Len);
count += sizeof(ushort);
count += {0}Len;";

		// {0} : 리스트 멤버 이름 (파스칼 표기)
		// {1} : 리스트 멤버 이름 (카멜 표기)
		public static string writeListFormat =
@"success &= BitConverter.TryWriteBytes(span.Slice(count, span.Length - count), (ushort){1}s.Count);
count += sizeof(ushort);
foreach ({0} {1} in {1}s)
	success &= {1}.Serialize(span, ref count);";

#endregion
	}
}
