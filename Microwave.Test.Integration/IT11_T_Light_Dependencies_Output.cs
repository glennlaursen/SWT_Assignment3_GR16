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
    public class IT11_T_Light_Dependencies_Output
    {
        private IOutput _output;
        private ILight _sut;
        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _sut = new Light(_output);
        }

        [Test]
        public void LightOn_OutputTest()
        {
            string Log;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _sut.TurnOn();
            Log = stringWriter.ToString();
            Assert.That(Log, Is.EqualTo("Light is turned on\r\n"));
        }

        [Test]
        public void LightOff_WhileLightOn_OutputTest()
        {
            _sut.TurnOn();
            string Log;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _sut.TurnOff();
            Log = stringWriter.ToString();
            Assert.That(Log, Is.EqualTo("Light is turned off\r\n"));
        }

        [Test]
        public void LightOff_OutputTest()
        {
            string Log;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _sut.TurnOff();
            Log = stringWriter.ToString();
            Assert.AreEqual(Log, "");
        }
    }
}
