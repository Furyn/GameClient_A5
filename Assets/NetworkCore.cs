using ENet;
using System;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using TMPro;

public class NetworkCore : MonoBehaviour
{
    ENet.Host m_enetHost = new ENet.Host();
    Peer peer;
    public TMP_InputField ipField;

    public bool Connect(string addressString)
    {
        ENet.Address address = new ENet.Address();
        if (!address.SetHost(addressString))
            return false;

        address.Port = 14768;

        m_enetHost.Create(1, 0);
        peer = m_enetHost.Connect(address, 0);
        return true;
    }

    public void ConnectLocalhostButton()
    {
        Connect("localhost");
    }

    public void ConnectWithIPInput()
    {
        Connect(ipField.text);
    }

    public void Disconnect()
    {
        peer.Disconnect(0);
        m_enetHost.Flush();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!ENet.Library.Initialize())
            throw new Exception("Failed to initialize ENet");

        DontDestroyOnLoad(this.gameObject);
    }
    private void OnApplicationQuit()
    {
        Disconnect();
        ENet.Library.Deinitialize();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!peer.IsSet)
            return;

        ENet.Event evt = new ENet.Event();
        if (m_enetHost.Service(0, out evt) > 0)
        {
            do
            {
                switch (evt.Type)
                {
                    case ENet.EventType.None:
                        Debug.Log("?");
                        break;

                    case ENet.EventType.Connect:
                        Debug.Log("Connect");
                        //SERIALISATION TEST
                        byte[] data = new byte[0];
                        int offset = 0;
                        string str = "THE WORLD !";

                        Serialize_u32(ref data, 42);
                        SerializeString(ref data, str);

                        Packet packet = default(Packet);

                        packet.Create(data);
                        peer.Send(0, ref packet);

                        //Debug.Log(Unserialize_u32(ref data, ref offset));
                        //Debug.Log(Unserialize_str(ref data, ref offset));
                        break;

                    case ENet.EventType.Disconnect:
                        Debug.Log("Disconnect");
                        break;

                    case ENet.EventType.Receive:
                        Debug.Log("Receive");
                        break;

                    case ENet.EventType.Timeout:
                        Debug.Log("Timeout");
                        break;
                }
            }
            while (m_enetHost.CheckEvents(out evt) > 0);
        }
    }

    public static String DisplayBinary(Byte[] data)
    {
        return string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
    }

#region Int Serialisation
    public static void Serialize_u32(ref byte[] byteArray, Int32 value)
    {
        int offset = byteArray.Length;
        Array.Resize(ref byteArray, offset + sizeof(Int32));
        Serialize_u32(ref byteArray, offset, value);
    }

    public static void Serialize_u32(ref byte[] byteArray, int offset, Int32 value)
    {
        //htonl;
        value = IPAddress.HostToNetworkOrder(value);

        byte[] valueByte = BitConverter.GetBytes(value);

        for (int i = 0; i < valueByte.Length; i++)
        {
            byteArray[offset + i] = valueByte[i];
        }
    }

    public static Int32 Unserialize_u32(ref byte[] byteArray, ref int offset)
    {
	    Int32 value;

        byte[] intByte = new byte[sizeof(Int32)];

        for (int i = offset; i < offset + sizeof(Int32); i++)
        {
            intByte[i - offset] = byteArray[i];
        }

        value = BitConverter.ToInt32(intByte);

        //ntohl
        value = IPAddress.NetworkToHostOrder(value);

        offset += sizeof(Int32);

	    return value;
    }
#endregion

#region String Serialisation
    public static void SerializeString(ref byte[] byteArray, string value)
    {
        int offset = byteArray.Length;
        Array.Resize(ref byteArray, offset + sizeof(Int32) + value.Length);
        SerializeString(ref byteArray, offset, value);
    }

    public static void SerializeString(ref byte[] byteArray, int offset, string value)
    {
        Serialize_u32(ref byteArray, offset ,value.Length);
        offset += sizeof(int);

        byte[] valueByte = System.Text.Encoding.ASCII.GetBytes(value);

        for (int i = 0; i < value.Length; i++)
        {
            byteArray[offset + i] = valueByte[i];
        }
    }

    public static string Unserialize_str(ref byte[] byteArray, ref int offset)
    {
	    int length = Unserialize_u32(ref byteArray, ref offset);

        byte[] strByte = new byte[length];

        for (int i = offset; i < offset+length; i++)
        {
            strByte[i - offset] = byteArray[i];
        }

        offset += length;
        return Encoding.ASCII.GetString(strByte);
    }
#endregion

}
