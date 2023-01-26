using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerPosition : GeneriqueOpCode
{
    public int playerNumber;
    public float playerPosX;
    public float playerPosY;
    public int inputIndex;

    public S_PlayerPosition(int playerNumber = 0, float playerPosX = 0f, float playerPosY = 0f, int inputIndex = 0)
    {
        opCode = EnetOpCode.OpCode.S_PlayerPosition;

        this.playerNumber = playerNumber;
        this.playerPosX = playerPosX;
        this.playerPosY = playerPosY;
        this.inputIndex = inputIndex;
    }

    public override void Serialize(ref byte[] byteArray)
    {
        NetworkCore.Serialize_i32(ref byteArray, playerNumber);
        NetworkCore.Serialize_float(ref byteArray, playerPosX);
        NetworkCore.Serialize_float(ref byteArray, playerPosY);
        NetworkCore.Serialize_i32(ref byteArray, playerNumber);
    }

    public override void Unserialize(ref byte[] byteArray, int offset)
    {
        this.playerNumber = NetworkCore.Unserialize_i32(ref byteArray, ref offset); 
        this.playerPosX = NetworkCore.Unserialize_float(ref byteArray, ref offset);
        this.playerPosY = NetworkCore.Unserialize_float(ref byteArray, ref offset);
        this.inputIndex = NetworkCore.Unserialize_i32(ref byteArray, ref offset);
    }
}
