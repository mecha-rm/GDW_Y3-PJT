using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    // the current page
    public Text text;

    // the index of the current page
    public int currPageIndex = 0;

    // the pages for the book
    public List<string> pages;

    // Start is called before the first frame update
    void Start()
    {
        // sets currPageIndex within bounds
        Mathf.Clamp(currPageIndex, 0, pages.Count - 1);

        // sets the current page
        if (pages.Count > 0)
            text.text = pages[currPageIndex];
    }

    // adds a page
    public void AddPage(string page)
    {
        pages.Add(page);

    }

    // removes a page
    public void RemovePage(string page)
    {
        pages.Remove(page);

        // clears out text if there are no pages left
        if (pages.Count == 0)
        {
            currPageIndex = -1;
            text.text = "";
        }
        else
        {
            // resets page count index.
            if (currPageIndex >= pages.Count)
            {
                // goes back a page
                currPageIndex--;

                // clmaps the page index
                currPageIndex = Mathf.Clamp(currPageIndex, 0, pages.Count - 1);

                // gets the new piece of text
                text.text = pages[currPageIndex];
            }
        }
    }

    // gets the current page
    public string GetCurrentPage()
    {
        // if the current page is not avialable or does not exist
        if (currPageIndex < 0 || currPageIndex >= pages.Count)
        {
            return "";
        }
        else
        {
            return pages[currPageIndex];
        }
    }

    // sets the current page with a provided index.
    public void SetCurrentPage(int newPage)
    {
        // the page is available
        if (newPage >= 0 && newPage < pages.Count)
        {
            currPageIndex = newPage;
            text.text = pages[currPageIndex];
        }
    }

    // goes onto the next page
    public void NextPage()
    {
        SetCurrentPage(currPageIndex + 1);
    }

    // goes to the previous page
    public void PreviousPage()
    {
        SetCurrentPage(currPageIndex - 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
