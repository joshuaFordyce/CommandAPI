using System;
using Xunit;
using CommandAPI.Models;

namespace CommandAPI.Tests
{
    // We inherit the IDisposable interface
    public class CommandTests : IDisposable
        {
        // Create a global instance of our Command class
         Command testCommand;

         //Create a class constructor where we perform the setup of our testCommand object instance
        public CommandTests()
        {
            testCommand = new Command
            {
                HowTo = "Do something",
                Platform = "Some platform",
                CommandLine = "Some commandline"

            };
        }
        //Implement a Dispose method, to clean up our code
        public void Dispose()
        {
            testCommand = null;
        }
    
    
    
        [Fact]
        public void CanChangHowTo()
        {
            //Arrange
            var testCommand = new Command
            {
                HowTo = "Do something awesome",
                Platform = "xUnit",
                CommandLine = "dotnet test"
            };
            //Act
            testCommand.HowTo = "Execute Unit Tests";

            //Assert
            Assert.Equal("Execute Unit Tests", testCommand.HowTo);
        }
        [Fact]
        public void CanChangePlatform()
        {
            //Arrange
            //Act
            testCommand.Platform = "xUnit";

            //Assert
            Assert.Equal("xUnit", testCommand.Platform);
        }

        [Fact]

        public void CanChangeCommandLine()
        {
            //Arrange
            //Act
            testCommand.CommandLine = "dotnet test";

            //Assert
            Assert.Equal("dotnet test", testCommand.CommandLine);
        }
    }
}