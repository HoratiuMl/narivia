using System.Xml.Serialization;

using NuciXNA.DataAccess.DataObjects;

namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// World data entity.
    /// </summary>
    public class WorldEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>The author.</value>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the resource pack.
        /// </summary>
        /// <value>The resource pack.</value>
        public string ResourcePack { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the base province income.
        /// </summary>
        /// <value>The base province income.</value>
        public int BaseProvinceIncome { get; set; }

        /// <summary>
        /// Gets or sets the base province recruitment.
        /// </summary>
        /// <value>The base province recruitment.</value>
        public int BaseProvinceRecruitment { get; set; }

        /// <summary>
        /// Gets or sets the base faction recruitment.
        /// </summary>
        /// <value>The base faction recruitment.</value>
        public int BaseFactionRecruitment { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of troops required to attack.
        /// </summary>
        /// <value>The minimum number of troops to attack.</value>
        public int MinTroopsPerAttack { get; set; }

        /// <summary>
        /// Gets or sets the number of holding slots per faction.
        /// </summary>
        /// <value>The number of holding slots per faction.</value>
        public int HoldingSlotsPerFaction { get; set; }

        /// <summary>
        /// Gets or sets the starting wealth.
        /// </summary>
        /// <value>The starting wealth.</value>
        public int StartingWealth { get; set; }

        /// <summary>
        /// Gets or sets the starting troops.
        /// </summary>
        /// <value>The starting troops.</value>
        public int StartingTroops { get; set; }

        /// <summary>
        /// Gets or sets the price of holdings;
        /// </summary>
        /// <value>The holdings price.</value>
        public int HoldingsPrice { get; set; }

        public string AssetsPack { get; set; }

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        /// <value>The tiles.</value>
        [XmlIgnore]
        public WorldTileEntity[,] Tiles { get; set; }
    }
}
