using Microsoft.Xna.Framework;

using Narivia.Audio;
using Narivia.Input.Events;
using Narivia.Gui.GuiElements.Enumerations;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Notification indicator GUI element.
    /// </summary>
    public class GuiNotificationIndicator : GuiElement
    {
        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public override Point Size
        => new Point(32, 32);

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public NotificationIcon Icon { get; set; }

        GuiImage background;
        GuiImage icon;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new GuiImage
            {
                ContentFile = "Interface/notification_small",
                SourceRectangle = new Rectangle(96, 96, 32, 32)
            };

            icon = new GuiImage
            {
                ContentFile = "Interface/notification_icons",
                SourceRectangle = CalculateIconSourceRectangle(Icon)
            };

            Children.Add(background);
            Children.Add(icon);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            background.Position = Position;
            icon.Position = new Point(Position.X + 6, Position.Y + 6);
        }

        Rectangle CalculateIconSourceRectangle(NotificationIcon notificationIcon)
        {
            return new Rectangle((int)notificationIcon % 8 * 20,
                                 (int)notificationIcon / 8 * 20,
                                 20,
                                 20);
        }

        /// <summary>
        /// Fired by the Clicked event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnClicked(object sender, MouseButtonEventArgs e)
        {
            base.OnClicked(sender, e);

            AudioManager.Instance.PlaySound("Interface/click");
            Dispose();
        }

        /// <summary>
        /// Fired by the MouseEntered event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnMouseEntered(object sender, MouseEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/select");
        }
    }
}
