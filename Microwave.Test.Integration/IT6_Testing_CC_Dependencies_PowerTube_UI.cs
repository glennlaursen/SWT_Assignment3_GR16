using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;
using NSubstitute;
using NUnit.Framework;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute.ExceptionExtensions;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT6_Testing_CC_Dependencies_PowerTube_UI
    {
        private CookController _sut;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IOutput _output;
        private IDisplay _display;
        private IUserInterface _userInterface;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _StartCancelButton;
        private IDoor _door;
        private ILight _light;


        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _light = Substitute.For<ILight>();
            _timeButton = Substitute.For<IButton>();
            _powerButton = Substitute.For<IButton>();
            _StartCancelButton = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();
            _timer = new MicrowaveOvenClasses.Boundary.Timer();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _sut = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_timeButton,_powerButton,_StartCancelButton,
                                                _door,_display,_light,_sut);
            _sut.UI = _userInterface;
        }
        //UI Test
        [Test]
        public void CookingIsDone_OnTimeExpired_DisplayCleared_Test()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _StartCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _sut.OnTimerExpired(this, EventArgs.Empty);
            _output.Received().OutputLine("Display cleared");
        }

        [Test]
        public void CookingIsDone_TurnOffLights_Test()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _StartCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _sut.OnTimerExpired(this, EventArgs.Empty);
            _light.Received().TurnOff();
        }
        //powertube test
        [Test]
        public void CookController_TurnOn_Test()
        {
            _sut.StartCooking(50, 2);
            _output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube works with 50 W"));
        }
        [Test]
        public void CookController_TurnOnAndTurnOff_Test()
        {
            _sut.StartCooking(50, 1);
            _sut.Stop();
            _output.Received().OutputLine(Arg.Is<string>(x => x == "PowerTube turned off"));
        }
    }
}
