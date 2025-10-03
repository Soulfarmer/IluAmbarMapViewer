using System;
using System.IO;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Config;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Google.Cloud.Firestore;

namespace RedsMapCommands;

public class RedsMapCommandsModSystem : ModSystem
{
    private ICoreClientAPI capi;
    private FirestoreDb firestoreDb;

    public override bool ShouldLoad(EnumAppSide side)
    {
        return side == EnumAppSide.Client;
    }
    
    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
        capi = api;
        Mod.Logger.Notification("Hello from template mod client side: " + Lang.Get("redsmapcommands:hello"));
            
        var mapAddCmd = api.ChatCommands.GetOrCreate("mapadd")
            .WithDescription("Adds information to the interactable map.")
            .RequiresPrivilege(Privilege.chat);

        mapAddCmd.BeginSubCommand("marker")
            .WithDescription("Adds a marker to the map at current position.")
            .WithArgs(
                new CommandArgumentParsers(api).Word("layer"),
                new CommandArgumentParsers(api).Word("title"))
            .HandleWith(OnMapAddMarker)
            .EndSubCommand();
        
        mapAddCmd.BeginSubCommand("markerat")
            .WithDescription("Adds a marker to the map at a given position.")
            .WithArgs(
                new CommandArgumentParsers(api).Word("layer"),
                new CommandArgumentParsers(api).Word("title"),
                new CommandArgumentParsers(api).Word("coords")
                )
            .HandleWith(OnMapAddMarkerAt)
            .EndSubCommand();
        
        mapAddCmd.BeginSubCommand("route")
            .WithDescription("Adds a route to the map.")
            .WithArgs(
                new CommandArgumentParsers(api).Word("name"))
            .WithExamples($"mapadd route my-route-name")
            .HandleWith(OnMapAddRoute)
            .EndSubCommand();

