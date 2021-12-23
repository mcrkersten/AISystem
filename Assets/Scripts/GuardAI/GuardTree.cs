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


    //Selectors: If a node fails, it tries the next one.
    //Sequence:  If the node fails, go back up the tree.
    protected override void Start()
    {
        base.Start();
        PlayerController.playerDies += OnPlayerDeath;
    }


    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new SearchForPlayer(agent.transform, player),
                new Blinded(agent),
                //Collect, attack return
                new Selector(new List<Node>
                {

                    new AttackPlayer(agent, player),
                    //Collect weapon
                    new Sequence(new List<Node>
                    {
                        new SearchForWeaponCase(agent.transform, weaponCases),
                        new GoToWeaponCase(agent),
                        new CollectWeapon(agent),
                    }),

                    //go to player
                    new Selector(new List<Node>
                    {
                        new GoToPlayerLocation(agent, player),
                        new GoToLastSeenPlayerPosition(agent, player),
                    }),

                    //Return weapon
                    new Selector(new List<Node>
                    {
                        new ReturnToWeaponCase(agent),
                        new ReturnWeapon(agent),
                    }),
                }),
            }),

            new Patrol(agent, waypoints),
        });

        return root;
    }

    private void OnPlayerDeath()
    {
        agent.GetComponent<GuardManager>().playerDied = true;
        root.ClearData("Player");
    }

    private void OnDestroy()
    {
        PlayerController.playerDies -= OnPlayerDeath;
    }
}
