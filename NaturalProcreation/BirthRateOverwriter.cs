using HarmonyLib;
using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.ConsoleSystem;
using TimberApi.SceneSystem;
using Timberborn.GameFactionSystem;
using Timberborn.Reproduction;
using Timberborn.SingletonSystem;

namespace NaturalProcreation
{
    internal class BirthRateOverwriter : ILoadableSingleton
    {
        private readonly IConsoleWriter? _log;
        private readonly FactionService _factionService;

        public BirthRateOverwriter(FactionService factionService, IConsoleWriter consoleWriter)
        {
            _log = consoleWriter;
            _factionService = factionService;
        }

        // save original game value during static initialization
        private static readonly float _defaultBirthRate = Traverse.Create<ProcreationHouse>()
            .Field(nameof(ProcreationHouse.DailySpawningChance))
            .GetValue<float>();

        public void Load()
        {
            var currentFactionId = _factionService.Current.Id;
            var birthRateConfig = NaturalProcreationPlugin.Config?.FactionBirthRate;

            float birthRate;
            if (birthRateConfig != null && birthRateConfig.TryGetValue(currentFactionId, out birthRate))
            {
                _log?.LogInfo($"Overwrite DailySpawningChance for faction {currentFactionId}");
            }
            else
            {
                _log?.LogInfo($"Restore default DailySpawningChance");
                birthRate = _defaultBirthRate;
            }

            _log?.LogInfo($"SetValue {nameof(ProcreationHouse.DailySpawningChance)} = {birthRate}");
            Traverse.Create<ProcreationHouse>()
                .Field(nameof(ProcreationHouse.DailySpawningChance))
                .SetValue(birthRate);
        }
    }

    [Configurator(SceneEntrypoint.InGame)]
    public class BirthRateConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<BirthRateOverwriter>().AsSingleton();
        }
    }
}
