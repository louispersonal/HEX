using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerBrain : AgentBrain
{
    public Explorer Explorer => Agent as Explorer;
    
    public ExplorerBrain(Explorer explorer) : base(explorer)
    {
        
    }
}
