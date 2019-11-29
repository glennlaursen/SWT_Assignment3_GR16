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
        private IUserInterface _ui;
        private IDisplay _display;
        private ILight _light;
        private IButton _sut_powerButton;
        private IButton _sut_timeButton;
        private IButton _sut_startCancelButton;
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

            // Classes Used
            _sut_powerButton = new Button();
            _sut_timeButton = new Button();
            _sut_startCancelButton = new Button();

            _light = new Light(_output);
            _powerTube = new PowerTube(_output);
            _display = new Display(_output);
            _timer = new Timer();
            _cookController = new CookController(_timer, _display, _powerTube);

            // Class under test
            _ui = new UserInterface(_sut_powerButton, _sut_timeButton, _sut_startCancelButton, _door, _display, _light, _cookController);
            _cookController = new CookController(_timer, _display, _powerTube, _ui);
        }
        
        [Test]
        public void UserInterface_OnPowerPressed_OnePress50W()
        {
            _sut_powerButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display shows: 50 W")));
        }
        

        [Test]
        public void UserInterface_OnPowerPressed_TwoPressIncreaseTo100W()
        {
            _sut_powerButton.Press();

            _sut_powerButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display shows: 100 W")));
        }

        [Test]
        public void UserInterface_OnPowerPressed_PressReachLimitResetTo50W()
        {
            for(int i = 0; i < 14; i++)
                _sut_powerButton.Press();
            
            _sut_powerButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display shows: 50 W")));
        }
        
        [Test]
        public void UserInterface_OnTimePressed_ShowTime()
        {
            _sut_powerButton.Press();
            _sut_timeButton.Press();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display shows: 01:00")));
        }
        
        [Test]
        public void UserInterface_OnTimePressed_IncreaseTime()
        {
            _sut_powerButton.Press();
            _sut_timeButton.Press();

            _sut_timeButton.Press();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display shows: 02:00")));
        }

        [Test]
        public void UserInterface_OnStartCancelPressed_StateSetPowerDisplayCleared()
        {
            _sut_powerButton.Press();

            _sut_startCancelButton.Press();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display cleared")));
        }
        
        [Test]
        public void UserInterface_OnStartCancelPressed_StateSetTime()
        {
            _sut_powerButton.Press();
            _sut_timeButton.Press();

            _sut_startCancelButton.Press();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("PowerTube works with 50 W")));
        }
        
        [Test]
        public void UserInterface_OnStartCancelPressed_StateCooking()
        {
            _sut_powerButton.Press();
            _sut_timeButton.Press();
            _sut_startCancelButton.Press();

            _sut_startCancelButton.Press();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display cleared")));
        }
    }
}
