/*
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent<string> displayMessageEvent;
    public UnityEvent<int> updateScoreEvent;

    private void Start()
    {
        if (displayMessageEvent == null)
            displayMessageEvent = new UnityEvent<string>();

        if (updateScoreEvent == null)
            updateScoreEvent = new UnityEvent<int>();

        MessageDisplay messageDisplay = FindObjectOfType<MessageDisplay>();
        if (messageDisplay != null)
        {
            displayMessageEvent.AddListener(messageDisplay.DisplayMessage);
            updateScoreEvent.AddListener(messageDisplay.DisplayScore);
        }
        else
        {
            Debug.LogError("MessageDisplay object not found in the scene!");
        }
    }

    public void HandleCollision(GameObject otherObject)
    {
        if (otherObject.CompareTag("RoadBoundry"))
        {
            UpdateScore(5);
            displayMessageEvent.Invoke("You went out of bound!");
        }
        else if (otherObject.CompareTag("Pedestrian"))
        {
            UpdateScore(30);
            displayMessageEvent.Invoke("You killed a pedestrian!");
        }
        else if (otherObject.CompareTag("AI"))
        {
            UpdateScore(5);
            displayMessageEvent.Invoke("Collided with AI car! You should be more careful");
        }
        else if (otherObject.CompareTag("Cones"))
        {
            UpdateScore(5);
            displayMessageEvent.Invoke("Collided with Construction cones! You should avoid the cones!");
        }
    }

    public void UpdateScore(int points)
    {
        updateScoreEvent.Invoke(points);
    }

    public void CheckGameConditions(int score)
    {
        if (score <= 0)
        {
            StartCoroutine(Failed(4));
        }
    }

    private IEnumerator Failed(float delay)
    {
        yield return new WaitForSeconds(3);
        displayMessageEvent.Invoke("Unfortunately, You failed the test!");
        yield return new WaitForSeconds(delay);
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
*/
