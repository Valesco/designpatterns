using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsProject
{
    class User
    {
        private ICommand _command;
        private bool undoable;
        public List<string> commandStates = new List<string>();
        public int currentStateIndex = 0;

        public void SetCommand(ICommand command, bool undoable, int offset = 0)
        {
            this._command = command;
            this.undoable = undoable;
        }

        public void Action(User user)
        {
            if (!undoable && user.currentStateIndex < user.commandStates.Count())
                user.commandStates.RemoveRange(user.currentStateIndex+1, user.commandStates.Count() - user.currentStateIndex-1);

            _command.Execute();
        }
    }
}

