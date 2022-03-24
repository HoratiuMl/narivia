using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers;
using Narivia.Gui.Helpers;

namespace Narivia.Gui.Controls
{
    /// <summary>
    /// Info bar GUI element.
    /// </summary>
    public class GuiInfoBar : GuiControl, IGuiControl
    {
        IGameManager gameManager;
        IWorldManager worldManager;
        IHoldingManager holdingManager;
        IMilitaryManager militaryManager;

        GuiImage background;

        GuiImage provincesIcon;
        GuiImage holdingsIcon;
        GuiImage troopsIcon;
        GuiImage wealthIcon;

        GuiText provincesText;
        GuiText holdingsText;
        GuiText troopsText;
        GuiText wealthText;

        GuiTooltip provincesTooltip;
        GuiTooltip holdingsTooltip;
        GuiTooltip troopsTooltip;
        GuiTooltip wealthTooltip;

        GuiButton turnButton;
        GuiText turnText;

        /// <summary>
        /// Occurs when clicked.
        /// </summary>
        public event MouseButtonEventHandler TurnButtonClicked;

        public GuiInfoBar(
            IGameManager gameManager,
            IWorldManager worldManager,
            IHoldingManager holdingManager,
            IMilitaryManager militaryManager)
        {
            ForegroundColour = Colour.Gold;
            FontName = "InfoBarFont";

            this.gameManager = gameManager;
            this.worldManager = worldManager;
            this.holdingManager = holdingManager;
            this.militaryManager = militaryManager;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            background = new GuiImage
            {
                ContentFile = "Interface/InfoBar/bar"
            };

            provincesIcon = new GuiImage
            {
                ContentFile = "Interface/InfoBar/icons",
                SourceRectangle = new Rectangle2D(0, 0, 26, 26),
                Location = new Point2D(16, 10),
                Size = new Size2D(26, 26)
            };
            holdingsIcon = new GuiImage
            {
                ContentFile = "Interface/InfoBar/icons",
                SourceRectangle = new Rectangle2D(26, 0, 26, 26),
                Location = new Point2D(52, 10),
                Size = new Size2D(26, 26)
            };
            troopsIcon = new GuiImage
            {
                ContentFile = "Interface/InfoBar/icons",
                SourceRectangle = new Rectangle2D(52, 0, 26, 26),
                Location = new Point2D(89, 10),
                Size = new Size2D(26, 26)
            };
            wealthIcon = new GuiImage
            {
                ContentFile = "Interface/InfoBar/icons",
                SourceRectangle = new Rectangle2D(78, 0, 26, 26),
                Location = new Point2D(124, 10),
                Size = new Size2D(26, 26)
            };

            provincesText = new GuiText
            {
                Location = new Point2D(13, 36),
                Size = new Size2D(32, 15)
            };
            holdingsText = new GuiText
            {
                Location = new Point2D(49, 36),
                Size = new Size2D(32, 15)
            };
            troopsText = new GuiText
            {
                Location = new Point2D(85, 36),
                Size = new Size2D(32, 15)
            };
            wealthText = new GuiText
            {
                Location = new Point2D(121, 36),
                Size = new Size2D(32, 15)
            };

            provincesTooltip = new GuiTooltip
            {
                FontName = "DefaultFont",
                Text = "Provinces",
                Size = new Size2D(100, 20),
                BackgroundColour = Colour.Black
            };
            holdingsTooltip = new GuiTooltip
            {
                FontName = "DefaultFont",
                Text = "Holdings",
                Size = new Size2D(100, 20),
                BackgroundColour = Colour.Black
            };
            troopsTooltip = new GuiTooltip
            {
                FontName = "DefaultFont",
                Text = "Troops",
                Size = new Size2D(128, 128),
                BackgroundColour = Colour.Black
            };
            wealthTooltip = new GuiTooltip
            {
                FontName = "DefaultFont",
                Text = "Wealth",
                Size = new Size2D(100, 20),
                BackgroundColour = Colour.Black
            };

            turnButton = new GuiButton
            {
                ContentFile = "Interface/Buttons/button-plus",
                Location = new Point2D(142, 59),
                Size = new Size2D(24, 24)
            };
            turnText = new GuiText
            {
                Location = new Point2D(15, 62),
                Size = new Size2D(138, 15)
            };

            RegisterChild(background);
            RegisterChildren(provincesIcon, holdingsIcon, troopsIcon, wealthIcon);
            RegisterChildren(provincesText, holdingsText, troopsText, wealthText);
            RegisterChildren(turnButton, turnText);
            RegisterChildren(provincesTooltip, holdingsTooltip, wealthTooltip);

            RegisterEvents();
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            UnregisterEvents();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            SetChildrenProperties();
        }

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

