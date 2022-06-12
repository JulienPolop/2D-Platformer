using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Assets.Scripts;

public class CanInteract : MonoBehaviour
{
    private InterractionInfoTextBoxManager theTextBox;
    GameObject PlayerContact = null;
    public bool isShowing = false;

    public UnityEvent methods;
    public int IdInScene { get; set; }

    // Use this for initialization
    void Start()
    {
        theTextBox = FindObjectOfType<InterractionInfoTextBoxManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerContact != null)
        {
            if (PlayerContact.gameObject.GetComponent<Player_Controller>().State == Player_Controller.StateEnum.Speaking && isShowing == true)
                Hide();

            if (PlayerContact.gameObject.GetComponent<Player_Controller>().State == Player_Controller.StateEnum.Normal && isShowing == false)
                Show();

            Player_Controller playerController = GameManager.Player;
            if (Input.GetButtonDown("Activate") && playerController.State == Player_Controller.StateEnum.Normal && playerController.IsGrounded)
                methods.Invoke();

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerContact = other.gameObject;
            Show();
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == PlayerContact)
        {
            PlayerContact = null;
            Hide();
        }
    }

    private void Show()
    {
        theTextBox = FindObjectOfType<InterractionInfoTextBoxManager>();
        theTextBox.Show();
        isShowing = true;
    }

    private void Hide()
    {
        theTextBox.Hide();
        isShowing = false;
    }
}
