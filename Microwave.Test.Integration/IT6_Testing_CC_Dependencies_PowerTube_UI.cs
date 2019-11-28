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
    public class IT6_Testing_CC_Dependencies_PowerTube_UI
    {
        private ICookController _T;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IUserInterface _userInterface;
        private IOutput _output;
        private IButton _pButton;
        private IButton _tButton;
        private IButton _scButton;
        private IDoor _door;
        private ILight _light;
        private IDisplay _display;
        private CookController controller;


            [SetUp]
        public void Setup()
        {
            //classes faked
            _output = Substitute.For<IOutput>();
            //classes used
            _powerTube = new PowerTube(_output);
            _userInterface = new UserInterface(_pButton,_tButton,_scButton,
                                                _door,_display,_light,controller);
            //Class undertest
            _T = new CookController(_timer, _display, _powerTube, _userInterface);
        }
    }
}
