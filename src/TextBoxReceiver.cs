using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxReceiver : MonoBehaviour, IHandReceiver
{
    private TextMesh targetText;

    // Start is called before the first frame update
    void Start()
    {
        this.targetText = this.GetComponent<TextMesh>();
        if (this.targetText != null) {
            this.targetText.text = "XXXX";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onMessage(Dictionary<string, Cmd> cmds) {
        // Debug.Log("onMessage");
        // Debug.Log(" cmd:" + cmd.cmd);
        // Debug.Log(" hand.type:" + cmd.hand.type);
        if (this.targetText != null) {
            foreach(KeyValuePair<string, Cmd> item in cmds) {
                this.targetText.text = item.Value.hand.type;
            }
        }
    }
}
