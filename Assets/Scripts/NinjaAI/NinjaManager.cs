using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaManager : MonoBehaviour
{
    public GameObject player;
    public Cloud cloudElement;
    private NinjaState state;
    public GameObject smokeBombPrefab;
    public float followDistance;
    public float reloadTime;
    private float currentReload;

    public bool CanFireSmokebomb { get { return canFireSmokebomb; } }
    private bool canFireSmokebomb;

    public Color hiddenHidingPlace;
    public Color exposedHidingPlace;
    public void ThrowSmokebomb(Vector3 endpos)
    {
        GameObject grenade = Instantiate(smokeBombPrefab, transform.position, Quaternion.identity, null);
        SmokeBomb smokeBomb = grenade.GetComponent<SmokeBomb>();
        smokeBomb.startPos = this.transform.position;
        smokeBomb.endPos = endpos;
        currentReload = reloadTime;
    }

    public void Update()
    {
        if(currentReload > 0)
        {
            currentReload -= Time.deltaTime;
            canFireSmokebomb = false;
        }
        else
        {
            canFireSmokebomb = true;
        }
    }

    public void SetState(NinjaState state)
    {
        if (state != this.state)
        {
            this.state = state;
            switch (state)
            {
                case NinjaState.Following:
                    cloudElement.SetText("Following");
                    break;
                case NinjaState.Hiding:
                    cloudElement.SetText("Hiding");
                    break;
                case NinjaState.Reloading:
                    cloudElement.SetText("Reloading");
                    break;
                default:
                    break;
            }
        }
    }

    public enum NinjaState
    {
        None,
        Following,
        Hiding,
        Reloading
    }
}
