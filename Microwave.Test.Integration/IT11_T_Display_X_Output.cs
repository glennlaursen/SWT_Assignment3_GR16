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
    public class IT11_T_Display_X_Output
    {
        private IDisplay _T;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _T = new Display(_output);
        }

        [Test]
        public void Display_ShowTime_()
        {
            string msgOut;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            _T.ShowTime(10, 15);

            msgOut = stringWriter.ToString();
            Assert.That(msgOut, Is.EqualTo("Display shows: 10:15\r\n"));
        }

        [Test]
        public void Display_ShowPower_()
        {
            string msgOut;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            _T.ShowPower(50);

            msgOut = stringWriter.ToString();
            Assert.That(msgOut, Is.EqualTo("Display shows: 50 W\r\n"));
        }

        [Test]
        public void Display_Clear_()
        {
            string msgOut;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            _T.Clear();

            msgOut = stringWriter.ToString();
            Assert.That(msgOut, Is.EqualTo("Display cleared\r\n"));
        }
    }
}
