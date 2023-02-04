//using directives, noting that we have brought in the namespace for our models
using System.Collections.Generic;
using CommandAPI.Models;

namespace CommandAPI.Data
{
    //We specify a public interface and give it a name starting with Capital "I" to denote its an interface
    public interface ICommandAPIRepo
    {


        //We specify that our Repo should provide a save chaneges method
        bool SaveChanges();

        IEnumberable<Command> GetAllCommands();
        Command GetCommandById( int id);
        void CreateCommand(Command cmd);
        void UpdateCommand(Command cmd);
        void DeleteCommand(Command cmd);
    }
}