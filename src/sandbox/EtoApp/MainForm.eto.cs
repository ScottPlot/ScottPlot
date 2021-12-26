using Eto.Drawing;
using Eto.Forms;
using System;

namespace EtoApp
{
    partial class MainForm : Form
    {
        void InitializeComponent()
        {
            Title = "My Eto Form";
            MinimumSize = new Size(200, 150);
            Size = MinimumSize * 4;
            Padding = 10;

            Content = new StackLayout
            {
                Items =
                {
                    "Hello World!",
                    // add more controls here
                }
            };

            // create a few commands that can be used for the menu and toolbar
            var clickMe = new Command { MenuText = "Click Me!", ToolBarText = "Click Me!" };
            clickMe.Executed += (sender, e) => MessageBox.Show(this, "I was clicked!");

            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();

            var aboutCommand = new Command { MenuText = "About..." };
            aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

            // create menu
            Menu = new MenuBar
            {
                Items =
                {
                    // File submenu
                    new SubMenuItem { Text = "&File", Items = { clickMe } },
                    new SubMenuItem { Text = "&Edit", Items = { clickMe } },
                    new SubMenuItem { Text = "&View", Items = { clickMe } },
                },
                ApplicationItems =
                {
                    // application (OS X) or file menu (others)
                    new ButtonMenuItem { Text = "&Preferences..." },
                },
                QuitItem = quitCommand,
                AboutItem = aboutCommand
            };

            // create toolbar
            ToolBar = new ToolBar { Items = { clickMe } };
        }
    }
}
