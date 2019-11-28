using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using NUnit.Framework;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT3_Timer
    {
        private ITimer timer;
        private IPowerTube powerTube;
        private IUserInterface userInterface;
        private IDisplay display;
        private ICookController cooker;
        private IOutput output;

        [SetUp]
        public void SetUp()
        {
            // Subs
            output = Substitute.For<IOutput>();
            userInterface = Substitute.For<IUserInterface>();
            powerTube = Substitute.For<IPowerTube>();

            //Real
            display = new Display(output);
            timer = new Timer();
            cooker = new CookController(timer, display, powerTube, userInterface);
        }

        [Test]
        public void StartTimer_TimerTickEvent_OutputShowsCurrentTime()
        {
            timer.Start(1);
            Thread.Sleep(1100);
            output.Received().OutputLine(Arg.Is<string>(str => str.Contains("Display shows:")));
        }

        [Test]
        public void StartTimer_TimerExpiredEvent_CookingIsDone()
        {
            cooker.StartCooking(333,1);
            Thread.Sleep(61*1000);
            userInterface.Received(1).CookingIsDone();
            powerTube.Received(1).TurnOff();
        }

        [Test]
        public void StartTimer_TimerExpiredEvent_OutputShowsNoTimeLeft()
        {
            timer.Start(1);
            Thread.Sleep(61 * 1000);
            output.Received(1).OutputLine(Arg.Is<string>(str => str.Equals("Display shows: 00:00")));
        }
    }
}