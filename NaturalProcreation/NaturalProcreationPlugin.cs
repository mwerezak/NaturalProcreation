using System.Linq;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using TimberApi.ConsoleSystem;
using TimberApi.ModSystem;
using Timberborn.DwellingSystem;
using Timberborn.GameFactionSystem;
using Timberborn.Reproduction;

namespace NaturalProcreation
{
    [HarmonyPatch]
    public class NaturalProcreationPlugin : IModEntrypoint
    {
        internal static IConsoleWriter? Log;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter;

            // Harmony patches
            new Harmony("com.orinoco.plugin.natural_procreation").PatchAll();
        }

        // Patch FactionObjectsCollection
        private static IEnumerable<Object>? _factionObjectCache = null;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(FactionObjectCollection), nameof(FactionObjectCollection.GetObjects))]
        static bool PatchUseCachedFactionObjects(ref IEnumerable<Object> __result)
        {
            if (_factionObjectCache != null)
            {
                __result = _factionObjectCache;
                return false;
            }
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FactionObjectCollection), nameof(FactionObjectCollection.GetObjects))]
        static IEnumerable<Object> PatchBuildFactionObjectCache(IEnumerable<Object> result)
        {
            // should never happen because of the prefix patch, but just in case...
            if (_factionObjectCache != null)
            {
                return _factionObjectCache;
            }

            Log?.LogInfo("Build faction object cache.");
            var objects = result.ToList();

            // Remove breeding pods
            objects.RemoveAll(o =>
            {
                if (o is GameObject obj && obj.TryGetComponent<BreedingPod>(out _))
                {
                    Log?.LogInfo($"Removing object {obj}.");
                    return true;
                }
                return false;
            });

            // Update all dwellings to have a ProcreationHouse
            var dwellings = objects.OfType<GameObject>()
                .Where(obj => obj.TryGetComponent<Dwelling>(out _))
                .Where(obj => !obj.TryGetComponent<ProcreationHouse>(out _));
            foreach (var obj in dwellings)
            {
                Log?.LogInfo($"Updating object {obj}.");
                obj.AddComponent<ProcreationHouse>();
            }

            _factionObjectCache = objects;
            return _factionObjectCache;
        }
    }
}