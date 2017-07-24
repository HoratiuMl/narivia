﻿using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;

namespace Narivia.GameLogic.Mapping
{
    /// <summary>
    /// Flag mapping extensions for converting between entities and domain models.
    /// </summary>
    static class FlagMappingExtensions
    {
        /// <summary>
        /// Converts the entity into a domain model.
        /// </summary>
        /// <returns>The domain model.</returns>
        /// <param name="flagEntity">Flag entity.</param>
        internal static Flag ToDomainModel(this FlagEntity flagEntity)
        {
            Flag flag = new Flag
            {
                Id = flagEntity.Id,
                Background = flagEntity.Background,
                Emblem = flagEntity.Emblem,
                Skin = flagEntity.Skin,
                PrimaryColour = ColourTranslator.FromHexadecimal(flagEntity.PrimaryColourHexadecimal),
                SecondaryColour = ColourTranslator.FromHexadecimal(flagEntity.SecondaryColourHexadecimal)
            };

            return flag;
        }

        /// <summary>
        /// Converts the domail model into an entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="flag">Flag.</param>
        internal static FlagEntity ToEntity(this Flag flag)
        {
            FlagEntity flagEntity = new FlagEntity
            {
                Id = flag.Id,
                Background = flag.Background,
                Emblem = flag.Emblem,
                Skin = flag.Skin,
                PrimaryColourHexadecimal = ColourTranslator.ToHexadecimal(flag.PrimaryColour),
                SecondaryColourHexadecimal = ColourTranslator.ToHexadecimal(flag.SecondaryColour)
            };

            return flagEntity;
        }

        /// <summary>
        /// Converts the entities into domain models.
        /// </summary>
        /// <returns>The domain models.</returns>
        /// <param name="flagEntities">Flag entities.</param>
        internal static IEnumerable<Flag> ToDomainModels(this IEnumerable<FlagEntity> flagEntities)
        {
            IEnumerable<Flag> flags = flagEntities.Select(flagEntity => flagEntity.ToDomainModel());

            return flags;
        }

        /// <summary>
        /// Converts the domain models into entities.
        /// </summary>
        /// <returns>The entities.</returns>
        /// <param name="flags">Flags.</param>
        internal static IEnumerable<FlagEntity> ToEntities(this IEnumerable<Flag> flags)
        {
            IEnumerable<FlagEntity> flagEntities = flags.Select(flag => flag.ToEntity());

            return flagEntities;
        }
    }
}