        mapAddCmd.BeginSubCommand("update")
            .WithDescription("Update the coordinates control value based on the spawn position on the web site.")
            .HandleWith(OnUpdateCoord)
            .WithExamples("mapadd update -43.102:-54.389")
            .EndSubCommand();
    }

    private void InitFirestore()
    {
        var configFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"VintagestoryData\ModConfig\RedsMapCommands\");
        try
        {
            if (string.IsNullOrEmpty( Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS"))){
                // Path to your credentials file. The server admin must place this file here.
                // Using "ModConfig" is a good practice.
                string credentialsPath = Path.Combine(configFolder, "myfirebasemod-credentials.json");

                if (!File.Exists(credentialsPath))
                {
                    Console.WriteLine("[MyFirebaseMod] Firebase credentials file not found at: {0}", credentialsPath);
                    return;
                }

                // Set the environment variable programmatically for the current process.
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
            }
            
            // Initialize FirestoreDb. The project ID is read from the credentials file.
            if (firestoreDb==null) 
                firestoreDb = FirestoreDb.Create("ilu-ambar-ce3ed"); // Pass your Firebase Project ID here if needed, e.g., FirestoreDb.Create("my-project-id")
            
            Console.WriteLine("[MyFirebaseMod] Successfully connected to Firestore.");
        }
        catch (Exception e)
        {
            Console.WriteLine("[MyFirebaseMod] Failed to initialize Firestore: {0}", e.ToString());
        }
    }
    
    private TextCommandResult OnMapAddMarkerAt(TextCommandCallingArgs args)
    {
        // The player who ran the command is available in args.Caller.Player
        IPlayer player = args.Caller.Player;
        if (player == null)
        {
            return TextCommandResult.Error("Command can only be run by a player.");
        }
        
        // Extract arguments from the parsed command
        string layer = (string)args[0];
        string title = (string)args[1];
        string position = (string)args[2];
        capi.ShowChatMessage($"Marker {title} at {position} on layer {layer} by player {player.PlayerName}.");
        
        var markerData = new
        {
            PlayerName = player.PlayerName,
            DateAdded = Timestamp.GetCurrentTimestamp(),
            Title=title,
            Coords=position
        };
        
        WaypointStore.WriteToMap("markers",player.PlayerUID+parsedTitle(title),markerData);
        
        return TextCommandResult.Success($"Marker added by  {player.PlayerName} at {position} on layer {layer}. ");
    }
    private TextCommandResult OnMapAddMarker(TextCommandCallingArgs args)
    {
        // The player who ran the command is available in args.Caller.Player
        IPlayer player = args.Caller.Player;
        if (player == null)
        {
            return TextCommandResult.Error("Command can only be run by a player.");
        }
        
        // Extract arguments from the parsed command
        string layer = (string)args[0];
        string title = (string)args[1];
        
        var position = GetWorldPosition(player);
        capi.ShowChatMessage($"Marker {title} at {position} on layer {layer} by player {player.PlayerName}.");
        
        var markerData = new
        {
            PlayerName = player.PlayerName,
            DateAdded = Timestamp.GetCurrentTimestamp(),
            Title=title,
            Coords=$"{position.X}:{position.Z}"
        };
        
        WaypointStore.WriteToMap("markers",player.PlayerUID+parsedTitle(title),markerData);
        
        return TextCommandResult.Success($"Marker added by  {player.PlayerName} at {position} on layer {layer}. ");
    }
    private TextCommandResult OnMapAddRoute(TextCommandCallingArgs args)
    {
        var _collectionName ="route";
        string[] coords = new string[1];
        IPlayer player = args.Caller.Player;
        if (player == null)
            return TextCommandResult.Error("Command can only be run by a player.");

        var position = GetWorldPosition(player);
        // Extract arguments from the parsed command
        string title = (string)args[0];
        
        coords[0] = $"{position.X}:{position.Z}";
        var markerData = new
        {
            PlayerName = player.PlayerName,
            DateAdded = Timestamp.GetCurrentTimestamp(),
            Title=title,
        };
        WaypointStore.WriteToMap(_collectionName,$"{player.PlayerUID}+{title.ToUpper()}",markerData,coords);
        return TextCommandResult.Success($"Route added by  {player.PlayerName}.");
    }

    private TextCommandResult OnUpdateCoord(TextCommandCallingArgs args)
    {
        string result = string.Empty;
        IPlayer player = args.Caller.Player;
        if (player == null)
        {
            return TextCommandResult.Error("Command can only be run by a player.");
        }
        string position = (string)args[0];
        try
        {
            result = WaypointStore.UpdateMapControlCoords(position);
        }
        catch (Exception e)
        {
            return TextCommandResult.Error(e.ToString());
        }

        return  TextCommandResult.Success($"{player.PlayerName}:{result}");
    }
    
    
    /// <summary>
    /// Converts the raw coordinates into a more readable format.
    /// Subtracting the world spawn point from the raw coordinates
    /// </summary>
    /// <param name="player">The Player instance. </param>
    /// <returns>A new BlockPos object with the world spawn subtracted from the raw coordinates.</returns>
    private BlockPos GetWorldPosition(IPlayer player)
    {
        try
        {
            var pos = player.Entity.Pos?.XYZ ?? new Vec3d(0, capi.World.SeaLevel, 0);
            var sp = capi?.World?.DefaultSpawnPosition;
            if (sp == null) return null;

            var bp = sp.AsBlockPos;
            return new BlockPos((int)pos.X-(int)bp.X, (int)pos.Y-(int)bp.Y, (int)pos.Z-(int)bp.Z);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Parses a waypoint name to a normalized state, all capitals and no spaces(replaced with a dash).
    /// </summary>
    /// <param name="title">The waypoint's title.</param>
    /// <returns>A normalized string in all caps without spaces("-" instead).</returns>
    private string parsedTitle(string title) => title.ToUpper().Replace(" ", "-");
}