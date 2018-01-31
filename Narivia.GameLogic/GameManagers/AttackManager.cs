using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Narivia.GameLogic.Enumerations;
using Narivia.GameLogic.Exceptions;
using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Common.Extensions;
using Narivia.Models;
using Narivia.Models.Enumerations;
using Narivia.Settings;

namespace Narivia.GameLogic.GameManagers
{
    /// <summary>
    /// Attack manager.
    /// </summary>
    public class AttackManager : IAttackManager
    {
        Random random;

        const int BLITZKRIEG_SOVEREIGNTY_IMPORTANCE = 30;
        const int BLITZKRIEG_HOLDING_CASTLE_IMPORTANCE = 30;
        const int BLITZKRIEG_HOLDING_CITY_IMPORTANCE = 20;
        const int BLITZKRIEG_HOLDING_TEMPLE_IMPORTANCE = 10;
        const int BLITZKRIEG_BORDER_IMPORTANCE = 15;
        const int BLITZKRIEG_RESOURCE_ECONOMY_IMPORTANCE = 5;
        const int BLITZKRIEG_RESOURCE_MILITARY_IMPORTANCE = 10;

        readonly IHoldingManager holdingManager;
        readonly IWorldManager worldManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttackManager"/> class.
        /// </summary>
        /// <param name="worldManager">World manager.</param>
        /// <param name="holdingManager">Holding manager.</param>
        public AttackManager(
            IHoldingManager holdingManager,
            IWorldManager worldManager)
        {
            this.holdingManager = holdingManager;
            this.worldManager = worldManager;

            random = new Random();
        }

        /// <summary>
        /// Chooses the province to attack.
        /// </summary>
        /// <returns>The province to attack.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public string ChooseProvinceToAttack(string factionId)
        {
            List<string> provincesOwnedIds = worldManager.GetFactionProvinces(factionId)
                                                .Select(x => x.Id)
                                                .ToList();

            // TODO: Do not target factions with good relations
            Dictionary<string, int> targets = worldManager.GetProvinces()
                                                   .Where(r => r.FactionId != factionId &&
                                                               r.FactionId != GameDefines.GAIA_FACTION &&
                                                               r.Locked == false)
                                                   .Select(x => x.Id)
                                                   .Except(provincesOwnedIds)
                                                   .Where(x => provincesOwnedIds.Any(y => worldManager.ProvinceBordersProvince(x, y)))
                                                   .ToDictionary(x => x, y => 0);

            Parallel.ForEach(worldManager.GetProvinces().Where(r => targets.ContainsKey(r.Id)).ToList(), (province) =>
            {
                if (province.SovereignFactionId == factionId)
                {
                    targets[province.Id] += BLITZKRIEG_SOVEREIGNTY_IMPORTANCE;
                }


                Parallel.ForEach(holdingManager.GetProvinceHoldings(province.Id), (holding) =>
                {
                    switch (holding.Type)
                    {
                        case HoldingType.Castle:
                            targets[province.Id] += BLITZKRIEG_HOLDING_CASTLE_IMPORTANCE;
                            break;

                        case HoldingType.City:
                            targets[province.Id] += BLITZKRIEG_HOLDING_CITY_IMPORTANCE;
                            break;

                        case HoldingType.Temple:
                            targets[province.Id] += BLITZKRIEG_HOLDING_TEMPLE_IMPORTANCE;
                            break;
                    }
                });

                Resource provinceResource = worldManager.GetResources().FirstOrDefault(x => x.Id == province.ResourceId);

                if (provinceResource != null)
                {
                    switch (provinceResource.Type)
                    {
                        case ResourceType.Military:
                            targets[province.Id] += BLITZKRIEG_RESOURCE_MILITARY_IMPORTANCE;
                            break;

                        case ResourceType.Economy:
                            targets[province.Id] += BLITZKRIEG_RESOURCE_ECONOMY_IMPORTANCE;
                            break;
                    }
                }

                targets[province.Id] += provincesOwnedIds.Count(x => worldManager.ProvinceBordersProvince(x, province.Id)) * BLITZKRIEG_BORDER_IMPORTANCE;
                targets[province.Id] -= worldManager.GetFactionRelations(factionId)
                                           .FirstOrDefault(r => r.TargetFactionId == province.FactionId)
                                           .Value;

                // TODO: Maybe add a random importance to each province in order to reduce predictibility a little
            });

            if (targets.Count == 0)
            {
                return null;
            }

            int maxScore = targets.Max(x => x.Value);
            List<string> topTargets = targets.Keys.Where(x => targets[x] == maxScore).ToList();
            string provinceId = topTargets[random.Next(0, topTargets.Count())];

            return provinceId;
        }

        /// <summary>
        /// Attacks the province.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="provinceId">Province identifier.</param>
        public BattleResult AttackProvince(string factionId, string provinceId)
        {
            Province targetProvince = worldManager.GetProvinces().FirstOrDefault(r => r.Id == provinceId);

            if (string.IsNullOrWhiteSpace(provinceId) ||
                targetProvince.Locked ||
                !worldManager.FactionBordersProvince(factionId, provinceId))
            {
                throw new InvalidTargetProvinceException(provinceId);
            }

            Faction attackerFaction = worldManager.GetFactions().FirstOrDefault(f => f.Id == factionId);
            Faction defenderFaction = worldManager.GetFactions().FirstOrDefault(f => f.Id == targetProvince.FactionId);

            if (defenderFaction.Id == attackerFaction.Id ||
                defenderFaction.Id == GameDefines.GAIA_FACTION)
            {
                throw new InvalidTargetProvinceException(provinceId);
            }

            while (worldManager.GetFactionTroopsAmount(attackerFaction.Id) > 0 &&
                   worldManager.GetFactionTroopsAmount(defenderFaction.Id) > 0)
            {
                Army attackerArmy = worldManager.GetFactionArmies(attackerFaction.Id)
                                         .Where(a => a.Size > 0)
                                         .GetRandomElement();
                Army defenderArmy = worldManager.GetFactionArmies(defenderFaction.Id)
                                         .Where(a => a.Size > 0)
                                         .GetRandomElement();

                Unit attackerUnit = worldManager.GetUnits().FirstOrDefault(u => u.Id == attackerArmy.UnitId);
                Unit defenderUnit = worldManager.GetUnits().FirstOrDefault(u => u.Id == defenderArmy.UnitId);

                // TODO: Attack and Defence bonuses

                int attackerTroopsLeft =
                    (attackerUnit.Health * attackerArmy.Size - defenderUnit.Power * defenderArmy.Size) /
                    attackerUnit.Health;

                int defenderTroopsLeft =
                    (defenderUnit.Health * defenderArmy.Size - attackerUnit.Power * attackerArmy.Size) /
                    defenderUnit.Health;

                attackerArmy.Size = Math.Max(0, attackerTroopsLeft);
                defenderArmy.Size = Math.Max(0, defenderTroopsLeft);
            }

            // TODO: In the GameDomainService I should change the realations based on wether the
            // province was sovereign or not

            if (worldManager.GetFactionTroopsAmount(attackerFaction.Id) >
                worldManager.GetFactionTroopsAmount(defenderFaction.Id))
            {
                worldManager.TransferProvince(provinceId, factionId);
                targetProvince.Locked = true;

                return BattleResult.Victory;
            }

            return BattleResult.Defeat;
        }
    }
}
