﻿using REPOLib.Modules;
using UnityEngine;

namespace REPOLib.Commands;

internal static class SpawnItemCommand
{
    [CommandExecution(
        "Spawn Item",
        "Spawn an instance of an item with the specified (case-insensitive) name. You can optionally leave out \"Item \" from the prefab name.",
        requiresDeveloperMode: true
    )]
    [CommandAlias("spawnitem")]
    [CommandAlias("si")]
    public static void Execute(string args)
    {
        Logger.LogInfo($"Running spawn command with args \"{args}\"", extended: true);

        if (string.IsNullOrWhiteSpace(args))
        {
            Logger.LogWarning("No args provided to spawn command.");
            return;
        }

        if (!SemiFunc.IsMasterClientOrSingleplayer())
        {
            Logger.LogError("Only the host can spawn items!");
            return;
        }

        if (StatsManager.instance == null)
        {
            Logger.LogError("Failed spawn item command, StatsManager is not initialized.");
            return;
        }

        if (PlayerAvatar.instance == null)
        {
            Logger.LogWarning("Can't spawn anything, player avatar is not initialized.");
            return;
        }

        string name = args;

        Vector3 position = PlayerAvatar.instance.transform.position + new Vector3(0f, 1f, 0f) + PlayerAvatar.instance.transform.forward * 1f;

        Logger.LogInfo($"Trying to spawn item \"{name}\" at {position}...", extended: true);

        if (!Items.TryGetItemThatContainsName(name, out Item? item))
        {
            Logger.LogWarning($"Spawn command failed. Unknown item with name \"{name}\"");
            return;
        }

        Items.SpawnItem(item, position, Quaternion.identity);
    }
}
