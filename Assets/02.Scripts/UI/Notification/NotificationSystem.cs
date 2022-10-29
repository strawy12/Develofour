using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationSystem : MonoBehaviour
{
    [SerializeField]
    private NotificationPanel notificationPanelTemp;

    [SerializeField]
    private Transform notificationPanelParant;

    [SerializeField]
    private int maxPanelCount = 6;

    private Queue<NotificationPanel> notificationPanelQueue;

    private Stack<NotificationPanel> notificationPanelPool;

    private void Awake()
    {
        Init();
        Subscribe();
    }

    private void Init()
    {
        notificationPanelQueue = new Queue<NotificationPanel>();
        notificationPanelPool = new Stack<NotificationPanel>();
    }

    private void Subscribe()
    {

    }

    public void CreatePanel()
    {
        NotificationPanel panel = Instantiate(notificationPanelTemp, notificationPanelParant);


    }

    public void EnqueuePanel(NotificationPanel panel)
    {
        if (notificationPanelQueue.Count > maxPanelCount)
        {
            NotificationPanel temp = notificationPanelQueue.Dequeue();
            Destroy(temp);
        }


    }

}
