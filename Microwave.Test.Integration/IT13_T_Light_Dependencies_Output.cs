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
    public class IT13_T_Light_Dependencies_Output
    {
        private IOutput _output;
        private ILight _IT;
        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _IT = new Light(_output);
        }

        [Test]
        public void LightOn_OutputTest()
        {
            string Log;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _IT.TurnOn();
            Log = stringWriter.ToString();
            Assert.That(Log, Is.EqualTo("Light is turned on\r\n"));
        }

        [Test]
        public void LightOff_WhileLightOn_OutputTest()
        {
            _IT.TurnOn();
            string Log;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _IT.TurnOff();
            Log = stringWriter.ToString();
            Assert.That(Log, Is.EqualTo("Light is turned off\r\n"));
        }

        [Test]
        public void LightOff_OutputTest()
        {
            string Log;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _IT.TurnOff();
            Log = stringWriter.ToString();
            Assert.AreEqual(Log, "");
        }
    }
}
