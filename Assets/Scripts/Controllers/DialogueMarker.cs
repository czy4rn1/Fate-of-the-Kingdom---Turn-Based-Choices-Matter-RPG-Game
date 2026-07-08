using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueMarker : Marker, INotification
{
    public string text;
    public bool isItLast;
    public PropertyName id => new PropertyName("DialogueMarker");
}
