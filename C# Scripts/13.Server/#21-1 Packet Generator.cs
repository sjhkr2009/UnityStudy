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

		// 앱이 빌드되는 경로는 프로젝트(PacketGenerator) 속성 - 빌드의 '출력 경로'에서 지정할 수 있다. (모든 구성에 대해 지정할 것)

		// 별도의 폴더를 만들지 않고 경로에 exe파일을 포함한 빌드된 파일들을 직접 저장하려면,
		// 프로젝트 파일(.csproj)을 열고 <PropertyGroup> 산하에
		// <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath> 을 추가하면 된다.
		// (XML 파일 등 파일 입출력 경로도 해당 위치를 기준으로 지정)

		static void Main(string[] args)
		{
			// XML 파일이 있는 경로가 들어갈 변수. 매개변수로 넘어온 경로를 받을 것이다.
			// 기본은 상위 폴더의 PDL.xml (./filename.xml 은 현재 폴더, 앞에 점을 붙일 때마다 한 단계 상위 폴더)
			string pdlPath = "../PDL.xml";

			XmlReaderSettings settings = new XmlReaderSettings()
			{
				IgnoreComments = true,
				IgnoreWhitespace = true
			};

			// 넘어온 인자가 있다면, XML파일 경로를 첫 번째 인자로 지정한다.
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
