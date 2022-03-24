using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers;
using Narivia.Models;
using Narivia.Models.Enumerations;
using Narivia.Settings;

namespace Narivia.Gui.Controls
{
    /// <summary>
    /// Unit recruitment panel GUI element.
    /// </summary>
    public class GuiBuildingPanel : GuiPanel
    {
        const int IconSize = 22;

        IGameManager gameManager;
        IWorldManager worldManager;
        IHoldingManager holdingManager;

        GuiImage holdingBackground;
        GuiImage holdingImage;
        GuiImage paper;

        GuiText holdingText;
        GuiText provinceText;

        GuiImage priceIcon;
        GuiText priceText;

        GuiButton previousHoldingButton;
        GuiButton nextHoldingButton;
        GuiButton previouseProvinceButton;
        GuiButton nextProvinceButton;

        GuiButton buildButton;

        List<HoldingType> holdingTypes;
        List<Province> provinces;

        int currentHoldingTypeIndex;
        int currentProvinceIndex;

        public GuiBuildingPanel(
            IGameManager gameManager,
            IWorldManager worldManager,
            IHoldingManager holdingManager)
        {
            this.gameManager = gameManager;
            this.worldManager = worldManager;
            this.holdingManager = holdingManager;

            Title = "Building";
            FontName = "ButtonFont";
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            holdingTypes = HoldingType.GetValues().Cast<HoldingType>().Where(x => x != HoldingType.Empty).ToList();

            holdingBackground = new GuiImage
            {
                Id = $"{Id}_{nameof(holdingBackground)}",
                ContentFile = "ScreenManager/FillImage",
                TextureLayout = TextureLayout.Tile,
                TintColour = Colour.Black,
                Size = new Size2D(100, 100),
                Location = new Point2D((Size.Width - 100) / 2, 64)
            };
            holdingImage = new GuiImage
            {
                Id = $"{Id}_{nameof(holdingImage)}",
                ContentFile = $"Icons/holdings/generic",
                SourceRectangle = new Rectangle2D(0, 0, 64, 64),
                Size = new Size2D(64, 64),
                Location = new Point2D(
                    holdingBackground.Location.X + (holdingBackground.Size.Width - 64) / 2,
                    holdingBackground.Location.Y + (holdingBackground.Size.Height - 64) / 2)
            };
            paper = new GuiImage
            {
                Id = $"{Id}_{nameof(paper)}",
                ContentFile = "Interface/ProvincePanel/paper",
                Size = new Size2D(248, 80),
                Location = new Point2D(
                    (Size.Width - 248) / 2,
                    holdingBackground.ClientRectangle.Bottom + GameDefines.GuiSpacing)
            };

            holdingText = new GuiText
            {
                Id = $"{Id}_{nameof(holdingText)}",
                ForegroundColour = Colour.Gold,
                Size = new Size2D(holdingBackground.Size.Width, 18),
                Location = holdingBackground.Location
            };
            provinceText = new GuiText
            {
                Id = $"{Id}_{nameof(provinceText)}",
                ForegroundColour = Colour.Gold,
                Size = new Size2D(holdingBackground.Size.Height, 18),
                Location = new Point2D(
                    holdingBackground.Location.X,
                    holdingBackground.Location.Y + holdingBackground.Size.Height - 18)
            };

            priceIcon = new GuiImage
            {
                Id = $"{Id}_{nameof(priceIcon)}",
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(IconSize * 3, 0, IconSize, IconSize),
                Size = new Size2D(IconSize, IconSize),
                Location = new Point2D(
                    paper.Location.X + (paper.Size.Width - IconSize * 3 - GameDefines.GuiSpacing) / 2,
                    paper.Location.Y + (paper.Size.Height - IconSize) / 2)
            };
            priceText = new GuiText
            {
                Id = $"{Id}_{nameof(priceText)}",
                HorizontalAlignment = Alignment.Beginning,
                Size = new Size2D(priceIcon.Size.Width * 2, priceIcon.Size.Height),
                Location = new Point2D(
                    priceIcon.Location.X + priceIcon.Size.Width + GameDefines.GuiSpacing,
                    priceIcon.Location.Y)
            };

            previousHoldingButton = new GuiButton
            {
                Id = $"{Id}_{nameof(previousHoldingButton)}",
                ContentFile = "Interface/Buttons/button-minus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    holdingBackground.Location.X - GameDefines.GuiSpacing - 24,
                    holdingBackground.Location.Y)
            };
            nextHoldingButton = new GuiButton
            {
                Id = $"{Id}_{nameof(nextHoldingButton)}",
                ContentFile = "Interface/Buttons/button-plus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    holdingBackground.Location.X + holdingBackground.Size.Width + GameDefines.GuiSpacing,
                    holdingBackground.Location.Y)
            };
            previouseProvinceButton = new GuiButton
            {
                Id = $"{Id}_{nameof(previouseProvinceButton)}",
                ContentFile = "Interface/Buttons/button-minus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    holdingBackground.Location.X - GameDefines.GuiSpacing - 24,
                    holdingBackground.Location.Y + holdingBackground.Size.Height - 24)
            };
            nextProvinceButton = new GuiButton
            {
                Id = $"{Id}_{nameof(nextProvinceButton)}",
                ContentFile = "Interface/Buttons/button-plus",
                Size = new Size2D(24, 24),
                Location = new Point2D(
                    holdingBackground.Location.X + holdingBackground.Size.Width + GameDefines.GuiSpacing,
                    holdingBackground.Location.Y + holdingBackground.Size.Height - 24)
            };

            buildButton = new GuiButton
            {
                Id = $"{Id}_{nameof(buildButton)}",
                ContentFile = "Interface/Buttons/green-button-large",
                ForegroundColour = Colour.White,
                Text = "Build",
                Size = new Size2D(128, 26),
                Location = new Point2D(
                    (Size.Width - 128) / 2,
                    Size.Height - 42 - GameDefines.GuiSpacing)
            };

            UpdateProvinceList();
            SelectHolding(0);
            SelectProvince(0);

            RegisterChildren(holdingBackground, holdingImage, paper);
            RegisterChildren(holdingText, provinceText);
            RegisterChildren(priceIcon, priceText);
            RegisterChildren(nextHoldingButton, previousHoldingButton, nextProvinceButton, previouseProvinceButton, buildButton);
            
            RegisterEvents();
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            base.DoUnloadContent();

            UnregisterEvents();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            base.DoUpdate(gameTime);

            SetChildrenProperties();
            UpdateProvinceList();
        }

        void RegisterEvents()
        {
            buildButton.Clicked += OnBuildButtonClicked;
            nextHoldingButton.Clicked += OnNextHoldingButtonClicked;
            nextProvinceButton.Clicked += OnNextProvinceButtonClicked;
            previousHoldingButton.Clicked += OnPreviousHoldingButtonClicked;
            previouseProvinceButton.Clicked += OnPreviousProvinceButtonClicked;
        }

        void UnregisterEvents()
        {
            buildButton.Clicked -= OnBuildButtonClicked;
            nextHoldingButton.Clicked -= OnNextHoldingButtonClicked;
            nextProvinceButton.Clicked -= OnNextProvinceButtonClicked;
            previousHoldingButton.Clicked -= OnPreviousHoldingButtonClicked;
            previouseProvinceButton.Clicked -= OnPreviousProvinceButtonClicked;
        }

        void UpdateProvinceList()
        {
            provinces = worldManager
                .GetFactionProvinces(gameManager.PlayerFactionId)
                .Where(r => holdingManager.DoesProvinceHaveEmptyHoldings(r.Id))
                .ToList();

            SelectProvince(currentProvinceIndex);
        }

        void SetChildrenProperties()
        {
            holdingImage.SourceRectangle = new Rectangle2D(64 * currentHoldingTypeIndex, 0, 64, 64);

            holdingText.Text = holdingTypes[currentHoldingTypeIndex].Name;
            priceText.Text = gameManager.GetWorld().HoldingsPrice.ToString();
        }

        void SelectHolding(int index)
        {
            if (index > holdingTypes.Count - 1)
            {
                currentHoldingTypeIndex = 0;
            }
            else if (index < 0)
            {
                currentHoldingTypeIndex = holdingTypes.Count - 1;
            }
            else
            {
                currentHoldingTypeIndex = index;
            }
        }

        void SelectProvince(int index)
        {
            if (provinces.Count == 0)
            {
                // TODO: Handle this properly

                return;
            }

            if (index > provinces.Count - 1)
            {
                currentProvinceIndex = 0;
            }
            else if (index < 0)
            {
                currentProvinceIndex = provinces.Count - 1;
            }
            else
            {
                currentProvinceIndex = index;
            }

            provinceText.Text = provinces[currentProvinceIndex].Name;
        }

        void OnBuildButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (provinces.Count == 0)
            {
                // TODO: Handle this properly

                return;
            }

            if (worldManager.GetFaction(gameManager.PlayerFactionId).Wealth >= gameManager.GetWorld().HoldingsPrice)
            {
                holdingManager.BuildHolding(provinces[currentProvinceIndex].Id, holdingTypes[currentHoldingTypeIndex]);
            }
        }

        void OnCancelButtonClicked(object sender, MouseButtonEventArgs e)
        {
            Hide();
            SelectHolding(0);

            currentProvinceIndex = 0;
        }

        void OnPreviousHoldingButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectHolding(currentHoldingTypeIndex - 1);
        }

        void OnPreviousProvinceButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectProvince(currentProvinceIndex - 1);
        }

        void OnNextHoldingButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectHolding(currentHoldingTypeIndex + 1);
        }

        void OnNextProvinceButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SelectProvince(currentProvinceIndex + 1);
        }
    }
}
