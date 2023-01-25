using System;

namespace EnetOpCode
{
    public enum OpCode : Int32
    {
        C_PlayerName = 0,
        S_PlayerNumber = 1,
    }
}


public abstract class GeneriqueOpCode
{
    public EnetOpCode.OpCode opCode;
    public abstract void Serialize(ref byte[] byteArray);
    public abstract void Unserialize(ref byte[] byteArray, Int32 offset);
}
