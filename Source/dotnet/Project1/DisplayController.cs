using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using System;

namespace Project1
{
    public class DisplayController
    {
        public DisplayScreen DisplayScreen { get; set; }

        private Label label;

        private int count = 0;

        public DisplayController(DisplayScreen displayScreen)
        {
            DisplayScreen = displayScreen;
        }

        public void LoadScreen()
        {
            DisplayScreen.BackgroundColor = Color.FromHex("FFFFFF");

            var assembly = typeof(Program).Assembly;
            var resourceName = $"{assembly.GetName().Name}.Resources.image.bmp";
            Resolver.Log.Info(resourceName);
            var image = Image.LoadFromResource(resourceName);
            DisplayScreen.Controls.Add(new Picture(
                left: 99,
                top: 18,
                width: 122,
                height: 129,
                image: image));

            label = new Label(
                left: 0,
                top: 162,
                width: DisplayScreen.Width,
                height: 16)
            {
                Text = "Hello, World",
                Font = new Font12x16(),
                TextColor = Color.FromHex("1E2834"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            DisplayScreen.Controls.Add(label);

            var button = new Button(
                left: 92,
                top: 193,
                width: 140,
                height: 28)
            {
                Text = "Click Me",
                Font = new Font12x16(),
                ForeColor = Color.White,
                TextColor = Color.FromHex("1E2834"),
                ShadowColor = Color.FromHex("1E2834"),
                HighlightColor = Color.FromHex("1E2834"),
            };
            button.Clicked += ButtonClicked;
            DisplayScreen.Controls.Add(button);
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            count++;
            string message = $"Clicked {count} times!";
            label.Text = message;
            Resolver.Log.Info(message);
        }
    }
}