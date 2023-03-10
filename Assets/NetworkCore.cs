using ENet;
using System;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using TMPro;

public class NetworkCore : MonoBehaviour
{
    public static NetworkCore instance;

    [Header("Network")]
    public bool isConnected = false;
    public bool attemptToConnect = false;

    private ENet.Host m_enetHost = new ENet.Host();
    private Peer peer;

    [Header("Player Value")]
    public int playerNumber;

    public bool Connect(string addressString)
    {
        ENet.Address address = new ENet.Address();
        if (!address.SetHost(addressString))
            return false;

        address.Port = 14768;

        m_enetHost.Create(1, 0);
        peer = m_enetHost.Connect(address, 0);
        attemptToConnect = true;
        return true;
    }

    public void ConnectLocalhostButton()
    {
        Connect("localhost");
    }

    public void ConnectWithIPInput(string ip)
    {
        Connect(ip);
    }

    public void Disconnect()
    {
        peer.Disconnect(0);
        m_enetHost.Flush();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            gameObject.SetActive(false);
        }
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
                        isConnected = true;
                        attemptToConnect = false;
                        //SERIALISATION TEST

                        byte[] data = new byte[0];

                        GeneriqueOpCode packet_to_send = new C_PlayerName("Test");
                        Packet packet = build_packet(ref packet_to_send, PacketFlags.None);

                        peer.Send(0, ref packet);

                        packet.Dispose();
                        break;

                    case ENet.EventType.Disconnect:
                        Debug.Log("Disconnect");
                        isConnected = false;
                        attemptToConnect = false;
                        break;

                    case ENet.EventType.Receive:
                        byte[] dataPacket = new byte[evt.Packet.Length];
                        evt.Packet.CopyTo(dataPacket);

                        handle_message(dataPacket, evt);

                        evt.Packet.Dispose();
                        break;

