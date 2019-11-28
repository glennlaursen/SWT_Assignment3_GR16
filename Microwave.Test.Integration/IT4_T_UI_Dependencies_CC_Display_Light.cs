using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using NUnit.Framework;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT4_T_UI_Dependencies_CC_Display_Light
    {
        private ICookController cooker;
        private IUserInterface ui;
        private IDisplay display;
        private ILight light;
        private IPowerTube powerTube;
        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;
        private IDoor door;
        private IOutput output;

        [SetUp]
        public void SetUp()
        {
            //Subs
            output = Substitute.For<IOutput>();
            powerTube = Substitute.For<IPowerTube>();
            powerButton = Substitute.For<IButton>();
            timeButton = Substitute.For<IButton>();
            startCancelButton = Substitute.For<IButton>();
            door = Substitute.For<IDoor>();
            cooker = Substitute.For<ICookController>();

            //Real
            display = new Display(output);
            light = new Light(output);
            ui = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cooker);
        }

        [TestCase(50)]
        [TestCase(250)]
        [TestCase(700)]
        public void UI_Display_Power(int power)
        {
            for (int i = 0; i < power / 50; i++)
            {
                powerButton.Pressed += Raise.Event();
            }

            output.Received().OutputLine(Arg.Is<string>(s => s.Contains(Convert.ToString(power))));
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void UI_Display_Time(int minutes)
        {
            powerButton.Pressed += Raise.Event();
            for (int i = 1; i <= minutes; i++)
            {
                timeButton.Pressed += Raise.Event();
            }
            
            output.Received().OutputLine(Arg.Is<string>(s => s.Contains(Convert.ToString(minutes))));
        }

        [Test]
        public void UI_Display_Clear()
        {
            powerButton.Pressed += Raise.Event();
            startCancelButton.Pressed += Raise.Event();
            output.Received().OutputLine(Arg.Is<string>(s => s.Contains("Display cleared")));
        }

        [Test]
        public void UI_Light_On()
        {
            door.Opened += Raise.Event();
            output.Received().OutputLine(Arg.Is<string>(s => s.Contains("on")));
        }

        [Test]
        public void UI_Light_Off()
        {
            door.Opened += Raise.Event();
            door.Closed += Raise.Event();

            output.Received().OutputLine(Arg.Is<string>(s => s.Contains("off")));
        }
    }
}