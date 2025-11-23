using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("No Restore In Zone Manager", "WhiteThunder", "1.0.2")]
    [Description("Prevents player inventories from being restored after dying in a ZoneManager zone.")]
    internal class NoRestoreInZoneManager : CovalencePlugin
    {
        private readonly object False = false;

        [PluginReference]
        private Plugin ZoneManager;

        private object OnRestoreUponDeath(BasePlayer player)
        {
            if (ZoneManager == null || player == null)
                return null;

            // Retrieve all zones player is currently in
            var zoneIdList = ZoneManager.Call("GetPlayerZoneIDs", player) as string[];
            if (zoneIdList == null || zoneIdList.Length == 0)
                return null;

            // If player is in any zone, block Restore Upon Death
            foreach (var zoneId in zoneIdList)
            {
                // Optional sanity check (though redundant)
                if (ZoneManager.Call("IsPlayerInZone", zoneId, player) is bool inZone && inZone)
                {
                    Puts($"Blocking RestoreUponDeath for {player.displayName} (in zone {zoneId}).");
                    return False;
                }
            }

            return null;
        }
    }
}
