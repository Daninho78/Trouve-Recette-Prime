using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookListUI : MonoBehaviour
{
    public GameObject livreItemPrefab;     // Le prefab LivreItem
    public Transform contentParent;        // Le Content du ScrollView
    public BookDetailsUI bookDetailsUI;

    private bool isLoading = false;
    private async void Start()
    {
        await AfficherTousLesLivres();
    }

    private async Task AfficherTousLesLivres()
    {
        if (isLoading)
        {
            return;
        }

        isLoading = true;

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        // 1. On récupère les livres depuis Supabase
        List<Book> livres = await BookService.GetAllBooks();

        // 2. Pour chaque livre, on instancie un item visuel
        foreach (var livre in livres)
        {
            GameObject item = Instantiate(livreItemPrefab, contentParent);
            // On récupère le script BookItem attaché au prefab instancié
            BookItem bookItem = item.GetComponent<BookItem>();

            // On lui envoie les infos du livre et le lien vers le script BookDetailsUI
            bookItem.Setup(livre, bookDetailsUI);

            // 3. On met à jour le texte du titre
            TextMeshProUGUI titreTexte = item.GetComponentInChildren<TextMeshProUGUI>();
            if (titreTexte != null)
            {
                titreTexte.text = livre.Title;
            }
            isLoading = false;
        }
    }

    public async void RefreshBooks()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        await AfficherTousLesLivres();
    }


}