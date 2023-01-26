using System;

namespace EnetOpCode
{
    public enum OpCode : Int32
    {
        C_PlayerName = 0,
        S_PlayerNumber = 1,
        C_PlayerInput = 2,
        S_PlayerPosition = 3,
        S_BallPosition = 4,
    }
}


public abstract class GeneriqueOpCode
{
    public EnetOpCode.OpCode opCode;
    public abstract void Serialize(ref byte[] byteArray);
    public abstract void Unserialize(ref byte[] byteArray, Int32 offset);
}
