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
    public class IT10_T_Display_Dependencies_Output
    {
        private IDisplay _IT;
        private IOutput _output;
        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _IT = new Display(_output);
        }
        [Test]
        public void Display_OutputTimeITS()
        {
            string Log;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _IT.ShowTime(5, 30);
            Log = stringWriter.ToString();
            Assert.That(Log, Is.EqualTo("Display shows: 05:30\r\n"));
        }
        [Test]
        public void Display_OutputPowerITS()
        {
            string Log;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _IT.ShowPower(50);
            Log = stringWriter.ToString();
            Assert.AreEqual(Log, "Display shows: 50 W\r\n");
        }
    }
}
