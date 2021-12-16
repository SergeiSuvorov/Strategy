using System;
using Abstractions.Commands;
using UnityEngine;

namespace UserControlSystem
{
    public abstract class CommandCreatorBase<T> where T : ICommand
    {
        public void ProcessCommandExecutor(ICommandExecutor commandExecutor, Action<T> callback)
        {
            var classSpecificExecutor = commandExecutor as ICommandExecutor<T>;
            if (classSpecificExecutor != null)
            {
                ClassSpecificCommandCreation(commandExecutor, callback);
            }
        }

        protected abstract void ClassSpecificCommandCreation(ICommandExecutor commandExecutor, Action<T> creationCallback);

        public virtual void ProcessCancel() { }
    }
}