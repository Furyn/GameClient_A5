using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class C_PlayerInput : GeneriqueOpCode
{
    public bool isUp;
    public bool isDown;

    public C_PlayerInput(bool isUp, bool isDown)
    {
        opCode = EnetOpCode.OpCode.C_PlayerInput;
        this.isUp = isUp;
        this.isDown = isDown;
    }

    public override void Serialize(ref byte[] byteArray)
    {
        Int32 value = 0;
        value |= isUp ? 1 << 0 : 0;
        value |= isDown ? 1 << 1 : 0;

        NetworkCore.Serialize_i32(ref byteArray, value);
    }

    public override void Unserialize(ref byte[] byteArray, int offset)
    {
        Int32 value = NetworkCore.Unserialize_i32(ref byteArray, ref offset);

        isUp = (value & (1 << 0)) != 0;
        isDown = (value & (1 << 1)) != 0;
    }
}
