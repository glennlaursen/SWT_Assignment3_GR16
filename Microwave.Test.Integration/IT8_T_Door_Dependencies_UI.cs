using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT8_T_Door_Dependencies_UI
    {
        private IUserInterface ui;
        private ICookController cooker;
        private IDisplay display;
        private ILight light;
        private IDoor _sut;
        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;

        [SetUp]
        public void SetUp()
        {
            //Subs
            powerButton = Substitute.For<IButton>();
            timeButton = Substitute.For<IButton>();
            startCancelButton = Substitute.For<IButton>();
            cooker = Substitute.For<ICookController>();
            light = Substitute.For<ILight>();
            display = Substitute.For<IDisplay>();
            
            //Real
            _sut = new Door();
            ui = new UserInterface(powerButton, timeButton, startCancelButton, _sut, display, light, cooker);
        }

        [Test]
        public void ReadyState_OnDoorOpened_LightTurnedOn()
        {
            _sut.Open();
            light.Received().TurnOn();
        }

        [Test]
        public void SetPowerState_OnDoorOpened_LightTurnedOn()
        {
            ui.OnPowerPressed(powerButton, EventArgs.Empty);
            _sut.Open();
            light.Received().TurnOn();
        }

        [Test]
        public void SetTimeState_OnDoorOpened_LightTurnedOn()
        {
            ui.OnTimePressed(timeButton, EventArgs.Empty);
            _sut.Open();
            light.Received().TurnOn();
        }

        [Test]
        public void CookingState_OnDoorOpened_CookingStops()
        {
            ui.OnPowerPressed(powerButton, EventArgs.Empty);
            ui.OnTimePressed(timeButton, EventArgs.Empty);
            ui.OnStartCancelPressed(startCancelButton, EventArgs.Empty);

            _sut.Open();
            cooker.Received().Stop();
        }

        [Test]
        public void DoorOpenedState_OnDoorClosed_LightTurnedOff()
        {
            _sut.Open();
            _sut.Close();
            light.Received().TurnOff();
        }

        
    }
}