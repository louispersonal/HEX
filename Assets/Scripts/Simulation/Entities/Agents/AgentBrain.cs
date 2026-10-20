using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBrain : Brain
{
    public Agent Agent => Pawn as Agent;
    
    public AgentBrain(Agent agent) : base(agent)
    {
        
    }
}
