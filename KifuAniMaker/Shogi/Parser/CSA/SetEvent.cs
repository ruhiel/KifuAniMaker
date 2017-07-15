using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetEvent : ICSAStatement
    {
        public string Event { get; set; }
        public SetEvent(string @event)
        {
            Event = @event;
        }

        public Board Execute(Board board)
        {
            board.Event = Event;

            return board;
        }
    }
}
