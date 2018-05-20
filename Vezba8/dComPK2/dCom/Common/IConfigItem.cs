namespace Common
{
	public interface IConfigItem
	{
		PointType RegistryType { get; }
		ushort NumberOfRegisters { get; }
		ushort StartAddress { get; }
		ushort DecimalSeparatorPlace { get; }
		ushort MinValue { get; }
		ushort MaxValue { get; }
		ushort DefaultValue { get; }
		string ProcessingType { get; }
		string Description { get; }
        int AcquisitionInterval { get; }
        int A { get; }
        int B { get; }
        int EGU_Min { get; }
        int EGU_Max { get; }
        int AbnormalValue { get; }
        int LowAlarm { get; }
        int HighAlarm { get; }
	}
}
