using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardTree : Tree
{
    public Transform[] waypoints;
    public List<WeaponCase> weaponCases = new List<WeaponCase>();
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask layerMask;


    //Selectors: If a node fails, it tries the next one.
    //Sequence:  If the node fails, go back up the tree.


    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new SearchForPlayer(agent.transform, player, layerMask),

                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new SearchForWeaponCase(agent.transform, weaponCases),
                        new GoToWeaponCase(agent),
                        new GrabWeapon(agent),
                    }),

                    new Selector(new List<Node>
                    {
                        new Selector(new List<Node>
                        {
                            new GoToLastKnownPlayerPosition(agent, player, layerMask),
                            new GoToPlayerLocation(agent, player, layerMask),
                        }),

                        new Selector(new List<Node>
                        {
                            new ReturnToWeaponCase(agent),
                            new ReturnWeapon(agent),
                        }),

                        new AttackPlayer(agent, player, layerMask),
                    }),
                }),
            }),

            new Patrol(agent, waypoints),
        });

        return root;
    }
}
