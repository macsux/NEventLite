﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using EventSourcingDemo.Domain;
using EventSourcingDemo.Repository;
using EventSourcingDemo.Storage;
using EventSourcingDemo.Util;

namespace EventSourcingDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            //Load dependecy resolver
            var Resolver = new DependencyResolver();
            var rep = Resolver.ResolveDependecy<IRepository<Note>>();

            //Create new note
            Note tmpNote = new Note("Test Note","Event Sourcing System Demo","Event Sourcing");

            Console.WriteLine("After Creation:");
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(tmpNote));
            Console.WriteLine();

            //Do some changes
            tmpNote.ChangeTitle("Test Note 123");
            tmpNote.ChangeCategory("Event Sourcing in .NET Example");

            //Commit chnages to the repository
            rep.Save(tmpNote);

            Console.WriteLine("After Committing Events:");
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(tmpNote));

            //Load same note using the aggregate id
            //This will replay the saved events and contruct a new note
            var tmpNoteToLoad = rep.GetById(tmpNote.Id);
            
            Console.WriteLine("");
            Console.WriteLine("After Replaying:");
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(tmpNoteToLoad));

            Console.WriteLine();
            Console.WriteLine("Press enter key to exit.");

            Console.ReadLine();

        }
    }
}
