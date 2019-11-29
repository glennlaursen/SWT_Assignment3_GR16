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
        private IDisplay _sut;
        private IOutput _output;
        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _sut = new Display(_output);
        }
        [Test]
        public void Display_ShowTime()
        {
            string Log;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _sut.ShowTime(5, 30);
            Log = stringWriter.ToString();
            Assert.That(Log, Is.EqualTo("Display shows: 05:30\r\n"));
        }
        [Test]
        public void Display_ShowPower()
        {
            string Log;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            _sut.ShowPower(50);
            Log = stringWriter.ToString();
            Assert.AreEqual(Log, "Display shows: 50 W\r\n");
        }

        [Test]
        public void Display_Clear()
        {
            string Log;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            _sut.Clear();

            Log = stringWriter.ToString();
            Assert.That(Log, Is.EqualTo("Display cleared\r\n"));
        }
    }
}
