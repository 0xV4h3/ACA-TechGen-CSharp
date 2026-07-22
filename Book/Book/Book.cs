using System.Collections;

namespace Book;

public class Book(List<string> pages) : IEnumerable<string>
{
    private readonly List<string> _pages = pages ?? throw new ArgumentNullException(nameof(pages));
    private int _step = 1;
    
    public int TotalPages => _pages.Count;
    
    public int Step
    {
        get => _step;
        set
        {
            if (value == 0) throw new ArgumentException("The step cannot be zero.");
            _step = value;
        }
    }
    
    public int CurrentIndex { get; private set; } = -1;
    public int CurrentPageNumber => (CurrentIndex >= 0 && CurrentIndex < TotalPages) ? CurrentIndex + 1 : 0;
    public string CurrentPageText => (CurrentIndex >= 0 && CurrentIndex < TotalPages) ? _pages[CurrentIndex] : string.Empty;

    public BookEnumerator GetEnumerator() => new BookEnumerator(this);
    IEnumerator<string> IEnumerable<string>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct BookEnumerator(Book book) : IEnumerator<string>
    {
        private readonly Book _book = book;
        private bool _isInitialized = false;
        
        public string Current => _book.CurrentPageText;
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_book.TotalPages == 0) return false;
            
            if (!_isInitialized)
            {
                _book.CurrentIndex = (_book._step > 0) ? 0 : _book.TotalPages - 1;
                _isInitialized = true;
                return true;
            }
            
            int targetIndex = _book.CurrentIndex + _book._step;
            
            if (_book._step > 0 && targetIndex >= _book.TotalPages)
            {
                _book.CurrentIndex = _book.TotalPages;
                return false;
            }
            
            if (_book._step < 0 && targetIndex < 0)
            {
                _book.CurrentIndex = -1;
                return false;
            }
            
            _book.CurrentIndex = targetIndex;
            return true;
        }

        public void Reset()
        {
            _isInitialized = false;
            _book.CurrentIndex = -1;
        }

        public void Dispose() { }
    }
}
