using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class IpAddrText : MonoBehaviour
{
    public string intafaceName;

    private TextMesh targetText;

    void Start()
    {
        this.targetText = this.GetComponent<TextMesh>();
        if (this.targetText != null) {
            GetIpAddress();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetIpAddress() {
        NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var ni in nis)
        {
            IPInterfaceProperties ipip = ni.GetIPProperties();
            UnicastIPAddressInformationCollection uipaic = ipip.UnicastAddresses;
            foreach (var uipai in uipaic)
            {
                if (ni.NetworkInterfaceType != NetworkInterfaceType.Loopback
                    && ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel
                    && uipai.Address.AddressFamily.ToString() == "InterNetwork"
                    && ni.Name != "lo"
                    && ni.Name != "lo0"
                    && uipai.Address.ToString().IndexOf("169.254") == -1)
                {
                    Debug.Log("[" + ni.Name + "] " + uipai.Address);
                    if ((intafaceName != null) || (ni.Name == intafaceName)) {
                        targetText.text = uipai.Address.ToString();
                    } else {
                        if (targetText.text == "") {
                            targetText.text = uipai.Address.ToString();
                        }
                    }
                }
            }
        }
    }
}