        void RegisterEvents()
        {
            provincesIcon.MouseEntered += OnProvincesMouseEntered;
            provincesIcon.MouseLeft += OnProvincesMouseLeft;

            holdingsIcon.MouseEntered += OnHoldingsMouseEntered;
            holdingsIcon.MouseLeft += OnHoldingsMouseLeft;

            troopsIcon.MouseEntered += OnTroopsMouseEntered;
            troopsIcon.MouseLeft += OnTroopsMouseLeft;

            wealthIcon.MouseEntered += OnWealthMouseEntered;
            wealthIcon.MouseLeft += OnWealthMouseLeft;

            turnButton.Clicked += TurnButtonClicked;
        }

        void UnregisterEvents()
        {
            provincesIcon.MouseEntered -= OnProvincesMouseEntered;
            provincesIcon.MouseLeft -= OnProvincesMouseLeft;

            holdingsIcon.MouseEntered -= OnHoldingsMouseEntered;
            holdingsIcon.MouseLeft -= OnHoldingsMouseLeft;

            troopsIcon.MouseEntered -= OnTroopsMouseEntered;
            troopsIcon.MouseLeft -= OnTroopsMouseLeft;

            wealthIcon.MouseEntered -= OnWealthMouseEntered;
            wealthIcon.MouseLeft -= OnWealthMouseLeft;

            turnButton.Clicked -= TurnButtonClicked;
        }

        void SetChildrenProperties()
        {
            Dictionary<string, int> troops = new Dictionary<string, int>();

            string factionId = gameManager.PlayerFactionId;

            militaryManager.GetUnits().ToList().ForEach(u => troops.Add(u.Name, militaryManager.GetArmy(factionId, u.Id).Size));

            int provincesCount = worldManager.GetFactionProvinces(factionId).Count();
            int holdingsCount = holdingManager.GetFactionHoldings(factionId).Count();
            int troopsCount = 0;
            int wealthCount = worldManager.GetFaction(factionId).Wealth;

            provincesText.Text = StringUtils.NumberToHumanReadable(provincesCount);
            holdingsText.Text = StringUtils.NumberToHumanReadable(holdingsCount);
            troopsText.Text = StringUtils.NumberToHumanReadable(troopsCount);
            wealthText.Text = StringUtils.NumberToHumanReadable(wealthCount);
            turnText.Text = $"Turn: {gameManager.Turn}";

            provincesTooltip.Location = new Point2D(provincesIcon.Location.X, provincesIcon.ClientRectangle.Bottom);
            holdingsTooltip.Location = new Point2D(holdingsIcon.Location.X, holdingsIcon.ClientRectangle.Bottom);
            wealthTooltip.Location = new Point2D(wealthIcon.Location.X, wealthIcon.ClientRectangle.Bottom);
            troopsTooltip.Location = new Point2D(troopsIcon.Location.X, troopsIcon.ClientRectangle.Bottom);

            troopsTooltip.Text = string.Empty;

            if (troops != null && troops.Count > 0)
            {
                troopsText.Text = troops.Values.Sum().ToString();
                troops.ToList().ForEach(t => troopsTooltip.Text += $"{t.Key}: {t.Value}\n");
            }
        }

        void OnHoldingsMouseEntered(object sender, MouseEventArgs e)
        {
            holdingsTooltip.Show();
        }

        void OnHoldingsMouseLeft(object sender, MouseEventArgs e)
        {
            holdingsTooltip.Hide();
        }

        void OnProvincesMouseEntered(object sender, MouseEventArgs e)
        {
            provincesTooltip.Show();
        }

        void OnProvincesMouseLeft(object sender, MouseEventArgs e)
        {
            provincesTooltip.Hide();
        }

        void OnTroopsMouseEntered(object sender, MouseEventArgs e)
        {
            troopsTooltip.Show();
        }

        void OnTroopsMouseLeft(object sender, MouseEventArgs e)
        {
            troopsTooltip.Hide();
        }

        void OnWealthMouseEntered(object sender, MouseEventArgs e)
        {
            wealthTooltip.Show();
        }

        void OnWealthMouseLeft(object sender, MouseEventArgs e)
        {
            wealthTooltip.Hide();
        }
    }
}
