using System;
using System.Runtime.CompilerServices;
using NSubstitute;
using NUnit.Framework;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework.Internal.Execution;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT1_T_CookController_X_Display_PowerTube_S_Timer_Output
    {
        private ICookController _sut;
        private IPowerTube _powerTube;
        private IDisplay _display;
        private ITimer _timer;
        private IOutput _output;

        //private EventHandler _expiredArgs;
        private bool _eventExpired;

        [SetUp]
        public void Setup()
        {
            // Classes Faked
            _timer = Substitute.For<ITimer>();
            _output = Substitute.For<IOutput>();

            // Classes Used
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);

            // Class under test
            _sut = new CookController(_timer, _display, _powerTube);

            
        }

        // === TEST DISPLAY ===
        #region Display
        [Test]
        public void CookController_OnTimerTick_ShowTime()
        {
            _timer.TimeRemaining.Returns(09);

            _sut.StartCooking(50,10);
            _timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);
            //_T.OnTimerTick(new object(), EventArgs.Empty);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("Display shows: 00:09")));
        }
        #endregion


        // === TEST POWERTUBE ===
        #region PowerTube

        [TestCase(50, 10)]
        [TestCase(700, 10)]
        [TestCase(350, 10)]
        public void CookController_StartCooking_TurnOn(int pwr, int time)
        {
            _sut.StartCooking(pwr, time);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains($"PowerTube works with {pwr} W")));
        }


        [TestCase(0, 10)]
        [TestCase(49, 10)]
        [TestCase(701, 10)]
        [TestCase(1000, 10)]
        [TestCase(-50, 10)]
        public void CookController_StartCooking_TurnOn_Exception(int pwr, int time)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.StartCooking(pwr, time));
        }

        [TestCase(50, 10)]
        [TestCase(700, 10)]
        [TestCase(350, 10)]
        public void CookController_StartCooking_TurnOn_AlreadyOn(int pwr, int time)
        {
            _sut.StartCooking(pwr, time);
            Assert.Throws<ApplicationException>(() => _sut.StartCooking(pwr, time));

        }

        [Test]
        public void CookController_Stop_TurnOff()
        {
            int pwr = 50;
            int time = 10;
            _sut.StartCooking(pwr, time);

            _sut.Stop();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains($"PowerTube turned off")));
        }

        [Test]
        public void CookController_Stop_TurnOff_AlreadyOff()
        {
            _sut.Stop();
            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }
        
        /* === DON'T NEED TO TEST THAT HER
        [Test]
        public void CookController_OnTimerExpired_TurnOff()
        {
            //_T.StartCooking(100, 60);
            _timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            //_timer.Expired += Raise.EventWith(this, System.EventArgs.Empty);

            //_output.Received().OutputLine(Arg.Is<string>(str => str.Contains($"PowerTube turned off")));
        }
        */
        #endregion

    }
}
