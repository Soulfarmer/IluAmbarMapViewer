using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace MapViewModSystem;

    /// <summary>
    /// The main entry point for the MapView client-side mod.
    /// This class is responsible for registering the new chat commands.
    /// </summary>
    public class MapViewModSystem : ModSystem
    {
        private ICoreClientAPI capi;

        /// <summary>
        /// Called by the game engine when the client is starting.
        /// This is the ideal place to register client-side only features like chat commands.
        /// </summary>
        /// <param name="api">The client-side API.</param>
        public override void StartClientSide(ICoreClientAPI api)
        {
            this.capi = api;

            // Register the base command "/mapview"
            var mapViewCmd = api.ChatCommands.GetOrCreate("mapview")
                .WithDescription("MapView command to manage map markers on different layers.")
                .RequiresPrivilege(Privilege.chat); // Player needs chat privilege to use

            // Register the "add" subcommand: /mapview add [layer] [color] [icon] [pinned] [title]
            mapViewCmd.BeginSubCommand("add")
                .WithDescription("Adds a map view marker at your current position.")
                // It's best practice to name the parsers for easy access later.
                .WithArgs(
                    new CommandArgumentParsers(api).Word("layer"),
                    new CommandArgumentParsers(api).Word("title")
                )
                .HandleWith(OnMapViewAdd)
                .EndSubCommand();

            // Register the "addat" subcommand: /mapview addat [layer] [coords] [color] [icon] [pinned] [title]
            mapViewCmd.BeginSubCommand("addat")
                .WithDescription("Adds a map view marker at the given coordinates.")
                .WithArgs(
                    new CommandArgumentParsers(api).Word("layer"),
                    new CommandArgumentParsers(api).WorldPosition("coords"),
                    new CommandArgumentParsers(api).Word("title")
                )
                .HandleWith(OnMapViewAddAt)
                .EndSubCommand();
        }

        /// <summary>
        /// Handles the logic for the "/mapview add" command.
        /// </summary>
        private TextCommandResult OnMapViewAdd(TextCommandCallingArgs args)
        {
            // The player who ran the command is available in args.Caller.Player
            IPlayer player = args.Caller.Player;
            if (player == null)
            {
                return TextCommandResult.Error("Command can only be run by a player.");
            }

            // Extract arguments from the parsed command
            string layer = (string)args[0];
            string color = (string)args[1];
            string icon = (string)args[2];
            bool pinned = (bool)args[3];
            string title = (string)args[4];

            // Get the player's current position for the marker
            BlockPos position = player.Entity.Pos.AsBlockPos;

            // --- Your Custom Code Area ---
            // You now have the player, position, and all the command arguments.
            // You can call your third-party code from here.
            capi.ShowChatMessage(
                $"MapView marker to be added on layer '{layer}' at {position} for player {player.PlayerName}.");
            capi.ShowChatMessage($"Title: {title}, Color: {color}, Icon: {icon}, Pinned: {pinned}");

            // TODO: Add your third-party code here.
            // Example: ThirdParty.AddMarker(player, position, layer, title, color, icon, pinned);
            // ---------------------------

            return TextCommandResult.Success($"Successfully processed MapView 'add' for layer '{layer}'.");
        }

        /// <summary>
        /// Handles the logic for the "/mapview addat" command.
        /// </summary>
        private TextCommandResult OnMapViewAddAt(TextCommandCallingArgs args)
        {
            // The player who ran the command is available in args.Caller.Player
            IPlayer player = args.Caller.Player;
            if (player == null)
            {
                return TextCommandResult.Error("Command can only be run by a player.");
            }

            // Extract arguments from the parsed command
            string layer = (string)args[0];
            Vec3d positionVec = (Vec3d)args[1];
            BlockPos position = new BlockPos((int)positionVec.X, (int)positionVec.Y, (int)positionVec.Z);
            string color = (string)args[2];
            string icon = (string)args[3];
            bool pinned = (bool)args[4];
            string title = (string)args[5];


            // --- Your Custom Code Area ---
            // You now have the player, the specified position, and all the command arguments.
            // You can call your third-party code from here.
            capi.ShowChatMessage(
                $"MapView marker to be added on layer '{layer}' at {position} for player {player.PlayerName}.");
            capi.ShowChatMessage($"Title: {title}, Color: {color}, Icon: {icon}, Pinned: {pinned}");

            // TODO: Add your third-party code here.
            // Example: ThirdParty.AddMarker(player, position, layer, title, color, icon, pinned);
            // ---------------------------

            return TextCommandResult.Success($"Successfully processed MapView 'addat' for layer '{layer}'.");
        }
    }