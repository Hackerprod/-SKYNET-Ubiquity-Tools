using Renci.SshNet.Common;
using System;
using System.Collections.Generic;

namespace Renci.SshNet.Messages.Authentication
{
	[Message("SSH_MSG_USERAUTH_INFO_REQUEST", 60)]
	internal class InformationRequestMessage : Message
	{
		public string Name
		{
			get;
			private set;
		}

		public string Instruction
		{
			get;
			private set;
		}

		public string Language
		{
			get;
			private set;
		}

		public IEnumerable<AuthenticationPrompt> Prompts
		{
			get;
			private set;
		}

		protected override void LoadData()
		{
			Name = ReadString();
			Instruction = ReadString();
			Language = ReadString();
			uint num = ReadUInt32();
			List<AuthenticationPrompt> list = new List<AuthenticationPrompt>();
			for (int i = 0; i < num; i++)
			{
				string request = ReadString();
				bool isEchoed = ReadBoolean();
				list.Add(new AuthenticationPrompt(i, isEchoed, request));
			}
			Prompts = list;
		}

		protected override void SaveData()
		{
			throw new NotImplementedException();
		}
	}
}
