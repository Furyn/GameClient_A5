using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCore : MonoBehaviour
{
    ENet.Host m_enetHost = new ENet.Host();

    public bool Connect(string addressString)
    {
        ENet.Address address = new ENet.Address();
        if (!address.SetHost(addressString))
            return false;

        address.Port = 14768;

        m_enetHost.Connect(address, 0);
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!ENet.Library.Initialize())
            throw new Exception("Failed to initialize ENet");

        DontDestroyOnLoad(this.gameObject);

        m_enetHost.Create(1, 0);

        // Ne laissez pas le Connect ici (un écran de connexion est si vite arrivé)
        Connect("localhost");
    }
    private void OnApplicationQuit()
    {
        ENet.Library.Deinitialize();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
}
