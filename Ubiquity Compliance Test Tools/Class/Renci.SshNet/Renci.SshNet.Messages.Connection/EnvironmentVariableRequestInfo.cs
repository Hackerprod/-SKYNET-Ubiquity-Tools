namespace Renci.SshNet.Messages.Connection
{
	internal class EnvironmentVariableRequestInfo : RequestInfo
	{
		public const string NAME = "env";

		public override string RequestName => "env";

		public string VariableName
		{
			get;
			set;
		}

		public string VariableValue
		{
			get;
			set;
		}

		public EnvironmentVariableRequestInfo()
		{
			base.WantReply = true;
		}

		public EnvironmentVariableRequestInfo(string variableName, string variableValue)
			: this()
		{
			VariableName = variableName;
			VariableValue = variableValue;
		}

		protected override void LoadData()
		{
			base.LoadData();
			VariableName = ReadString();
			VariableValue = ReadString();
		}

		protected override void SaveData()
		{
			base.SaveData();
			Write(VariableName);
			Write(VariableValue);
		}
	}
}
