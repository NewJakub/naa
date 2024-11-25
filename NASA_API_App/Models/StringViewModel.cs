using System.ComponentModel;
namespace NASA_API_App.Models
{
    public class StringViewModel
    {
        private string _textFieldContent = "PlgKdthvA7rmL9RpBz2i91Y6Nfy9A5LqtDoh3eKt";

        public string TextFieldContent
        {
            get => _textFieldContent;
            set
            {
                if (_textFieldContent != value)
                {
                    _textFieldContent = value;
                    OnPropertyChanged(nameof(TextFieldContent));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
