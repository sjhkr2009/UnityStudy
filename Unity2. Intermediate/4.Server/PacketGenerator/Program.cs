using System;
using System.IO;
using System.Xml;

namespace PacketGenerator
{
	class Program
	{
		static string genPackets;
		static string packetEnums;
		static ushort packetId;

		// 패킷의 많은 비율이 클라에서만 또는 서버에서만 수신될 패킷이다.
		// 따라서 클라/서버용 패킷 매니저를 따로 만들어 올바른 종류의 패킷만 처리하게 한다.
		// 패킷의 용도는 XML 파일의 패킷 이름에 C_ 또는 S_ 를 붙여 구분한다.
		static string serverRegister;
		static string clientRegister;

		static void Main(string[] args)
		{
			string pdlPath = "../PDL.xml";

			XmlReaderSettings settings = new XmlReaderSettings()
			{
				IgnoreComments = true,
				IgnoreWhitespace = true
			};

			if (args.Length >= 1)
				pdlPath = args[0];

			using (XmlReader pdl = XmlReader.Create(pdlPath, settings))
			{
				pdl.MoveToContent();

				while(pdl.Read())
				{
					if (pdl.Depth == 1 && pdl.NodeType == XmlNodeType.Element)
						ParsePacket(pdl);
				}
				
				string fileText = string.Format(PacketFormat.fileFormat, packetEnums, genPackets);
				File.WriteAllText("GenPackets.cs", fileText);

				// 클라/서버용 패킷 매니저도 생성해준다. 내용은 파싱하는 부분에서 작성.
				string clientManagerText = string.Format(PacketFormat.managerFormat, clientRegister);
				File.WriteAllText("ClientPacketManager.cs", clientManagerText);
				string serverManagerText = string.Format(PacketFormat.managerFormat, serverRegister);
				File.WriteAllText("ServerPacketManager.cs", serverManagerText);
			}

		}

		// <packet>에 진입했을 때 호출된다. 모든 멤버를 파싱하여 genPackets에 넣는다.
		public static void ParsePacket(XmlReader xml)
		{
			if (xml.NodeType == XmlNodeType.EndElement)
				return;

			if (xml.Name.ToLower() != "packet")
			{
				Console.WriteLine("Invalid packet node.");
				return;
			}

			string packetName = xml["name"];
			if(string.IsNullOrEmpty(packetName))
			{
				Console.WriteLine("Packet without name.");
				return;
			}

			Tuple<string, string, string> t = ParseMembers(xml);
			genPackets += string.Format(PacketFormat.packetFormat, packetName, t.Item1, t.Item2, t.Item3);
			packetEnums += string.Format(PacketFormat.packetEnumFormat, packetName, ++packetId) + Environment.NewLine + "\t";

			// 패킷 이름으로 패킷 매니저 클래스 내용을 작성한다.
			// 타입에 맞는 패킷만 처리하도록 함수를 만든다. 타입은 패킷 이름 앞글자로 구분한다.
			// 포맷 자체에 들여쓰기를 했으므로 여기선 줄바꿈만 해준다. PacketFormat 참고.
			if (packetName.StartsWith("S_") || packetName.StartsWith("s_"))
				clientRegister += string.Format(PacketFormat.managerRegisterFormat, packetName) + Environment.NewLine;
			else
				serverRegister += string.Format(PacketFormat.managerRegisterFormat, packetName) + Environment.NewLine;
		}

