using System;

public class S_PlayerNumber : GeneriqueOpCode
{
    public Int32 playerNumber;

    public S_PlayerNumber(Int32 playerNumber = 0)
    {
        opCode = EnetOpCode.OpCode.C_PlayerName;
        this.playerNumber = playerNumber;
    }

    public override void Serialize(ref byte[] byteArray)
    {
        NetworkCore.Serialize_i32(ref byteArray, playerNumber);
    }

    public override void Unserialize(ref byte[] byteArray, int offset)
    {
        this.playerNumber = NetworkCore.Unserialize_i32(ref byteArray, ref offset);
    }
}
