using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFNotesApp.Models;

namespace WPFNotesApp.Events
{
    public class NoteSavedEvent: PubSubEvent<NoteRead>
    {
    }
}
