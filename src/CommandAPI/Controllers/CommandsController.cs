
/// we included two using directive here;
    // - system.Collections.Generic(supports IEnumberable)
    //  - Microsoft.AspNetCore.Mvc(supports pretty much everythign else detailed below) 
using CommandAPI.Data;
using System.Collections.Generic;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Models;

namespace CommandAPI.Controllers
{   // To enable routing we have decorated our CommandsController class with a [Route] attribute
    [Route("api/[controller")]

   
   
   
    // now we decorate our class with the APIController attribute which provides the following out-of-the-box behaviors for our controller:
    // - attribute Routing, Automatic HTTP 400 Error responses, Defaults Binding Sources, Problem details for error status codes
    [ApiController]

   
   
   
    //our controller class inherits from ControllerBase( it does not provide view support wwhich we dont need)
    // you can hinherit from Controller but it provides additoinsal support for Views that we dont need
    public class CommandsController : ControllerBase
    {   


        //we create a private read-only field_repository that will be assigned the inected MockComandAPIRepo object in our constructor and used throughout the rest of our code
        private readonly ICommandAPIRepo _repository;

        
        //At the point when the constructor is called, the DI system will spring into action and inject the required dependnecy when we aks for an instance ofICommandPIRepo. This is Constructo rDepenedency Injection
        public CommandsController(ICommandAPIRepo repository)
        {
            // We assignt he injected dependency to our private field
            _repository = repository;
        }
        //decorating our first simple action with HttpGet is really just specifying which verb our action responds to.
        [HttpGet]
        //The controller action should return an enumeration of Command objects
        public ActionResult<IEnumerable<Command>> GetAllCommands()
        {   //We call getallCommands on our repository and populate a local variable with the result
            var commandItems = _repository.GetAllCommands();
            

            // we return a HTP 200 Results(OK) and pass back our result set
            return Ok(commandItems);
        }  


        // The route to this controller action includes an additonal route parameter, in this case theId of the resource we want to retrieve; we can specify this in the HttpGet attribute as shown  
        [HttpGet("{id}")]

        //The controller action requires an id to be passed in as a parameter and returns an ActionResult of type Command
        public ActionResult<Command> GetCommandbyId(int id)
        {
            // We call GetCommandByID on our repository passing in the Id from our route, storing the result in a local variable.
            var commandItem = _repository.GetCommandById(id);
            if (commandItem == null)
            {
                // we check to see if our result is null annd, if so, return a 404 Not Found result
                return NotFound();
            };
            // Otherwise if we have a Command object, we return a 200 OK and the result
            return Ok(commandItem);
        }
        
    }
}