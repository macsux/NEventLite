﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEventLite.Command_Handlers;
using NEventLite.Exceptions;
using NEventLite.Repository;
using NEventLite_Example.Commands;
using NEventLite_Example.Domain;

namespace NEventLite_Example.Command_Handlers
{
    public class NoteCommandHandler :
        ICommandHandler<CreateNoteCommand>, 
        ICommandHandler<EditNoteCommand>
    {
        private readonly RepositoryDecorator<Note> _repositoryBase;
        public Guid LastCreatedNoteGuid { get; private set; }

        public NoteCommandHandler(RepositoryDecorator<Note> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public int Handle(CreateNoteCommand command)
        {
            var newNote = new Note(command.title, command.desc, command.cat);
            _repositoryBase.Save(newNote);
            LastCreatedNoteGuid = newNote.Id;
            return 0;
        }

        public int Handle(EditNoteCommand command)
        {
            var LoadedNote = _repositoryBase.GetById(command.AggregateId);

            if (LoadedNote != null)
            {
                if (LoadedNote.CurrentVersion == command.TargetVersion)
                {
                    if (LoadedNote.Title != command.title)
                        LoadedNote.ChangeTitle(command.title);

                    if (LoadedNote.Category != command.cat)
                        LoadedNote.ChangeCategory(command.cat);

                    _repositoryBase.Save(LoadedNote);

                    return LoadedNote.CurrentVersion;
                }
                else
                {
                    throw new ConcurrencyException($"The version of the Note ({LoadedNote.CurrentVersion}) and Command ({command.TargetVersion}) didn't match.");
                }
            }
            else
            {
                throw new AggregateNotFoundException($"Note with ID {command.AggregateId} was not found.");
            }
        }
    }
}
