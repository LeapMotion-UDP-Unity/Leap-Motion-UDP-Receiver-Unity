using System.Collections.Generic;

public interface IHandReceiver
{
    void onMessage(Dictionary<string, Cmd> cmds);
}
