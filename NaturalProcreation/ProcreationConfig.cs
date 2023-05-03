using System.Linq;
using System.Collections.Generic;
using TimberApi.ConfigSystem;
using Timberborn.GameFactionSystem;
using Timberborn.Reproduction;
using HarmonyLib;

namespace NaturalProcreation
{
    public class ProcreationConfig : IConfig
    {
        public string ConfigFileName => "ProcreationConfig";

        public Dictionary<string, float> FactionBirthRate { get; set; }

        public ProcreationConfig() {
            var defaultFactions = new[]
            {
                "Folktails",
                "IronTeeth",
            };

            // read the default birth rate from game
            var defaultBirthRate = Traverse.Create<ProcreationHouse>()
                 .Field(nameof(ProcreationHouse.DailySpawningChance))
                 .GetValue<float>();

           FactionBirthRate = defaultFactions
               .ToDictionary(id => id, _ => defaultBirthRate);
        }
    }
}
