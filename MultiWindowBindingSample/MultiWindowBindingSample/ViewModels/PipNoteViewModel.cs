using System;

using MultiWindowBindingSample.Helpers;
using MultiWindowBindingSample.Models;

namespace MultiWindowBindingSample.ViewModels
{
    public class PipNoteViewModel : Observable
    {
        private Note _myNote;

        public Note MyNote
        {
            get => _myNote;
            set => Set(ref _myNote, value, nameof(MyNote));
        }

        public void FillMyNote(Note note) => MyNote = note;
    }
}
