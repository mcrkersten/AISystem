using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NinjaTree : Tree
{
    public List<GuardManager> guards = new List<GuardManager>();
    public List<Transform> hidingPlaces = new List<Transform>();
    public NavMeshAgent agent;
    public NinjaManager ninjaManager;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node> {

            new Sequence(new List<Node>{
                new NeedToHide(ninjaManager.player, agent, guards),

                new Sequence(new List<Node> {
                    new SearchHidingPlaces(agent, hidingPlaces),
                    new GoHide(agent, ninjaManager),
                }),
                new ThrowSmoke(agent, ninjaManager, ninjaManager.player.transform),
            }),

            new FollowPlayer(agent, ninjaManager.player.transform),
        });
        return root;
    }
}
