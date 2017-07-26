using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;

namespace Narivia.Gui.GuiElements
{
    // TODO: Requires more refactoring and cleaning
    /// <summary>
    /// Region bar GUI element.
    /// </summary>
    public class GuiRegionBar : GuiElement
    {
        const int HOLDING_SPACING_HORIZONTAL = 5;

        /// <summary>
        /// Gets the region identifier.
        /// </summary>
        /// <value>The region identifier.</value>
        [XmlIgnore]
        public string RegionId { get; private set; }

        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Colour TextColour { get; set; }

        IGameManager game;

        GuiImage background;

        GuiText regionNameText;
        GuiFactionFlag factionImage;

        GuiImage resourceImage;
        GuiText resourceText;

        List<GuiImage> holdingImages;
        List<GuiText> holdingTexts;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            holdingImages = new List<GuiImage>();
            holdingTexts = new List<GuiText>();

            background = new GuiImage
            {
                ContentFile = "Interface/backgrounds",
                SourceRectangle = new Rectangle(32, 0, 32, 32),
                FillMode = TextureFillMode.Tile
            };

            regionNameText = new GuiText
            {
                Size = new Vector2(240, 32),
                FontName = "SideBarFont" // TODO: Consider providing a dedicated font
            };
            factionImage = new GuiFactionFlag();

            resourceImage = new GuiImage
            {
                SourceRectangle = new Rectangle(0, 0, 64, 64)
            };
            resourceText = new GuiText
            {
                FontName = "RegionBarHoldingFont",
                TextColour = Colour.Black,
                HorizontalAlignment = HorizontalAlignment.Top
            };
            
            RegionId = game.GetFactionRegions(game.PlayerFactionId).First().Id;

            Children.Add(background);

            Children.Add(regionNameText);
            Children.Add(factionImage);

            Children.Add(resourceImage);
            Children.Add(resourceText);

            base.LoadContent();
        }

        /// <summary>
        /// Sets the region.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        public void SetRegion(string regionId)
        {
            if (string.IsNullOrWhiteSpace(regionId) ||
                RegionId == regionId)
            {
                return;
            }

            RegionId = regionId;

            holdingTexts.ForEach(w => w.Destroy());
            holdingImages.ForEach(w => w.Destroy());
            holdingTexts.Clear();
            holdingImages.Clear();
            
            List<Holding> holdings = game.GetRegionHoldings(RegionId).ToList();

            holdingImages = new List<GuiImage>();

            foreach (Holding holding in holdings)
            {
                GuiImage holdingImage = new GuiImage
                {
                    ContentFile = $"World/Assets/{game.WorldId}/holdings/generic",
                    SourceRectangle = new Rectangle(64 * ((int)holding.Type - 1), 0, 64, 64),
                    Position = new Vector2(Position.X + HOLDING_SPACING_HORIZONTAL * (holdingImages.Count + 2) + 64 * (holdingImages.Count + 1),
                                           Position.Y + Size.Y - 64)
                };

                GuiText holdingText = new GuiText
                {
                    Position = new Vector2(holdingImage.Position.X - HOLDING_SPACING_HORIZONTAL, Position.Y + 2),
                    Text = holding.Name,
                    Size = new Vector2(holdingImage.SourceRectangle.Width + HOLDING_SPACING_HORIZONTAL * 2,
                                             Size.Y - holdingImage.SourceRectangle.Height + 10),
                    FontName = "RegionBarHoldingFont",
                    TextColour = Colour.Black,
                    HorizontalAlignment = HorizontalAlignment.Top
                };

                holdingTexts.Add(holdingText);
                holdingText.LoadContent();
                Children.Add(holdingText);

                holdingImages.Add(holdingImage);
                holdingImage.LoadContent();
                Children.Add(holdingImage);
            }
        }

        // TODO: Handle this better
        /// <summary>
        /// Associates the game manager.
        /// </summary>
        /// <param name="game">Game.</param>
        public void AssociateGameManager(ref IGameManager game)
        {
            this.game = game;
        }

        protected override void SetChildrenProperties()
        {
            background.Position = Position;
            background.Scale = Size / background.SourceRectangle.Width;

            regionNameText.TextColour = TextColour;
            regionNameText.Position = new Vector2(Position.X + (Size.X - regionNameText.ScreenArea.Width) / 2,
                                                  Position.Y - regionNameText.ScreenArea.Height);

            if (!string.IsNullOrWhiteSpace(RegionId))
            {
                string resourceId = game.GetRegionResource(RegionId);
                string factionId = game.GetRegionFaction(RegionId);

                Colour factionColour = game.GetFactionColour(game.GetRegionFaction(RegionId));
                Flag factionFlag = game.GetFactionFlag(factionId);

                regionNameText.Text = game.GetRegionName(RegionId);
                regionNameText.BackgroundColour = new Colour(factionColour.R, factionColour.G, factionColour.B);
                
                factionImage.Position = new Vector2(regionNameText.Position.X - 64, regionNameText.Position.Y - 48);
                factionImage.Size = new Vector2(regionNameText.Size.Y, regionNameText.Size.Y);
                factionImage.Background = factionFlag.Background;
                factionImage.Emblem = factionFlag.Emblem;
                factionImage.Skin = factionFlag.Skin;
                factionImage.BackgroundPrimaryColour = factionFlag.BackgroundPrimaryColour;
                factionImage.BackgroundSecondaryColour = factionFlag.BackgroundSecondaryColour;
                factionImage.EmblemColour = factionFlag.EmblemColour;

                resourceImage.Position = new Vector2(Position.X + HOLDING_SPACING_HORIZONTAL, Position.Y + Size.Y - 64);
                resourceImage.ContentFile = $"World/Assets/{game.WorldId}/resources/{resourceId}_big";

                resourceText.Position = new Vector2(Position.X, Position.Y + 2);
                resourceText.Size = new Vector2(64 + HOLDING_SPACING_HORIZONTAL * 2, Size.Y - 74);
                resourceText.Text = game.GetResourceName(game.GetRegionResource(RegionId));
            }
        }
    }
}
