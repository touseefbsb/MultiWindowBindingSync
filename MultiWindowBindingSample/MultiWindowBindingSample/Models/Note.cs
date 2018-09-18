using MultiWindowBindingSample.Helpers;

namespace MultiWindowBindingSample.Models
{
    public class Note : Observable
    {
        private string _description;

        public long Id { get; set; }
        public string Description
        {
            get => _description;
            set => Set(ref _description, value, nameof(Description));
        }
        public Note THIS => this;

    }
}
