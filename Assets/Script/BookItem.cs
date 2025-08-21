using UnityEngine;
using UnityEngine.UI;

public class BookItem : MonoBehaviour
{
    private Book book;
    private BookDetailsUI bookDetailsUI;

    public void Setup(Book _book, BookDetailsUI _ui)
    {
        book = _book;
        bookDetailsUI = _ui;

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        bookDetailsUI.ShowDetails(book.Title, book.Id);
    }
}