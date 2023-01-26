using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BallPosition : GeneriqueOpCode
{
    public float ballPosX;
    public float ballPosY;

    public S_BallPosition(float ballPosX = 0f, float ballPosY = 0f)
    {
        opCode = EnetOpCode.OpCode.S_BallPosition;

        this.ballPosX = ballPosX;
        this.ballPosY = ballPosY;
    }

    public override void Serialize(ref byte[] byteArray)
    {
        NetworkCore.Serialize_float(ref byteArray, ballPosX);
        NetworkCore.Serialize_float(ref byteArray, ballPosY);
    }

    public override void Unserialize(ref byte[] byteArray, int offset)
    {
        this.ballPosX = NetworkCore.Unserialize_float(ref byteArray, ref offset);
        this.ballPosY = NetworkCore.Unserialize_float(ref byteArray, ref offset);
    }
}
