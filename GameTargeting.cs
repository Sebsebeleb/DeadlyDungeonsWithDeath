using UnityEngine;

public static class GameTargeting
{
    public static bool IsTargeting = false; // While true, regular input shouldnt work (movement, certain keys)
    public static Vector2 Result;

    private static GameObject gm;


    //public Transform uHighlights;

    //Returns number of valid targets, if 0, casting should be interupted.
    public static int GetTarget(GameObject who, TargetingScheme scheme, int ox, int oy, int range)
    {
        int NumTargets = 0;

        gm = GameObject.FindGameObjectWithTag("GM");
        var lvl = gm.GetComponent<Level>();

        IsTargeting = true;

        //TODO: Check if we have line of sight as well
        foreach (Vector2 pos in Utils.CoordsInRange(range, ox, oy))
        {
            var x = (int) pos.x;
            var y = (int) pos.y;
            TileData tile = lvl.getAt(x, y);
            if (tile == null)
            {
                continue;
            }
            if (TargetValid(who, tile, scheme, x, y))
            {
                NumTargets++;
                TargetType typ = GetTargetTypeAt(who, tile, scheme, x, y);
                gm.SendMessage("DisplayTarget", new paramsTargetingDisplay(typ, new Vector2(x, y)));
                //Transform hightlight = Instantiate(prefabHighlight, new Vector3(x, y, 0), Quaternion.identity) as Transform;
                //hightlight.parent = uHighlights;
            }
        }

        return NumTargets;
    }

    // Returns the target we will hit by targeting at the speciified position with the specified scheme
    private static TargetType GetTargetTypeAt(GameObject who, TileData tile, TargetingScheme scheme, int x, int y)
    {
        switch (scheme)
        {
            case TargetingScheme.ACTORS:
                if (tile.actor.tag == who.tag)
                {
                    return TargetType.ALLY;
                }
                return TargetType.ENEMY;
            case TargetingScheme.UNOCCUPIED:
            {
                return TargetType.UNOCCUPIED;
            }
            case TargetingScheme.EVERYTHING:
            {
                if (tile.actor != null)
                {
                    if (tile.actor.tag == who.tag)
                    {
                        return TargetType.ALLY;
                    }
                    return TargetType.ENEMY;
                }
                return TargetType.UNOCCUPIED;
            }
            case TargetingScheme.ALLIES:
            {
                return TargetType.ALLY;
            }
            case TargetingScheme.ENEMIES:
            {
                return TargetType.ENEMY;
            }
            default:
            {
                return TargetType.UNOCCUPIED;
            }
        }
    }

    private static bool TargetValid(GameObject who, TileData tile, TargetingScheme scheme, int x, int y)
    {
        switch (scheme)
        {
            case TargetingScheme.EVERYTHING:
                return true;
            case TargetingScheme.ACTORS:
                return tile.actor != null;
            case TargetingScheme.ALLIES:
                if (tile.actor == null)
                {
                    return false;
                }
                return tile.actor.tag == who.tag;
            case TargetingScheme.ENEMIES:
                if (tile.actor == null)
                {
                    return false;
                }
                return tile.actor.tag != who.tag;
            case TargetingScheme.UNOCCUPIED:
                return tile.actor == null;
            default:
                return false;
        }
    }

    public static void SetResult(Vector3 where)
    {
        Result = new Vector2(where.x, where.y);
        IsTargeting = false;
        gm.BroadcastMessage("RemoveTargets");
    }

    public static void CancelTargeting()
    {
        IsTargeting = false;
        gm.BroadcastMessage("RemoveTargets");
    }
}