                    case ENet.EventType.Timeout:
                        isConnected = false;
                        attemptToConnect = false;
                        Debug.Log("Timeout");
                        break;
                }
            }
            while (m_enetHost.CheckEvents(out evt) > 0);
        }
    }

    private void handle_message(byte[] dataPacket, ENet.Event evt)
    {
        int offset = 0;
        EnetOpCode.OpCode opcode = (EnetOpCode.OpCode)Unserialize_i32(ref dataPacket, ref offset);

        switch (opcode)
        {
            case EnetOpCode.OpCode.C_PlayerName:
                {
                    C_PlayerName playerName = new C_PlayerName();
                    playerName.Unserialize(ref dataPacket, offset);
                    Debug.Log(playerName.name);
                    break;
                }
            case EnetOpCode.OpCode.S_PlayerNumber:
                {
                    S_PlayerNumber playerNumber = new S_PlayerNumber();
                    playerNumber.Unserialize(ref dataPacket, offset);
                    this.playerNumber = playerNumber.playerNumber;
                    break;
                }
            case EnetOpCode.OpCode.S_PlayerPosition:
                {
                    if (GameManager.instance == null)
                    {
                        return;
                    }
                    S_PlayerPosition playerPosition = new S_PlayerPosition();
                    playerPosition.Unserialize(ref dataPacket, offset);
                    if (playerPosition.playerNumber == 1)
                    {
                        GameManager.instance.player1.SetPosition(playerPosition.playerPosX, playerPosition.playerPosY, playerPosition.inputIndex);

                    }
                    else if (playerPosition.playerNumber == 2)
                    {
                        GameManager.instance.player2.SetPosition(playerPosition.playerPosX, playerPosition.playerPosY, playerPosition.inputIndex);
                    }
                    break;
                }
            case EnetOpCode.OpCode.S_BallPosition:
                {
                    if (GameManager.instance == null)
                    {
                        return;
                    }
                    S_BallPosition ballPosition = new S_BallPosition();
                    ballPosition.Unserialize(ref dataPacket, offset);

                    GameManager.instance.ball.SetPosition(ballPosition.ballPosX, ballPosition.ballPosY);

                    break;
                }
            default:
                break;
        }

    }

    public static String DisplayBinary(Byte[] data)
    {
        return string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
    }

    #region Int Serialisation
    public static void Serialize_i32(ref byte[] byteArray, Int32 value)
    {
        int offset = byteArray.Length;
        Array.Resize(ref byteArray, offset + sizeof(Int32));
        Serialize_i32(ref byteArray, offset, value);
    }

    public static void Serialize_i32(ref byte[] byteArray, int offset, Int32 value)
    {
        //htonl;
        value = IPAddress.HostToNetworkOrder(value);

        byte[] valueByte = BitConverter.GetBytes(value);

        for (int i = 0; i < valueByte.Length; i++)
        {
            byteArray[offset + i] = valueByte[i];
        }
    }

    public static Int32 Unserialize_i32(ref byte[] byteArray, ref int offset)
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

    #region Float Serialisation
    public static void Serialize_float(ref byte[] byteArray, float value)
    {
        int offset = byteArray.Length;
        Array.Resize(ref byteArray, offset + sizeof(float));
        Serialize_float(ref byteArray, offset, value);
    }

    public static void Serialize_float(ref byte[] byteArray, int offset, float value)
    {
        byte[] valueByte = BitConverter.GetBytes(value);

        int x = IPAddress.HostToNetworkOrder(BitConverter.ToInt32(valueByte));

        valueByte = BitConverter.GetBytes(x);

        for (int i = 0; i < valueByte.Length; i++)
        {
            byteArray[offset + i] = valueByte[i];
        }
    }

    public static float Unserialize_float(ref byte[] byteArray, ref int offset)
    {
        float value;

        byte[] intByte = new byte[sizeof(float)];

        for (int i = offset; i < offset + sizeof(float); i++)
        {
            intByte[i - offset] = byteArray[i];
        }

        int x = BitConverter.ToInt32(intByte);
        x = IPAddress.NetworkToHostOrder(x);
        byte[] valueByte = BitConverter.GetBytes(x);

        value = BitConverter.ToSingle(valueByte);

        offset += sizeof(float);

        return value;
    }

    #endregion

    #region String Serialisation
    public static void Serialize_str(ref byte[] byteArray, string value)
    {
        int offset = byteArray.Length;
        Serialize_str(ref byteArray, offset, value);
    }

    public static void Serialize_str(ref byte[] byteArray, int offset, string value)
    {
        byte[] valueByte = Encoding.UTF8.GetBytes(value);

        Array.Resize(ref byteArray, offset + sizeof(Int32) + valueByte.Length);
        Serialize_i32(ref byteArray, offset, valueByte.Length);
        offset += sizeof(int);

        for (int i = 0; i < valueByte.Length; i++)
        {
            byteArray[offset + i] = valueByte[i];
        }
    }

    public static string Unserialize_str(ref byte[] byteArray, ref int offset)
    {
        int length = Unserialize_i32(ref byteArray, ref offset);

        byte[] strByte = new byte[length];

        for (int i = offset; i < offset + length; i++)
        {
            strByte[i - offset] = byteArray[i];
        }

        offset += length;
        return Encoding.UTF8.GetString(strByte);
    }
    #endregion

    public static Packet build_packet(ref GeneriqueOpCode packetOpCode, PacketFlags flags)
    {
        // On s?rialise l'opcode puis le contenu du packet dans un byte[]
        byte[] byteArray = new byte[0];

        Serialize_i32(ref byteArray, (int)(packetOpCode.opCode));
        packetOpCode.Serialize(ref byteArray);

        Packet packet = default(Packet);
        packet.Create(byteArray, flags);

        return packet;
    }

    public void SendPlayerInput(Player player)
    {
        byte[] data = new byte[0];

        GeneriqueOpCode packet_to_send = new C_PlayerInput(player.up, player.down);
        Packet packet = build_packet(ref packet_to_send, PacketFlags.None);

        peer.Send(0, ref packet);

        packet.Dispose();
    }

}
