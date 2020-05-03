using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.Events;
using System.Collections.Generic;

/* 
利用方法
GameObject に このスクリプトを関連付ける事でUDPデータを受信できる
Inspector の UDP Received Event に IHandReceiver.onMessage を指定する
*/
public class UdpReceiver : MonoBehaviour
{
    public int LOCA_LPORT = 22222;

    static UdpClient udp = null;
    Thread thread;
    private static bool isRun = true;

    [System.Serializable] 
    public class UDPReceivedEvent: UnityEvent<Dictionary<string, Cmd>> { };
    public UDPReceivedEvent uDPReceivedEvent;
    
    private static Dictionary<string, Cmd> cmds;

    private static Mutex mutex = new Mutex();

    void Start ()
    {
        // UnityEvent を 初期化
        if (uDPReceivedEvent == null)
            uDPReceivedEvent = new UDPReceivedEvent();

        if (cmds == null)
            cmds = new Dictionary<string, Cmd>();

        Debug.Log("Start PORT:" + LOCA_LPORT);
        udp = new UdpClient(LOCA_LPORT);
        thread = new Thread(new ThreadStart(threadMethod));
        thread.IsBackground = true;
        thread.Start(); 
    }

    void OnEnable() {

    }

    void Update ()
    {
        // プロセス間の同期
        mutex.WaitOne();
        if (cmds == null)
            cmds = new Dictionary<string, Cmd>();
        if (cmds.Count > 0) {
            uDPReceivedEvent.Invoke(cmds);
            cmds.Clear();
        }
        mutex.ReleaseMutex();
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        isRun = false;
        //thread.Abort();
        if (udp != null) udp.Close();
    }

    void threadMethod()
    {
        Debug.Log("ThreadMethod Start");
        IPEndPoint remoteEP = null;
        while(isRun)
        {
            try {
                byte[] data = udp.Receive(ref remoteEP);
                string text = Encoding.ASCII.GetString(data);
                // 受信データを Json として cmd オブジェクトに変換
                Cmd cmd = JsonUtility.FromJson<Cmd> (text);
                string key = cmd.cmd + "-" + cmd.hand.type;
                // プロセス間の同期
                mutex.WaitOne();
                cmds[key] = cmd;
                mutex.ReleaseMutex();
            } catch (SocketException err) {
                Debug.Log(err.ToString());
            }
        }
        udp.Close();
        udp = null;
        // Debug.Log("ThreadMethod Stop");
    } 
}
