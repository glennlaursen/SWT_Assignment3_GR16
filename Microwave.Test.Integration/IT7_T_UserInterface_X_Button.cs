using System;
using System.Threading;
using Castle.Core.Smtp;
using NSubstitute;
using NUnit.Framework;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute.ExceptionExtensions;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT7_T_UserInterface_X_Button
    {
        private ICookController _cookController;
        private IUserInterface _T;
        private IDisplay _display;
        private ILight _light;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private IOutput _output;
        private IPowerTube _powerTube;
        private ITimer _timer;

        [SetUp]
        public void Setup()
        {
            // Classes Faked
            _output = Substitute.For<IOutput>();
            _door = Substitute.For<IDoor>();
            _cookController = Substitute.For<ICookController>();
            _light = Substitute.For<ILight>();
            _timer = Substitute.For<ITimer>();

            // Classes Used
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();

            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);

            _display = new Display(_output);

            // Class under test
            _T = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void UserInterface_OnPowerPressed_OnePress50W()
        {
            _powerButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display shows: 50 W")));
        }

        [Test]
        public void UserInterface_OnPowerPressed_TwoPressIncreaseTo100W()
        {
            _powerButton.Press();

            _powerButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display shows: 100 W")));
        }

        [Test]
        public void UserInterface_OnPowerPressed_PressReachLimitResetTo50W()
        {
            for(int i = 0; i < 14; i++)
                _powerButton.Press();
            
            _powerButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display shows: 50 W")));
        }

        [Test]
        public void UserInterface_OnTimePressed_ShowTime()
        {
            _powerButton.Press();
            _timeButton.Press();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display shows: 01:00")));
        }

        [Test]
        public void UserInterface_OnTimePressed_IncreaseTime()
        {
            _powerButton.Press();
            _timeButton.Press();

            _timeButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display shows: 02:00")));
        }

        [Test]
        public void UserInterface_OnStartCancelPressed_StateSetPower()
        {
            _powerButton.Press();

            _startCancelButton.Press();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display cleared")));
        }

        [Test]
        public void UserInterface_OnStartCancelPressed_StateSetTime()
        {
            _powerButton.Press();
            _timeButton.Press();

            _startCancelButton.Press();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("PowerTube works with 50 W")));
        }

        [Test]
        public void UserInterface_OnStartCancelPressed_StateCooking()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _startCancelButton.Press();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display cleared")));
        }
    }
}
