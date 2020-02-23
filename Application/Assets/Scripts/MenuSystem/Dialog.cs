using UnityEngine;

public abstract class Dialog<T> : Dialog where T : Dialog<T>
{
    protected static T alreadyCreatedInstance;


    protected virtual void Awake()
    {
    }

    protected virtual void OnDestroy()
    {
        alreadyCreatedInstance = null;
    }

    protected static T Create(T dialogPrefab)
    {
        T newDialog = null;
        if (alreadyCreatedInstance == null || dialogPrefab.DestroyWhenClosed) {
            newDialog = MenuManager.Instance.CreateDialog(dialogPrefab);
            alreadyCreatedInstance = newDialog;
        } else
        {
            newDialog = alreadyCreatedInstance;
        }

        MenuManager.Instance.ShowDialog(newDialog);

        return newDialog;
    }

    public override void Close()
    { 
        MenuManager.Instance.CloseDialog(this);

        if (DestroyWhenClosed)
        {
            Destroy(gameObject);
        } else
        {
            gameObject.SetActive(false);
        }
    }

    public override void OnBackPressed()
    {
        Close();
    }

   
}

public abstract class Dialog : MonoBehaviour
{
    [Tooltip("Destroy the Game Object when menu is closed (reduces memory usage)")]
    public bool DestroyWhenClosed = true;

    [Tooltip("Disable menus that are under this one in the stack")]
    public bool DisableMenusUnderneath = true;

    public abstract void OnBackPressed();

    public abstract void Close();
}