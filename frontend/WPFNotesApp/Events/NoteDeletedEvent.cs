using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFNotesApp.Models;
using WPFNotesApp.ViewModels;

namespace WPFNotesApp.Events
{
    public class NoteDeletedEvent : PubSubEvent<NoteViewModel>
    {
    }
}
