using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT5_T_UI_Dependencies_CookController
    {
        private IUserInterface ui;
        private ICookController cooker;
        private ITimer timer;
        private IDisplay display;
        private ILight light;
        private IPowerTube powerTube;
        private IOutput output;
        private IDoor door;
        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;

        [SetUp]
        public void SetUp()
        {
            //Subs
            output = Substitute.For<IOutput>();
            powerButton = Substitute.For<IButton>();
            timeButton = Substitute.For<IButton>();
            startCancelButton = Substitute.For<IButton>();
            door = Substitute.For<IDoor>();
            powerTube = Substitute.For<IPowerTube>();
            light = Substitute.For<ILight>();
            display = Substitute.For<IDisplay>();
            timer = Substitute.For<ITimer>();

            //Real
            cooker = new CookController(timer, display, powerTube);
            ui = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cooker);
            cooker = new CookController(timer, display, powerTube, ui);
        }

        [Test]
        public void UI_CC_PowerPressed_CookerStartsPowerTube()
        {
            ui.OnPowerPressed(powerButton, EventArgs.Empty); //Pressed once : 50 W
            ui.OnTimePressed(timeButton, EventArgs.Empty);
            ui.OnStartCancelPressed(startCancelButton, EventArgs.Empty);

            powerTube.Received(1).TurnOn(50);
        }

        [Test]
        public void UI_CC_CancelCooking_PowerTubeOff()
        {
            ui.OnPowerPressed(powerButton, EventArgs.Empty);
            ui.OnTimePressed(timeButton, EventArgs.Empty);
            ui.OnStartCancelPressed(startCancelButton, EventArgs.Empty);
            ui.OnStartCancelPressed(startCancelButton, EventArgs.Empty);

            powerTube.Received(1).TurnOff();
        }

        
    }
}