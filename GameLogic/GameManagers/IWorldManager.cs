using System.Collections.Generic;

using NuciXNA.Primitives;

using Narivia.Models;

namespace Narivia.GameLogic.GameManagers
{
    public interface IWorldManager
    {
        int HoldingSlotsPerFaction { get; }

        void LoadContent();

        void UnloadContent();

        /// <summary>
        /// Checks wether the specified provinces share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified provinces share a border, <c>false</c> otherwise.</returns>
        /// <param name="province1Id">First province identifier.</param>
        /// <param name="province2Id">Second province identifier.</param>
        bool DoesProvinceBorderProvince(string province1Id, string province2Id);

        /// <summary>
        /// Checks wether the specified provinces share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified provinces share a border, <c>false</c> otherwise.</returns>
        /// <param name="faction1Id">First faction identifier.</param>
        /// <param name="faction2Id">Second faction identifier.</param>
        bool DoesFactionBorderFaction(string faction1Id, string faction2Id);

        /// <summary>
        /// Checks wether the specified faction shares a border with the specified province.
        /// </summary>
        /// <returns><c>true</c>, if the faction share a border with that province, <c>false</c> otherwise.</returns>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="provinceId">Province identifier.</param>
        bool DoesFactionBorderProvince(string factionId, string provinceId);

        /// <summary>
        /// Transfers the specified province to the specified faction.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        void TransferProvinceToFaction(string provinceId, string factionId);

        /// <summary>
        /// Gets the borders.
        /// </summary>
        /// <returns>The borders.</returns>
        IEnumerable<Border> GetBorders();

        Culture GetCulture(string cultureId);

        /// <summary>
        /// Gets the cultures.
        /// </summary>
        /// <returns>The cultures.</returns>
        IEnumerable<Culture> GetCultures();

        /// <summary>
        /// Gets the factions.
        /// </summary>
        /// <returns>The factions.</returns>
        IEnumerable<Faction> GetFactions();

        Faction GetFaction(string factionId);

        /// <summary>
        /// Returns the faction at the given location.
        /// </summary>
        /// <returns>The faction.</returns>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        Faction GetFaction(int x, int y);

        /// <summary>
        /// Gets the faction capital.
        /// </summary>
        /// <returns>Province.</returns>
        /// <param name="factionId">Faction identifier.</param>
        Province GetFactionCapital(string factionId);

        Point2D GetFactionCentre(string factionId);

        /// <summary>
        /// Gets the provinces of a faction.
        /// </summary>
        /// <returns>The provinces.</returns>
        /// <param name="factionId">Faction identifier.</param>
        IEnumerable<Province> GetFactionProvinces(string factionId);

        /// <summary>
        /// Gets the flag.
        /// </summary>
        /// <returns>The flag.</returns>
        /// <param name="flagId">Flag identifier.</param>
        Flag GetFlag(string flagId);

        /// <summary>
        /// Gets the flags.
        /// </summary>
        /// <returns>The flags.</returns>
        IEnumerable<Flag> GetFlags();

        Province GetProvince(string provinceId);

        Province GetProvince(int x, int y);

        /// <summary>
        /// Gets the provinces.
        /// </summary>
        /// <returns>The provinces.</returns>
        IEnumerable<Province> GetProvinces();

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="resourceId">Resource identifier.</param>
        Resource GetResource(string resourceId);

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <returns>The resources.</returns>
        IEnumerable<Resource> GetResources();

        /// <summary>
        /// Gets the terrain.
        /// </summary>
        /// <returns>The terrain.</returns>
        /// <param name="terrainId">Terrain identifier.</param>
        Terrain GetTerrain(string terrainId);

        /// <summary>
        /// Gets the terrains.
        /// </summary>
        /// <returns>The terrains.</returns>
        IEnumerable<Terrain> GetTerrains();

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <returns>The world.</returns>
        World GetWorld();

        void InitialiseProvince(string provinceId);

        void InitialiseFaction(string factionId);
    }
}
