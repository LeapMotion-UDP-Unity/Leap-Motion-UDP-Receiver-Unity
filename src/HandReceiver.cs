using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* 
利用方法
Sphere 等のオブジェクトに　Rigidbody を設定して このスクリプトを関連付ける
*/
public class HandReceiver : MonoBehaviour, IHandReceiver
{
    public enum HandType {
        left,
        right
    }

    public HandType type;

    private Renderer targetRenderer;
    public Color defaultColor = Color.gray;
    public Color color = Color.red;

    public Vector3 offsetPosition = new Vector3(5, -13, 0);

    private Vector3 targetOffsetPosition;
    private Rigidbody rb;

    void Start()
    {
        // 左手,右手 でオフセット入れ替える
        this.offsetPosition.x = (this.type == HandType.left) ? this.offsetPosition.x : this.offsetPosition.x * -1;

        this.rb = this.GetComponent<Rigidbody>();
        this.targetOffsetPosition = new Vector3(this.rb.position.x + this.offsetPosition.x,
                                                this.rb.position.y + this.offsetPosition.y,
                                                this.rb.position.z + this.offsetPosition.z);
        Debug.Log("targetOffsetPosition type " + this.type);                   
        Debug.Log(this.type + " x:" + this.targetOffsetPosition.x);
        Debug.Log(this.type + " y:" + this.targetOffsetPosition.y);
        Debug.Log(this.type + " z:" + this.targetOffsetPosition.z);

        this.targetRenderer = this.GetComponent<Renderer>();
        this.targetRenderer.material.color = this.defaultColor;
    }

    // https://tama-lab.net/2017/05/unity%E3%81%A7%E3%82%AD%E3%83%A3%E3%83%A9%E3%82%AF%E3%82%BF%E3%83%BC%E3%82%92%E7%A7%BB%E5%8B%95%E3%81%95%E3%81%9B%E3%82%8B%E6%96%B9%E6%B3%95%E3%80%903d%E7%B7%A8%E3%80%91/
    void Update() 
    {

    }

    // 色や位置などの更新
    private void onTargetUpdate(Cmd cmd){
       if (cmd.hand.type != type.ToString()) {
            return;
        }
        if (this.targetRenderer == null){
            return;
        }

        // 「にぎる」度合い 0〜1
        if (cmd.hand.grabStrength != 0) {
            // 手を閉じてる時
            this.targetRenderer.material.color = this.color;
        }
        else {
            // 手を開いてる時
            this.targetRenderer.material.color = this.defaultColor;        
        }

        // palmPosition 手のひらの中心位置 [X座標, Y座標, Z座標]
        float x = (float) Math.Round((float) cmd.hand.palmPosition[0]/10.0f, 1,  MidpointRounding.AwayFromZero); // cmd.hand.palmPosition[0]/100;
        float y = (float) Math.Round((float) cmd.hand.palmPosition[1]/10.0f, 1,  MidpointRounding.AwayFromZero); // cmd.hand.palmPosition[1]/100;
        float z = (float) Math.Round((float) cmd.hand.palmPosition[2]/10.0f, 1,  MidpointRounding.AwayFromZero) * -1;

        this.rb.MovePosition(new Vector3(this.targetOffsetPosition.x + x, 
                                    this.targetOffsetPosition.y + y,
                                    this.targetOffsetPosition.z + z));
    }

    public void onMessage(Dictionary<string, Cmd> cmds) {
        foreach(KeyValuePair<string, Cmd> item in cmds) {
            this.onTargetUpdate(item.Value);
        }
    }
}
