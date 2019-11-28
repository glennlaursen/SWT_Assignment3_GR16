using System;
using NSubstitute;
using NUnit.Framework;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute.ExceptionExtensions;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT1_T_CookController_X_Display_PowerTube_S_Timer_Output
    {
        private ICookController _T;
        private IPowerTube _powerTube;
        private IDisplay _display;
        private ITimer _timer;
        private IOutput _output;

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
            _T = new CookController(_timer, _display, _powerTube);
        }

        /* === TIMER IS NOT TESTED IN T1 -> TESTED IN T2
        #region Timer

        [Test]
        public void Cooking()
        {
            _timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);
        }

        [Test]
        public void expire()
        {
            _timer.Expired += Raise.EventWith(this, EventArgs.Empty);
        }

        #endregion
        */

        // === TEST DISPLAY ===
        #region Display
        [Test]
        public void CookController_OnTimerTick_ShowTime()
        {
            _timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("Display shows:")));
        }
        #endregion


        // === TEST POWERTUBE ===
        #region PowerTube

        [TestCase(50, 10)]
        [TestCase(700, 10)]
        [TestCase(350, 10)]
        public void CookController_StartCooking_TurnOn(int pwr, int time)
        {
            _T.StartCooking(pwr, time);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains($"PowerTube works with {pwr} W")));
        }


        [TestCase(0, 10)]
        [TestCase(49, 10)]
        [TestCase(701, 10)]
        [TestCase(1000, 10)]
        [TestCase(-50, 10)]
        public void CookController_StartCooking_TurnOn_Exception(int pwr, int time)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _T.StartCooking(pwr, time));
        }

        [TestCase(50, 10)]
        [TestCase(700, 10)]
        [TestCase(350, 10)]
        public void CookController_StartCooking_TurnOn_AlreadyOn(int pwr, int time)
        {
            _T.StartCooking(pwr, time);
            Assert.Throws<ApplicationException>(() => _T.StartCooking(pwr, time));

        }

        [Test]
        public void CookController_Stop_TurnOff()
        {
            int pwr = 50;
            int time = 10;
            _T.StartCooking(pwr, time);

            _T.Stop();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains($"PowerTube turned off")));
        }

        [Test]
        public void CookController_Stop_TurnOff_AlreadyOff()
        {
            //_T.StartCooking(50, 10);
            //_powerTube.TurnOff();

            _T.Stop();
            _output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void CookController_OnTimerExpired_TurnOff()
        {

        }

        #endregion

    }
}
