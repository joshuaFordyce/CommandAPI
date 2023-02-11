using System;
using System.Collections.Generic;
using Moq;
using AutoMapper;
using CommandAPI.Models;
using CommandAPI.Data;
using CommandAPI.Profiles;
using Xunit;
using CommandAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Dtos;
using Newtonsoft.Json.Serialization;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandAPIRepo> mockRepo;
        CommandsProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;

        public CommandsControllerTests()
        {
            mockRepo = new Mock<ICommandAPIRepo>();
            realProfile = new CommandsProfile();
            configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }

        public void Dispose()
        {
            mockRepo = null;
            mapper = null;
            configuration = null;
            realProfile = null;
        }

        
        [Fact]
        public void GetCommandItems_Returns200OK_WhenDBIsEmpty()

        {
            var mockRepo = new Mock<ICommandAPIRepo>();

            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));

            //we set up a commandsporfile instance and assigin it to a mapperConfiguration
            var realProfile = new CommandsProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            
            //We create a concrete instance of IMapper and give it our MapperConfiguration
            IMapper mapper = new Mapper(configuration);
            
            //We pass our Imapper instance to our commandcontroller constructor
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetAllCommands();
            
            Assert.IsType<OkObjectResult>(result.Result);

            //Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCommands_ReturnsOneItem_WhenDBHasOneResource()
        {
            //Arrange
            //We arrange our mockRepo to return a single command resource
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetAllCommands();

            //Asssert

            // in order to obtain the Value, we need to convert our original result to an OkObjectResult object so we can then navigate the object hierarcy
            var okResult = result.Result as OkObjectResult;

            // We obtain a listof CommandReadDtos
            var commands = okResult.Value as List<CommandReaDto>;

            // we assert that we have a single result set on our commands List
            Assert.Single(commands);
        }
        [Fact]
        public void GetAllCommands_Returns200OK_WhenDBHasOneResource()
        {
             //Arrange
             mockRepo.Setup(repo =>
             repo.GetAllCommands()).Returns(GetCommands(1));
             var controller = new CommandsController(mockRepo.Object, mapper);
            //Act
            var result = controller.GetAllCommands();
            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCommands_ReturnsCorrectType_WhenDBHasOneResource()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetAllCommands();
            //Assert
            Assert.IsType<ActionResult<IEnumerable<CommandReaDto>>>(result);
        }

        [Fact]
        public void GetCommandByID_Returns404NotFound_WhenNonExistentIDProvided()
        {
            //Arrange
            mockRepo.Setup(repo =>
            repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]

        public void GetCommandByID_Returns200OK__WhenValidIDProvided()
        {
            //Arrange
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(new Command { Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"});
            
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
            
        }

        [Fact]
       public void GetCommandByID_ReturnsCorrectResouceType_WhenValidIDProvided()
        {
            //Arrange
            mockRepo.Setup(repo =>
            repo.GetCommandById(1)).Returns(new Command { Id = 1,
            HowTo = "mock",
            Platform = "Mock",
            CommandLine = "Mock" });
            var controller = new CommandsController(mockRepo.Object, mapper);
            //Act
            var result = controller.GetCommandById(1);
            //Assert
            Assert.IsType<ActionResult<CommandReaDto>>(result);
        }

        [Fact]
        public void CreateCommand_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
        {
            //Arrange
            mockRepo.Setup(realProfile =>
            realProfile.GetCommandById(1)).Returns(new Command { Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"});

                var controller = new CommandsController(mockRepo.Object, mapper);

                //Act 
                var result = controller.CreateCommand(new CommandCreateDto { });

                //Assert
                Assert.IsType<ActionResult<CommandReaDto>>(result);
        }
            
        [Fact]
        public void CreateCommand_Returns201Created_whenValidObjectSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(new Command { Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"});

                var controller = new CommandsController(mockRepo.Object, mapper);

                //Act
                var result = controller.CreateCommand(new CommandCreateDto { });

                //Assert
                Assert.IsType<CreatedAtRouteResult>(result.Result);
        }

        [Fact]

        public void UpdateCommand_Returns204NoContent_WhenValidObjectSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo =>
            repo.GetCommandById(1)).Returns(new Command { Id = 1,
            HowTo = "mock",
            Platform = "Mock",
            CommandLine = "Mock"});

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.UpdateCommand(1, new CommandUpdateDto { });

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        public void PartialCommandUpdate_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo =>
                repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.PartialCommandUpdate(0,
                new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CommandUpdateDto> { });

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }


        [Fact]
        public void DeleteCommand_Returns204NoContent_WhenValidResourceIDSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "Mock", CommandLine = "Mock"});
            
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.DeleteCommand(1);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCommand_Returns_404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo =>
                repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.DeleteCommand(0);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        
        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if (num > 0){
                commands.Add(new Command
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration",
                    Platform = ".Net Core EF"
                });
            }
            return commands;
        }

    }
}