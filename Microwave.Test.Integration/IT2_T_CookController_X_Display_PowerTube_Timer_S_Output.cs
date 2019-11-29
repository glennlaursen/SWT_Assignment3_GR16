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
    public class IT2_T_CookController_X_Display_PowerTube_Timer_S_Output
    {
        private ICookController _sut;
        private IPowerTube _powerTube;
        private IDisplay _display;
        private ITimer _timer;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            // Classes Faked
            _output = Substitute.For<IOutput>();

            // Classes Used
            _timer = new Timer();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);

            // Class under test
            _sut = new CookController(_timer, _display, _powerTube);
        }

        // === TEST TIMER ===
        #region Timer

        [Test]
        public void CookController_TimerTick_ShortEnough()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            _timer.TimerTick += (sender, args) => pause.Set();
            _sut.StartCooking(50,1);
            Assert.That(pause.WaitOne(1100));
        }

        [Test]
        public void CookController_TimerTick_LongEnough()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            _timer.TimerTick += (sender, args) => pause.Set();
            _sut.StartCooking(50, 1);
            Assert.That(!pause.WaitOne(900));
        }
        #endregion
    }
}
