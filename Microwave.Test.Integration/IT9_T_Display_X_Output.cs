using System;
using System.IO;
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
    public class IT9_T_Display_X_Output
    {
        private IPowerTube _sut;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _sut = new PowerTube(_output);
        }

        [Test]
        public void PowerTube_TurnOn_OutputPower()
        {
            string msgOut;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            _sut.TurnOn(50);

            msgOut = stringWriter.ToString();
            Assert.That(msgOut, Is.EqualTo("PowerTube works with 50 W\r\n"));
        }

        [Test]
        public void PowerTube_TurnOn_OutputTurnOff()
        {
            string msgOut;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            _sut.TurnOn(50);
            _sut.TurnOff();

            msgOut = stringWriter.ToString();
            Assert.That(msgOut, Is.EqualTo("PowerTube works with 50 W\r\nPowerTube turned off\r\n"));
        }
    }
}
