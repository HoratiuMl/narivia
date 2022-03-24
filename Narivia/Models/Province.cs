using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using NuciXNA.Primitives;

using Narivia.Models.Enumerations;

namespace Narivia.Models
{
    /// <summary>
    /// Province domain model.
    /// </summary>
    public sealed class Province : ModelBase
    {
        /// <summary>
        /// Gets or sets the resource identifier.
        /// </summary>
        /// <value>The resource identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string ResourceId { get; set; }

        /// <summary>
        /// Gets the faction identifier.
        /// </summary>
        /// <value>The faction identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string FactionId { get; set; }

        /// <summary>
        /// Gets the sovereign faction identifier.
        /// </summary>
        /// <value>The sovereign faction identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string SovereignFactionId { get; set; }

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>The colour.</value>
        [XmlIgnore]
        public Colour Colour { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Province"/> is locked.
        /// A province is normally locked as soon as it is conquered, and is unlocked when the turn has passed.
        /// This is done in order to prevent the same province from being conquered multiple times in one turn.
        /// </summary>
        /// <value><c>true</c> if locked; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlIgnore]
        public ProvinceType Type { get; set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        [XmlIgnore]
        public ProvinceState State
        {
            get
            {
                if (FactionId == SovereignFactionId)
                {
                    return ProvinceState.Sovereign;
                }

                return ProvinceState.Occupied;
            }
        }
    }
}
