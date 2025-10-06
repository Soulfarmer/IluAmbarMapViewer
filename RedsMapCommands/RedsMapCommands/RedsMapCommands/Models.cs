using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Google.Cloud.Firestore;

namespace RedsMapCommands;

public static class Collections
{
    public const string  USERS="users";
    public const string  MARKERS="markers";
    public const string  ROUTES="routes";
    public const string  STORES="stores";
}
[FirestoreData]
public class LayerGroup
{
    [FirestoreProperty]
    public Waypoint[] waypoints { get; set; }

    public LayerGroup(Waypoint waypoint)
    {
        waypoints = new[] { waypoint };
    }
    public LayerGroup(){}
}

[FirestoreData]
public class Waypoint
{
    [FirestoreProperty]
    public string PlayerName { get; set; }
    [FirestoreProperty]
    public string DateAdded { get; set; }
    [FirestoreProperty]
    public string Title { get; set; }
    [FirestoreProperty]
    public string[] Coords { get; set; }
    [FirestoreProperty]
    public string Layer { get; set; }
    
    public Waypoint(){}
    public Waypoint(string playerName, string title, string coords, string layer)
    {
        var pos = new List<string>();
        pos.Add(coords);
        Coords = pos.ToArray();
        PlayerName = playerName;
        DateAdded = Timestamp.GetCurrentTimestamp().ToDateTime().ToString();
        Title = title;
        Layer = layer;
    }
}