		// ParsePacket에서 호출된다. 현재 읽은 지점의 모든 내부 멤버를 파싱하여 멤버 변수명, 직렬화, 역직렬화 포맷으로 반환한다.
		private static Tuple<string, string, string> ParseMembers(XmlReader xml)
		{
			string packetName = xml["name"];

			string memberCode = "";
			string readCode = "";
			string writeCode = "";

			int depth = xml.Depth + 1;

			while (xml.Read())
			{
				if (xml.Depth != depth)
					break;

				string memberName = xml["name"];
				if(string.IsNullOrEmpty(memberName))
				{
					Console.WriteLine("Member without name.");
					return null;
				}

				if (!string.IsNullOrEmpty(memberCode))
					memberCode += Environment.NewLine;
				if (!string.IsNullOrEmpty(readCode))
					readCode += Environment.NewLine;
				if (!string.IsNullOrEmpty(writeCode))
					writeCode += Environment.NewLine;

				string memberType = xml.Name.ToLower();

				switch (memberType)
				{
					case "byte":
					case "sbyte":
						memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);
						readCode += string.Format(PacketFormat.readByteFormat, memberName, memberType);
						writeCode += string.Format(PacketFormat.writeByteFormat, memberName, memberType);
						break;
					case "bool":
					case "short":
					case "ushort":
					case "int":
					case "long":
					case "float":
					case "double":
						memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);
						readCode += string.Format(PacketFormat.readFormat, memberName, ToConvertFuncName(memberType), memberType);
						writeCode += string.Format(PacketFormat.writeFormat, memberName, memberType);
						break;
					case "string":
						memberCode += string.Format(PacketFormat.memberFormat, memberType, memberName);
						readCode += string.Format(PacketFormat.readStringFormat, memberName);
						writeCode += string.Format(PacketFormat.writeStringFormat, memberName);
						break;
					case "list":
						Tuple<string, string, string> t = ParseStructList(xml);
						memberCode += t.Item1;
						readCode += t.Item2;
						writeCode += t.Item3;
						break;
					default:
						break;
				}
			}

			memberCode = memberCode.Replace("\n", "\n\t");
			readCode = readCode.Replace("\n", "\n\t\t");
			writeCode = writeCode.Replace("\n", "\n\t\t");

			return new Tuple<string, string, string>(memberCode, readCode, writeCode);
		}

		// 리스트 내부를 읽어서, 선언부와 직렬화/역직렬화 포맷에 맞게 파싱하여 반환한다.
		// 리스트의 요소를 파싱하는 방법은 포맷에 따라 선언부에서 구조체로 정의되어 있다.
		private static Tuple<string, string, string> ParseStructList(XmlReader xml)
		{
			string listName = xml["name"];
			if(string.IsNullOrEmpty(listName))
			{
				Console.WriteLine("List without name.");
				return null;
			}

			Tuple<string, string, string> t = ParseMembers(xml);

			string memberCode = string.Format(PacketFormat.memberListFormat,
				FirstCharToUpper(listName),
				FirstCharToLower(listName),
				t.Item1,
				t.Item2,
				t.Item3
			);

			string readCode = string.Format(PacketFormat.readListFormat,
				FirstCharToUpper(listName),
				FirstCharToLower(listName)
			);

			string writeCode = string.Format(PacketFormat.writeListFormat,
				FirstCharToUpper(listName),
				FirstCharToLower(listName)
			);

			return new Tuple<string, string, string>(memberCode, readCode, writeCode);
		}

		// 타입 이름을 입력받아 To~ 계열의 비트 변환 함수명을 반환한다.
		// 변환 함수가 없는 타입이면 빈 문자열을 반환한다.
		public static string ToConvertFuncName(string typeName)
		{
			switch (typeName)
			{
				case "bool":
					return "ToBoolean";
				case "short":
					return "ToInt16";
				case "ushort":
					return "ToUInt16";
				case "int":
					return "ToInt32";
				case "long":
					return "ToInt64";
				case "float":
					return "ToSingle";
				case "double":
					return "ToDouble";

				default:
					return "";
			}
		}

		// 문자열의 첫 글자를 대문자로 바꿔 반환한다.
		// 타입명 등 파스칼 표기법에서 사용된다.
		public static string FirstCharToUpper(string input)
		{
			if (string.IsNullOrEmpty(input))
				return "";

			return input[0].ToString().ToUpper() + input.Substring(1);
		}
		// 문자열의 첫 글자를 소문자로 바꿔 반환한다.
		// 변수 등 카멜 표기법에서 사용된다.
		public static string FirstCharToLower(string input)
		{
			if (string.IsNullOrEmpty(input))
				return "";

			return input[0].ToString().ToLower() + input.Substring(1);
		}
	}
}
