using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.UI;
using MultiWindowBindingSample.Helpers;
using MultiWindowBindingSample.Models;

namespace MultiWindowBindingSample.ViewModels
{
    public class MainViewModel : Observable
    {
        public ObservableCollection<Note> NotesPrivate { get; }
        public AdvancedCollectionView Notes { get; set; }//Advanced Collection View is being used for filtering and sorting feature.
        public MainViewModel()
        {
            NotesPrivate = new ObservableCollection<Note>();
            Notes = new AdvancedCollectionView(NotesPrivate, true);
            for (var i = 0; i < 10; i++)
            {
                var note = new Note
                {
                    Id = i,
                    Description = "description" + i.ToString()
                };
                NotesPrivate.Add(note);
            }
        }

    }
}
