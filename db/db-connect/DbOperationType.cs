namespace DbConnect
{
    /// <summary>
    /// Enum for database operation types
    /// </summary>
    public enum DbOperationType : uint
    {
        CreateUser              = 0x0,
        CreateMessage           = 0x1,
        CreateChatRooom         = 0x2,
        CreateVerificationCode  = 0x3,
        CreateNotification      = 0x4,
        VerifyUser              = 0x5
    }
}